using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class BattleLogic
{

	public static KeyValuePair<bool, Dictionary<string, System.Object>> GetAttackParam (Dictionary<bool, Dictionary<string, object>> currentParam)
	{


		KeyValuePair<bool, Dictionary<string, System.Object>> param = new KeyValuePair<bool, Dictionary<string, object>>();
		foreach (KeyValuePair<bool, Dictionary<string, System.Object>> newParam in currentParam) {
			param = newParam;
		}
		return param;
	}

	public static void ChangeOrder(Action <List<bool> , List<Dictionary<string, System.Object>>> attackparam, List<bool> userHome, List<Dictionary<string, System.Object>> param){
		//change order of list if host or visitor
		if (SystemGlobalDataController.Instance.isHost) {
			if (userHome [0] != SystemGlobalDataController.Instance.isHost) {
				ChangeOrderReduce (0, 1, attackparam, userHome, param);
			}
		} else {
			if (userHome [1] != SystemGlobalDataController.Instance.isHost) {
				ChangeOrderReduce (1, 0, attackparam, userHome, param);
			}

		}
	}

	private static void ChangeOrderReduce (int index0, int index1,Action <List<bool> , List<Dictionary<string, System.Object>>> attackparam, List<bool> userHome, List<Dictionary<string, System.Object>> param)
	{
		bool tempName = userHome [index0];
		Dictionary<string, System.Object> tempParam = param [index0];

		userHome.Insert (index0, userHome [index1]);
		userHome.Insert (index1, tempName);
		param.Insert (index0, param [index1]);
		param.Insert (index1, tempParam);

		attackparam (userHome, param);
	}

	public static int GetAttackOrder(){
		int attackOrder = 0;

		if (SystemGlobalDataController.Instance.hAnswer > SystemGlobalDataController.Instance.vAnswer) {
			attackOrder = 0;
		} else if (SystemGlobalDataController.Instance.hAnswer < SystemGlobalDataController.Instance.vAnswer) {
			attackOrder = 1;
		} else {
			if (SystemGlobalDataController.Instance.hTime > SystemGlobalDataController.Instance.vTime) {
				attackOrder = 0;
			} else if (SystemGlobalDataController.Instance.hTime < SystemGlobalDataController.Instance.vTime) {
				attackOrder = 1;
			} else {
				attackOrder = 2;
			}
		}
		return attackOrder;
	}

	public static void CheckBattle(bool secondCheck,float enemyHP, float playerHP, Action<string, bool> battleResult){

		if (enemyHP <= 0 || playerHP <= 0) {
			SystemLoadScreenController.Instance.StopWaitOpponentScreen ();

			if (enemyHP > 0 && playerHP <= 0) {
				battleResult ("lose", true);

			} else if (playerHP > 0 && enemyHP <= 0) {
				battleResult ("win", true);

			} else {
				battleResult ("draw", true);

			}

		} else {
			battleResult ("", false);
		}

	}


	public static int AttackLogic(Action<float, float> hp, float enemyHP, float playerHP, bool attackerBool, Dictionary<string, System.Object> attackerParam, bool sameAttack)
	{
		int attackSequence = 0;

		if (attackerParam [ParamNames.Attack.ToString ()] != null) {
			int damage = int.Parse (attackerParam [ParamNames.Attack.ToString ()].ToString ());

			if (attackerBool.Equals (SystemGlobalDataController.Instance.isHost)) {
				Debug.Log ("PLAYER DAMAGE: " + damage);
				enemyHP -= damage;
				if (sameAttack == false) {
					attackSequence = 1;
				}

			} else {
				Debug.Log ("ENEMY DAMAGE: " + damage);
				playerHP -= damage;
				if (sameAttack == false) {
					attackSequence = 2;
				}
			}

			hp (enemyHP, playerHP);
		}

		return attackSequence;
	}
}
