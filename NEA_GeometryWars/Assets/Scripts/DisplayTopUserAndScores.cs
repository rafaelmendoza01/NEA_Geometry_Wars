using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class DisplayTopUserAndScores : MonoBehaviour
{
    private string Filename = "GW_Scores.txt";
    
    [SerializeField]
    private TextMeshProUGUI Top1;
    [SerializeField]
    private TextMeshProUGUI Top2;
    [SerializeField]
    private TextMeshProUGUI Top3;

    List<int> score = new List<int>();
    List<string> names = new List<string>();

    public struct ScoreAndUsername
    {
        public string Name;
        public int score;
        public ScoreAndUsername(string TheirName, int Score)
        {
            Name = TheirName;
            score = Score;
        }
    }
    List<ScoreAndUsername> Top3Scores = new List<ScoreAndUsername>();

    private void BubbleSort()
    {
        for (int x = 0; x < Top3Scores.Count - 1; x++)
        {
            for (int y = 0; y < Top3Scores.Count - 1 - x; y++)
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
        bool justCreated = false;

        //create file to store score and username
        if (!File.Exists(Filename))
        {
            File.Create(Filename);
            File.AppendAllText(Filename, ScoreTracker.Score.ToString());
            File.AppendAllText(Filename, Username);
            justCreated = true;
        }
        StreamReader ReadScoresAndUsername = new StreamReader(Filename);
        string line;
        int i = 0;
        
        //transfer all info from the file to the lists
        while((line = ReadScoresAndUsername.ReadLine()) != null && i < 3)
        {
            if(int.TryParse(line, out int TheirScore))
            {
                score.Add(TheirScore);
            }
            else
            {
                names.Add(line);
                i++;
            }
        }
        ReadScoresAndUsername.Close();

        i = 0;
        while(i < score.Count){
            Top3Scores.Add(new ScoreAndUsername(names[i], score[i]));
            i++;
        }

        if (Top3Scores.Count > 1)
        {
            BubbleSort();

            if (ScoreTracker.Score > Top3Scores[0].score && Top3Scores.Count < 3)
            {
                Top3Scores.Add(new ScoreAndUsername(Username, ScoreTracker.Score));
            } 
            else if(ScoreTracker.Score < Top3Scores[0].score && Top3Scores.Count < 3)
            {
                Top3Scores.Add(new ScoreAndUsername(Username, ScoreTracker.Score));
            }
            else
            {
                Top3Scores[0] = new ScoreAndUsername(Username, ScoreTracker.Score);
            }
            BubbleSort();
        }
        else if(Top3Scores.Count == 1)
        {
            Top3Scores.Add(new ScoreAndUsername(Username, ScoreTracker.Score));
            BubbleSort();
        }
        else
        {
            Top3Scores.Add(new ScoreAndUsername(Username, ScoreTracker.Score));
        }

        File.Delete(Filename);
        using (StreamWriter sw = new StreamWriter(Filename))
        {
            for (int x = 0; x < Top3Scores.Count; x++)
            {
                sw.WriteLine(Top3Scores[x].score);
                sw.WriteLine(Top3Scores[x].Name);
            }
            sw.Close();
        }
        

        for (int j = 0; j < Top3Scores.Count; j++)
        {
            if (j == 0)
            {
                Top1.text = "1. " + Top3Scores[j].Name + " : " + Top3Scores[j].score;
            }
            else if(j == 1)
            {
                Top2.text = "2. " + Top3Scores[j].Name + " : " + Top3Scores[j].score;
            }
            else
            {
                Top3.text = "3. " + Top3Scores[j].Name + " : " + Top3Scores[j].score;
            }
        }
    }
}
