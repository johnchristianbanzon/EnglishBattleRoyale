using System;
public interface IGameTimeObserver {
	void OnStartPreBattleTimer (int timer);
	void OnStartSkillTimer (Action action,int timer);
	void OnStartQuestionTimer (Action action, int timer);
	void OnHasAnswered (bool hasAnswered);
	void OnToggleTimer (bool toggleTimer);
	void OnStopTimer();

}
