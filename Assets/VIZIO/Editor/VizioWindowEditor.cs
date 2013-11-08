using System.IO;
using UnityEngine;
using UnityEditor;
using Antares.Vizio;
using System;
#pragma warning disable

public class VizioWindowEditor : VizioWindow
{
    [MenuItem("Antares/UNIVERSE/CREATE")]
    public static void CreateNewWindow()
    {
        if (EditorUtility.DisplayDialog("UNIVERSE", "Create the UNIVERSE graph on the new empty Game Object ?",
                                                     "Create", "Cancel"))
        {
            owner =
                new GameObject("UNIVERSE Logic Container", typeof(VizioComponent)).GetComponent
                    <VizioComponent>();
            owner.id = Guid.NewGuid().ToString();

            window = GetWindow<VizioWindowEditor>();
            if (window == null)
                return;
            window.title = "UNIVERSE";
            window.minSize = new Vector2(512, 512);
            window.Show();
            window.ShowUtility();
            window.wantsMouseMove = true;

            CreateVIZIOWindow();
            Selection.activeGameObject = owner.gameObject;
        }
        else
        {
            return;
        }
    }

     [MenuItem("Antares/UNIVERSE/OPEN or ADD &u")]
    public static void CreateWindow()
    {
        try
        {
            GameObject goSelected = Selection.activeGameObject;
            
            if (goSelected && goSelected.GetComponent<VizioComponent>())
            {
                owner = goSelected.GetComponent<VizioComponent>();
            }
            else if (goSelected && !EditorApplication.isPlaying)
            {
                owner = FindLastOwner();
                switch (
                    EditorUtility.DisplayDialogComplex("UNIVERSE",
                                                       "Create the UNIVERSE graph on this Game Object or create the new empty Game Object ?",
                                                       "Add", "Create new", "Open Last"))
                {
                    case 0:
                        Undo.RegisterUndo(goSelected, "Add UNIVERSE component");
                        owner = goSelected.AddComponent<VizioComponent>();
                        owner.id = Guid.NewGuid().ToString();
                        break;
                    case 1 :
                        owner =
                             new GameObject("VizioLogicContainer", typeof(VizioComponent)).GetComponent
                                <VizioComponent>();
                        owner.id = Guid.NewGuid().ToString();
                        Selection.activeGameObject = owner.gameObject;
                        break;
                    case 2:
                        if (owner)
                            owner.id = Guid.NewGuid().ToString();
                        break;
                }
            }
            else owner = FindLastOwner();
            if (owner == null)
            {
                if(window != null)
                    window.Close();
                return;
            }

            window = GetWindow<VizioWindowEditor>();
            if (window == null)
                return;
            window.title = "UNIVERSE";
            window.minSize = new Vector2(512, 512);
            window.Show();
            window.ShowUtility();
            window.wantsMouseMove = true;

            CreateVIZIOWindow();
            //window.autoRepaintOnSceneChange = true;
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
}

[InitializeOnLoad]
public class VizioInstallHelper
{
    static VizioInstallHelper()
    {
        var dirs = Directory.GetDirectories(Application.dataPath + "/");

        if (ArrayUtility.Contains(dirs, Application.dataPath + "/" + "Editor Default Resources"))
        {
            //EditorUtility.DisplayDialog("INSTALLATION", "COMPLITED", "ok");
        }
        else
        {
            if (EditorUtility.DisplayDialog("INSTALLATION", "Universe installation process is not complited yet. Would you like to finish it?", "yes", "no"))
            {
                //AssetDatabase.MoveAsset(Application.dataPath + "/VIZIO/" + "Editor Default Resources",
                //                        Application.dataPath + "/" + "Editor Default Resources");

                FileUtil.MoveFileOrDirectory(Application.dataPath + "/VIZIO/" + "Editor Default Resources", Application.dataPath + "/" + "Editor Default Resources");
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("INSTALLATION", "COMPLITED", "ok");
            }
        }
    }
}