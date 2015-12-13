using UnityEngine;
using System.Collections;
using System;
using SocketIO;
using System.Collections.Generic;

public class ClientConnection : MonoBehaviour {
	SocketIOComponent socket;
	Character me;
	Character enemy;
	// Use this for initialization
	void Start () {

		me = GameObject.Find("Bear").GetComponent<Bear>();
		enemy = GameObject.Find("Robot").GetComponent<Robot>();

		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();

		if (Application.platform == RuntimePlatform.Android){
			socket.url = "ws://192.168.1.7:3000/socket.io/?EIO=4&transport=websocket";
		}
		socket.On("message", Message);
		socket.On("update", UpdateFromServer);
		socket.On("connection", OnSocketOpen);
		socket.Connect();
		Debug.Log("client contection start");


	}

	public void Message(SocketIOEvent e){
		Debug.Log("Message from server:");
		Debug.Log(string.Format("[message: {0}]", e.data));
	}

	public void UpdateFromServer(SocketIOEvent e){
		Character updateCharacter = enemy;

		if(e.data.GetField("player").str == socket.sid.ToString()){
			updateCharacter = me;
		}

		Debug.Log(e.data.GetField("action").str );
		Debug.Log(e.data.GetField("action").str  == "hit");

		if(e.data.GetField("action").str == "hit"){
			updateCharacter.Hit();
		}else if(e.data.GetField("action").str ==  "block"){
			updateCharacter.Block();
		}
	}

	public void OnSocketOpen(SocketIOEvent e){
		Debug.Log("updated socket id " + socket.sid);
	}
		
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")){
			Dictionary<string, string> data = new Dictionary<string, string>();
			data["type"] = "hit";
			socket.Emit("action", new JSONObject(data));
			//Hit();
		} else if (Input.GetKeyDown("left shift") || Input.GetKeyDown("right shift")){
			Dictionary<string, string> data = new Dictionary<string, string>();
			data["type"] = "block";
			socket.Emit("action", new JSONObject(data));
			//Block();
		}
	}
}
