using System.Collections.Generic;
public static class ListShuffleUtility{

	public static List<T> Shuffle<T>(List<T> list){
		list.Shuffle ();
		return list;
	}


	//Fisher-Yates shuffle
	private static System.Random rng = new System.Random();  
	private static void Shuffle<T>(this IList<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}
}
