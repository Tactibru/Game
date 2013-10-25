using Antares.Vizio.Editor;
using UnityEditor;
using UnityEngine;

public class UniverseTooltipsWindow : EditorWindow
{
    public static void CreateNewWindow()
    {
        TooltipsEditor.window = GetWindow<TooltipsEditor>();
        if (TooltipsEditor.window == null)
            return;

        TooltipsEditor.window.title = "";
        TooltipsEditor.window.minSize = new Vector2(650, 512);
        TooltipsEditor.window.wantsMouseMove = true;
        TooltipsEditor.window.ShowUtility();
    }
    
    public static void ShowWindow()
    {
        CreateNewWindow();
        var r = TooltipsEditor.window.position;
        var rv = VizioWindow.window.position;

        r.x = (rv.x + rv.width*.5f) - r.width*.5f;
        r.y = (rv.y + rv.height * .5f) - r.height * .5f;

        TooltipsEditor.window.position = r;

        TooltipsEditor.window.title = "Did you know about Universe ?";
        TooltipsEditor.window.guiDelegate = TooltipsEditorImplementation.DrawTooltipsWindow;
    }
}
