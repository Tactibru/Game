using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StatsMenu : ButtonManagerBehavior
{
    public List<EntryBehavior> entries;
    public Dictionary<string, int> myDictionary;

	protected override void Start()
	{
		base.Start();

        entries = new List<EntryBehavior>();
		myDictionary = new Dictionary<string,int>();
        Show(myDictionary.Keys.ToList(), myDictionary.Values.ToList());

	}

    public void Show(Dictionary<string, int> values)
    {
        List<string> keys = values.Keys.ToList();

        for (int index = 0; index < entries.Count; index++)
        {
            entries[index].Set(keys[index], values[keys[index]]);

        }

        foreach (KeyValuePair<string, int> pair in values)
        {
            Debug.Log(pair.Key + " " + pair.Value);
        }
    }

    public void Show(List<string> labels, List<int> values)
    {
        // Check lists lengths
        for (int index = 0; index < labels.Count; index++)
        {
            entries[index].Set(labels[index], values[index]);
        }
    }
}
