﻿using UnityEngine;

/* Manages phases */
public class PartPhaseController: SingletonMonoBehaviour<PartPhaseController>
{

	public BasePhase phaseAnswerController = new PhaseAnswerController();
	public BasePhase phaseSkillController = new PhaseSkillController();
	public BasePhase phaseAttackController = new PhaseAttackController();


	public void StartPhase1 ()
	{
		if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
			PhaseActivate (false, true, false);
		} else {

			PhaseActivate (true, false, false);
		}
	}

	public void StartPhase2 ()
	{
		
		if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
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