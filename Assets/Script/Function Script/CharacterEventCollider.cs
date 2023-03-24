using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class CharacterEventCollider : MonoBehaviour
{

    public GameObject indicatorWrapper;
    public GameObject informationLocation;
    Text desc;
    
    private float countDown = 3f;
    private float countDownNonActive = 3f;

    void Start()
    {
        this.desc = indicatorWrapper.transform.GetChild(0).gameObject.GetComponent<Text>() as Text;
    }

    private void OnTriggerEnter(Collider other)
    {
        AbstractPlayerData.userSession.setUserEnterLogTimeRadius(other.name);
    }

    private void OnTriggerStay(Collider other)
    {
        
        if(this.desc.isActiveAndEnabled == false){
            this.indicatorWrapper.SetActive(true);
        }
        
        if(this.countDown < 1f){
            this.desc.text = "Battle Begin!";
            StartCoroutine(MoveBattleScene(other.name));
        }
        else if(this.countDown < 4f) {
            this.desc.text = (Mathf.Floor(countDown)).ToString();
            this.countDown -= Time.deltaTime;
        }
        else if(this.countDown < 6f) {
            this.desc.text = "Hantu ditemukan!";
            this.countDown -= Time.deltaTime;
        }
        else {
            this.desc.text= "Mencari!";
            this.countDown -= Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        AbstractPlayerData.userSession.setUserExitLogTimeRadius();
        StartCoroutine(ShowTextTimer("Anda keluar!", 2.5f));
    }

    IEnumerator MoveBattleScene(string name){
        PlayerPrefs.SetString("location", name);
        yield return new WaitForSecondsRealtime(1f);

        LoadingData.sceneToLoad = "InGameBattle";
        SceneManager.LoadScene("Loading");
    }


    IEnumerator ShowTextTimer(string kalimat, float timer){
        this.desc.text = kalimat;
        this.countDown = 8f;

        yield return new WaitForSecondsRealtime(timer);

        this.indicatorWrapper.SetActive(false);
    }
}
