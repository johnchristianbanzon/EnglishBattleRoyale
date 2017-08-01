using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using System.Linq;

public class Typing : MonoBehaviour, ISelection
{
	private string questionAnswer;
	public GameObject[] selectionButtons = new GameObject[26];
	public void ShowSelectionType (string questionAnswer,Action<List<GameObject>> onSelectCallBack){
		this.questionAnswer = questionAnswer;
		gameObject.SetActive (true);
	}

	public void ShowCorrectAnswer(){

	}


	private bool initHideHint = false;
	private List<int> hideSelectionIndex = new List<int> ();

	private List<int> InitHideHint ()
	{
		hideSelectionIndex.AddRange (Enumerable.Range (0, selectionButtons.Length));
		for (int i = 0; i < hideSelectionIndex.Count; i++) {
			if (questionAnswer.Contains (selectionButtons [hideSelectionIndex [i]].GetComponentInChildren<Text> ().text)) {
				hideSelectionIndex.RemoveAt (i);
				i--;
			}
		}
		return hideSelectionIndex;
	}

	public void HideSelectionHint ()
	{
		if (QuestionSystemConst.ALLOW_REMOVE_SELECTLETTER.Equals (1)) {
			if (!initHideHint) {
				InitHideHint ();
				initHideHint = true;
			}

			if (hideSelectionIndex.Count > 0) {
				int randomHintIndex = UnityEngine.Random.Range (0, hideSelectionIndex.Count);
				selectionButtons [hideSelectionIndex [randomHintIndex]].SetActive (false);
				hideSelectionIndex.RemoveAt (randomHintIndex);
			}
		}
	}

	public void HideSelectionType(){
		gameObject.SetActive (false);
	}

	public void ShowSelectionHint(int hintIndex, GameObject correctAnswerContainer){
		correctAnswerContainer.GetComponent<Button> ().enabled = false;
		correctAnswerContainer.GetComponentInChildren<Text> ().text = questionAnswer [hintIndex].ToString ();
		TweenFacade.TweenScaleToLarge (correctAnswerContainer.transform, Vector3.one, 0.3f);
		correctAnswerContainer.GetComponent<Image> ().color = new Color (255 / 255, 102 / 255f, 51 / 255f);
	}

	public void OnSelect(){
		QuestionSystemController.Instance.partAnswer.fillAnswer.
		SelectionLetterGot(EventSystem.current.currentSelectedGameObject);
	}
}
