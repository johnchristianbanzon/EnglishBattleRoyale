using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WordChoice : MonoBehaviour, ISelection
{
	public bool justAnswered = false;
	private string questionAnswer = "";
	public GameObject[] selectionButtons = new GameObject[4];
	private List<GameObject> answerClicked = new List<GameObject> ();
	private List<GameObject> answerButtons = new List<GameObject> ();
	private List<string> answerString = new List<string>();

	public void OnClickSelection ()
	{
		if (!justAnswered) {
			AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
			GameObject wordClicked = EventSystem.current.currentSelectedGameObject;
			string wordClickedString = wordClicked.GetComponentInChildren<Text>().text;
			if (wordClicked.GetComponent<Image> ().color == Color.gray) {
				wordClicked.GetComponent<Image> ().color = new Color (94f / 255, 255f / 255f, 148f / 255f);
				answerClicked.Remove (wordClicked);

			} else {
				wordClicked.GetComponent<Image> ().color = Color.gray;
				answerClicked.Add (wordClicked);
				if (answerClicked.Count == 2){
					string answerClicked1 = answerClicked [0].GetComponentInChildren<Text>().text.ToUpper();
					string answerClicked2 = answerClicked [1].GetComponentInChildren<Text>().text.ToUpper();
					CheckIfCorrect (answerClicked1,answerClicked2);
				}
			}
		}
	}

	public void RemoveSelectionHint(int hintIndex){

	}

	private void CheckIfCorrect(string answerClicked1, string answerClicked2){
		answerClicked.Clear ();

		if (answerString.Contains (answerClicked1) && answerString.Contains (answerClicked2)) {
			QuestionSystemController.Instance.CheckAnswer(true);
		} else {
			QuestionSystemController.Instance.CheckAnswer(false);
		}
	}
	public void DeploySelectionType(string questionAnswer){
		this.gameObject.SetActive (true);
		this.questionAnswer = questionAnswer;
	}
	public void ShuffleSelection ()
	{
		
		answerButtons.Clear ();
		answerString.Clear ();
		int numberOfAnswers = 2;
		List <int> randomList = new List<int> ();
		string[] temp = questionAnswer.Split ('/');

		for (int i = 0; i < selectionButtons.Length; i++) {
			int randomNum = UnityEngine.Random.Range (0, 4); 
			while (randomList.Contains (randomNum)) {
				randomNum = UnityEngine.Random.Range (0, selectionButtons.Length);
			}
			randomList.Add (randomNum);
			string wrongChoiceGot = QuestionBuilder.GetRandomChoices ();
		
			if (i < numberOfAnswers) {
				selectionButtons [randomNum].GetComponentInChildren<Text> ().text = temp [i].ToString ().ToUpper ();
				answerButtons.Add (selectionButtons [randomNum]);

			} else {
				selectionButtons [randomNum].GetComponentInChildren<Text> ().text = wrongChoiceGot;
			}
			selectionButtons[randomNum].GetComponent<Image> ().color = new Color(94f/255,255f/255f,148f/255f);
		}
		QuestionSystemController.Instance.correctAnswerButtons = answerButtons;
		answerString.Add (answerButtons [0].GetComponentInChildren<Text> ().text.ToUpper ());
		answerString.Add (answerButtons [1].GetComponentInChildren<Text> ().text.ToUpper ());
	}
}
