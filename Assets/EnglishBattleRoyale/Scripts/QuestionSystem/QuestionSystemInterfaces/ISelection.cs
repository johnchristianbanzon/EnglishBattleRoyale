using System;
using UnityEngine;
using System.Collections.Generic;
public interface ISelection {
	void ShowSelectionType (string questionAnswer,Action<List<GameObject>> onSelectCallBack);
	void ShowSelectionHint (int hintIndex,GameObject answerContainer);
	void HideSelectionHint();
	void HideSelectionType();
	void ShowCorrectAnswer(bool isAnswerCorrect);
}