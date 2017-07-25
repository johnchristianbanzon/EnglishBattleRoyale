using UnityEngine;
public class NoAnswerType : MonoBehaviour,IAnswer {
	public GameObject correctAnswerContainer;

	public void CheckAnswerFromSelection(string selectedAnswer, string questionAnswer){
		Debug.Log (selectedAnswer + "/" + questionAnswer);
		if(selectedAnswer.Equals(questionAnswer)){
			QuestionSystemController.Instance.CheckAnswer (true);
		}
	}

	public void DeployAnswerType(){
		gameObject.SetActive (true);
	}

}
