using System.Collections.Generic;
using System;
using UnityEngine;
/* Global variables*/
public static class GlobalDataManager
{
	public static Canvas gameCanvas;

	public static PlayerModel player{ get; set; }

	public static int answerQuestionTime{ get; set; }

	public static bool isHost{ get; set; }

	public static bool attackerBool{ get; set; }

	public static ModeEnum modePrototype { get; set; }
	 
	public static Action playerSkillChosen{ get; set; }

	public static int skillChosenCost{ get; set; }

	public static int hAnswer{ get; set; }

	public static int hTime{ get; set; }

	public static int vAnswer{ get; set; }

	public static int vTime{ get; set; }

	public static int gpEarned{ get; set; }

	//load default mode if none
	static void Start(){
		modePrototype = ModeEnum.Mode1;
	}

	public static void ResetPlayer(){
		player = GameManager.GetPlayer ();
	}

}
