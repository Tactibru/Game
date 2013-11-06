using UnityEngine;
using System.Collections;

/// <summary>
/// This is the unit member. this combat member would attached to if needed.
/// 
/// Alex Reiss
/// </summary>

public class MemberBehaviour : MonoBehaviour 
{
    /// <summary>
    /// This is for saving purposes and indentifation of the unit member.
    /// </summary>
    public int uniqueID;

    /// <summary>
    /// For the unit size and state machine when built.
    /// </summary>

    public enum SizeOfMember
    {
        OneByOne,
        OneByTwo,
        TwoByOne,
        TwoByTwo,
        NUMBER_OF_SIZES
    }

    /// <summary>
    /// the size of unit will be used in state maching when built.
    /// </summary>

    public SizeOfMember theSizeOfMember;

    /// <summary>
    /// For the filter when built.
    /// </summary>

    public bool inUnit = false;

    /// <summary>
    /// Defined by the state maching in the start function, will help with ensuring the position are oppucied.
    /// </summary>

    int height = 0;
    int width = 0;
	
    /// <summary>
    /// Currently sets the size of the unit member in height and width.
    /// 
    /// ALex Reiss
    /// </summary>

	void Start () 
    {
        renderer.enabled = false;

        switch (theSizeOfMember)
        {
            case SizeOfMember.OneByOne:
                height = 1;
                width = 1;
                break;
            case SizeOfMember.OneByTwo:
                height = 2;
                width = 1;
                break;
            case SizeOfMember.TwoByOne:
                height = 1;
                width = 2;
                break;
            case SizeOfMember.TwoByTwo:
                height = 2;
                width = 2;
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
