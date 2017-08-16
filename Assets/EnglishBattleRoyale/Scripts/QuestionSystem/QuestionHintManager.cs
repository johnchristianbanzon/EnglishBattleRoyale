using UnityEngine;
using System;
using UnityEngine.UI;

public class QuestionHintManager :MonoBehaviour{
	public Button hintButton;
	private int hintLimit = MyConst.HINT_SHOW_LIMIT;
	private int hintIndex = 0;
	public int hintUsed = 0;
	private int hintRemovalRate = MyConst.HINT_REMOVE_TIME;
	private int hintRemoveInterval = 3;
	private int hintCooldown = MyConst.HINT_BUTTON_COOLDOWN;
	private int hintCooldownCounter = 0;
	private Action<int> onHintResult;

	public void OnClick(){
		hintButton.interactable = false;
		int questionAnswerLength = QuestionSystemController.Instance.questionAnswer.Length;
		if (hintUsed < hintLimit && QuestionSystemController.Instance.correctAnswerButtons.Count > hintIndex) {
			QuestionSystemController.Instance.answerType.OnClickHint (hintIndex,delegate(bool onHintResult) {
				if(onHintResult){
					InitHints();
				}
			});
			hintIndex ++;
			hintUsed ++;
		}
			InitCooldown ();		
	}
		
	public void disableHintButton(){
		hintButton.interactable = false;
	}

	public void enableHintButton(){
		hintButton.interactable = true;
		TweenFacade.TweenScaleToLarge (hintButton.transform, Vector3.one, 0.3f);
	}

	public void InitHints(){
		hintRemoveInterval = hintRemovalRate;
		hintIndex = 0;
	}

	public void OnTimeInterval(){
		if (hintRemoveInterval <= 0) {
			QuestionSystemController.Instance.selectionType.HideSelectionHint ();
			hintRemoveInterval = hintRemovalRate;
		}
		hintRemoveInterval--;
	}

	public void InitCooldown(){
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
