using System;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill {

	void Activate ();

	void SetSkill(Action<string,string, int> skillParam);

}
