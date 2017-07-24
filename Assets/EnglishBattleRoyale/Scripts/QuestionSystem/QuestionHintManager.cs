using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionHintManager : MonoBehaviour{
	public void OnClick(){
		QuestionSystemController.Instance.selectionType.RemoveSelection ();
	}
}
