using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordChoiceEvent : MonoBehaviour {
	public bool isSelected = false;
	public WordChoice wordChoiceParent;
	private static int clickedCounter = 0;
	public bool isCorrect;

	public void Init(bool isCorrect,string answer){
		this.isCorrect = isCorrect;
		GetComponentInChildren<Text> ().text = answer;
	}

	public void OnClick(){
		if (!isSelected) {
			clickedCounter++;

			SelectContainer ();
			if (clickedCounter >= 2) {
				wordChoiceParent.CheckAnswerClicked ();
				clickedCounter = 0;
			}
		} else {
			DeselectContainer ();
		}
	}

	private void SelectContainer(){
		isSelected = true;
		GetComponentInChildren<Image> ().color = Color.gray;
	}

	private void DeselectContainer(){
		isSelected = false;
		GetComponentInChildren<Image> ().color = Color.white;
	}
}
