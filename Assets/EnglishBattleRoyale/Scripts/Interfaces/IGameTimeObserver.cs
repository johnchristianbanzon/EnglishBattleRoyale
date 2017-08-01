using System;
public interface IGameTimeObserver {
	void OnStartPreBattleTimer (int timer);
	void OnStartSkillTimer (Action action,int timer);
	void OnStartQuestionTimer (Action<int> action, int timer);
	void OnToggleTimer (bool toggleTimer);
	void OnStopTimer();

}
