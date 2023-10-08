using System;
using ImGuiNET;
using System.Collections.Generic;
using MonoGame.ImGui.Standard.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using UmbrellaToolsKit.Input;
using UmbrellaToolsKit.EditorEngine.Nodes.Interfaces;
using UmbrellaToolsKit.Storage;
using UmbrellaToolsKit.EditorEngine.Windows;
using UmbrellaToolsKit.EditorEngine.Windows.DialogueEditor;

namespace UmbrellaToolsKit.EditorEngine.Nodes
{
    public class BasicNode : INode
    {
        protected Load _storage;
        protected Vector2 _titleSize = new Vector2(200, 30);
        protected Vector2 _mainSquareSize = new Vector2(200, 30);
        protected bool _isDragableNode = true;
        private int _index = 0;
        private INode _parentNode;

        private Vector2 _position;
        protected string _name;
        protected string _content;

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
        public bool IsDragbleNode { get => _isDragableNode; set => _isDragableNode = value; }
        public INode ParentNode { get => _parentNode; set => _parentNode = value; }
        public string Content { get => _content; set => _content = value; }

        public Color TitleColor = Color.Blue;

        public float ItemPedding = 30f;

        public Vector2 MousePosition;
        public Vector2 NodePositionOnClick;

        public bool CanMoveNode = false;

        public static event Action<BasicNode> OnSelectNode;
        public static event Action<BasicNode> OnDestroyNode;
        public static BasicNode currentSelectedNode;

        public BasicNode(Load storage, int id, string name, Vector2 position)
        {
            _storage = storage;
            _index= id;
            Id = id;
            Name = name;
            Position = position;
            DialogueEditorWindow.OnSave += SaveNode;
        }

        public virtual void Update() => HandlerMoveNode();

        public void HandlerMoveNode()
        {
            if (!IsDragbleNode) return;

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
            if (currentSelectedNode != this && currentSelectedNode != null) return;

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

        public virtual void Draw(ImDrawListPtr imDraw)
        {
            DrawSelectionArea(imDraw);
            DrawNodeSquare(imDraw);
            DrawNodeText(imDraw);
        }

        public virtual void OnDelete() 
        {
            _storage.AddItemFloat("Id", Id);
            _storage.DeleteNode($"name-{Id}");
            _storage.DeleteNode($"content-{Id}");
            _storage.DeleteNode($"position-{Id}-vector-x");
            _storage.DeleteNode($"position-{Id}-vector-y");

            var ids = _storage.getItemsFloat("Id");
            ids.Remove(Id);
            _storage.AddItemFloat("Id", ids);

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
            _name = _storage.getItemsString($"name-{Id}")[0];
            var contents = _storage.getItemsString($"content-{Id}");
            if(contents.Count > 0)
                _content = contents[0];

            float x = _storage.getItemsFloat($"position-{Id}-vector-x")[0];
            float y = _storage.getItemsFloat($"position-{Id}-vector-y")[0];
            _position = new Vector2(x, y);

            var parentsNode = _storage.getItemsFloat($"parent-{Id}");
            if(parentsNode.Count > 0)
            {
                INode parentNode = DialogueData.Nodes.FindAll(x => x.Id == (int)parentsNode[0])[0];
                ParentNode = parentNode;
            }
        }

        public virtual void OnSave() 
        {
            _storage.SetString($"name-{Id}", Name);
            _storage.SetString($"content-{Id}", Content);
            _storage.SetFloat($"position-{Id}-vector-x", Position.X);
            _storage.SetFloat($"position-{Id}-vector-y", Position.Y);
            if(ParentNode != null)
                _storage.SetFloat($"parent-{Id}", ParentNode.Id);
        }

        public virtual void DrawInspector()
        {
            ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4(1, 0, 0, 1));
            if (ImGui.Button("Delete"))
                OnDelete();
            ImGui.PopStyleColor();

            InspectorClass.DrawAllFields(this);
        }

        protected void DrawNodeText(ImDrawListPtr imDraw)
        {
            var titleTextPos = Position + Vector2.One * 8f;
            imDraw.AddText(titleTextPos.ToNumericVector2(), Color.White.PackedValue, Name);
        }

        protected void DrawNodeSquare(ImDrawListPtr imDraw)
        {
            Primativas.Square.Draw(imDraw, Position, MainSquareSize, Color.Black);

            Primativas.Square.Draw(imDraw, Position, TitleSize, TitleColor);
        }

        protected void DrawSelectionArea(ImDrawListPtr imDraw)
        {
            if (CanMoveNode)
                Primativas.Square.Draw(imDraw, SelectedNodePosition, SelectedNodeSize, Color.White);
        }
    }
}