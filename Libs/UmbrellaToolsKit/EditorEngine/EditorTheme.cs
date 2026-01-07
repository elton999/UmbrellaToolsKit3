#if !RELEASE
using ImGuiNET;
using System.Numerics;

public static class EditorTheme
{
    public static void ApplyBlueprintTheme()
    {
        var style = ImGui.GetStyle();
        var colors = style.Colors;

        style.WindowPadding = new Vector2(8, 8);
        style.FramePadding = new Vector2(6, 4);
        style.ItemSpacing = new Vector2(8, 6);
        style.IndentSpacing = 18;
        style.ScrollbarSize = 14;
        style.WindowBorderSize = 1;
        style.FrameBorderSize = 1;
        style.TabBorderSize = 1;

        style.WindowRounding = 4;
        style.FrameRounding = 3;
        style.PopupRounding = 4;
        style.ScrollbarRounding = 3;
        style.GrabRounding = 3;
        style.TabRounding = 3;

        colors[(int)ImGuiCol.WindowBg] = new Vector4(0.10f, 0.11f, 0.13f, 1f);
        colors[(int)ImGuiCol.ChildBg] = new Vector4(0.12f, 0.13f, 0.15f, 1f);
        colors[(int)ImGuiCol.PopupBg] = new Vector4(0.08f, 0.09f, 0.11f, 1f);

        colors[(int)ImGuiCol.Border] = new Vector4(0.22f, 0.24f, 0.28f, 1f);
        colors[(int)ImGuiCol.BorderShadow] = new Vector4(0, 0, 0, 0);

        colors[(int)ImGuiCol.Text] = new Vector4(0.90f, 0.91f, 0.93f, 1f);
        colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.50f, 0.52f, 0.55f, 1f);

