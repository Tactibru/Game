using UnityEngine;
using System.Collections;
using System.Text;
using HTTP;

public class SpamExample : MonoBehaviour {

	StringBuilder mLog = new StringBuilder();
	Vector2 mScrollPosition = Vector2.zero;
	
	void Awake () {
		Application.RegisterLogCallback(HandleLog);
	}
	
	void HandleLog(string logString, string stackTrace, LogType type) {
		if (type == LogType.Log)
			mLog.AppendLine(string.Format("{0}: {1}", type, logString));
		else
			mLog.AppendLine(string.Format("{0}: {1}\n{2}", type, logString, stackTrace));
    }
	
	void OnGUI() {
		if (GUI.Button(new Rect(10, 10, 100, 50), "Send")) Send();
		if (GUI.Button(new Rect(10, 70, 100, 50), "Clear Log")) ClearLog();
		mScrollPosition = GUI.BeginScrollView(new Rect(120, 10, 800, 500), mScrollPosition, new Rect(0,0,800,2000));
		GUI.Label(new Rect(120, 10, 500, 500), mLog.ToString());
		GUI.EndScrollView();
		
	}
	
	void ClearLog() {
		mLog = new StringBuilder();
	}

	void Send() {
		Debug.Log("Sending request!");
		var request = new Request("GET", "http://www.google.com");
		request.Send(OnRequestFinished);
	}
	
	void OnRequestFinished(Response response) {
		Debug.Log(response.Text);
	}
}
