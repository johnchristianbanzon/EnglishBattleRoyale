using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Invokes the notificaton method
public static class RPCQueryObserver
{
	//A list with observers that are waiting for something to happen
	static List<IRPCQueryObserver> observers = new List<IRPCQueryObserver> ();

	//Send notifications if something has happened
	public static void NotifyQuery (Firebase.Database.DataSnapshot dataSnapshot)
	{
		for (int i = 0; i < observers.Count; i++) {
			//Notify all observers even though some may not be interested in what has happened
			//Each observer should check if it is interested in this event
			observers [i].OnNotifyQuery (dataSnapshot);
		}
	}

	//Add observer to the list
	public static void AddObserver (IRPCQueryObserver observer)
	{
		observers.Add (observer);
	}

	//Remove observer from the list
	public static void RemoveObserver (IRPCQueryObserver observer)
	{
		observers.Remove (observer);
	}
}
