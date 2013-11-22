using UnityEngine;
using System.Collections;

public class TimeoutTest : MonoBehaviour
{

	IEnumerator Start ()
	{

		float initialTime = Time.realtimeSinceStartup;


		HTTP.Request webRequest = new HTTP.Request ("GET", "http://krogh.no/wp-content/uploads/2011/09/earth-huge.png");
		webRequest.acceptGzip = false;
		webRequest.useCache = false;
		webRequest.timeout = 3;
		webRequest.Send ();
		while (!webRequest.isDone) {
			yield return new WaitForEndOfFrame();
		}


		Debug.Log ("Time: " + (Time.realtimeSinceStartup - initialTime) + "s");


		if (webRequest.response == null) {

			Debug.Log ("Request timeout");

		} else if (webRequest.exception != null) {

			Debug.Log ("Exception: " + webRequest.exception);

		} else {

			Debug.Log ("Success");

			Debug.Log (webRequest.response.Text);

		}

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
