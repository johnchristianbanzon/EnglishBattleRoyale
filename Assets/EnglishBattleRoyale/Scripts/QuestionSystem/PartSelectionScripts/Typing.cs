using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Typing : MonoBehaviour, ISelection
{
	public void DeploySelectionType(string questionAnswer){
		gameObject.SetActive (true);
	}


	public void RemoveSelectionHint(int hintIndex){

	}


	public void OnSelect(){
		QuestionSystemController.Instance.partAnswer.
		SelectionLetterGot(EventSystem.current.currentSelectedGameObject);
	}
}
