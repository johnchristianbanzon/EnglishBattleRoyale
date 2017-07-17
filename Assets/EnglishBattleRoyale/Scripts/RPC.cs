using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class RPCLogic {

	public static void ReceiveDBConnection(bool isConnectedDB){
		RPCBoolObserver.Notify (isConnectedDB);
	}

	public static void ReceiveRPC(Firebase.Database.DataSnapshot dataSnapShot){
		RPCDicObserver.Notify (dataSnapShot);
	}

	public static void ReceiveRPCQuery(Firebase.Database.DataSnapshot dataSnapShot){
		RPCQueryObserver.NotifyQuery (dataSnapShot);
	}

}
