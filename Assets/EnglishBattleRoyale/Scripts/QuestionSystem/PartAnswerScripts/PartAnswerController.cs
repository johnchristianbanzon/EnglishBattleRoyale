using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartAnswerController : MonoBehaviour {
	public List<GameObject> answerContainer = new List<GameObject>();

	private QuestionSystemEnums.AnswerType answerType;
	public FillAnswerType fillAnswer;
	public NoAnswerType noAnswer;
	public ShowAnswer showAnswer;

	public void DeployAnswerType(IAnswer answerType){
		answerType.DeployAnswerType ();
		answerContainer = QuestionSystemController.Instance.partAnswer.fillAnswer.answerContainers;
	}

}
