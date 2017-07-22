using System.Collections.Generic;
using UnityEngine;
using PapaParse.Net;

public static class CSVParser
{
	public static List<Dictionary<string,System.Object>> ParseCSV (string csvName)
	{
		int csvHeaderLines = 1;
		TextAsset csvData = SystemResourceController.Instance.LoadCSV (csvName);
		Result parsed = Papa.parse (csvData.ToString ());
		List<List<string>> rows = parsed.data;
		List<string> csvHeader = new List<string> ();
		List<Dictionary<string,System.Object>> csvParsedData = new List<Dictionary<string,System.Object>> ();
		int csvLineIndex = 0;

		for (int listIndex = 0; listIndex < rows.Count; listIndex++) {
			csvParsedData.Add (new Dictionary<string,object> ());
			for (int subListIndex = 0; subListIndex < rows [listIndex].Count; subListIndex++) {
				if (listIndex < csvHeaderLines) {
					csvHeader.Add (rows [listIndex] [subListIndex]);
				} else {
					//NON HEADER BELOW 
					csvParsedData [csvLineIndex].Add (csvHeader [subListIndex], rows [listIndex] [subListIndex]);
					if (subListIndex.Equals (rows [listIndex].Count - 1)) {
						csvLineIndex += 1;
					}
				}
			}
		}
		return csvParsedData;
	}
}