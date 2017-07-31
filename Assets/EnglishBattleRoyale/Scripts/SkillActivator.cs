using System.Collections.Generic;
using UnityEngine;

/*This class activates the skill received from database*/
public class SkillActivator: IRPCDicObserver
{

	public PartAvatarsController partAvatar;

	void Start ()
	{
		RPCDicObserver.AddObserver (this);
	}

	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		try {
			Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
			if (rpcReceive.ContainsKey ("param")) {

				bool userHome = (bool)rpcReceive ["userHome"];
				SystemGlobalDataController.Instance.isSender = userHome;

				Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive ["param"];
				if (param.ContainsKey ("CharacterRPC")) {
					
					CharacterModel character = (CharacterModel) JsonConverter.StringToObject (param ["CharacterRPC"].ToString());
				}
				
			}
		} catch (System.Exception e) {
			//do something with exception in future
		}
	}
	
}
