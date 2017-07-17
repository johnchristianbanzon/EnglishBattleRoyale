using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IQuestion {
	void Activate (Action<int,int> Result);
}