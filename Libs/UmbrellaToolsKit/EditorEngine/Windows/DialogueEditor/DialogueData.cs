﻿using System.Collections.Generic;
using UmbrellaToolsKit.EditorEngine.Nodes;

namespace UmbrellaToolsKit.EditorEngine.Windows.DialogueEditor
{
    internal class DialogueData
    {
        private static List<BasicNode> _nodes = new List<BasicNode>();

        public static int LastNodeId = 0;
        public static List<BasicNode> Nodes { get => _nodes; }

        public static int GetNewNodeId()
        {
            int id = LastNodeId;
            LastNodeId++;
            return id;
        }

        public static void AddNode(BasicNode node)
        {
            if(_nodes.Contains(node)) return;
            _nodes.Add(node);
        }

        public static void RemoveNode(BasicNode node) => _nodes.Remove(node);
    }
}
