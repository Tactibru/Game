using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SocketIOConnection))]
public class SocketIOExample : MonoBehaviour {
	
	SocketIOConnection conn;
		
	IEnumerator Start () {
		conn = GetComponent<SocketIOConnection>();
		conn.handler.OnConnect += OnConnect;
		conn.handler.OnEvent += OnEvent;
		
		while(!conn.Ready)
			yield return new WaitForSeconds(2);
		conn.Emit("boo");
		
	}
	
	void OnConnect(SocketIOMessage msg) {
		Debug.Log("Socket.IO connection established.");	
		
	}

	void OnEvent (SocketIOMessage msg, string name, ArrayList args)
	{
		Debug.Log ("Event Received: " + name);
		
	}
}
