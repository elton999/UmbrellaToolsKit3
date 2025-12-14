using System.Collections.Generic;
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

            SetVariables(dialogue);

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
                SetDialogueNode(basicNode, node);
                SetSpriteNode(basicNode, node);
                SetNodeVariables(basicNode, node);

                dialogue.Nodes.Add(node);
            }

            Export(path, dialogue);
        }

        public static void Export(string path, DialogueFormat dialogue)
        {
            Log.Write("Saving Dialogue ...");
            File.WriteAllText(path, dialogue.ToJson());
            Log.Write($"Saved: {path}");
        }

        private static void SetVariables(DialogueFormat dialogue)
        {
            var variableNames = new List<string>();
            foreach (var variable in DialogueData.Fields.Variables)
                variableNames.Add(variable.Value.Name);
            dialogue.Variables = variableNames.ToArray();

            var variableTypes = new List<int>();
            foreach (var variable in DialogueData.Fields.Variables)
                variableTypes.Add((int)variable.Value.Type);
            dialogue.VariablesType = variableTypes.ToArray();
        }

        private static void SetNodeVariables(BasicNode basicNode, Node node)
        {
            if (basicNode.VariableFields.Count == 0) return;

            var variables = new List<VariableList>();
            foreach (var variable in basicNode.VariableFields)
            {
                var variableList = new VariableList
                {
                    VariableId = variable.Id,
                    VariableType = (int)variable.GetType(),
                    VariableValue = variable.GetType() switch
                    {
                        VariableType.INT => variable.IntValue,
                        VariableType.FLOAT => variable.FloatValue,
                        VariableType.STRING => variable.StringValue,
                        _ => null
                    },
                };
                variables.Add(variableList);
            }
            node.Variables = variables.ToArray();
        }


        private static void SetDialogueNode(BasicNode basicNode, Node node)
        {
            if (basicNode is not DialogueNode) return;

            var dialogueNode = (DialogueNode)basicNode;
            foreach (var nodeOption in dialogueNode.NodeOptions)
                node.Options.Add(nodeOption.Id);
        }

        private static void SetSpriteNode(BasicNode basicNode, Node node)
        {
            if (basicNode is not SpriteNode) return;

            var spriteNode = (SpriteNode)basicNode;
            foreach (var nodeOption in spriteNode.NodeOptions)
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
