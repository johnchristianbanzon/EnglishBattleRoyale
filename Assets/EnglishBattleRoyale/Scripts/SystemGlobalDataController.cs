using System.Collections.Generic;
using System;
using UnityEngine;

/* Global variables*/
public class SystemGlobalDataController: SingletonMonoBehaviour<SystemGlobalDataController>
{
	public Canvas gameCanvas;

	public PlayerModel player{ get; set; }

	public bool isHost{ get; set; }

	public bool isSender{ get; set; }

	public QuestionResultCountModel playerAnswerParam{ get; set; }

	public QuestionResultCountModel enemyAnswerParam{ get; set; }

	public Dictionary<Firebase.Database.DataSnapshot, bool> InitialState{ get; set;}

	public void ResetPlayer(){
		player = GameManager.GetPlayer ();
	}

}
