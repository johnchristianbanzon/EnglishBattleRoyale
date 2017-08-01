using UnityEngine;
using System;
using UnityEngine.UI;

public class QuestionHintManager :MonoBehaviour{
	public Button hintButton;
	private int hintLimit = QuestionSystemConst.HINT_SHOW_LIMIT;
	private int hintIndex = 0;
	private int hintUsed = 0;
	private int hintRemovalRate = QuestionSystemConst.HINT_REMOVE_TIME;
	private int hintRemoveInterval = 3;
	private int hintCooldown = QuestionSystemConst.HINT_BUTTON_COOLDOWN;
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

	public void InitHints(){
		hintRemoveInterval = hintRemovalRate;
	}

	public void OnTimeInterval(){
		Debug.Log (hintRemoveInterval);
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
		if (hintUsed >= hintLimit) {
			hintButton.interactable = false;
		} 
	}
}
