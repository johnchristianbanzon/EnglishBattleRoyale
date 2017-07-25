using System;
public interface ISelection {
	void DeploySelectionType (string questionAnswer);
	void RemoveSelectionHint (int hintIndex);
}