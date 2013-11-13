using UnityEngine;
using System.Collections;

public class EntryBehavior : MonoBehaviour 
{
    public TextMesh label;
    public TextMesh value;

    public void Set(string label, int value)
    {
        this.label.text = label;
        this.value.text = value.ToString();
    }
}
