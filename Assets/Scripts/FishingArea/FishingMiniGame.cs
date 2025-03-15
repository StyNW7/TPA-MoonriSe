using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FishingMiniGame : MonoBehaviour
{

    public Slider fishBar; // Bar ikan
    public Slider hookBar; // Bar kail
    public Slider progressBar; // Bar progres menangkap ikan
    public float fishMoveSpeed = 2f;
    public float hookMoveSpeed = 3f;
    public float progressDecayRate = 0.2f;

    private bool isFishing = false;
    private float fishTargetPos;

    public NPCInteraction2 npc2;

    public float hookFallSpeed = 1.5f; // Kecepatan turun saat tombol tidak ditekan

    public Button hookButton; // Tombol untuk menaikkan hook
    private bool isHookPressed = false; // Status tombol ditekan
    private float progressTarget = 0f;

    void Start()
    {
        //StartFishingGame();
        hookButton.onClick.AddListener(() => isHookPressed = true);
        hookButton.onClick.AddListener(() => isHookPressed = false);
    }

    void Update()
    {
        if (isFishing)
        {
            // Gerakan ikan secara bertahap ke target posisi baru
            fishBar.value = Mathf.Lerp(fishBar.value, fishTargetPos, Time.deltaTime * fishMoveSpeed);

            // Gerakan hook sesuai input pemain
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                hookBar.value += hookMoveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                hookBar.value -= hookMoveSpeed * Time.deltaTime;

            // Jika tombol ditekan, naikkan hook, jika tidak turun perlahan
            if (isHookPressed)
            {
                hookBar.value += hookMoveSpeed * Time.deltaTime;
            }
            else
            {
                hookBar.value -= hookFallSpeed * Time.deltaTime;
            }

            // // Cek apakah hook sejajar dengan ikan
            // if (Mathf.Abs(fishBar.value - hookBar.value) < 0.1f)
            // {
            //     progressBar.value += Time.deltaTime;
            // }
            // else
            // {
            //     progressBar.value -= progressDecayRate * Time.deltaTime;
            // }

            
            // Cek apakah hook sejajar dengan ikan
            if (Mathf.Abs(fishBar.value - hookBar.value) < 0.1f)
            {
                progressTarget = Mathf.Clamp(progressTarget + Time.deltaTime, 0f, progressBar.maxValue);
            }
            else
            {
                progressTarget = Mathf.Clamp(progressTarget - (progressDecayRate * Time.deltaTime), 0f, progressBar.maxValue);
            }

            // Smooth update progress bar
            progressBar.value = Mathf.Lerp(progressBar.value, progressTarget, Time.deltaTime * 5f);


            // Jika progress penuh, pemain mendapatkan ikan
            if (progressBar.value >= progressBar.maxValue-0.1)
            {
                SuccessFishing();
            }
        }
    }

    public void StartFishingGame()
    {
        isFishing = true;
        progressBar.value = 0;
        hookBar.value = 0;
        StartCoroutine(MoveFishBarRandomly());
    }

    public void SuccessFishing()
    {
        isFishing = false;
        npc2.AddResource("Fish", 1);
        FindObjectOfType<FishingArea>().EndFishing();
        StopCoroutine(MoveFishBarRandomly()); // Hentikan pergerakan ikan
    }

    IEnumerator MoveFishBarRandomly()
    {
        while (isFishing)
        {
            fishTargetPos = Random.Range(0.1f, 0.9f); // Pilih posisi acak baru untuk ikan
            yield return new WaitForSeconds(3f); // Tunggu 3 detik sebelum berpindah lagi
        }
    }
}

// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;

// public class FishingMiniGame : MonoBehaviour
// {

//     public Slider fishBar; // Bar ikan
//     public Slider hookBar; // Bar kail
//     public Slider progressBar; // Bar progres menangkap ikan
//     public float fishMoveSpeed = 2f;
//     public float hookMoveSpeed = 3f;
//     public float progressDecayRate = 0.2f;

//     private bool isFishing = false;
//     private float fishTargetPos;

//     public NPCInteraction2 npc2;

//     public float hookFallSpeed = 1.5f; // Kecepatan turun saat tombol tidak ditekan

//     public Button hookButton; // Tombol untuk menaikkan hook
//     private bool isHookPressed = false; // Status tombol ditekan

//     void Start()
//     {
//         //StartFishingGame();
//         hookButton.onClick.AddListener(() => isHookPressed = true);
//         hookButton.onClick.AddListener(() => isHookPressed = false);
//     }

//     void Update()
//     {
//         if (isFishing)
//         {
//             // Gerakan ikan secara bertahap ke target posisi baru
//             fishBar.value = Mathf.Lerp(fishBar.value, fishTargetPos, Time.deltaTime * fishMoveSpeed);

//             // Gerakan hook sesuai input pemain
//             if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
//                 hookBar.value += hookMoveSpeed * Time.deltaTime;
//             if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
//                 hookBar.value -= hookMoveSpeed * Time.deltaTime;

//             // Jika tombol ditekan, naikkan hook, jika tidak turun perlahan
//             if (isHookPressed)
//             {
//                 hookBar.value += hookMoveSpeed * Time.deltaTime;
//             }
//             else
//             {
//                 hookBar.value -= hookFallSpeed * Time.deltaTime;
//             }

//             // Cek apakah hook sejajar dengan ikan
//             if (Mathf.Abs(fishBar.value - hookBar.value) < 0.1f)
//             {
//                 progressBar.value += Time.deltaTime;
//             }
//             else
//             {
//                 progressBar.value -= progressDecayRate * Time.deltaTime;
//             }

//             // Jika progress penuh, pemain mendapatkan ikan
//             if (progressBar.value >= progressBar.maxValue)
//             {
//                 SuccessFishing();
//             }
//         }
//     }

//     public void StartFishingGame()
//     {
//         isFishing = true;
//         progressBar.value = 0;
//         hookBar.value = 0;
//         StartCoroutine(MoveFishBarRandomly());
//     }

//     public void SuccessFishing()
//     {
//         isFishing = false;
//         npc2.AddResource("Fish", 1);
//         FindObjectOfType<FishingArea>().EndFishing();
//         StopCoroutine(MoveFishBarRandomly()); // Hentikan pergerakan ikan
//     }

//     IEnumerator MoveFishBarRandomly()
//     {
//         while (isFishing)
//         {
//             fishTargetPos = Random.Range(0.1f, 0.9f); // Pilih posisi acak baru untuk ikan
//             yield return new WaitForSeconds(3f); // Tunggu 3 detik sebelum berpindah lagi
//         }
//     }
// }
