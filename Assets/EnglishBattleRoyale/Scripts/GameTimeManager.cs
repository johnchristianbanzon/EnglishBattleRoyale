using System;

public class GameTimeManager
{
	private static int preBattleTimer = 3;
	private static int skillTimer = 5;
	private static int selectQuestionTimer = 5;

	static IGameTimeObserver observer;

	public static void StartPreBattleTimer ()
	{
		observer.OnStartPreBattleTimer (preBattleTimer);
	}

	public static void StartSkillTimer (Action action)
	{
		observer.OnStartSkillTimer (action, skillTimer);
	}

	public static void StartQuestionTimer (Action<int> action, int questionTimer)

	{
		observer.OnStartQuestionTimer (action, questionTimer);
	}

	public static void ToggleTimer (bool toggleTimer)
	{
		observer.OnToggleTimer (toggleTimer);
	}

	public static void StopTimer ()
	{
		observer.OnStopTimer ();
	}

	public static void AddObserver (IGameTimeObserver addObserver)
	{
		observer = addObserver;
	}

	public static void RemoveObserver ()
	{
		observer = null;
	}

}
