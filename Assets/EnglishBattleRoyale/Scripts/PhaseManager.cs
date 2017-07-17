using UnityEngine;
using System.Collections.Generic;

/* Manages phases */
public class PhaseManager : SingletonMonoBehaviour<PhaseManager>
{
	public BasePhase phaseAnswerController;
	public BasePhase phaseSkillController;
	public BasePhase phaseAttackController;

	public void StartPhase1 ()
	{
		if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
			PhaseActivate (false, true, false);
		} else {

			PhaseActivate (true, false, false);
		}
	}

	public void StartPhase2 ()
	{
		
		if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
			PhaseActivate (true, false, false);
		} else {
			PhaseActivate (false, true, false);
		}
	}

	public void StartPhase3 ()
	{
		
		PhaseActivate (false, false, true);
	}

	public void StopAll ()
	{
		Debug.Log ("Stopped phases");
		PhaseActivate (false, false, false);
	}

	private void PhaseActivate (bool answer, bool skill, bool attack)
	{
		if (answer) {
			phaseAnswerController.OnStartPhase ();
		} else {
			phaseAnswerController.OnEndPhase ();
		}

		if (skill) {
			phaseSkillController.OnStartPhase ();
		} else {
			phaseSkillController.OnEndPhase ();
		}

		if (attack) {
			phaseAttackController.OnStartPhase ();
		} else {
			phaseAttackController.OnEndPhase ();
		}
	}

}
