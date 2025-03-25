#if !RELEASE
using Eto.Forms;
#endif

namespace UmbrellaToolsKit.EditorEngine.Windows
{
    public class OpenFileDialogue
    {
#if !RELEASE
        public static OpenFileDialog OpenFileDialog(string title, string extensionTile, string extension)
        {
            return new OpenFileDialog
            {
                Title = title,
                Filters = { new FileDialogFilter(extensionTile, extension) }
            };
        }

        public static bool SaveFileDialog(OpenFileDialog openFileDialog)
        {
            return openFileDialog.ShowDialog(Application.Instance.MainForm) == DialogResult.Ok;
        }
#endif
    }
}
