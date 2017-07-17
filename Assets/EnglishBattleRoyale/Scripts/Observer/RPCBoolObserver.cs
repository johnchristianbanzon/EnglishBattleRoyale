using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Invokes the notificaton method
public static class RPCBoolObserver
{
	//A list with observers that are waiting for something to happen
	static List<IRPCBoolObserver> observers = new List<IRPCBoolObserver> ();

	//Send notifications if something has happened
	public static void Notify (bool boolValue)
	{
		for (int i = 0; i < observers.Count; i++) {
			//Notify all observers even though some may not be interested in what has happened
			//Each observer should check if it is interested in this event
			observers [i].OnNotify (boolValue);
		}
	}

	//Add observer to the list
	public static void AddObserver (IRPCBoolObserver observer)
	{
		observers.Add (observer);
	}

	//Remove observer from the list
	public static void RemoveObserver (IRPCBoolObserver observer)
	{
		observers.Remove (observer);
	}
}
