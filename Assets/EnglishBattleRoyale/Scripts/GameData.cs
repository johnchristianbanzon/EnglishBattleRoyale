using System.Collections.Generic;
using System;
using UnityEngine;
/* Global variables*/
public class GameData: SingletonMonoBehaviour<GameData>
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

	public int answerQuestionTime{ get; set; }

	public int gpEarned{ get; set; }

}
