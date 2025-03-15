using UnityEngine;

public class DungeonMusic : MonoBehaviour
{

    public AudioSource dungeonMusic;
    private bool isMusicPlay;
    void Start()
    {
        dungeonMusic.Play();
        isMusicPlay = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && isMusicPlay)
        {
            dungeonMusic.Stop();
        }
        else
        {
            dungeonMusic.Play();
        }
    }
}
