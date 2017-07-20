using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Net;
using System.IO;

public class SlotMachineIcon : BaseQuestion, IQuestion{

	public GameObject gpText;
	public GameObject[] roulletes = new GameObject[12];
	public Text questionText;
	private List<GameObject> roulleteText = new List<GameObject> ();
	private bool gotAnswer = true;
	private SlotMachineOnChange smoc;
	private List<Color> previousSlotColor = new List<Color>();

	public void Activate(Action<int,int> result){
//		QuestionBuilder.PopulateQuestion ("slotmachine",gameObject);
		roulleteText.Clear ();
		answerButtons.Clear ();
		gotAnswer = true;
		questionAnswer = "";
		correctAnswers = 0;
		currentRound = 1;
		NextQuestion ();
		QuestionController.Instance.OnResult = result;
	}

	void Start()
	{
		smoc = new SlotMachineOnChange ();
	}

	private void ProvideHint(){
		CheckAnswer (true);
	}
	public void NextQuestion(){
		
		LoadQuestion (QuestionSystemEnums.QuestionType.Definition);
		findSlotMachines ();
		ShuffleAlgo ();
	}
	void Update(){
		Debug.Log (smoc.WrittenAnswer);
		string ans = smoc.WrittenAnswer.Substring(0, smoc.WrittenAnswer.Length - (smoc.WrittenAnswer.Length - questionAnswer.Length));
		Debug.Log (ans);
		if ((questionAnswer == ans )&& gotAnswer) {
			gotAnswer = false;
			CheckAnswer (true);
		}
	}
		
	public void TweenCallBack ()
	{
		TweenFacade.TweenTextScale (gpText.transform, Vector3.one, 1.0f);
		gpText.GetComponent<Text> ().text = " ";
	}

	public void findSlotMachines(){
		roulleteText.Clear ();
		GameObject content;
		for (int i = 0; i < questionAnswer.Length; i++) {
			content = roulletes [i];
			for (int j = 0; j < 3; j++) {
				roulleteText.Add (content.transform.GetChild(j).gameObject);
		 	}
		}
		Debug.Log (questionAnswer+"/"+questionAnswer.Length	);
		for (int i = 0; i < questionAnswer.Length; i++) {
			roulletes [i].transform.parent.parent.parent.parent.gameObject.SetActive (true);
		}
		for(int i = 6 ; i > questionAnswer.Length ;i--){
			roulletes[i-1].transform.parent.parent.parent.parent.gameObject.SetActive(false);
		}
	}

	public void OnFinishQuestion ()
	{
		int i = 0;
		foreach (GameObject g in answerButtons) {
			g.GetComponent<Image>().color = previousSlotColor[i];
			i++;
		}
		answerButtons.Clear ();
		gotAnswer = true;
		TweenCallBack ();
		hasSkippedQuestion = false;
		currentRound += 1;
		NextQuestion ();
		QuestionController.Instance.Returner (delegate {
			QuestionController.Instance.onFinishQuestion = true;
		}, currentRound, correctAnswers);
	}

	public void ShuffleAlgo ()
	{
		string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		int letterIndex = 0;
		int letterStartIndex = 0;
		int letterEndIndex = 3;
		int randomnum = UnityEngine.Random.Range (letterStartIndex+1, letterEndIndex);
		for (int i = 0; i < roulleteText.Count; i++) {
			roulleteText [i].transform.GetChild (0).GetComponent<Text> ().text = (i%randomnum)==0 ?
				questionAnswer [letterIndex].ToString ().ToUpper ():
				Letters [UnityEngine.Random.Range (0, Letters.Length)].ToString ().ToUpper ();
			if ((i % randomnum) == 0) {
				letterIndex += 1;
				letterStartIndex = letterEndIndex;
				letterEndIndex = letterEndIndex + 3;
				randomnum = UnityEngine.Random.Range (letterStartIndex, letterEndIndex);
				answerButtons.Add (roulleteText [i]);
				previousSlotColor.Add (roulleteText [i].GetComponent<Image> ().color);
			}
		}
	}

}
