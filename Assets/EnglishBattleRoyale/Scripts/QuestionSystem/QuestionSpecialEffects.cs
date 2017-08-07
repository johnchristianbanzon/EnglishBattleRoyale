using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionSpecialEffects : MonoBehaviour  {
	private bool answerResult;

	public void DeployEffect(bool result , List<GameObject> answerButtons, string answer){
		answerResult = result;
		ShowAnswer (answerButtons);
		if (result) {
			AudioEffect (AudioEnum.Correct);
		} else {
			TweenFacade.TweenShakePosition (QuestionSystemController.Instance.transform, 1.0f, 30.0f, 50, 90f);
			AudioEffect (AudioEnum.Mistake);
		}

	}

	private void AudioEffect(AudioEnum audioNum){
		AudioController.Instance.PlayAudio (audioNum);
	}

	private void GpGotEffect(GameObject gpText){
		gpText.GetComponent<Text> ().text = "1 GP";
		TweenFacade.TweenTextScale (gpText.transform, new Vector3 (3, 3, 3), 1.0f);
	}


	private void ShowAnswer(List<GameObject> answerButtons){
		Color correctColor = new Color (255f / 255, 249f / 255f, 149f / 255f);
		Color wrongColor = new Color (229f / 255, 114f / 255f, 114f / 255f);
		for (int i = 0; i < answerButtons.Count; i++) {
			if (answerResult) {
				answerButtons [i].GetComponent<Image> ().color = correctColor;
			} else {
				answerButtons [i].GetComponent<Image> ().color = wrongColor;
			}
		}
	}
}
