using System;
using System.Collections.Generic;

public class TimeManager
{
	private static IGameTimeObserver gameTimeObserver;
	private static IQuestionTimeObserver questionTimeObserver;

	//PANTOY HERE

	public static void StartPreBattleTimer(int timer){
		gameTimeObserver.OnStartPreBattleTimer (timer);
	}

	public static void StartCharacterSelectTimer(int timer, Action action){
		gameTimeObserver.OnStartCharacterSelectTimer (timer, action);
	}

	//PANTOY END HERE

	//JOCHRIS HERE
	public static void StartQuestionTimer (Action<int> action, int timer)
	{	
		questionTimeObserver.OnStartQuestionTimer (action, timer);
	}

	public static void StopQuestionTimer ()
	{
		questionTimeObserver.OnStopQuestionTimer ();
	}
	//JOCHRIS END HERE

	#region Manage Observers

	public static void AddGameTimeObserver (IGameTimeObserver observer)
	{
		gameTimeObserver = observer;
	}

	public static void RemoveGameTimeObserver ()
	{
		gameTimeObserver = null;
	}

	public static void AddQuestionTimeObserver (IQuestionTimeObserver observer)
	{
		questionTimeObserver = observer;
	}

	public static void RemoveQuestionTimeObserver ()
	{
		questionTimeObserver = null;
	}
	#endregion

}
