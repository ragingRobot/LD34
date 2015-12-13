using UnityEngine;
using System.Collections;
using System;
using SocketIO;

public class ClientConnection : MonoBehaviour {
	SocketIOComponent socket;
	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		socket.On("message", TestBoop);
		socket.On("connection", OnSocketOpen);
		socket.Connect();

		socket.On("message", (SocketIOEvent e) => {
			Debug.Log(string.Format("[name: {0}, data: {1}]", e.name, e.data));
		});
		Debug.Log("client contection start");


	}

	public void TestBoop(SocketIOEvent e){
		Debug.Log("Message from server:");
		Debug.Log(string.Format("[message: {0}]", e.data));
	}

	public void OnSocketOpen(SocketIOEvent ev){
		Debug.Log("updated socket id " + socket.sid);
	}
		
	// Update is called once per frame
	void Update () {
	
	}
}
