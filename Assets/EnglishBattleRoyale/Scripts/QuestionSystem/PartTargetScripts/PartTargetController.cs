using UnityEngine;

public class PartTargetController : MonoBehaviour {
	public SingleQuestion singleQuestion;
	public Association association;

	public void DeployPartTarget(ITarget targetType, string targetString){
		targetType.ShowTargetType (targetString);
	}
}
