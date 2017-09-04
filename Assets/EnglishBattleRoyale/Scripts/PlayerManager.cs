using System.Collections.Generic;
using UnityEngine;

public static class PlayerManager
{
	private static PlayerModel player;

	private static PlayerModel enemy;

	private static QuestionResultCountModel playerAnswerParam;

	private static QuestionResultCountModel enemyAnswerParam;

	private static bool isPlayer = false;

	public static void SetIsPlayer (bool isPlayerBool)
	{
		isPlayer = isPlayerBool;
	}

	public static PlayerModel Player {
		get { 
			if (isPlayer) {
				return player;
			} else {
				return enemy;
			}
		}

		set {
			if (isPlayer) {
				player = value;
			} else {
				enemy = value;
			}

		}
	}

	public static PlayerModel GetPlayer (bool isPlayer)
	{
		if (isPlayer) {
			return player;
		} else {
			return enemy;
		}
	}


	public static QuestionResultCountModel QuestionResultCount {
		get { 
			if (isPlayer) {
				return playerAnswerParam;
			} else {
				return enemyAnswerParam;
			}
		}

		set {
			if (isPlayer) {
				playerAnswerParam = value;
			} else {
				enemyAnswerParam = value;
			}
		}
	}

	public static QuestionResultCountModel GetQuestionResultCount (bool isPlayer)
	{
		if (isPlayer) {
			return playerAnswerParam;
		} else {
			return enemyAnswerParam;
		}
	}

	public static void Init ()
	{
		//get initial state for enemy and player stored in GameManager
		foreach (KeyValuePair<Firebase.Database.DataSnapshot, bool> initialState in GameManager.initialState) {
			SetStateParam (initialState.Key, initialState.Value);
		}
	}

	private static void SetStateParam (Firebase.Database.DataSnapshot dataSnapShot, bool isPlayer)
	{
		Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
		if (rpcReceive.ContainsKey (MyConst.RPC_DATA_PARAM)) {
			Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive [MyConst.RPC_DATA_PARAM];

			ReceiveInitialState (param, isPlayer);
		}
	}

	private static void ReceiveInitialState (Dictionary<string, System.Object> initialState, bool isPlayer)
	{
		PlayerModel playerModel = JsonUtility.FromJson<PlayerModel> (initialState [MyConst.RPC_DATA_PLAYER].ToString ());

		SetIsPlayer (isPlayer);
		Player = playerModel;
		ScreenBattleController.Instance.partState.UpdatePlayerUI (isPlayer, playerModel);
	}

	public static void UpdateStateUI(bool isPlayer){
		if (isPlayer) {
			ScreenBattleController.Instance.partState.UpdatePlayerUI (true, player);
		} else {
			ScreenBattleController.Instance.partState.UpdatePlayerUI (false, enemy);
		}

		CheckPlayerState ();

	}


	public static void CheckPlayerState ()
	{
		if (PlayerManager.GetPlayer (true) != null && PlayerManager.GetPlayer (false) != null) {
			if (player.hp > 100) {
				player.hp = 100;
			}

			if (enemy.hp > 100) {
				enemy.hp = 100;
			}

			if (player.hp < 0) {
				player.hp = 0;
			}

			if (enemy.hp < 0) {
				enemy.hp = 0;
			}

			if (player.gp > player.maxGP) {
				player.gp = player.maxGP;
			}

			if (player.gp < 0) {
				player.gp = 0;
			}

			if (enemy.gp > enemy.maxGP) {
				enemy.gp = enemy.gp;
			}

			if (enemy.gp < 0) {
				enemy.gp = 0;
			}

		}
	}


}