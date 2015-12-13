using UnityEngine;
using System.Collections;
using SocketIOClient;
using System;
public class ClientConnection : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Client client = new Client("localhost:3000");

		client.Opened += SocketOpened;
		client.Message += SocketMessage;
		client.SocketConnectionClosed += SocketConnectionClosed;
		client.Error +=SocketError;

		client.Connect();
	}

	private void SocketOpened(object sender, EventArgs e) {
		
	}

	private void SocketError(object sender, EventArgs e) {
		Debug.Log("ERROR");
		if ( e!= null) {
			string msg = ((MessageEventArgs)e).Message.MessageText;
			Debug.Log(msg);
		}
	}

	private void SocketConnectionClosed(object sender, EventArgs e) {

	}

	private void SocketMessage (object sender, MessageEventArgs e) {
		if ( e!= null && e.Message.Event == "message") {
			string msg = e.Message.MessageText;
			Debug.Log(msg);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
