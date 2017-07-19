using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PhaseAttackController : BasePhase
{
	public override void OnStartPhase ()
	{
		FindObjectOfType<PhaseSkillController> ().ShowAutoActivateButtons (false);
		Debug.Log ("Starting attack phase");
		AnswerIndicatorController.Instance.ResetAnswer ();

		GameTimerController.Instance.ToggleTimer (false);
		stoptimer = true;
		timeLeft = 20;
		InvokeRepeating ("StartTimer", 0, 1);
		Attack ();
	}

	public void Attack ()
	{
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.Attack.ToString ()] = SystemGlobalDataController.Instance.player.playerBaseDamage + SystemGlobalDataController.Instance.gpEarned;
		SystemFirebaseDBController.Instance.AttackPhase (new AttackModel(JsonConverter.DicToJsonStr(param)));
	}


	public override void OnEndPhase(){
		FindObjectOfType<PhaseSkillController> ().ShowAutoActivateButtons (true);
		CancelInvoke ("StartTimer");
	}

	private void StartTimer ()
	{
		if (stoptimer) {
			if (timeLeft > 0) {
				timeLeft--;
				return;
			} 

//			app.component.firebaseDatabaseComponent.CheckAttackPhase();

			stoptimer = false;

		}
	}
		
}
