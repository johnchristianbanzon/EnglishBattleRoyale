/* Manages phases */
public static class PhaseManager
{
	public static void StartPhase1 ()
	{
		if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
			PhaseActivate (false, true, false);
		} else {

			PhaseActivate (true, false, false);
		}
	}

	public static void StartPhase2 ()
	{
		
		if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
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
		BasePhase phaseAnswerController = new PhaseAnswerController();
		BasePhase phaseSkillController = new PhaseSkillController();
		BasePhase phaseAttackController = new PhaseAttackController();

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
