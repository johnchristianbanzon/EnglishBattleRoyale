using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartTargetController : MonoBehaviour {
	public SingleQuestion singleQuestionController;
	public Association associationController;

	public void DeployPartTarget(ITarget targetType, string targetString){
		targetType.DeployTargetType (targetString);
	}


}