        colors[(int)ImGuiCol.FrameBg] = new Vector4(0.16f, 0.18f, 0.21f, 1f);
        colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.22f, 0.26f, 0.30f, 1f);
        colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.28f, 0.32f, 0.38f, 1f);

        colors[(int)ImGuiCol.Button] = new Vector4(0.18f, 0.20f, 0.24f, 1f);
        colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.25f, 0.29f, 0.35f, 1f);
        colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.30f, 0.36f, 0.44f, 1f);

        colors[(int)ImGuiCol.Header] = new Vector4(0.20f, 0.23f, 0.27f, 1f);
        colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.30f, 0.34f, 0.40f, 1f);
        colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.36f, 0.42f, 0.50f, 1f);

        colors[(int)ImGuiCol.Tab] = new Vector4(0.15f, 0.17f, 0.20f, 1f);
        colors[(int)ImGuiCol.TabHovered] = new Vector4(0.28f, 0.32f, 0.38f, 1f);
        colors[(int)ImGuiCol.TabActive] = new Vector4(0.22f, 0.26f, 0.32f, 1f);
        colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.12f, 0.14f, 0.17f, 1f);
        colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4(0.18f, 0.21f, 0.26f, 1f);

        colors[(int)ImGuiCol.TitleBg] = new Vector4(0.08f, 0.09f, 0.11f, 1f);
        colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.14f, 0.16f, 0.19f, 1f);
        colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.06f, 0.07f, 0.09f, 1f);

        colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.08f, 0.09f, 0.11f, 1f);
        colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.20f, 0.23f, 0.27f, 1f);
        colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.30f, 0.34f, 0.40f, 1f);
        colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.36f, 0.42f, 0.50f, 1f);

        colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.28f, 0.32f, 0.38f, 0.6f);
        colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.36f, 0.42f, 0.50f, 0.8f);
        colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.40f, 0.48f, 0.58f, 1f);

        colors[(int)ImGuiCol.CheckMark] = new Vector4(0.52f, 0.68f, 0.90f, 1f);
        colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.52f, 0.68f, 0.90f, 1f);
        colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.62f, 0.78f, 1.00f, 1f);
    }

    public static void ApplyDarkPro()
    {
        var style = ImGui.GetStyle();

        style.WindowPadding = new Vector2(10, 10);
        style.FramePadding = new Vector2(6, 4);
        style.ItemSpacing = new Vector2(8, 6);
        style.ItemInnerSpacing = new Vector2(6, 4);

        style.ScrollbarSize = 14;
        style.GrabMinSize = 10;

        style.WindowRounding = 4;
        style.FrameRounding = 3;
        style.PopupRounding = 4;
        style.ScrollbarRounding = 6;
        style.GrabRounding = 3;

        style.TabRounding = 4;

        var colors = style.Colors;

        Vector4 bg = new(0.11f, 0.11f, 0.12f, 1.00f);
        Vector4 bgAlt = new(0.16f, 0.16f, 0.17f, 1.00f);
        Vector4 frame = new(0.20f, 0.20f, 0.22f, 1.00f);
        Vector4 frameHover = new(0.26f, 0.26f, 0.28f, 1.00f);
        Vector4 frameActive = new(0.30f, 0.30f, 0.32f, 1.00f);

        Vector4 text = new(0.92f, 0.92f, 0.92f, 1.00f);
        Vector4 textDisabled = new(0.55f, 0.55f, 0.55f, 1.00f);

        Vector4 accent = new(0.30f, 0.55f, 0.85f, 1.00f);
        Vector4 accentHover = new(0.35f, 0.60f, 0.90f, 1.00f);
        Vector4 accentActive = new(0.25f, 0.50f, 0.80f, 1.00f);

        colors[(int)ImGuiCol.Text] = text;
        colors[(int)ImGuiCol.TextDisabled] = textDisabled;

        colors[(int)ImGuiCol.WindowBg] = bg;
        colors[(int)ImGuiCol.ChildBg] = bg;
        colors[(int)ImGuiCol.PopupBg] = bgAlt;

        colors[(int)ImGuiCol.Border] = new Vector4(0, 0, 0, 0.30f);
        colors[(int)ImGuiCol.BorderShadow] = Vector4.Zero;

        colors[(int)ImGuiCol.FrameBg] = frame;
        colors[(int)ImGuiCol.FrameBgHovered] = frameHover;
        colors[(int)ImGuiCol.FrameBgActive] = frameActive;

        colors[(int)ImGuiCol.TitleBg] = bgAlt;
        colors[(int)ImGuiCol.TitleBgActive] = bgAlt;
        colors[(int)ImGuiCol.TitleBgCollapsed] = bgAlt;

        colors[(int)ImGuiCol.Button] = frame;
        colors[(int)ImGuiCol.ButtonHovered] = frameHover;
        colors[(int)ImGuiCol.ButtonActive] = frameActive;

        colors[(int)ImGuiCol.Header] = frame;
        colors[(int)ImGuiCol.HeaderHovered] = frameHover;
        colors[(int)ImGuiCol.HeaderActive] = frameActive;

        colors[(int)ImGuiCol.Tab] = frame;
        colors[(int)ImGuiCol.TabHovered] = frameHover;
        colors[(int)ImGuiCol.TabActive] = frameActive;
        colors[(int)ImGuiCol.TabUnfocused] = frame;
        colors[(int)ImGuiCol.TabUnfocusedActive] = frameActive;

        colors[(int)ImGuiCol.CheckMark] = accent;
        colors[(int)ImGuiCol.SliderGrab] = accent;
        colors[(int)ImGuiCol.SliderGrabActive] = accentActive;

        colors[(int)ImGuiCol.ResizeGrip] = accent;
        colors[(int)ImGuiCol.ResizeGripHovered] = accentHover;
        colors[(int)ImGuiCol.ResizeGripActive] = accentActive;

        colors[(int)ImGuiCol.ScrollbarGrab] = frameHover;
        colors[(int)ImGuiCol.ScrollbarGrabHovered] = frameActive;
        colors[(int)ImGuiCol.ScrollbarGrabActive] = frameActive;

        colors[(int)ImGuiCol.Separator] = frameHover;
        colors[(int)ImGuiCol.SeparatorHovered] = accentHover;
        colors[(int)ImGuiCol.SeparatorActive] = accentActive;
    }
}
#endif