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

	public PlayerModel player{ get; set; }

	public PlayerModel enemy{ get; set; }

	private List<bool> userHome = new List<bool> ();
	private List<Dictionary<string, System.Object>> param = new List<Dictionary<string, object>> ();
	private int attackCount = 0;

	void Update ()
	{
		if (player != null && enemy != null) {
			playerHPText.text = player.playerHP.ToString ();
			playerHPBar.value = player.playerHP;

			playerGPText.text = player.playerGP.ToString ();
			playerGPBar.value = player.playerGP;

			enemyHPText.text = enemy.playerHP.ToString ();
			enemyHPBar.value = enemy.playerHP;
		}
	}


	void Start ()
	{
		Init ();
	}

	private void Init ()
	{
		TimeManager.AddGameTimeObserver (this);
		TimeManager.StartPreBattleTimer (3);
		AudioController.Instance.PlayAudio (AudioEnum.Bgm);

		ScreenBattleController.Instance.partCameraWorks.StartIntroCamera ();

		//get initial state for enemy and player stored in SystemGlobalDataController
		foreach (KeyValuePair<Firebase.Database.DataSnapshot, bool> initialState in SystemGlobalDataController.Instance.InitialState) {
			SetStateParam (initialState.Key, initialState.Value);
		}
	}

	#region INITIAL STATE

	private void SetStateParam (Firebase.Database.DataSnapshot dataSnapShot, bool isHome)
	{
		Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
		if (rpcReceive.ContainsKey (MyConst.RPC_DATA_PARAM)) {
			Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive [MyConst.RPC_DATA_PARAM];

			ReceiveInitialState (param, isHome);
		}
	}

	private void ReceiveInitialState (Dictionary<string, System.Object> initialState, bool isHome)
	{
		PlayerModel player = JsonUtility.FromJson<PlayerModel> (initialState [MyConst.RPC_DATA_PLAYER].ToString ());

		if (isHome) {
			
			SetInitialPlayerUI (player);
		} else {
			SetInitialEnemyUI (player);
		}
	}

	public void SetInitialPlayerUI (PlayerModel player)
	{
		this.player = player;
		playerNameText.text = player.playerName;

		playerHPText.text = player.playerHP.ToString ();
		playerHPBar.maxValue = player.playerHP;
		playerHPBar.value = player.playerHP;


		playerGPText.text = player.playerGP.ToString ();
		playerGPBar.maxValue = player.playerGP;
		playerGPBar.value = player.playerGP;
	
	}

	public void SetInitialEnemyUI (PlayerModel enemy)
	{
		this.enemy = enemy;
		enemyNameText.text = enemy.playerName;
		enemyHPText.text = enemy.playerHP.ToString ();
		enemyHPBar.maxValue = enemy.playerHP;
		enemyHPBar.value = enemy.playerHP;

	}

	#endregion

	#region DamageCoroutine

	//For draw results, doesn't need the other coroutine to finish
	public void SetActions (Queue<Action> playersAction)
	{
		StartCoroutine (StartBattleActions(playersAction));
	}

	//activate the player actions
	IEnumerator StartBattleActions (Queue<Action> playersAction)
	{
		Queue<Action> playerActionsQueue = playersAction;
		for (int i = 0; i < playerActionsQueue.Count; i++) {
			playersAction.Dequeue ().Invoke();
			yield return new WaitForSeconds (1);

			//if no winner, start phase 1 again
			if (CheckHP ()) {
				ScreenBattleController.Instance.StartPhase1 ();
			}	
		}
	}

	private bool CheckHP(){
		if (enemy.playerHP <= 0 || player.playerHP <= 0) {
			SystemLoadScreenController.Instance.StopWaitOpponentScreen ();

			if (enemy.playerHP > 0 &&  player.playerHP <= 0) {
				Debug.Log ("Lose");

			} else if ( player.playerHP > 0 && enemy.playerHP <= 0) {
				Debug.Log ("Win");
			} else {
				Debug.Log ("Draw");
			}
			StopAllCoroutines ();
			return false;
		} 

		return true;
	}

	#endregion

	#region TIMER Subscriber

	public Text gameTimerText;
	public Text preBattleTimerText;

	public void OnStartGameTimer (int timer)
	{
		StartCoroutine (StartTimer (timer, true));
	}


	public void OnStartPreBattleTimer (int timer)
	{
		StartCoroutine (StartTimer (timer, false, delegate() {
			preBattleTimerText.enabled = false;
			ScreenBattleController.Instance.StartPhase1 ();
			TimeManager.StartGameTimer (180);
		}));
	}


	IEnumerator StartTimer (int timer, bool isGameTimer, Action action = null)
	{
		int timeLeft = timer;

		while (timeLeft > 0) {
			if (isGameTimer) {
				gameTimerText.text = "" + timeLeft;
			} else {
				preBattleTimerText.text = "" + timeLeft;
			}
			timeLeft--;
			yield return new WaitForSeconds (1);

		}

		action ();
	}

	#endregion

}
