using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WWWFormTest : MonoBehaviour
{
	
	string url = "http://footyboss.local:8080/";

	IEnumerator Start ()
	{
		var w = new WWWForm();
		
		
		w.AddField("hello", "world");
		//w.AddBinaryData("file", new byte[] { 65,65,65,65 });
		
		var r = new HTTP.Request (url, w);
		r.Send();
		Debug.Log ("Sending Form");
		while(!r.isDone) {
			yield return null;
		}
		
		Debug.Log ("Response Text:");
		Debug.Log(r.response.Text);
		Debug.Log("Stream has finished");
		
		
	}
	
	
}
