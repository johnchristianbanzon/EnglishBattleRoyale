using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System;

#if UNITY_EDITOR
using Firebase.Unity.Editor;
#endif

public class SystemFirebaseDBController : SingletonMonoBehaviour<SystemFirebaseDBController>,IRPCDicObserver, IRPCQueryObserver
{
	DatabaseReference reference;
	DatabaseReference roomReference;
	private bool searchingRoom;
	private Action<bool> onSuccessMatchMake;
	private int joinCounter = 0;
	string gameRoomKey = null;
	string battleStatusKey = null;
	private bool isMatchMakeSuccess = false;
	private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
	private  Dictionary<DataSnapshot, bool> InitialState = new Dictionary<DataSnapshot, bool>();

	void Start ()
	{

		SystemLoadScreenController.Instance.StartLoadingScreen (delegate() {
			dependencyStatus = FirebaseApp.CheckDependencies ();
			if (dependencyStatus != DependencyStatus.Available) {
				FirebaseApp.FixDependenciesAsync ().ContinueWith (task => {
					dependencyStatus = FirebaseApp.CheckDependencies ();
					if (dependencyStatus == DependencyStatus.Available) {
						InitializeFirebase ();
					} else {
						SystemLoadScreenController.Instance.StopLoadingScreen ();
						Debug.LogError (
							"Could not resolve all Firebase dependencies: " + dependencyStatus);
					}
				});
			} else {

				InitializeFirebase ();
			}
		});
	}

	private void InitializeFirebase ()
	{
		SystemLoadScreenController.Instance.StopLoadingScreen ();
		#if UNITY_EDITOR
		// Set this before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl (MyConst.URL_FIREBASE_DATABASE);
		#endif
		SetReference ();
		FirebaseDBFacade.CheckDBConnection (FirebaseDatabase.DefaultInstance.GetReferenceFromUrl (MyConst.URL_FIREBASE_DATABASE_CONNECTION));
	}

	private void SetReference ()
	{
		reference = FirebaseDatabase.DefaultInstance.RootReference;
		roomReference = reference.Child (MyConst.GAMEROOM_NAME);
	}

	//Do something when receives rpc from facade
	public void OnNotify (DataSnapshot dataSnapShot)
	{
		
		try {
			//TEMPORARY SOLUTION FOR PLAYER DETAILS
			if (dataSnapShot.Key.ToString ().Equals ("Home")) {
				if (SystemGlobalDataController.Instance.isHost) {
					InitialState.Add (dataSnapShot, true);
				} else {
					InitialState.Add (dataSnapShot, false);
				}


			}
			//TEMPORARY SOLUTION FOR PLAYER DETAILS
			if (dataSnapShot.Key.ToString ().Equals ("Visitor")) {
				isMatchMakeSuccess = true;
				onSuccessMatchMake (true);
				if (SystemGlobalDataController.Instance.isHost) {
					InitialState.Add (dataSnapShot, false);
				} else {
					InitialState.Add (dataSnapShot, true);
				}
				Debug.Log ("Matching Success!");
			}

			SystemGlobalDataController.Instance.InitialState = InitialState;
		} catch (Exception e) {
			//do someting in future
		}


	}

	public void OnNotifyQuery (DataSnapshot dataSnapshot)
	{
		if (searchingRoom) {
			if (dataSnapshot.HasChildren) {
				Debug.Log ("has game rooms");
				foreach (DataSnapshot snapshot in dataSnapshot.Children) {
				
					//get prototype mode type from host
					SystemGlobalDataController.Instance.modePrototype = (ModeEnum)int.Parse (snapshot.Child (MyConst.GAMEROOM_PROTOTYPE_MODE).Value.ToString ());

					GameManager.SetSettings ();

					if (snapshot.Child (MyConst.GAMEROOM_STATUS).Value.ToString ().Equals ("0")) {

						gameRoomKey = snapshot.Key.ToString ();
						JoinRoom ();
						searchingRoom = false;
						return;
					}
				}

			} else {
				Debug.Log ("has NO game rooms");
				CreateRoom ();
				searchingRoom = false;
			}
		}
	}

