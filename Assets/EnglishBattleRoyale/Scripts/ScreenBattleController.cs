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
		PhaseActivate (true, false);
	}

	public void StartPhase2 ()
	{
		PhaseActivate (false, true);
	}

	private void PhaseActivate (bool answer, bool attack)
	{
		if (answer) {
			partQuestion.OnStartPhase ();
		} else {
			partQuestion.OnEndPhase ();
		}

		BattleManager battleManager = new BattleManager ();
		if (attack) {
			battleManager.OnStartPhase ();
		} else {
			battleManager.OnEndPhase ();
		}
	}
}
