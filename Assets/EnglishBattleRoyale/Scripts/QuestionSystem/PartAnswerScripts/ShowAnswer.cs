using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowAnswer : MonoBehaviour {

	public GameObject showLetterView;
	public GameObject showLetterPrefab;

	public void ShowLetterInView(GameObject selectedLetter){
		GameObject letterPrefab = Instantiate (showLetterPrefab) as GameObject; 
		letterPrefab.transform.SetParent (showLetterView.transform, false);
		letterPrefab.GetComponentInChildren<Text> ().text = selectedLetter.GetComponentInChildren<Text> ().text;
		TweenFacade.TweenScaleToLarge (letterPrefab.transform,Vector3.one,0.3f);
	}

	public void ClearLettersInView(){
		foreach (Transform letter in showLetterView.transform) {
			GameObject.Destroy(letter.gameObject);
		}
	}

}
