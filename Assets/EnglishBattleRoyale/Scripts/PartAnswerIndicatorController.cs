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
		Init ();
	}

	private void Init ()
	{
		ResetAnswer ();
	}

	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		try {
			Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
			if (rpcReceive.ContainsKey (MyConst.RPC_DATA_PARAM)) {

				bool userHome = (bool)rpcReceive [MyConst.RPC_DATA_USERHOME];

				Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive [MyConst.RPC_DATA_PARAM];
				if (param.ContainsKey (MyConst.RPC_DATA_ANSWER_INDICATOR)) {

					if (userHome.Equals (SystemGlobalDataController.Instance.isHost)) {
						playerAnswerCounter++;
						playerAnswerText.text = playerAnswerCounter.ToString ();

					} else {
						enemyAnswerCounter++;
						enemyAnswerText.text = enemyAnswerCounter.ToString ();
					}

				}

			}
		} catch (System.Exception e) {
			//do something with exception in future
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
