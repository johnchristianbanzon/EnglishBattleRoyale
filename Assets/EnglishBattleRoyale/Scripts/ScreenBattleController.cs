using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBattleController: SingletonMonoBehaviour<ScreenBattleController>
{
	public PartStateController partState;
	public PartCharacterController partCharacter;
	public PartQuestionController partQuestion;
	public PartGestureController partGesture;
	public PartCameraWorksController partCameraWorks;
	public PartAvatarsController partAvatars;

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

	private void PhaseActivate (bool answer, bool skill, bool attack)
	{
		if (answer) {
			partQuestion.OnStartPhase ();
		} else {
			partQuestion.OnEndPhase ();
		}

		if (skill) {
			partCharacter.OnStartPhase ();
		} else {
			partCharacter.OnEndPhase ();
		}

		if (attack) {
			partState.OnStartPhase ();
		} else {
			partState.OnEndPhase ();
		}
	}
}
