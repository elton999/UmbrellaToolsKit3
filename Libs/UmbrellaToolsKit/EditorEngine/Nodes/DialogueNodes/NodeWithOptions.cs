﻿using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Standard.Extensions;
using System.Collections.Generic;
using UmbrellaToolsKit.EditorEngine.Nodes.Interfaces;
using UmbrellaToolsKit.EditorEngine.Windows.DialogueEditor;
using UmbrellaToolsKit.Input;
using UmbrellaToolsKit.Storage;

namespace UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes
{
    public class NodeWithOptions : NodeInPut, INodeOptions
    {
        private List<INodeOutPutle> _nodeOptions;
        private Vector2 _nodePosition => TitleSize - (Vector2.UnitY * TitleSize.Y / 2f);
        private Vector2 _optionSize => Vector2.UnitY * 30;

        public NodeWithOptions(Load storage, int id, string name, Vector2 position) : base(storage, id, name, position)
        {
            _nodeOptions = new List<INodeOutPutle>();
            UpdateBodyNodeSize();
            KeyBoardHandler.AddInput(Microsoft.Xna.Framework.Input.Keys.K);
        }

        public List<INodeOutPutle> NodeOptions { get => _nodeOptions; }

        public void AddNodeOption(INodeOutPutle node)
        {
            if (_nodeOptions.Contains(node)) return;
            _nodeOptions.Add(node);
        }

        public void CreateAnOption()
        {
            int id = DialogueData.GetNewNodeId();
            AddNodeOption(new NodeOptionOutPut(_storage, id, "option", Vector2.Zero));
        }

        public override void Update()
        {
            base.Update();
            if (KeyBoardHandler.KeyPressed(Microsoft.Xna.Framework.Input.Keys.K))
                CreateAnOption();
            foreach(var node in NodeOptions)
            {

            }
            UpdateBodyNodeSize();
        }

        public override void Draw(ImDrawListPtr imDraw)
        {
            base.Draw(imDraw);
            for(int i = 0; i < NodeOptions.Count; i++)
            {
                int optionNumber = i + 1;
                Vector2 circlePosition = GetCircleNodePosition(i);

                Vector2 textPosition = Position + _optionSize * (Vector2.UnitY * optionNumber);
                textPosition += Vector2.One * 8.0f;

                imDraw.AddCircleFilled(circlePosition.ToNumericVector2(), 5f, Color.Yellow.PackedValue);
                imDraw.AddText(textPosition.ToNumericVector2(), Color.White.PackedValue, $"Node Option {optionNumber}");
            }
        }

        private void UpdateBodyNodeSize()
        {
            _mainSquareSize = new Vector2(200, 30);
            _mainSquareSize += Vector2.UnitY * NodeOptions.Count * _mainSquareSize.Y;
        }

        private Vector2 GetCircleNodePosition(int index)
        {
            index += 1;
            Vector2 circlePosition = Position + _nodePosition;
            circlePosition += _optionSize * (Vector2.UnitY * index);

            return circlePosition;
        }
    }
}
