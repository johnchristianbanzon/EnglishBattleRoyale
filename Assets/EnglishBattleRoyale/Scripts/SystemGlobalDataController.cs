using System.Collections.Generic;
using System;
using UnityEngine;
/* Global variables*/
public class SystemGlobalDataController: SingletonMonoBehaviour<SystemGlobalDataController>
{
	public Canvas gameCanvas;

	public PlayerModel player{ get; set; }

	public bool isHost{ get; set; }

	public bool attackerBool{ get; set; }

	public ModeEnum modePrototype { get; set; }
	 
	public Action playerSkillChosen{ get; set; }

	public int skillChosenCost{ get; set; }

	public int hAnswer{ get; set; }

	public int hTime{ get; set; }

	public int vAnswer{ get; set; }

	public int vTime{ get; set; }

	public int gpEarned{ get; set; }



	public Dictionary<Firebase.Database.DataSnapshot, bool> InitialState{ get; set;}

	//load default mode if none
	void Start(){
		modePrototype = ModeEnum.Mode1;
	}

	public void ResetPlayer(){
		player = GameManager.GetPlayer ();
	}

}
