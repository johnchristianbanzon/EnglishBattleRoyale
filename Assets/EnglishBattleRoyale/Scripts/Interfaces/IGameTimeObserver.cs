using System;
public interface IGameTimeObserver {
	void OnStartPreBattleTimer(int timer);
	void OnStartCharacterSelectTimer(int timer, Action action);
}
