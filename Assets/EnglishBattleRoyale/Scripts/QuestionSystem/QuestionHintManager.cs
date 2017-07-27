using UnityEngine;
using System;
using UnityEngine.UI;

public class QuestionHintManager :MonoBehaviour{
	public Button hintButton;
	private int hintLimit = 10;
	private int hintIndex = 0;
	private int hintUsed = 0;
	private int hintCooldown = 2;
	private int hintCooldownCounter = 0;
	private Action<int> onHintResult;

	public void OnClick(){
		hintButton.enabled = false;
		int questionAnswerLength = QuestionSystemController.Instance.questionAnswer.Length;
		if (hintUsed < hintLimit && questionAnswerLength > hintIndex) {
			hintIndex ++;
			QuestionSystemController.Instance.answerType.OnClickHint (hintIndex-1,delegate(bool onHintResult) {
				if(onHintResult){
					hintIndex = 0;
				}
				else{
					
				}
			});
			InitCooldown ();
			hintUsed ++;
		}

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
			hintButton.enabled = true;
		}
	}
}
