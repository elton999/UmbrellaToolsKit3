using System;
using System.IO;
using UmbrellaToolsKit.EditorEngine.Nodes;
using UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes;

namespace UmbrellaToolsKit.EditorEngine.Windows.DialogueEditor
{
    public class DialogueJsonExport
    {
        public static void Export(string path)
        {
            var dialogue = new DialogueFormat();
            dialogue.Ids = new();
            dialogue.Nodes = new();
            foreach (var basicNode in DialogueData.Nodes)
            {
                dialogue.Ids.Add(basicNode.Id);
                SetStartNode(dialogue, basicNode);

                var node = new Node();
                node.Id = basicNode.Id;
                node.Name = basicNode.Name;
                node.Content = basicNode.Content;
                node.NextNode = -1;
                node.Options = new();

                SetNextNode(basicNode, node);
                SetNodeOption(basicNode, node);

                dialogue.Nodes.Add(node);
            }

            Export(path, dialogue);
        }

        public static void Export(string path, DialogueFormat dialogue)
        {
            Console.WriteLine("Saving Dialogue ...");
            File.WriteAllText(path, dialogue.ToJson());
            Console.WriteLine($"Saved: {path}");
        }

        private static void SetNodeOption(BasicNode basicNode, Node node)
        {
            if (basicNode is not NodeWithOptions) return;

            var nodeWithOptions = (NodeWithOptions)basicNode;
            foreach (var nodeOption in nodeWithOptions.NodeOptions)
                node.Options.Add(nodeOption.Id);
        }

        private static void SetNextNode(BasicNode basicNode, Node node)
        {
            if (basicNode is not NodeOutPut) return;

            var nodeOutPut = (NodeOutPut)basicNode;
            if (nodeOutPut.NodesConnectionIn.Count > 0)
                node.NextNode = nodeOutPut.NodesConnectionIn[0].Node.Id;
        }

        private static void SetStartNode(DialogueFormat dialogue, BasicNode node)
        {
            if (node is not StartNode) return;
            dialogue.StartNode = node.Id;
        }
    }
}
