using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    //https://youtu.be/6OT43pvUyfY - primarily used for helping me create the sound/music
    public AudioSource Music1;
    public AudioSource Music2;
    public AudioSource Music3;

    static public bool Music1Wanted = false;
    static public bool Music2Wanted = false;
    static public bool Music3Wanted = false;

    static public bool KeyBoardToShoot = false;
    static public bool MouseToShoot = true;

    static public bool SpecialGameMode = false;

    public void Music1Status()
    {
        Music2Wanted = false;
        Music3Wanted = false;
        Music1Wanted = true;
        Music2.Stop();
        Music3.Stop();
        Music1.Play();
    }

    public void Music2Status()
    {
        Music2Wanted = true;
        Music3Wanted = false;
        Music1Wanted = false;
        Music3.Stop();
        Music1.Stop();
        Music2.Play();

    }
    public void Music3Status()
    {
        Music2Wanted = false;
        Music3Wanted = true;
        Music1Wanted = false;
        Music1.Stop();
        Music2.Stop();
        Music3.Play();
    }

    public void KeyBoardShooting()
    {
        KeyBoardToShoot= true;
        MouseToShoot = false;
    }

    public void MouseShooting()
    {
        KeyBoardToShoot = false;
        MouseToShoot = true;
    }

    public void SpecialMode()
    {
        SpecialGameMode = true;
    }
}
