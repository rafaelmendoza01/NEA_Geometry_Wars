using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class DisplayTopUserAndScores : MonoBehaviour
{
    private string Filename = "GW_Scores.txt";
    private RandomSpawner ToGetLevel;
    [SerializeField]
    private TextMeshProUGUI ToDisplayScore;

    void Start()
    {
        if (File.Exists(Filename))
        {
            File.AppendAllText(Filename, ToGetLevel.CurrentScore.ToString());
        }
        else
        {
            File.WriteAllText(Filename, ToGetLevel.CurrentScore.ToString());
        }
    }



    public void GetTheirUsername(string Username)
    {
        File.WriteAllText(Filename, Username);
        StreamReader ReadScoresAndUsername = new StreamReader(Filename);
        string line;
        while((line = ReadScoresAndUsername.ReadLine()) != null)
        {

        }
        
    }
}
