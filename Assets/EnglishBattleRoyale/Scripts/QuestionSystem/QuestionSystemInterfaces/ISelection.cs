using System;
using UnityEngine;
using System.Collections.Generic;
public interface ISelection {
	void ShowSelectionType (string questionAnswer,Action<List<GameObject>> onSelectCallBack);
	void ShowSelectionHint (int hintIndex);
	void HideSelectionType();
	void ShowCorrectAnswer();
}