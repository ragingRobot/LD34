using UnityEngine;
using System.Collections;
using SocketIO;
using System.Collections.Generic;

public class Block : MonoBehaviour {
	SocketIOComponent socket;
	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BlockPressed(){
		Dictionary<string, string> data = new Dictionary<string, string>();
		data["type"] = "block";
		socket.Emit("action", new JSONObject(data));
	}


}
