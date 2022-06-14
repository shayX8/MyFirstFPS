using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    static AudioClip shoot, jump, flip, win, swich, enemyDead;
    static AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        shoot = Resources.Load<AudioClip>("shoot");
        jump = Resources.Load<AudioClip>("jump");
        flip = Resources.Load<AudioClip>("flip");
        win = Resources.Load<AudioClip>("win");
        swich = Resources.Load<AudioClip>("swich");
        enemyDead = Resources.Load<AudioClip>("enemyDead");

        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(string name)
    {
        if (name == "shoot")
        {
            audioSource.PlayOneShot(shoot);
        }
        else if (name == "jump")
        {
            audioSource.PlayOneShot(jump);
        }
        else if (name == "win")
        {
            audioSource.PlayOneShot(win);
        }
        else if (name == "flip")
        {
            audioSource.PlayOneShot(flip);
        }
        else if (name == "swich")
        {
            audioSource.PlayOneShot(swich);
        }
        else if (name == "enemyDead")
        {
            audioSource.PlayOneShot(enemyDead);
        }
    }
}
