using System;
using System.Collections;
public interface IQuestionTimeObserver {
	void OnStartQuestionTimer (Action<int> action, int timer);
	void OnStopQuestionTimer ();

}
