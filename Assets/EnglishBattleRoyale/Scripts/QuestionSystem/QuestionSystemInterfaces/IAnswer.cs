using System;
public interface IAnswer{
	void DeployAnswerType();
	void OnClickHint (int hintCounter, Action<bool> onHintResult);
}