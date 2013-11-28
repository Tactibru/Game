using UnityEngine;
using System.Collections;
using Tactibru.SaveSystem;

public class SaveTestBehavior : MonoBehaviour {
	public string testString = "Hello, world!";

	// Use this for initialization
	void Start () {
		// Saving Data
		SaveData saveData = new SaveData();
		saveData.testString = testString;

		SaveEngine.Save ("testSave01", saveData);

		// Loading Data
		/*SaveData saveData = SaveEngine.Load ("testSave01");

		testString = saveData.testString;

		Debug.Log (testString);*/
	}
}
