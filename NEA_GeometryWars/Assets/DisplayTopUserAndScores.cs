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
    protected int[] score = new int[3];
    protected string[] names = new string[3];

    public struct ScoreAndUsername
    {
        public string Name;
        public int score;
        public ScoreAndUsername(string Username, int Score)
        {
            Name = Username;
            score = Score;
        }
    }
    ScoreAndUsername[] Top3Scores = new ScoreAndUsername[3];
    

    void Start()
    {
        if (!File.Exists(Filename))
        {
            File.Create(Filename);
        }
    }

    private void BubbleSort()
    {
        for (int x = 0; x < Top3Scores.Length; x++)
        {
            for (int y = 0; y < Top3Scores.Length - x; y++)
            {
                if (Top3Scores[y].score > Top3Scores[y+1].score)
                {
                    ScoreAndUsername Temp = Top3Scores[y];
                    Top3Scores[y] = Top3Scores[y + 1];
                    Top3Scores[y + 1] = Temp;
                }
            }
        }
    }

    public void GetTheirUsername(string Username)
    {
        StreamReader ReadScoresAndUsername = new StreamReader(Filename);
        string line;
        int i = 0;
        while((line = ReadScoresAndUsername.ReadLine()) != null && i < 3)
        {
            if(int.TryParse(line, out int TheirScore))
            {
                score[i] = TheirScore;
            }
            else
            {
               names[i] = line;
                i++;
            }
        }

        while(i < score.Length){
            Top3Scores[i] = new ScoreAndUsername(names[i], score[i]);
            i++;
        }

        BubbleSort();

        if(ToGetLevel.CurrentScore > Top3Scores[0].score)
        {
            Top3Scores[0] = new ScoreAndUsername(Username, ToGetLevel.CurrentScore);
        }

        BubbleSort();

        File.Create(Filename);
        for(int x = 0; x < Top3Scores.Length; x++)
        {
            StreamWriter sw = new StreamWriter(Filename);
            sw.WriteLine(Top3Scores[x].score);
            sw.WriteLine(Top3Scores[x].Name);
        }
    }
}
