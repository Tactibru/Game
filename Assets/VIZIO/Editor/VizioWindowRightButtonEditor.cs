//Created by Neodrop
//mailto : neodrop@unity3d.ru
using UnityEngine;
//#pragma warning disable

public class VizioWindowRightButtonEditor : VizioWindowRightButtonSeparate
{
    public static void CreateNewWindow()
    {
        window = GetWindow<VizioWindowRightButtonSeparate>();
        if (window == null)
            return;
        window.title = "Logic Block Settings";
        window.minSize = new Vector2(512, 400);
        window.wantsMouseMove = true;
        window.ShowUtility();
    }
}