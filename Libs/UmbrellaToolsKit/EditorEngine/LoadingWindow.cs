#if !RELEASE
using ImGuiNET;
#endif

namespace UmbrellaToolsKit.EditorEngine
{
    public class LoadingWindow
    {
        private static bool _loading = false;

        public static void Begin()
        {
            _loading = true;
            Draw();
        }

        public static void End() => _loading = false;

        public static void Draw()
        {
            if (!_loading) return;
#if !RELEASE
            ImGui.Begin("loading", ImGuiWindowFlags.NoMove);
            {
                ImGui.Text("Loaging...");
            }
            ImGui.End();
#endif
        }
    }
}