	public void SearchRoom (Action<bool> onResult)
	{
		searchingRoom = true;
		RPCDicObserver.AddObserver (this);
		RPCQueryObserver.AddObserver (this);
		//Order first to search fast
		FirebaseDBFacade.QueryTable ("SearchRoom", roomReference.OrderByChild (MyConst.GAMEROOM_STATUS).EqualTo ("0"));

		onSuccessMatchMake = onResult;

	}

	private void CreateRoom ()
	{
		GameManager.SetSettings ();
		gameRoomKey = FirebaseDBFacade.CreateKey (reference.Child (MyConst.GAMEROOM_NAME));
		RoomCreateJoin (true, MyConst.GAMEROOM_HOME);

		//set prototype mode type
		FirebaseDBFacade.SetTableValueAsync (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_PROTOTYPE_MODE), "" + (int)SystemGlobalDataController.Instance.modePrototype);
	}

	public void CancelRoomSearch ()
	{
		if (!isMatchMakeSuccess) {
			if (SystemGlobalDataController.Instance.isHost) {
				DeleteRoom ();
				SystemGlobalDataController.Instance.isHost = false;
				//return;
			} 
		}
		RPCDicObserver.RemoveObserver (this);
		RPCQueryObserver.RemoveObserver (this);
		gameRoomKey = null;
		searchingRoom = false;
		onSuccessMatchMake (false);
		FirebaseDBFacade.RemoveQuery ("SearchRoom");
		FirebaseDBFacade.RemoveReference ("InitialStateListener");
		FirebaseDBFacade.RemoveReference ("BattleStatusValueChanged");
		FirebaseDBFacade.RemoveReference ("BattleStatusChildAdded");
		FirebaseDBFacade.RemoveReference ("RPCListener");
	}

	private void DeleteRoom ()
	{
		//add check room deleted successfully using childremove in fdfacade
		FirebaseDBFacade.RemoveTableValueAsync (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey));
	}

	public void JoinRoom ()
	{
		FirebaseDBFacade.RunTransaction (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_STATUS), delegate(MutableData mutableData) {
			int playerCount = int.Parse (mutableData.Value.ToString ());
		
			playerCount++;
			joinCounter++;

			mutableData.Value = playerCount.ToString ();
		});
			
		StartCoroutine (StartJoinDelay ());
	}

	IEnumerator StartJoinDelay ()
	{
		yield return new WaitForSeconds (5);
		Debug.Log ("JoinCounter " + joinCounter);
		if (joinCounter < 2) {
			RoomCreateJoin (false, MyConst.GAMEROOM_VISITOR);
		} else {
			FindObjectOfType<PartMatchController> ().SearchRoom ();
		}

		joinCounter = 0;
	}

	private void RoomCreateJoin (bool isHost, string userPlace)
	{
		SystemGlobalDataController.Instance.isHost = isHost;
		RPCListener ();

		Dictionary<string, System.Object> entryValues = SystemGlobalDataController.Instance.player.ToDictionary ();

		string directory = MyConst.GAMEROOM_NAME + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_INITITAL_STATE + "/" + userPlace + "/param/";
		FirebaseDBFacade.CreateTableChildrenAsync (directory, reference, entryValues);


		//set room status to open when create room
		if (isHost) {
			FirebaseDBFacade.SetTableValueAsync (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_STATUS), "0");
		}
		//set battle status to answer when start of game
		CheckInitialPhase ();

		InitialStateListener ();
	}

	private void RPCListener ()
	{
		if (gameRoomKey != null) {
			FirebaseDBFacade.CreateTableChildAddedListener ("RPCListener", reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_RPC));
		}
	}

	private void CheckInitialPhase ()
	{
		FirebaseDBFacade.GetTableValueAsync (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS), delegate(DataSnapshot dataSnapshot) {


			if (dataSnapshot.Value == null) {
				if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode1) {
					UpdateAnswerBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0, 0, 0, 0, 0);
				} else if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
					UpdateBattleStatus (MyConst.BATTLE_STATUS_SKILL, 0);
				}
			} else {
				Dictionary<string, System.Object> battleStatus = (Dictionary<string, System.Object>)dataSnapshot.Value;

				foreach (KeyValuePair<string , System.Object> battleKey in battleStatus) {
					battleStatusKey = battleKey.Key;
				}
			}

			FirebaseDBFacade.CreateTableChildAddedListener ("BattleStatusChildAdded", reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS));
			FirebaseDBFacade.CreateTableValueChangedListener ("BattleStatusValueChanged", reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS));
		});

	}

	private void InitialStateListener ()
	{
		if (gameRoomKey != null) {
			FirebaseDBFacade.CreateTableChildAddedListener ("InitialStateListener", reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_INITITAL_STATE));
		}
	}

	public void UpdateBattleStatus (string stateName, int stateCount)
	{
		battleStatusKey = reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).Push ().Key;
		Dictionary<string, System.Object> entryValues = new Dictionary<string, System.Object> ();
		entryValues.Add (MyConst.BATTLE_STATUS_STATE, stateName);
		entryValues.Add (MyConst.BATTLE_STATUS_COUNT, "" + stateCount);

		string directory = "/" + MyConst.GAMEROOM_NAME + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_BATTLE_STATUS + "/" + battleStatusKey + "/";
		FirebaseDBFacade.CreateTableChildrenAsync (directory, reference, entryValues);
	}

	//for mode 2
	public void UpdateAnswerBattleStatus (string stateName, int stateCount, int hTime, int hAnswer, int vTime, int vAnswer)
	{
		battleStatusKey = FirebaseDBFacade.CreateKey (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS));
		Dictionary<string, System.Object> entryValues = new Dictionary<string, System.Object> ();
		entryValues.Add (MyConst.BATTLE_STATUS_STATE, stateName);
		entryValues.Add (MyConst.BATTLE_STATUS_COUNT, "" + stateCount);
		entryValues.Add (MyConst.BATTLE_STATUS_HTIME, "" + hTime);
		entryValues.Add (MyConst.BATTLE_STATUS_HANSWER, "" + hAnswer);
		entryValues.Add (MyConst.BATTLE_STATUS_VTIME, "" + vTime);
		entryValues.Add (MyConst.BATTLE_STATUS_VANSWER, "" + vAnswer);

		string directory = "/" + MyConst.GAMEROOM_NAME + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_BATTLE_STATUS + "/" + battleStatusKey + "/";
		FirebaseDBFacade.CreateTableChildrenAsync (directory, reference, entryValues);
	}

	public void SetAttackParam (AttackModel attack)
	{
		SetParam (attack.ToDictionary ());
	}

	public void SetAnswerParam (AnswerModel answer)
	{
		SetParam (answer.ToDictionary ());
	}

	public void SetGestureParam (GestureModel gesture)
	{
		SetParam (gesture.ToDictionary ());
	}

	public void SetSkillParam (SkillModel skill)
	{
		SetParam (skill.ToDictionary ());
	}

	private void SetParam (Dictionary<string, System.Object> toDictionary)
	{
		string	rpcKey = reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_RPC).Push ().Key;

		Dictionary<string, System.Object> result = new Dictionary<string, System.Object> ();
		result ["userHome"] = SystemGlobalDataController.Instance.isHost;
		result ["param"] = toDictionary;

		string directory = "/" + MyConst.GAMEROOM_NAME + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_RPC + "/" + rpcKey;
		FirebaseDBFacade.CreateTableChildrenAsync (directory, reference, result);
	}

	public void AnswerPhase (int receiveTime, int receiveAnswer)
	{
		SystemLoadScreenController.Instance.StartWaitOpponentScreen ();
		int modulusNum = 1;
		if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
			modulusNum = 2;
		} else {
			modulusNum = 1;
		}
		GetLatestKey (modulusNum, delegate(string resultString) {
			FirebaseDBFacade.RunTransaction (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).Child (resultString), delegate(MutableData mutableData) {

				mutableData.Value = PhaseMutate (mutableData, MyConst.BATTLE_STATUS_ANSWER, delegate(Dictionary<string, System.Object> battleStatus, int battleCount) {
					if (SystemGlobalDataController.Instance.isHost) {
						battleStatus [MyConst.BATTLE_STATUS_HANSWER] = receiveAnswer.ToString ();
						battleStatus [MyConst.BATTLE_STATUS_HTIME] = receiveTime.ToString ();
					} else {
						battleStatus [MyConst.BATTLE_STATUS_VANSWER] = receiveAnswer.ToString ();
						battleStatus [MyConst.BATTLE_STATUS_VTIME] = receiveTime.ToString ();
					}
					if (battleCount == 2) {
						if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
							UpdateBattleStatus (MyConst.BATTLE_STATUS_ATTACK, 0);
						} else {
							UpdateBattleStatus (MyConst.BATTLE_STATUS_SKILL, 0);
						}
					}
				});
			});
		});
	}


	public void SkillPhase ()
	{
		SystemLoadScreenController.Instance.StartWaitOpponentScreen ();
		int modulusNum = 2;
		if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
			modulusNum = 1;
		} else {
			modulusNum = 2;
		}
		GetLatestKey (modulusNum, delegate(string resultString) {
			FirebaseDBFacade.RunTransaction (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).Child (resultString), delegate(MutableData mutableData) {
				mutableData.Value = PhaseMutate (mutableData, MyConst.BATTLE_STATUS_SKILL, delegate(Dictionary<string, System.Object> battleStatus, int battleCount) {
					if (battleCount == 2) {
						if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
							UpdateAnswerBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0, 0, 0, 0, 0);
						} else {
							UpdateBattleStatus (MyConst.BATTLE_STATUS_ATTACK, 0);
						}
					}
				});
			});
		});
	}


	public void AttackPhase (AttackModel param)
	{
		SystemLoadScreenController.Instance.StartWaitOpponentScreen ();
		GetLatestKey (3, delegate(string resultString) {
			FirebaseDBFacade.RunTransaction (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).Child (resultString), delegate(MutableData mutableData) {
				mutableData.Value = PhaseMutate (mutableData, MyConst.BATTLE_STATUS_ATTACK, delegate(Dictionary<string, System.Object> battleStatus, int battleCount) {
					SetAttackParam (param);
				});
			});
		});
	}

	private Dictionary<string, System.Object> PhaseMutate (MutableData mutableData, string battleStatusName, Action<Dictionary<string, System.Object>,int> action)
	{
		Dictionary<string, System.Object> battleStatus = (Dictionary<string, System.Object>)mutableData.Value;
		string battleState = battleStatus [MyConst.BATTLE_STATUS_STATE].ToString ();
		int battleCount = int.Parse (battleStatus [MyConst.BATTLE_STATUS_COUNT].ToString ());

		if (battleState.Equals (battleStatusName) && battleCount < 2) {

			battleCount++;
			battleStatus [MyConst.BATTLE_STATUS_COUNT] = battleCount.ToString ();
			action (battleStatus, battleCount);
		} 
		Debug.Log ("NEW BATTLE COUNT" + battleCount);
		return battleStatus;
	}


	private void GetLatestKey (int numMod, Action<string> action)
	{
		FirebaseDBFacade.GetTableValueAsync (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS), delegate(DataSnapshot dataSnapshot) {
		

			if (dataSnapshot != null) {
				Dictionary<string, System.Object> battleStatus = (Dictionary<string, System.Object>)dataSnapshot.Value;
				switch (numMod) {
				case 1:
					LatestKeyCompute (battleStatus, 1, 1, action);
					break;
				case 2:
					LatestKeyCompute (battleStatus, 2, 2, action);
					break;
				case 3:
					LatestKeyCompute (battleStatus, 3, 0, action);
					break;
				}
			}

		});

	}

	private void LatestKeyCompute (Dictionary<string, System.Object> battleStatus, int numMod, int numCompare, Action<string> action)
	{
		string latestKey = "";

		if ((float)battleStatus.Count % 3 == numCompare) {
			foreach (KeyValuePair<string , System.Object> battleKey in battleStatus) {
				latestKey = battleKey.Key;
			}
			action (latestKey);
		} else {
			GetLatestKey (numMod, action);
		}
	}

}
