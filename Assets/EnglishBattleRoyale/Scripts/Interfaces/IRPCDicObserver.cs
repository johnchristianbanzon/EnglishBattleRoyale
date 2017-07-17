using System.Collections.Generic;
public interface IRPCDicObserver
{
	void OnNotify (Firebase.Database.DataSnapshot dataSnapShot);
}

