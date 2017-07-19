using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseQuestion : MonoBehaviour
{

	protected int currentRound = 1;
	protected int correctAnswers;
	protected int answerIndex = 1;
	protected string answerWrote;
	protected bool hasSkippedQuestion = false;
	protected string questionAnswer = "";
	protected List<GameObject> answerButtons = new List<GameObject> ();
	protected List<GameObject> selectionButtons = new List<GameObject> ();
	protected List<GameObject> answerGameObject = new List<GameObject> ();
	protected GameObject[] answerIdentifier = new GameObject[30];
	protected QuestionSystemEnums.SelectionType selectionType;

	protected void LoadQuestion (QuestionSystemEnums.QuestionType questionType)
	{
		Question questionLoaded = QuestionBuilder.GetQuestion (questionType);
		string question = questionLoaded.question;
		gameObject.transform.GetChild (0).GetComponent<Text> ().text = question;
		questionAnswer = questionLoaded.answers.Length == 2 ? 
			(questionLoaded.answers [0].ToUpper () + "/" + questionLoaded.answers [1].ToUpper ()) :
			questionLoaded.answers [UnityEngine.Random.Range (0, questionLoaded.answers.Length)].ToUpper ();
	}

	protected void PopulateAnswerHolder (GameObject g, GameObject outputPrefab, GameObject answerContent)
	{
		answerButtons.Clear ();
		string[] temp = questionAnswer.Split ('/');
		if (temp.Length > 1) {
			questionAnswer = temp [UnityEngine.Random.Range (0, temp.Length)];
			Debug.Log (questionAnswer);
		}
		for (int i = 0; i < questionAnswer.Length; i++) {
			GameObject answerPrefab = Instantiate (outputPrefab) as GameObject; 
			answerPrefab.transform.SetParent (answerContent.transform, false);
			answerPrefab.GetComponent<Button> ().onClick.AddListener (() => {
				OnAnswerClick (answerPrefab.GetComponent<Button> ());
			});
			answerPrefab.name = "output" + (i + 1);
			answerButtons.Add (answerPrefab);
			answerPrefab.transform.GetChild (0).GetComponent<Text> ().text = "";
			answerPrefab.GetComponent<Image> ().color = new Color (136f / 255, 236f / 255f, 246f / 255f);
		}
	}

	public void PopulateSelectionHolder (GameObject g, GameObject outputPrefab, GameObject selectionContent)
	{
		selectionButtons.Clear ();
		for (int i = 0; i < questionAnswer.Length; i++) {
			GameObject input = Instantiate (outputPrefab) as GameObject; 
			input.transform.SetParent (selectionContent.transform, false);
			input.name = "input" + (i + 1);
			input.GetComponent<Button> ().onClick.AddListener (() => {
				OnSelectionClick (EventSystem.current.currentSelectedGameObject.GetComponent<Button> ());
			});
			selectionButtons.Add (input);
			input.transform.GetChild (0).GetComponent<Text> ().text = "";
		}
	}

	protected void OnAnswerClick (Button answerButton)
	{
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
		string answerclicked = "";
		if (string.IsNullOrEmpty (answerButton.transform.GetChild (0).GetComponent<Text> ().text)) {
			TweenFacade.TweenShakePosition (answerButton.transform, 0.5f, 15.0f, 50, 90f);
		} else {
			Debug.Log (answerButton.name);
			for (int i = 1; i < 12 + 1; i++) {
				if (answerButton.name.Equals ("output" + i)) {
					answerclicked = answerButtons [i - 1].transform.GetChild (0).GetComponent<Text> ().text;
					answerButtons [i - 1].transform.GetChild (0).GetComponent<Text> ().text = "";
					answerIdentifier [i].transform.GetChild (0).GetComponent<Text> ().text = answerclicked;
				}
			}
			CheckAnswerHolder ();
		}
	}

	protected void CheckAnswer (bool result)
	{
		QuestionSpecialEffects spe = new QuestionSpecialEffects ();
		Debug.Log (answerButtons.Count);
		spe.DeployEffect (result, answerButtons, questionAnswer, gameObject);
		correctAnswers = result ? correctAnswers + 1 : correctAnswers;
		UpdateFirebaseAnswerModel (result);
		hasSkippedQuestion = true;
		QuestionController.Instance.Stoptimer = false;
		Invoke ("OnFinishQuestion", 1f);
	}

	protected void UpdateFirebaseAnswerModel (bool isCorrect)
	{
		if (isCorrect) {
			Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
			param [ParamNames.AnswerCorrect.ToString ()] = currentRound;
			SystemFirebaseDBController.Instance.SetAnswerParam (new AnswerModel (JsonConverter.DicToJsonStr (param).ToString ()));
		}
	}

	protected void CheckAnswerHolder ()
	{
		for (int j = 0; j < questionAnswer.Length; j++) {
			GameObject findEmpty = answerButtons [j].transform.GetChild (0).gameObject;
			if (string.IsNullOrEmpty (findEmpty.GetComponent<Text> ().text)) {
				answerIndex = j + 1;
				break;
			}
		}

	}

	public void ClearAnswerList ()
	{
		if (answerButtons.Count > 0) {
			answerIndex = 1;
			foreach (GameObject o in answerButtons) {
				Destroy (o);
			}
		}
	}

	public void OnSkipClick ()
	{
		if (!hasSkippedQuestion) {
			CheckAnswer (false);
			hasSkippedQuestion = true;
		}
	}

	public void OnSelectionClick (Button letterButton)
	{
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);

		if (string.IsNullOrEmpty (letterButton.transform.GetChild (0).GetComponent<Text> ().text)) {
			TweenFacade.TweenShakePosition (letterButton.transform, 1.0f, 30.0f, 50, 90f);
		} else {
			answerIdentifier [answerIndex] = letterButton.gameObject;
			answerWrote = "";
			answerButtons [(answerIndex - 1)].GetComponentInChildren<Text> ().text 
			= letterButton.GetComponentInChildren<Text> ().text;
			letterButton.GetComponentInChildren<Text> ().text = selectionType == QuestionSystemEnums.SelectionType.Typing
				? letterButton.GetComponentInChildren<Text> ().text : "";

			for (int j = 0; j < questionAnswer.Length; j++) {
				answerWrote += answerButtons [j].transform.GetChild (0).GetComponent<Text> ().text;
			}

			if (answerWrote.Length.Equals (questionAnswer.Length)) {
				if (answerWrote.ToUpper ().Equals (questionAnswer.ToUpper ())) {
					CheckAnswer (true);
				} else {
					CheckAnswer (false);
				}
			}
			CheckAnswerHolder ();
		}
	}
}
