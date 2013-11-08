using UnityEditor;
using UnityEngine;


public class AntaresVizioLogicBlockWizard : VizioLogicBlockWizard
{
    [MenuItem("Antares/UNIVERSE/Wizards/LogicBlock Wizard")]
    public static void CreateWindow()
    {
        var window = (AntaresVizioLogicBlockWizard)GetWindow(typeof(AntaresVizioLogicBlockWizard));
        window.title = "LogicBlock Wizard";
        window.minSize = new Vector2(512, 386);
        window.Show();
        window.ShowUtility();
        window.wantsMouseMove = true;
        window.autoRepaintOnSceneChange = true;
        window.antiAlias = 4;
    }
}

