using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	protected Animator animator;
	// Use this for initialization
	public virtual void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	public virtual void Update () {
		/*
		if (Input.GetKeyDown("space")){
			Hit();
		} else if (Input.GetKeyDown("left shift") || Input.GetKeyDown("right shift")){
			Block();
		}*/
	}

	public virtual void Hit(){
		if(animator != null){
			animator.SetTrigger("hit");
		}
	}

	public virtual void Block(){
		if(animator != null){
			animator.SetTrigger("block");
		}
	}
}
