using UnityEngine;

/* Manages phases */
public class PhaseManager
{
	public static void StartPhase1 ()
	{
		if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
			PhaseActivate (false, true, false);
		} else {

			PhaseActivate (true, false, false);
		}
	}

	public static void StartPhase2 ()
	{
		
		if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
			PhaseActivate (true, false, false);
		} else {
			PhaseActivate (false, true, false);
		}
	}

	public static void StartPhase3 ()
	{
		
		PhaseActivate (false, false, true);
	}

	public static void StopAll ()
	{
		PhaseActivate (false, false, false);
	}

	private static void PhaseActivate (bool answer, bool skill, bool attack)
	{


		if (answer) {
			ScreenBattleController.Instance.partQuestion.OnStartPhase ();
		} else {
			ScreenBattleController.Instance.partQuestion.OnEndPhase ();
		}

		if (skill) {
			ScreenBattleController.Instance.partSkill.OnStartPhase ();
		} else {
			ScreenBattleController.Instance.partSkill.OnEndPhase ();
		}

		if (attack) {
			ScreenBattleController.Instance.partState.OnStartPhase ();
		} else {
			ScreenBattleController.Instance.partState.OnEndPhase ();
		}
	}

}
