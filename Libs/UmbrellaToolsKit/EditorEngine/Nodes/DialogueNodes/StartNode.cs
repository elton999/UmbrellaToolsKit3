using Microsoft.Xna.Framework;
using System.Collections.Generic;
using UmbrellaToolsKit.EditorEngine.Windows;
using UmbrellaToolsKit.Storage;

namespace UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes
{
    public class StartNode : NodeOutPut
    {
        public StartNode(Load storage, int id, Vector2 position) : base(storage, id, "start", position)
        {
            TitleColor = Color.Red;
            _titleSize = new Vector2(100, 30);
            _mainSquareSize = new Vector2(100, 30);

            DialogueEditorWindow.OnSave += OnSave;
        }

        public override void OnSave()
        {
            base.OnSave();
            _storage.SetString($"Nodes-Object-{Id}", typeof(StartNode).Namespace + "." + typeof(StartNode).Name);
        }

        public override void OnDelete()
        {
            base.OnDelete();
            DialogueEditorWindow.OnSave -= OnSave;
        }
    }
}
