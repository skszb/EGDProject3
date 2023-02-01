using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitWoodenFish()
    {
        audioSource.PlayOneShot(audioClip);

        Transform mt = Instantiate(meritText, meritTextParent);
        mt.gameObject.SetActive(true);

        hitAmountText.text = $"Total Merits: {++hitAmount}";

        Debug.Log(_randomTextCoroutine);
        if (_randomTextCoroutine == null)
        {
            _randomTextCoroutine = StartCoroutine(RandomText());
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
