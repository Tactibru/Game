//Created by Neodrop
//mailto : neodrop@unity3d.ru
using UnityEngine;
//#pragma warning disable

public class VizioWindowMessagesEditor : VizioWindowMessages
{
    public static void CreateNewWindow()
    {
        window = GetWindow<VizioWindowMessages>();
        if (window == null)
            return;
        window.title = "Advanсed Messages Manager";
        window.minSize = new Vector2(512, 512);
        window.wantsMouseMove = true;
        window.ShowUtility();
    }
}