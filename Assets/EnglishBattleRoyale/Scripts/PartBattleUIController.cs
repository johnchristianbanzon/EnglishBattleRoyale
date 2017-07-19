using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/* Controls the battle */
public class PartBattleUIController : SingletonMonoBehaviour<PartBattleUIController>
{
	public Text[] skillName;
	public Text[] skillGpCost;

	public Text playerNameText;


	public Slider playerHPBar;
	public Text playerHPText;
	public Text playerGPText;

	public Slider playerGPBar;


	public Slider enemyHPBar;
	public Text enemyNameText;
	public Text enemyHPText;


	private int timeLeft = 3;
	private bool stoptimer = true;

	void Start(){
		StartPreTimer ();
		AudioController.Instance.PlayAudio (AudioEnum.Bgm);

		PartCameraWorksController.Instance.StartIntroCamera ();
	}

	/// <summary>
	/// Delay before start of battle
	/// </summary>
	private void StartPreTimer ()
	{
		timeLeft = 3;
		stoptimer = true;
		InvokeRepeating ("StartTimer", 0, 1);
	}

	private void StartTimer ()
	{
		if (stoptimer) {
			GameTimerController.Instance.ToggleTimer (true);
			if (timeLeft > 0) {
				GameTimerController.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 
			PartPhaseController.Instance.StartPhase1 ();
			GameTimerController.Instance.ToggleTimer (false);
			stoptimer = false;
			CancelInvoke ("StartTimer");
		}
	}

	public void SetSkillUI (int skillNumber, string skillName, int skillGp)
	{
		this.skillName [skillNumber - 1].text = skillName.ToString ();
		this.skillGpCost [skillNumber - 1].text = "" + skillGp + "GP";
	}

	public void SetInitialPlayerUI (string name, float hP, float gP)
	{
		Debug.Log (name);
		playerHPBar.value = hP;
		playerHPBar.maxValue = hP;
		playerNameText.text = name;
		playerGPText.text = gP.ToString();
		playerGPBar.value = gP;
		playerGPBar.maxValue = SystemGlobalDataController.Instance.player.playerMaxGP;
	}

	public void SetInitialEnemyUI (string name, float hP)
	{
		enemyNameText.text = name;
		enemyHPText.text = hP.ToString();
		enemyHPBar.value = hP;
		enemyHPBar.maxValue = hP;
	}



}
