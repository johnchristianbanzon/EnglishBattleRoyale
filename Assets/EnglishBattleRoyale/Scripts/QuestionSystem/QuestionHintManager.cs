using UnityEngine;

public class QuestionHintManager :MonoBehaviour{
	int hintLimit = 10;
	int hintIndex = 0;
	int hintUsed = 0;

	public void OnClick(){
		int questionAnswerLength = QuestionSystemController.Instance.questionAnswer.Length;
		if (hintUsed < hintLimit && questionAnswerLength > hintIndex) {
			QuestionSystemController.Instance.selectionType.RemoveSelectionHint (hintIndex);
			hintIndex ++;
			hintUsed ++;
		}
	}

	public void HintIndexReset(){
		hintIndex = 0;
	}

}
