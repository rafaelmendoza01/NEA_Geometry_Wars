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

    bool StartedReadingFile = false;

    //to display any intial top scorers if the file already exists
    private void Start()
    {
        if (File.Exists(Filename))
        {
            ReadScoresFromFile();
            int i = 0;
            while (i < score.Count)
            {
                Top3Scores.Add(new ScoreAndUsername(names[i], score[i]));
                i++;
            }
            ReverseList();
            ShowTopScorers();       
            StartedReadingFile = true;
        }
    }

    //custom data structure to hold both the name of a user and their corresponding score
    //becomes important for displaying the name of a top user and corresponding score
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


    //to sort the list of top scorers from lowest to highest
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

    //to reverse the list of top scorers (highest to lowest)
    private void ReverseList()
    {
        int LastIndex = Top3Scores.Count - 1;
        if (LastIndex > 1)
        {
            for (int i = 0; i < LastIndex / 2; i++)
            {
                ScoreAndUsername Temp = Top3Scores[i];
                Top3Scores[i] = Top3Scores[LastIndex - i];
                Top3Scores[LastIndex - i] = Temp;
            }
        }
        else
        {
            ScoreAndUsername Temp = Top3Scores[LastIndex];
            Top3Scores[LastIndex] = Top3Scores[0];
            Top3Scores[0] = Temp;
        }
    }

    private void ShowTopScorers()
    {
        for (int j = 0; j < Top3Scores.Count; j++)
        {
            if (j == 0)
            {
                Top1.text = "1. " + Top3Scores[j].Name + " : " + Top3Scores[j].score;
            }
            else if (j == 1)
            {
                Top2.text = "2. " + Top3Scores[j].Name + " : " + Top3Scores[j].score;
            }
            else
            {
                Top3.text = "3. " + Top3Scores[j].Name + " : " + Top3Scores[j].score;
            }
        }
    }

    //to read the text files that contain usernames and scores
    private void ReadScoresFromFile()
    {
        StreamReader ReadScoresAndUsername = new StreamReader(Filename);
        string line;
        int i = 0;

        //transfer all info from the file to the lists
        while ((line = ReadScoresAndUsername.ReadLine()) != null && i < 3)
        {
            if (int.TryParse(line, out int TheirScore))
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
       
    }

    public void GetTheirUsername(string Username)
    {
        bool justCreated = false;

        //create file to store score and username
        if (!File.Exists(Filename))
        {
            using (StreamWriter sw = new StreamWriter(Filename))
            {
                sw.WriteLine(ScoreTracker.Score.ToString());
                sw.WriteLine(Username);
                justCreated = true;
                sw.Close();
            }
        }
        if (!StartedReadingFile && !justCreated)
        {
            ReadScoresFromFile();
            int i = 0;
            while (i < score.Count)
            {
                Top3Scores.Add(new ScoreAndUsername(names[i], score[i]));
                i++;
            }
        }

        if (Top3Scores.Count >= 1)
        {
            BubbleSort();

            if (Top3Scores.Count < 3)
            {
                Top3Scores.Add(new ScoreAndUsername(Username, ScoreTracker.Score));
            } 
            else
            {
                if (Top3Scores[0].score < ScoreTracker.Score)
                {
                    Top3Scores[0] = new ScoreAndUsername(Username, ScoreTracker.Score);
                }
            }
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

        ReverseList();
        ShowTopScorers();
    }
}
