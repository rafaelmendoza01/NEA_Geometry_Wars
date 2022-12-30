using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUpdate : MonoBehaviour
{
    private RandomSpawner spawnerToGetStats;
    TextMeshProUGUI DisplayLevel;


    //To find the text mesh objects so that the stats can be updated.
    void Start()
    {
        spawnerToGetStats = GameObject.FindObjectOfType<RandomSpawner>();
        DisplayLevel = GameObject.FindObjectOfType<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        int CurrentLevel = spawnerToGetStats.level;
        int Lives = spawnerToGetStats.Life;
        DisplayLevel.text = "Level: " + CurrentLevel.ToString() + "\n" + "Lives: " + Lives.ToString();
        
        
    }
}
