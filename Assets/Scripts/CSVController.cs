using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class CSVController 
{
    private static string filepath = "/SaveFiles/verben.csv";

    public List<Verb> ReadCSV()
    {
        List<Verb> verbList = new List<Verb>();
        
        try{
            string fullPath = Application.dataPath + filepath;
            if (!File.Exists(fullPath))
            {
                Debug.LogError($"File not exists in route: {fullPath}");
                return verbList;
            }

            string[] allLines = File.ReadAllLines(fullPath, System.Text.Encoding.UTF8);
            foreach (string line in allLines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] splittedLine = line.Split(';');
                Verb newVerb = new Verb(splittedLine);
                verbList.Add(newVerb);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error reading CSV file: {ex.Message}");
        }

        return verbList;
    }
}