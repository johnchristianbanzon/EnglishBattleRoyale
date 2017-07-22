using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SingleQuestion : MonoBehaviour {
	public Text targetText;
	public void ActivateSingleQuestion(string question){
		targetText.text = question;
	}
}
