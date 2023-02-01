using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Audios")]
    public int beatCount = 4;
    int currentBeat = 0;
    bool enabled = true;
    float interval = 0;
    public TextMeshProUGUI bpmText;

    [Header("WoddenFish")]
    public AudioClip WoodenFishClip;

    [Header("CowBell")]
    public AudioClip cowBellClip;
    [Range(0.0f, 100.0f)]
    public float chanceCowBell = 10.0f;

    [Header("Snare 808")]
    public AudioClip snareClip;
    [Range(0.0f, 100.0f)]
    public float chanceSnare = 10.0f;


    [Header("Clap 808")]
    public AudioClip clapClip;
    [Range(0.0f, 100.0f)]
    public float chanceClap = 10.0f;


    [Header("Bell")]
    public AudioClip bellClip;
    [Range(0.0f, 100.0f)]
    public float chanceBell = 5.0f;

    [Space]



    public Transform meritText;
    public Transform meritTextParent;

    public Text hitAmountText;
    public int hitAmount;

    // For random text
    public string[] randomTexts;
    public float[] randomTextsTime;
    private int randomTextLength;
    [SerializeField] public GameObject randomText;
    private Coroutine _randomTextCoroutine = null;
    
    // Start is called before the first frame update
    void Start()
    {
        hitAmount = PlayerPrefs.GetInt(key: "HitAmount");
        hitAmountText.text = $"Total Merits: {PlayerPrefs.GetInt(key: "HitAmount")}";

        if(randomTexts.Length != randomTextsTime.Length){
            Debug.LogError("Random Texts and Random Texts Time are not the same length!");
            randomTextLength = Mathf.Min(randomTexts.Length, randomTextsTime.Length);
        }
        else {
            randomTextLength = randomTexts.Length;
        }

        audioSource.clip = WoodenFishClip;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        interval += Time.deltaTime;
    }

    public void HitWoodenFish()
    {

        if (enabled)
        {

            if (currentBeat >= beatCount)
            {
                float rand = Random.Range(0.0f, 100.0f);
                if (rand < chanceBell)
                {
                    audioSource.clip = bellClip;
                    audioSource.Play();
                    enabled= false;
                    bpmText.SetText("0");
                    return;
                }
                else if (rand < chanceBell + chanceCowBell)
                {
                    audioSource.clip = cowBellClip;
                }
                else if (rand < chanceBell + chanceCowBell + chanceSnare)
                {
                    audioSource.clip = snareClip;
                }
                else if (rand < chanceBell + chanceCowBell + chanceSnare + chanceClap)
                {
                    audioSource.clip = clapClip;
                }

                else
                {
                    audioSource.clip = WoodenFishClip;
                }
                currentBeat = 0;
            }


            audioSource.Play();

            float bpm = 60 / interval;

            bpmText.SetText(Mathf.Round(bpm).ToString());

            interval = 0.0f;

            currentBeat++;

            Transform mt = Instantiate(meritText, meritTextParent);
            mt.gameObject.SetActive(true);

            hitAmountText.text = $"Total Merits: {++hitAmount}";

            if (_randomTextCoroutine == null)
            {
                _randomTextCoroutine = StartCoroutine(RandomText());
            }

        }
        if (!enabled && !audioSource.isPlaying)
        {
            enabled = true;
            currentBeat = beatCount;
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("HitAmount", hitAmount);
    }

    IEnumerator RandomText(){
        // pick a random text and time from the arrays
        int randomTextIndex = Random.Range(0, (randomTextLength-1)*10);

        if(randomTextIndex >= randomTextLength){
            yield break;
        }
        
        randomText.SetActive(true);
        randomText.GetComponentInChildren<TMP_Text>().text = randomTexts[randomTextIndex];
        yield return new WaitForSeconds(randomTextsTime[randomTextIndex]);
        randomText.SetActive(false);
        _randomTextCoroutine = null;
    }
}
