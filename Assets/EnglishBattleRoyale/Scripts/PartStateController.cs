using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/* Controls the battle */
public class PartStateController : MonoBehaviour, IGameTimeObserver
{
	public Text playerNameText;


	public Slider playerHPBar;
	public Text playerHPText;
	public Text playerGPText;

	public Slider playerGPBar;


	public Slider enemyHPBar;
	public Text enemyNameText;
	public Text enemyHPText;



	void Start ()
	{
		
		GameTimeManager.AddObserver (this);
		GameTimeManager.StartPreBattleTimer ();
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

























	#region TIMER

	public Text gameTimerText;
	private bool hasAnswered = false;

	public void OnStartPreBattleTimer (int timer)
	{
		StopTimer ();
		StartCoroutine (PreBattleStartTimer (timer));
	}

	public void OnStartSkillTimer (Action action,int timer)
	{
		StopTimer ();
		StartCoroutine (StartSkillTimer (action, timer));
	}

	public void OnStartSelectQuestionTimer (Action action,int timer)
	{
		StopTimer ();
		StartCoroutine (StartSelectQuestionTimer (action, timer));
	}

	public void OnStartQuestionTimer (Action action, int timer)
	{
		StopTimer ();
		StartCoroutine (StartQuestionTimer (action, timer));
	}

	public void OnHasAnswered (bool hasAnswered)
	{
		this.hasAnswered = hasAnswered;
	}

	public void OnStopTimer(){
		StopTimer ();
	}

	public void OnToggleTimer (bool toggleTimer)
	{
		gameTimerText.enabled = toggleTimer;
	}

	#region PrebattleTimer


	IEnumerator PreBattleStartTimer (int timer)
	{
		OnToggleTimer (true);
		int timeLeft = timer;

		while (timeLeft > 0) {
			gameTimerText.text = "" + timeLeft;
			timeLeft--;
			yield return new WaitForSeconds (1);

		}

		OnToggleTimer (false);
		PhaseManager.StartPhase1 ();
	}

	#endregion

	#region SkillTimer


	IEnumerator StartSkillTimer (Action action,int timer)
	{
		OnToggleTimer (true);
		int timeLeft = timer;

		while (timeLeft > 0) {
			gameTimerText.text = "" + timeLeft;
			timeLeft--;
			yield return new WaitForSeconds (1);
		}

		action ();
		OnToggleTimer (false);
		SystemFirebaseDBController.Instance.SkillPhase ();
	}

	#endregion

	#region SelectQuestionTimer

	IEnumerator StartSelectQuestionTimer (Action action,int timer)
	{
		OnToggleTimer (true);
		int timeLeft = timer;

		while (timeLeft > 0 && hasAnswered == false) {
			gameTimerText.text = "" + timeLeft;
			timeLeft--;
			yield return new WaitForSeconds (1);
		}

		action ();
		OnToggleTimer (false);
	}

	#endregion

	#region QuestionTimer

	IEnumerator StartQuestionTimer (Action action, int timer)
	{
		OnToggleTimer (true);
		int timeLeft = timer;

		while (timeLeft > 0) {
			gameTimerText.text = "" + timeLeft;
			timeLeft--;
			yield return new WaitForSeconds (1);
		}

		action ();
		OnToggleTimer (false);
	}

	#endregion

	public void StopTimer ()
	{
		StopAllCoroutines ();
	}

	#endregion

}
