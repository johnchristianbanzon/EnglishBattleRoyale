using System;
public interface IQuestionTimeObserver {
	void OnStartQuestionTimer (Action<int> action, int timer);
	void OnStopQuestionTimer ();

}
