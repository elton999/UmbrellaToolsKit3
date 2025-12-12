using System;
#if !RELEASE
using ImGuiNET;
using MonoGame.ImGui.Extensions;
#endif
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using UmbrellaToolsKit.Input;
using UmbrellaToolsKit.EditorEngine.Nodes.Interfaces;
using UmbrellaToolsKit.Storage;
using UmbrellaToolsKit.EditorEngine.Windows;
using UmbrellaToolsKit.EditorEngine.Windows.DialogueEditor;
using System.Collections.Generic;

namespace UmbrellaToolsKit.EditorEngine.Nodes
{
    public class BasicNode : INode
    {
        protected Load _storage;
        protected Vector2 _titleSize = new Vector2(200, 30);
        protected Vector2 _mainSquareSize = new Vector2(200, 30);
        protected bool _isDraggableNode = true;
        private int _index = 0;
        private INode _parentNode;

        private Vector2 _position;
        protected string _name;
        protected string _content;

        private List<VariableFields> _variableFields = new List<VariableFields>();

        public int Id
        {
            get => _index;
            set => _index = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
            }
        }

        public Vector2 MainSquareSize { get => _mainSquareSize; }
        public Vector2 SelectedNodeSize { get => MainSquareSize + Vector2.One * 4; }
        public Vector2 SelectedNodePosition { get => Position - Vector2.One * 2f; }
        public Vector2 TitleSize { get => _titleSize; }
        public bool IsDraggableNode { get => _isDraggableNode; set => _isDraggableNode = value; }
        public INode ParentNode { get => _parentNode; set => _parentNode = value; }
        public string Content { get => _content; set => _content = value; }

        public Color TitleColor = Color.Blue;

        public float ItemPadding = 30f;

        public Vector2 MousePosition;
        public Vector2 NodePositionOnClick;

        public bool CanMoveNode = false;
        public List<VariableFields> VariableFields { get => _variableFields; set => _variableFields = value; }

        public static event Action<BasicNode> OnSelectNode;
        public static event Action<BasicNode> OnDestroyNode;
        public static BasicNode currentSelectedNode;

        public const string ID_KEY = "Id";
        public const string NAME_KEY = "n-";
        public const string CONTENT_KEY = "c-";
        public const string POS_VECTOR_X_KEY = "pos-v-x-";
        public const string POS_VECTOR_Y_KEY = "pos-v-y-";
        public const string PARENT_NODE_KEY = "p-";

        public BasicNode(Load storage, int id, string name, Vector2 position)
        {
            _storage = storage;
            _index = id;
            Id = id;
            Name = name;
            Position = position;
            DialogueEditorWindow.OnSave += SaveNode;
        }

        public virtual void Update() => HandlerMoveNode();

        public void HandlerMoveNode()
        {
            if (!IsDraggableNode) return;

            var rectangle = new Rectangle(Position.ToPoint(), MainSquareSize.ToPoint());
            var mousePosition = Mouse.GetState().Position;
            var mousePoint = new Rectangle(mousePosition, new Point(1));

            if (rectangle.Contains(mousePoint)) CanMoveNode = true;
            if (MouseHandler.ButtonLeftReleased) CanMoveNode = false;

            if (CanMoveNode) MoveNode();
            else if (currentSelectedNode == this) currentSelectedNode = null;
        }

        public void MoveNode()
        {
            if (currentSelectedNode != this && currentSelectedNode is not null) return;

            if (MouseHandler.ButtonLeftOneClick)
            {
                MousePosition = MouseHandler.Position;
                NodePositionOnClick = Position;
                currentSelectedNode = this;
                OnSelectNode?.Invoke(this);
            }

            if (MouseHandler.ButtonLeftPressing)
            {
                var currentMousePosition = MouseHandler.Position;
                var direction = currentMousePosition - MousePosition;
                Position = NodePositionOnClick + direction;
            }
        }
#if !RELEASE
        public virtual void Draw(ImDrawListPtr imDraw)
        {
            DrawSelectionArea(imDraw);
            DrawNodeSquare(imDraw);
            DrawNodeText(imDraw);
        }
#endif
        public virtual void OnDelete()
        {
            _storage.AddItemFloat(ID_KEY, Id);
            _storage.DeleteNode(NAME_KEY + Id);
            _storage.DeleteNode(CONTENT_KEY + Id);
            _storage.DeleteNode(POS_VECTOR_X_KEY + Id);
            _storage.DeleteNode(POS_VECTOR_Y_KEY + Id);

            var ids = _storage.getItemsFloat(ID_KEY);
            ids.Remove(Id);
            _storage.AddItemFloat(ID_KEY, ids);

            DialogueData.RemoveNode(this);
            OnDestroyNode?.Invoke(this);

            DialogueEditorWindow.OnSave -= SaveNode;
        }

        public void SaveNode()
        {
            OnSave();
            _storage.Save();
        }

        public virtual void Load()
        {
            _name = _storage.getItemsString(NAME_KEY + Id)[0];
            var contents = _storage.getItemsString(CONTENT_KEY + Id);
            if (contents.Count > 0)
                _content = contents[0];

            float x = _storage.getItemsFloat(POS_VECTOR_X_KEY + Id)[0];
            float y = _storage.getItemsFloat(POS_VECTOR_Y_KEY + Id)[0];
            _position = new Vector2(x, y);

            var parentsNode = _storage.getItemsFloat(PARENT_NODE_KEY + Id);
            if (parentsNode.Count > 0)
            {
                INode parentNode = DialogueData.Nodes.FindAll(x => x.Id == (int)parentsNode[0])[0];
                ParentNode = parentNode;
            }
        }

        public virtual void OnSave()
        {
            _storage.SetString(NAME_KEY + Id, Name);
            _storage.SetString(CONTENT_KEY + Id, Content);
            _storage.SetFloat(POS_VECTOR_X_KEY + Id, Position.X);
            _storage.SetFloat(POS_VECTOR_Y_KEY + Id, Position.Y);
            if (ParentNode is not null)
                _storage.SetFloat(PARENT_NODE_KEY + Id, ParentNode.Id);
        }

#if !RELEASE
        public virtual void DrawInspector()
        {
            if (Fields.Buttons.RedButton("Delete"))
                OnDelete();

            InspectorClass.DrawAllFields(this);
        }

        protected void DrawNodeText(ImDrawListPtr imDraw)
        {
            var titleTextPos = Position + Vector2.One * 8f;
            imDraw.AddText(titleTextPos.ToNumericVector2(), Color.White.PackedValue, Name);
        }

        protected void DrawNodeSquare(ImDrawListPtr imDraw)
        {
            Primitives.Square.Draw(imDraw, Position, MainSquareSize, Color.Black);

            Primitives.Square.Draw(imDraw, Position, TitleSize, TitleColor);
        }

        protected void DrawSelectionArea(ImDrawListPtr imDraw)
        {
            if (!CanMoveNode) return;
            Primitives.Square.Draw(imDraw, SelectedNodePosition, SelectedNodeSize, Color.White);
        }
#endif
    }
}