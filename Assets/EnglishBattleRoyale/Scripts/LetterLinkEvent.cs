using System;
using UnityEngine;
using UnityEngine.UI;

public class LetterLinkEvent : MonoBehaviour
{	
	public LetterLink letterlink;
	private static bool startSelection = false;
	private static string writtenAnswer;
	private Color selectedColor = new Color (36f / 255, 189f / 255f, 88f / 255f);
	private Color defaultColor = new Color32 (255,255,255,255);
	public GameObject specialEffectObject = null;
	private Vector3 _initialPosition;
	private Vector3 _currentPosition;

	public void OnBeginDrag (GameObject currentSelectedLetter)
	{
		startSelection = true;
		writtenAnswer = "";
		OnDragSelection (currentSelectedLetter);
		letterlink.lineRender.sortingOrder = 5;
//		letterlink.lineRender.SetPosition (0, currentSelectedLetter.transform.position);

		_initialPosition = currentSelectedLetter.transform.position;
		letterlink.lineRender.SetPosition(0, _initialPosition);
		letterlink.lineRender.numPositions = 1;	
//		letterlink.lineRender.enabled = true;
	}

	public void ShowCorrectAnswer(bool isAnswerCorrect){
		Color containerColor = new Color ();
		if (isAnswerCorrect) {
			containerColor = new Color32 (36, 189, 88, 255);
		} else {
			containerColor = new Color32 (255, 100, 100, 255);
		}
		if (letterlink.questionAnswer.Contains (GetComponentInChildren<Text>().text)) {
			GetComponent<Image> ().color = containerColor;
		}
	}

	private static int answerCounter = 1;
	public void OnDragSelection (GameObject currentSelectedLetter)
	{
		if (startSelection) {
//			letterlink.lineRender.SetPosition (1, pos);

//			_currentPosition = pos;
//			letterlink.lineRender.numPositions = 2;
//			letterlink.lineRender.SetPosition(1, _currentPosition);
		}

		if (startSelection && (currentSelectedLetter.GetComponent<Image> ().color != selectedColor)) {
//			letterlink.lineRender.SetPosition (2, currentSelectedLetter.transform.position);


			Vector2 pos = new Vector2 (0, 0);
			Canvas myCanvas = SystemGlobalDataController.Instance.gameCanvas;
			this.GetComponent<Image> ().raycastTarget = false;
			RectTransformUtility.ScreenPointToLocalPointInRectangle (myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);

			_currentPosition = pos;

			letterlink.lineRender.numPositions = answerCounter + 1;
			letterlink.lineRender.SetPosition(answerCounter, currentSelectedLetter.transform.position);	
			answerCounter++;
			writtenAnswer += currentSelectedLetter.GetComponentInChildren<Text> ().text;
			QuestionSystemController.Instance.partAnswer.showAnswer.ShowLetterInView (currentSelectedLetter);
			currentSelectedLetter.GetComponent<Image> ().color = selectedColor;
			SystemSoundController.Instance.PlaySFX ("SFX_ClickButton");
		}
	}

	public void ShowHint(){
		specialEffectObject = SystemResourceController.Instance.LoadPrefab ("LetterLinkSpecialEffect", gameObject);
	}

	public void OnEndDrag ()
	{
		answerCounter = 1;
		letterlink.lineRender.numPositions = 2;
		if (letterlink.questionAnswer == writtenAnswer) {
			startSelection = false;
			QuestionSystemController.Instance.CheckAnswer (true);
		} else {
			ClearSelection ();
		}
	}

	public void ClearSelection ()
	{
		writtenAnswer = "";
		startSelection = false;
		QuestionSystemController.Instance.partAnswer.showAnswer.ClearLettersInView ();
		foreach (LetterLinkEvent selection in letterlink.connectLetterButtons) {
			selection.GetComponent<Image> ().color = defaultColor;
		}
	}
}


