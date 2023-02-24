using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUpdate : MonoBehaviour
{
    //For displaying bomb and heart icons: https://youtu.be/Ay159WsGDJQ
    private RandomSpawner spawnerToGetStats;
    TextMeshProUGUI DisplayLevel;
    [SerializeField]
    private GameObject[] hearts;
    [SerializeField]
    private GameObject[] bombs;
    [SerializeField]

    public static int num_lives;

    //To find the text mesh objects so that the stats can be updated.
    void Start()
    {
        spawnerToGetStats = GameObject.FindObjectOfType<RandomSpawner>();
        DisplayLevel = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    { 
        int num_bombs = spawnerToGetStats.BombsUsed;
        num_lives = spawnerToGetStats.Life;
        if (!OptionsMenu.SpecialGameMode)
        {
            int CurrentLevel = spawnerToGetStats.level;
            DisplayLevel.text = "Level: " + CurrentLevel.ToString();
        }

        if(num_lives == 0)
        {
            Destroy(hearts[num_lives].gameObject);
        }
        else if(num_lives == 1)
        {
            Destroy(hearts[num_lives].gameObject);
        }
        else if(num_lives == 2)
        { 
            Destroy(hearts[num_lives].gameObject);
        }

        if (num_bombs == 0)
        {
            Destroy(bombs[num_bombs].gameObject);
        }
        else if (num_bombs == 1)
        {
            Destroy(bombs[num_bombs].gameObject);
        }
        else if (num_bombs == 2)
        {
            Destroy(bombs[num_bombs].gameObject);
        }

    }
}
