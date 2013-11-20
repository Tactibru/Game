using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Will the members in the tray and will also deal scrolls the possible units
/// </summary>

public class MemberTrayBehaviour : MonoBehaviour 
{
    public List<MemberBehaviour> members = new List<MemberBehaviour>();

    public MemberBehaviour memberOneByOne;
    public MemberBehaviour memberOneByTwo;
    public MemberBehaviour memberTwoByOne;
    public MemberBehaviour memberTwoByTwo;
	
    /// <summary>
    /// Just makes the tray invisable, at the moment
    /// </summary>

    int magicNumberForTesting = 25;

	void Start () 
    {
        for(int index = 0; index < magicNumberForTesting; index++)
        {
            MemberBehaviour newMember = null;
            newMember = (MemberBehaviour)Instantiate(memberOneByOne, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            members[index] = newMember;
        }

        //for (int index = 0; index < magicNumberForTesting; index++)
        //{
 
        //}

        //for (int index = 0; index < magicNumberForTesting; index++)
        //{
 
        //}

        //for (int index = 0; index < magicNumberForTesting; index++)
        //{
 
        //}

        renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void ShowMemberTray()
    {
        transform.renderer.enabled = true;

        for (int index = 0; index < members.Count; index++)
        {
            members[index].transform.renderer.enabled = true;
        }
    }
}
