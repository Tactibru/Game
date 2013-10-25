//Created by Neodrop
//mailto : neodrop@unity3d.ru
using UnityEngine;
//#pragma warning disable

public class VizioWindowSettingsEditor : VizioWindowSettings
{
    //[MenuItem("Antares/VIZIO/Open Inspector")]
    public static void CreateNewWindow()
    {
        window = GetWindow<VizioWindowSettingsEditor>();
        if (window == null)
            return;
        window.title = "Editor Settings";
        window.minSize = new Vector2(512, 256);
        window.wantsMouseMove = true;
        window.ShowUtility();
    }
}