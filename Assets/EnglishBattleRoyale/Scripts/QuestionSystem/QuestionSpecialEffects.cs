using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionSpecialEffects : MonoBehaviour  {
	private GameObject questionTypeComponent;
	private string questionAnswer;
	private bool answerResult;

	public void DeployEffect(bool result , List<GameObject> answerButtons, string answer, GameObject questionType){

		answerResult = result;
		questionAnswer = answer;
		questionTypeComponent = questionType;
		ShowAnswer (answerButtons);

		if (result) {
		//	GpGotEffect (gpText);
			CorrectAnswerEffect (questionAnswer , answerButtons, questionType);
			AudioEffect (AudioEnum.Correct);
		} else {
			
			AudioEffect (AudioEnum.Mistake);
		}
		questionTypeComponent = questionType;
		TweenLogic.TweenShakePosition (questionTypeComponent.transform, 1.0f, 30.0f, 50, 90f);
	}

	private void AudioEffect(AudioEnum audioNum){
		AudioController.Instance.PlayAudio (audioNum);
	}

	private void GpGotEffect(GameObject gpText){
		gpText.GetComponent<Text> ().text = "1 GP";
		TweenLogic.TweenTextScale (gpText.transform, new Vector3 (3, 3, 3), 1.0f);
	}

	private void CorrectAnswerEffect(string questionAnswer, List<GameObject> answerButtons, GameObject questionType){
		GameObject ballInstantiated = Resources.Load ("Prefabs/scoreBall") as GameObject;
		for (int i = 0; i < answerButtons.Count; i++) {
			Instantiate (ballInstantiated, 
				answerButtons [i].transform.position, 
				Quaternion.identity, questionType.transform);
		}
	}

	private void ShowAnswer(List<GameObject> answerButtons){

		for (int i = 0; i < answerButtons.Count; i++) {
			if (questionTypeComponent.name == "WordChoiceModal") {
				
				string[] answerSplit = questionAnswer.Split ('/');
			
				answerButtons [i].transform.GetChild (0).GetComponent<Text> ().text = 
					answerSplit [i].ToString ().ToUpper ();

				if (i >= answerSplit.Length) {
					break;
				}
				answerButtons [i].GetComponent<Image> ().color = answerResult ?
					new Color (255f / 255, 249f / 255f, 149f / 255f) :
					new Color (229f / 255, 114f / 255f, 114f / 255f);
				

			} else {

			answerButtons [i].transform.GetChild (0).GetComponent<Text> ().text = 
				questionAnswer [i].ToString ().ToUpper ();
			
			answerButtons [i].GetComponent<Image> ().color = answerResult ?
				new Color (255f / 255, 249f / 255f, 149f / 255f) :
				new Color (229f / 255, 114f / 255f, 114f / 255f);

			}
		}
	}
}
