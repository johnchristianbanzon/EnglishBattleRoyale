using UnityEngine;
using System;
using UnityEngine.UI;

public class QuestionHintManager :MonoBehaviour{
	public Button hintButton;
	private int hintLimit = 10;
	private int hintIndex = 0;
	private int hintUsed = 0;
	private int hintRemovalRate = 3;
	private int hintRemoveInterval = 3;
	private int hintCooldown = 2;
	private int hintCooldownCounter = 0;
	private Action<int> onHintResult;

	public void OnClick(){
		hintButton.interactable = false;
		int questionAnswerLength = QuestionSystemController.Instance.questionAnswer.Length;
		if (hintUsed < hintLimit && questionAnswerLength >= hintIndex) {
			QuestionSystemController.Instance.answerType.OnClickHint (hintIndex,delegate(bool onHintResult) {
				if(onHintResult){
					hintIndex = 0;
				}
			});
			hintIndex ++;
			hintUsed ++;
		}
		InitCooldown ();
	}

	public void OnTimeInterval(){
		if (hintRemoveInterval <= 0) {
			QuestionSystemController.Instance.selectionType.HideSelectionHint ();
			hintRemoveInterval = hintRemovalRate;
		}
		hintRemoveInterval--;
	}

	private void InitCooldown(){
		hintCooldownCounter = hintCooldown;
		InvokeRepeating ("CooldownTimer", 0, 1f);
	}

	private void CooldownTimer(){
		if (hintCooldownCounter > 0) {
			hintButton.GetComponentInChildren<Text>().text = hintCooldownCounter.ToString();
			hintCooldownCounter--;
		} else {
			CancelInvoke ();
			hintButton.GetComponentInChildren<Text> ().text = "HINT ("+(hintLimit-hintUsed)+")";
			hintButton.interactable = true;
		}
	}
}
