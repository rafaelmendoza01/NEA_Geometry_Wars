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
    private TextMeshProUGUI ToDisplayTopScore;
    List<int> score = new List<int>();
    List<string> names = new List<string>();

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
    List<ScoreAndUsername> Top3Scores = new List<ScoreAndUsername>();
    

    void Start()
    {
        
    }

  

    private void BubbleSort()
    {
        for (int x = 0; x < Top3Scores.Count; x++)
        {
            for (int y = 0; y < Top3Scores.Count - x; y++)
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
        if (!File.Exists(Filename))
        {
            File.Create(Filename);
            File.AppendAllText(Filename, ToGetLevel.CurrentScore.ToString());
            File.AppendAllText(Filename, Username);
        }
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

        while(i < score.Count){
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
        for(int x = 0; x < Top3Scores.Count; x++)
        {
            StreamWriter sw = new StreamWriter(Filename);
            sw.WriteLine(Top3Scores[x].score);
            sw.WriteLine(Top3Scores[x].Name);
        }

        for (int j = 0; j < Top3Scores.Count; j++)
        {
            if (j == 0)
            {
                ToDisplayTopScore.text = "1. " + Top3Scores[j].Name + " : " + Top3Scores[j].score;
            }
            else
            {
                ToDisplayTopScore.text = "\n" + j + ". " + Top3Scores[j].Name + " : " + Top3Scores[j].score;
            }
        }
    }
}
