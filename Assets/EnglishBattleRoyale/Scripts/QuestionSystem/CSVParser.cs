using System.Collections.Generic;
using UnityEngine;
using PapaParse.Net;

public static class CSVParser
{
	
	public static List<List<string>> ParseCSV (string csvName)
	{
		TextAsset csvData = SystemResourceController.Instance.LoadCSV (csvName);
		Result parsed = Papa.parse (csvData.ToString ());
		List<List<string>> rows = parsed.data;
		return rows;
	}
}

