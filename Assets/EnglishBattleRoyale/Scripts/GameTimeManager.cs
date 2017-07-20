using System;

public class GameTimeManager
{
	static IGameTimeObserver observer;

	public static void StartPreBattleTimer (int timer)
	{
		observer.OnStartPreBattleTimer (timer);
	}

	public static void StartSkillTimer (Action action,int timer)
	{
		observer.OnStartSkillTimer (action, timer);
	}

	public static void StartSelectQuestionTimer (Action action,int timer)
	{
		observer.OnStartSelectQuestionTimer (action, timer);
	}

	public static void StartQuestionTimer (Action action, int timer)
	{
		observer.OnStartQuestionTimer (action, timer);
	}

	public static void HasAnswered (bool hasAnswered)
	{
		observer.OnHasAnswered (hasAnswered);
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
