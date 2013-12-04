// Generated with Antares LogicBlock Wizard at Monday, December 02, 2013  1:16:05 AM
using Antares.Vizio.Runtime;
using UnityEngine;

[VisualLogicBlockDescription("Performs a test!")]
[VisualLogicBlock("Test Block", "Tactibru", ParentName = "Tactibru")]
public class TestBlock: LogicBlock
{
    [EntryTrigger("In")]
    public void In()
    {
		Debug.Log("Test");
		ActivateTrigger();
    }

    public override void OnInitializeDefaultData()
    {
        RegisterOutputTrigger("Out");
    }
}
