using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject creditPage;
    public void PlayGame()
    {
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene(){
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("Game");
    }

    public void CreditPage(bool open){
        creditPage.SetActive(open);
    }

}
