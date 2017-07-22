using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartQuestionController : MonoBehaviour {
	public SingleQuestion singleQuestionController;
	public Association associationController;

	public void ActivatePartTarget(QuestionSystemEnums.QuestionType questionType, string targetString){
		switch (questionType) {
		case QuestionSystemEnums.QuestionType.Association:
			associationController.gameObject.SetActive (true);
			associationController.ChangeTargetToAssociation (targetString);
			break;
		default:
			singleQuestionController.gameObject.SetActive (true);
			singleQuestionController.ActivateSingleQuestion (targetString);
			break;
		}
	}
}
