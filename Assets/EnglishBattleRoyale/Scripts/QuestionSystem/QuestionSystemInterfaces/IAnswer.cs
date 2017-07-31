using System;
public interface IAnswer{
	void DeployAnswerType();
	void OnClickHint (int hintIndex, Action<bool> onHintResult);
	void ClearHint ();
}