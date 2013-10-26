// Generated with Antares LogicBlock Wizard at Friday, December 31, 2010  8:55:07 AM
using Antares.Vizio.Runtime;
using UnityEngine;

[VisualLogicBlockDescription("Smooth per second movement")]
[VisualLogicBlock("Translate per second", "Transform", ParentName = "Per second")]
public class Translatepersecond : LogicBlock
{

    [Parameter(VariableType.In, typeof(UnityEngine.Transform), Name = "Target Transform")]
    public Variable Target;

    [Parameter(VariableType.In, typeof(UnityEngine.Vector3), Name = "Movement Vector")]
    public Variable Vector;

    [Parameter(VariableType.In, typeof(Space), Name = "Relative To", DefaultValue = Space.Self)]
    public Variable space;

    [Parameter(VariableType.Out, typeof(System.String), Name = "Error Description")]
    public Variable error;

    [EntryTrigger("In")]
    public void In()
    {
        Transform tr = Target.Value as Transform;
        if (tr == null)
        {
            error.Value = "Transform is NULL";
            ActivateTrigger(1);
            return;
        }
        Vector3 vec = (Vector3)Vector.Value;
        tr.Translate(vec * Time.deltaTime, (Space)space.Value);
        error.Value = "";
        ActivateTrigger(0);
    }

    public override void OnInitializeDefaultData()
    {
        RegisterOutputTrigger("Out");
        RegisterOutputTrigger("Error");
    }
}

[VisualLogicBlockDescription("Smooth per second rotation")]
[VisualLogicBlock("Rotate per second", "Transform", ParentName = "Per second")]
public class Rotatepersecond : LogicBlock
{

    [Parameter(VariableType.In, typeof(UnityEngine.Transform), Name = "Target Transform")]
    public Variable Target;

    [Parameter(VariableType.In, typeof(UnityEngine.Vector3), Name = "Rotation Vector")]
    public Variable Vector;

    [Parameter(VariableType.In, typeof(Space), Name = "Relative To", DefaultValue = Space.Self)]
    public Variable space;

    [Parameter(VariableType.Out, typeof(System.String), Name = "Error Description")]
    public Variable error;

    [EntryTrigger("In")]
    public void In()
    {
        Transform tr = Target.Value as Transform;
        if (tr == null)
        {
            error.Value = "Transform is NULL";
            ActivateTrigger(1);
            return;
        }
        Vector3 vec = (Vector3)Vector.Value;
        tr.Rotate(vec * Time.deltaTime, (Space)space.Value);
        error.Value = "";
        ActivateTrigger(0);
    }

    public override void OnInitializeDefaultData()
    {
        RegisterOutputTrigger("Out");
        RegisterOutputTrigger("Error");
    }
}
