using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SocketIOConnection))]
public class SocketIOManager : MonoBehaviour {
	SocketIOConnection socket;
	
	void Start(){
		Debug.Log("Opening socket: https://notify.staging.ligadastorcidas.com.br:8443/");
		socket = GetComponent<SocketIOConnection>();
		
		socket.handler.OnConnect += OnSocketOpened;
		socket.handler.OnDisconnect += OnSocketClosed;
		socket.handler.OnEvent += OnSocketEvent;
		socket.handler.OnMessage += OnSocketMessage;
		socket.handler.OnError += OnSocketError;
	}
	
	void OnSocketOpened (SocketIOMessage e)
	{
		Debug.Log("Socket opened: " + e);
	}

	void OnSocketClosed (SocketIOMessage e)
	{
		Debug.Log("Socket closed: " + e);
	}

	void OnSocketMessage (SocketIOMessage e)
	{
		Debug.Log("Socket message: " + e);
	}
	
	void OnSocketEvent (SocketIOMessage e, string message, ArrayList list)
	{
		Debug.Log("Socket event: " + e);
	}

	void OnSocketError (SocketIOMessage e)
	{
		Debug.Log("Socket error: " + e);
	}
}