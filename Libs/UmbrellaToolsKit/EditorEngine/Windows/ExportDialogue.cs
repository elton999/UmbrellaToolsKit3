#if !RELEASE
using Eto.Forms;
#endif
namespace UmbrellaToolsKit.EditorEngine.Windows
{
    public class ExportDialogue
    {
#if !RELEASE
        public static SaveFileDialog SaveFileDialog(string title, string extensionTile, string extension)
        {
            return new SaveFileDialog
            {
                Title = title,
                Filters = { new FileDialogFilter(extensionTile, extension) }
            };
        }

        public static bool ShowSaveDialog(SaveFileDialog saveFileDialog)
        {
            return saveFileDialog.ShowDialog(Application.Instance.MainForm) == DialogResult.Ok;
        }
#endif
    }
}
