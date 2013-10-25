//Created by Neodrop
//mailto : neodrop@unity3d.ru
using UnityEngine;
//#pragma warning disable

public class VizioWindowLocalVarEditor : VizioWindowLocalVariables
{
    public static void CreateNewWindow()
    {
        window = GetWindow<VizioWindowLocalVariables>();
        if (window == null)
            return;
        window.title = "Local Variables Manager";
        window.minSize = new Vector2(512, 512);
        window.wantsMouseMove = true;
        window.ShowUtility();
    }
}