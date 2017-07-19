using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*This class activates the skill received from database*/
public class SkillActivator: SingletonMonoBehaviour<SkillActivator>, IRPCDicObserver
{

	public PartAvatarsController partAvatar;

	void Start(){
		RPCDicObserver.AddObserver (this);
	}
	
	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		try {
			Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
			if (rpcReceive.ContainsKey ("param")) {

				bool userHome = (bool)rpcReceive ["userHome"];
				SystemGlobalDataController.Instance.attackerBool = userHome;

				Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive ["param"];
				if (param.ContainsKey ("SkillParam")) {
					string stringParam = param ["SkillParam"].ToString ();
					if (SystemGlobalDataController.Instance.attackerBool.Equals (SystemGlobalDataController.Instance.isHost)) {
						SetPlayerSkillParameter (stringParam);
					} else {
						SetEnemySkillParameter (stringParam);
					}

				}
				if (param.ContainsKey ("SkillName")) {
					string stringParam = param ["SkillName"].ToString ();
					SetSkillAnimation (stringParam);
				}
			}
		} catch (System.Exception e) {
			//do something with exception in future
		}
	}

	/// <summary>
	/// Sets the player skill parameter.
	/// </summary>
	/// <param name="skillParameter">Skill parameter.</param>
	public void SetPlayerSkillParameter (string skillParameter)
	{
		SkillParameterList skillResult = JsonUtility.FromJson<SkillParameterList> (skillParameter);

		foreach (SkillParameter skill in skillResult.skillList) {

			if (skill.skillKey == ParamNames.Damage.ToString ()) {
				SystemGlobalDataController.Instance.player.playerBaseDamage += skill.skillValue;
				Debug.Log ("skill player " + skill.skillKey + " value " + skill.skillValue);
			}

			if (skill.skillKey == ParamNames.Recover.ToString ()) {
				ScreenBattleController.Instance.PlayerHP += skill.skillValue;
				Debug.Log ("skill player " + skill.skillKey + " value " + skill.skillValue);
			}
		}
	}

	public void SetEnemySkillParameter (string skillParameter)
	{
		SkillParameterList skillResult = JsonUtility.FromJson<SkillParameterList> (skillParameter);

		foreach (SkillParameter skill in skillResult.skillList) {
			if (skill.skillKey == ParamNames.Recover.ToString ()) {
				ScreenBattleController.Instance.EnemyHP += skill.skillValue;
				Debug.Log ("skill enemy " + skill.skillKey + " value " + skill.skillValue);
			}
		}
	}

	/// <summary>
	/// Checks the name of the skill and set animation
	/// </summary>
	/// <param name="newParam">New parameter.</param>
	public void SetSkillAnimation (string skillName)
	{
		if (SystemGlobalDataController.Instance.attackerBool.Equals (SystemGlobalDataController.Instance.isHost)) {
			partAvatar.SetTriggerAnim (true, skillName);
		} else {
			partAvatar.SetTriggerAnim (false, skillName);
		}
	}
}
