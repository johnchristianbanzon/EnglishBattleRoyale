using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PartAnswerIndicatorController : SingletonMonoBehaviour<PartAnswerIndicatorController>, IRPCDicObserver
{

	public Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
	public Text playerAnswerText;
	public Text enemyAnswerText;

	private int playerAnswerCounter;
	private int enemyAnswerCounter;

	void Start ()
	{
		ResetAnswer ();
	}

	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		//do something here
	}

	public void SetAnswerParameter (string answerParameter)
	{

		Dictionary<string, System.Object> result = new Dictionary<string, System.Object> ();
		result ["AnswerRPC"] = JsonUtility.ToJson (answerParameter);
		Dictionary<string, System.Object> answerResult = result;

		foreach (KeyValuePair<string, System.Object> answer in answerResult) {

			if (answer.Key == ParamNames.AnswerCorrect.ToString ()) {
				if (SystemGlobalDataController.Instance.isSender.Equals (SystemGlobalDataController.Instance.isHost)) {
					playerAnswerCounter++;
					playerAnswerText.text = "" +playerAnswerCounter;

				} else {
					enemyAnswerCounter++;
					enemyAnswerText.text = "" +enemyAnswerCounter;
				}
			}
		}
	}

	public void ResetAnswer ()
	{
		playerAnswerText.text = "0";
		enemyAnswerText.text = "0";
		playerAnswerCounter = 0;
		enemyAnswerCounter = 0;
		Debug.Log ("reset answer indicators!");
	}
}
