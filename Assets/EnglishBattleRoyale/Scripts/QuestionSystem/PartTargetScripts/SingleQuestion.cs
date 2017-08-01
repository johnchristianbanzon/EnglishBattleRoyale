using UnityEngine;
using UnityEngine.UI;

public class SingleQuestion : MonoBehaviour,ITarget {
	public Text targetText;

	public void ShowTargetType(string question){
		gameObject.SetActive (true);
		targetText.text = question;
	}

	public void HideTargetType(){
		gameObject.SetActive (false);
	}

}
