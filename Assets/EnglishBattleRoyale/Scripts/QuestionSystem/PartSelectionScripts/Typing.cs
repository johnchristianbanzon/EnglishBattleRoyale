using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Typing : MonoBehaviour
{
	public void OnSelect(){
		//gameObject.transform.parent.GetComponent<getSelectedObject> ().GetSelectedObject (selectedButton.gameObject);
		QuestionSystemController.Instance.answerController.SelectionLetterGot(EventSystem.current.currentSelectedGameObject);
	}
}
