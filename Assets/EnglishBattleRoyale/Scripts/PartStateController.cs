using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Controls the battle */
public class PartStateController : MonoBehaviour
{
	public Text playerNameText;


	public Slider playerHPBar;
	public Text playerHPText;
	public Text playerGPText;

	public Slider playerGPBar;


	public Slider enemyHPBar;
	public Text enemyNameText;
	public Text enemyHPText;


	public GameTimerController gameTimer;

	void Start ()
	{
		gameTimer.PreBattleTimer ();
		AudioController.Instance.PlayAudio (AudioEnum.Bgm);

		ScreenBattleController.Instance.partCameraWorks.StartIntroCamera ();
	}

	public void SetInitialPlayerUI (string name, float hP, float gP)
	{
		Debug.Log (name);
		playerHPBar.value = hP;
		playerHPBar.maxValue = hP;
		playerNameText.text = name;
		playerGPText.text = gP.ToString ();
		playerGPBar.value = gP;
		playerGPBar.maxValue = SystemGlobalDataController.Instance.player.playerMaxGP;
	}

	public void SetInitialEnemyUI (string name, float hP)
	{
		enemyNameText.text = name;
		enemyHPText.text = hP.ToString ();
		enemyHPBar.value = hP;
		enemyHPBar.maxValue = hP;
	}


	public void OnStartPhase ()
	{
		ScreenBattleController.Instance.partSkill.ShowAutoActivateButtons (false);
		Debug.Log ("Starting attack phase");
		PartAnswerIndicatorController.Instance.ResetAnswer ();
		Attack ();
	}

	public void Attack ()
	{
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.Attack.ToString ()] = SystemGlobalDataController.Instance.player.playerBaseDamage + SystemGlobalDataController.Instance.gpEarned;
		SystemFirebaseDBController.Instance.AttackPhase (new AttackModel (JsonConverter.DicToJsonStr (param)));
	}


	public void OnEndPhase ()
	{
		ScreenBattleController.Instance.partSkill.ShowAutoActivateButtons (true);
	}



}
