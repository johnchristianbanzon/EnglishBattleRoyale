using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrollBallScript : MonoBehaviour {
	private Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();//VARIABLE NEVER USED
	public float speed = 200.0f;
	Transform r2d;
	private int indicatornum = 1;
	//public GameObject[] indicators = new GameObject[]
	// Function called once when the bullet is created
	void Start () {
		r2d = GetComponent<Transform>();
		for (int i = 1; i < 4; i++) {
			if (GameObject.Find ("PlayerPlaceHolder" + i).GetComponent<Image> ().sprite.name
				=="UI-new-empty-indicator") {
				indicatornum = i;

				break;
			} else {
				indicatornum = 3;
			}
		}
		//TweenController.TweenJumpTo (r2d.transform,);
	}
	void Update(){
		r2d.position = Vector3.MoveTowards(r2d.transform.position, GameObject.Find("PlayerPlaceHolder"+indicatornum).transform.position, speed);
		if (r2d.position == GameObject.Find ("PlayerPlaceHolder" + indicatornum).transform.position) {
		  Destroy (r2d.gameObject);
		}
	}
		
}
