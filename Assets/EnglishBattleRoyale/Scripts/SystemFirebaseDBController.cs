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
	private  Dictionary<DataSnapshot, bool> InitialState = new Dictionary<DataSnapshot, bool> ();

	void Start ()
	{
		Init ();
	}

	private void Init ()
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
		roomReference = reference.Child (MyConst.GAMEROOM_ROOM);
	}

	//Do something when receives rpc from facade
	public void OnNotify (DataSnapshot dataSnapShot)
	{
		
		try {
			//TEMPORARY SOLUTION FOR PLAYER DETAILS
			if (dataSnapShot.Key.ToString ().Equals (MyConst.GAMEROOM_HOME)) {
				if (SystemGlobalDataController.Instance.isHost) {
					InitialState.Add (dataSnapShot, true);
				} else {
					InitialState.Add (dataSnapShot, false);
				}


			}
			//TEMPORARY SOLUTION FOR PLAYER DETAILS
			if (dataSnapShot.Key.ToString ().Equals (MyConst.GAMEROOM_VISITOR)) {
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
		gameRoomKey = FirebaseDBFacade.CreateKey (reference.Child (MyConst.GAMEROOM_ROOM));
		RoomCreateJoin (true, MyConst.GAMEROOM_HOME);
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
		FirebaseDBFacade.RemoveTableValueAsync (reference.Child (MyConst.GAMEROOM_ROOM).Child (gameRoomKey));
	}

	public void JoinRoom ()
	{
		FirebaseDBFacade.RunTransaction (reference.Child (MyConst.GAMEROOM_ROOM).Child (gameRoomKey).Child (MyConst.GAMEROOM_STATUS), delegate(MutableData mutableData) {
			int playerCount = int.Parse (mutableData.Value.ToString ());
		
			playerCount++;
			joinCounter++;

			mutableData.Value = playerCount.ToString ();
		});
			
		StartCoroutine (StartJoinDelay ());
	}

	IEnumerator StartJoinDelay ()
	{
		yield return new WaitForSeconds (2);
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

		Dictionary<string, System.Object> result = new Dictionary<string, System.Object> ();
		result [MyConst.RPC_DATA_PLAYER] = JsonUtility.ToJson (SystemGlobalDataController.Instance.player);
		Dictionary<string, System.Object> entryValues = result;

		string directory = MyConst.GAMEROOM_ROOM + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_INITITAL_STATE + "/" + userPlace + "/" + MyConst.RPC_DATA_PARAM + "/";
		FirebaseDBFacade.CreateTableChildrenAsync (directory, reference, entryValues);

		//set room status to open when create room
		if (isHost) {
			FirebaseDBFacade.SetTableValueAsync (reference.Child (MyConst.GAMEROOM_ROOM).Child (gameRoomKey).Child (MyConst.GAMEROOM_STATUS), "0");
		}
		//set battle status to answer when start of game
		CheckInitialPhase ();

		InitialStateListener ();
	}

	private void RPCListener ()
	{
		if (gameRoomKey != null) {
			FirebaseDBFacade.CreateTableChildAddedListener ("RPCListener", reference.Child (MyConst.GAMEROOM_ROOM).Child (gameRoomKey).Child (MyConst.GAMEROOM_RPC));
		}
	}

	private void CheckInitialPhase ()
	{
		FirebaseDBFacade.GetTableValueAsync (reference.Child (MyConst.GAMEROOM_ROOM).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS), delegate(DataSnapshot dataSnapshot) {


			if (dataSnapshot.Value == null) {
				UpdateBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0, "0", "0");
				
			} else {
				Dictionary<string, System.Object> battleStatus = (Dictionary<string, System.Object>)dataSnapshot.Value;

				foreach (KeyValuePair<string , System.Object> battleKey in battleStatus) {
					battleStatusKey = battleKey.Key;
				}
			}

			FirebaseDBFacade.CreateTableChildAddedListener ("BattleStatusChildAdded", reference.Child (MyConst.GAMEROOM_ROOM).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS));
			FirebaseDBFacade.CreateTableValueChangedListener ("BattleStatusValueChanged", reference.Child (MyConst.GAMEROOM_ROOM).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS));
		});

	}

	private void InitialStateListener ()
	{
		if (gameRoomKey != null) {
			FirebaseDBFacade.CreateTableChildAddedListener ("InitialStateListener", reference.Child (MyConst.GAMEROOM_ROOM).Child (gameRoomKey).Child (MyConst.GAMEROOM_INITITAL_STATE));
		}
	}

	public void UpdateBattleStatus (string stateName, int stateCount, string playerParam = "", string enemyParam = "")
	{
		Dictionary<string, System.Object> entryValues = new Dictionary<string, System.Object> ();
		entryValues.Add (MyConst.BATTLE_STATUS_STATE, stateName);
		entryValues.Add (MyConst.BATTLE_STATUS_COUNT, stateCount.ToString ());
		if (!string.IsNullOrEmpty (playerParam)) {
			entryValues.Add (MyConst.RPC_DATA_PLAYER_ANSWER_PARAM, playerParam);
		}

		if (!string.IsNullOrEmpty (enemyParam)) {
			entryValues.Add (MyConst.RPC_DATA_ENEMY_ANSWER_PARAM, enemyParam);
		}

		battleStatusKey = reference.Child (MyConst.GAMEROOM_ROOM).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).Push ().Key;
		string directory = "/" + MyConst.GAMEROOM_ROOM + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_BATTLE_STATUS + "/" + battleStatusKey + "/";
		FirebaseDBFacade.CreateTableChildrenAsync (directory, reference, entryValues);
	}

	public void SetParam<T> (string objectName, T myObject)
	{
		string	rpcKey = reference.Child (MyConst.GAMEROOM_ROOM).Child (gameRoomKey).Child (MyConst.GAMEROOM_RPC).Push ().Key;

		Dictionary<string, System.Object> result = new Dictionary<string, System.Object> ();
		result [MyConst.RPC_DATA_USERHOME] = SystemGlobalDataController.Instance.isHost;

		Dictionary<string, System.Object> jsonResult = new Dictionary<string, System.Object> ();
		jsonResult [objectName] = JsonUtility.ToJson (myObject);
		result [MyConst.RPC_DATA_PARAM] = jsonResult;

		string directory = "/" + MyConst.GAMEROOM_ROOM + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_RPC + "/" + rpcKey;
		FirebaseDBFacade.CreateTableChildrenAsync (directory, reference, result);
	}

	public void AnswerPhase (string param)
	{
		SystemLoadScreenController.Instance.StartWaitOpponentScreen ();
		GetLatestKey (1, delegate(string resultString) {
			FirebaseDBFacade.RunTransaction (reference.Child (MyConst.GAMEROOM_ROOM).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).Child (resultString), delegate(MutableData mutableData) {

				mutableData.Value = PhaseMutate (mutableData, MyConst.BATTLE_STATUS_ANSWER, delegate(Dictionary<string, System.Object> battleStatus, int battleCount) {
					if (SystemGlobalDataController.Instance.isHost) {
						battleStatus [MyConst.RPC_DATA_PLAYER_ANSWER_PARAM] = param;
					} else {
						battleStatus [MyConst.RPC_DATA_ENEMY_ANSWER_PARAM] = param;
					}
					//Reminders: change to 2 if not testing
					if (battleCount == 2) {
						UpdateBattleStatus (MyConst.BATTLE_STATUS_ATTACK, 0);
					}
				});
			});
		});
	}
		
	public void AttackPhase (AttackModel param)
	{
		SystemLoadScreenController.Instance.StartWaitOpponentScreen ();
		GetLatestKey (2, delegate(string resultString) {
			FirebaseDBFacade.RunTransaction (reference.Child (MyConst.GAMEROOM_ROOM).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).Child (resultString), delegate(MutableData mutableData) {
				mutableData.Value = PhaseMutate (mutableData, MyConst.BATTLE_STATUS_ATTACK, delegate(Dictionary<string, System.Object> battleStatus, int battleCount) {
					SetParam (MyConst.RPC_DATA_ATTACK, (param));
				});
			});
		});
	}

	//Phasemutate uses transaction to update values in the table and increments battlecount
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
		return battleStatus;
	}

	//gets the latest push key from the database
	private void GetLatestKey (int numMod, Action<string> action)
	{
		FirebaseDBFacade.GetTableValueAsync (reference.Child (MyConst.GAMEROOM_ROOM).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS), delegate(DataSnapshot dataSnapshot) {
		

			if (dataSnapshot != null) {
				Dictionary<string, System.Object> battleStatus = (Dictionary<string, System.Object>)dataSnapshot.Value;
				switch (numMod) {
				case 1:
					LatestKeyCompute (battleStatus, 1, 1, action);
					break;
				case 2:
					LatestKeyCompute (battleStatus, 2, 0, action);
					break;
				}
			}

		});

	}

	private void LatestKeyCompute (Dictionary<string, System.Object> battleStatus, int numMod, int numCompare, Action<string> action)
	{
		string latestKey = "";

		if ((float)battleStatus.Count % 2 == numCompare) {
			foreach (KeyValuePair<string , System.Object> battleKey in battleStatus) {
				latestKey = battleKey.Key;
			}
			action (latestKey);
		} else {
			GetLatestKey (numMod, action);
		}
	}

}
