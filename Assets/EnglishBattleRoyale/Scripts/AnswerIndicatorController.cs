using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AnswerIndicatorController : SingletonMonoBehaviour<AnswerIndicatorController>, IRPCDicObserver
{

	public Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
	public Image[] playerPlaceHolder;
	public Image[] enemyPlaceHolder;

	void Start ()
	{
		ResetAnswer ();
	}

	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
		if (rpcReceive.ContainsKey ("param")) {
			bool userHome = (bool)rpcReceive ["userHome"];
			GlobalDataManager.attackerBool = userHome;

			Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive ["param"];
			if (param.ContainsKey ("AnswerIndicator")) {
				string stringParam = param ["AnswerIndicator"].ToString ();
				SetAnswerParameter (stringParam);
			}
		}
	}

	public void SetAnswerParameter (string answerParameter)
	{

		Dictionary<string, System.Object> answerResult = JsonConverter.JsonStrToDic (answerParameter);

		foreach (KeyValuePair<string, System.Object> answer in answerResult) {

			if (answer.Key == ParamNames.AnswerCorrect.ToString ()) {
				ValidateAnswer (true, int.Parse (answer.Value.ToString ()));
			} else {
				ValidateAnswer (false, int.Parse (answer.Value.ToString ()));

			}
		}
	}

	public void ValidateAnswer (bool isCorrect, int questionNumber)
	{
		Debug.Log ("AnswerCorrect: " + isCorrect);
		Debug.Log ("Question No : " + questionNumber);
		if (GlobalDataManager.attackerBool.Equals (GlobalDataManager.isHost)) {
			SetValidateAnswer (isCorrect, playerPlaceHolder [questionNumber - 1]);
		} else {
			SetValidateAnswer (isCorrect, enemyPlaceHolder [questionNumber - 1]);
		}
	}

	private void SetValidateAnswer(bool isCorrect, Image image){
		if (isCorrect) {
			image.color = new Color32 (237, 232, 54, 255);
		} else {
			image.color = new Color32 (239, 87, 86, 255);
		}
	}

	public void ResetAnswer ()
	{
		for (int i = 0; i < playerPlaceHolder.Length; i++) {
			playerPlaceHolder [i].color = new Color32 (7, 61, 58, 255);
			enemyPlaceHolder [i].color = new Color32 (7, 61, 58, 255);
		}
		Debug.Log ("reset answers");
	}
}
