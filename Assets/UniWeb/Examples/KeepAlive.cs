using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeepAlive : MonoBehaviour
{

	string url = "http://gamma.local:1337/";
	public int count = 30;

	IEnumerator Start ()
	{
		var requests = new List<HTTP.Request> ();
		while (count > 0) {
			count --;
			var r = new HTTP.Request ("GET", url);
			r.Send ();
			requests.Add (r);
		}
		
		while (true) {
			var done = true;
			foreach (var r in requests) {
				done = done & r.isDone;
				if (r.exception != null) {
					Debug.Log (r.exception.ToString ());
				}
			}
			yield return new WaitForSeconds (1);
			if (done)
				break;
		}
		
		foreach (var r in requests) {
			if (r.exception != null) {
				Debug.Log (r.exception.ToString ());
			} else {
				Debug.Log(r.response.Text);	
			}
		}
		Debug.Log("Finished.");
		
	}
	
	
	
}
