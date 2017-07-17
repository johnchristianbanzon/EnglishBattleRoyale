using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Invokes the notificaton method
public static class RPCDicObserver
{
	//A list with observers that are waiting for something to happen
	static List<IRPCDicObserver> observers = new List<IRPCDicObserver> ();

	//Send notifications if something has happened
	public static void Notify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		for (int i = 0; i < observers.Count; i++) {
			//Notify all observers even though some may not be interested in what has happened
			//Each observer should check if it is interested in this event
			observers [i].OnNotify (dataSnapShot);
		}
	}

	//Add observer to the list
	public static void AddObserver (IRPCDicObserver observer)
	{
		observers.Add (observer);
	}

	//Remove observer from the list
	public static void RemoveObserver (IRPCDicObserver observer)
	{
		observers.Remove (observer);
	}
}
