using System;
using System.Collections;
public interface IQuestionTimeObserver {
	void OnStartQuestionTimer (Action<float> action, float timer);
	void OnStopQuestionTimer ();
}