using System.Collections.Generic;
using UmbrellaToolsKit.EditorEngine.Attributes;
using Microsoft.Xna.Framework.Input;
using UmbrellaToolsKit.Input;

namespace UmbrellaToolsKit.EditorEngine.Windows.GameSettings
{
    [GameSettingsProperty(nameof(InputGameSettings), "/Content/")]
    public class InputGameSettings : GameSettingsProperty
    {
        [System.Serializable]
        public class InputData
        {
            [ShowEditor] public string InputName;
            [ShowEditor] public List<Keys> Keys = new();
        }

        [ShowEditor] public string Name;
        [ShowEditor] public List<InputData> InputDataList = new();

        public void BindAllInputs()
        {
            foreach (var input in InputDataList)
                KeyBoardHandler.AddInput(input.InputName, input.Keys.ToArray());
        }
    }
}
