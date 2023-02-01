using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;

    public Transform meritText;
    public Transform meritTextParent;

    public Text hitAmountText;
    public int hitAmount;

    // Start is called before the first frame update
    void Start()
    {
        hitAmount = PlayerPrefs.GetInt(key: "HitAmount");
        hitAmountText.text = $"Total Merits: {PlayerPrefs.GetInt(key: "HitAmount")}";
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
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("HitAmount", hitAmount);
    }
}
