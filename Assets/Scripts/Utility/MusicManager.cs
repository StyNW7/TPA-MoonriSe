using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public AudioSource peacefulMusic;
    public AudioSource battleMusic;
    [SerializeField] bool isInCombat = false;
    public PlayerZombieDetector zombieDetector;
    void Update()
    {
        if (isInCombat)
        {
            if (!battleMusic.isPlaying)
            {
                Debug.Log("Battle Music");
                peacefulMusic.Stop();
                battleMusic.Play();
            }
        }
        else
        {
            if (!peacefulMusic.isPlaying)
            {
                Debug.Log("Peaceful Music");
                battleMusic.Stop();
                peacefulMusic.Play();
            }
        }
       
        if (zombieDetector.IsZombieNearby())
        {
            SetCombat(true);
        }
        else SetCombat(false);
    }

    public void SetCombat(bool state)
    {
        isInCombat = state;
    }

}
