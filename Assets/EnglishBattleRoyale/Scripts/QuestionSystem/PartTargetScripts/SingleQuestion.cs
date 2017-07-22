using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SingleQuestion : MonoBehaviour,ITarget {
	public Text targetText;

	public void DeployTargetType(string question){
		gameObject.SetActive (true);
		targetText.text = question;
	}

}
