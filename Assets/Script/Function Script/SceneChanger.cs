using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using static PlayerModel;
using static QuestModel;
using System.IO;

public class SceneChanger : MonoBehaviour
{
    public TextAsset textJsonPlayer;
    public TextAsset textJsonQuest;
    public GameObject notification;
    public GameObject timerObj;

    public void InGameScene(){
        
        Instantiate(timerObj); //Baru ini

        AbstractPlayerData.playermodel = new PlayerModel(textJsonPlayer);
        AbstractPlayerData.player = AbstractPlayerData.playermodel.ReaderData();
        AbstractPlayerData.questmodel = new QuestModel(textJsonQuest);

        AbstractPlayerData.player.Name = AbstractPlayerData.username;
        
        if (AbstractPlayerData.mainQuest != null && AbstractPlayerData.items != null && AbstractPlayerData.skillicons != null && AbstractPlayerData.bursticons != null)
        {
            AbstractPlayerData.mainQuest.Clear();
            AbstractPlayerData.sideQuest.Clear();
            AbstractPlayerData.items.Clear();
            AbstractPlayerData.skillicons.Clear();
            AbstractPlayerData.bursticons.Clear();
        }
        AbstractPlayerData.mainQuest = AbstractPlayerData.questmodel.ReaderData();
        AbstractPlayerData.sideQuest = AbstractPlayerData.questmodel.ReaderDataSQ();     
        AbstractPlayerData.checkpoint.serializedplayer = JsonConvert.SerializeObject(AbstractPlayerData.playermodel.GetRoot());//AbstractPlayerData.playermodel.ReaderDataRoot()
        AbstractPlayerData.checkpoint.serializedquest = JsonConvert.SerializeObject(AbstractPlayerData.questmodel.ReaderDataRoot());
        AbstractPlayerData.checkpoint.SaveDataCheckpoint();
        LoadingData.sceneToLoad = "InGame";
        SceneManager.LoadScene("Loading");
        
    }

    public void Continue(){
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation) && Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            string PathPlayerData = Application.persistentDataPath + "/DataCheckpointPlayer.txt";
            string PathQuestData = Application.persistentDataPath + "/DataCheckpointQuest.txt";
            if(File.Exists(PathQuestData) && File.Exists(PathQuestData))
            {
                Instantiate(timerObj);

                CheckpointData checkpoint = new CheckpointData();
                checkpoint.LoadDataCheckpoint();
                AbstractPlayerData.userSession.setContinueUserData();
                LoadingData.sceneToLoad = "InGame";
                SceneManager.LoadScene("Loading");
            }
            else 
            {
                StartCoroutine(Notification("Save file checkpoint tidak ada!"));
            }
        }
        else 
        {
            StartCoroutine(Notification("Tidak bisa memainkan game"));
        }
        
    }

    public void exitGame(){
        Application.Quit();
    }

    public void locationScene() {
        SceneManager.LoadScene("MiniMap");
    }

    public void mainmenuScene() {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitMainMenu() {
        if(GameObject.Find("Timer(Clone)") != null)
        {
            AbstractPlayerData.userSession.setFinalTotalTimeLogUser();
        }
        LoadingData.sceneToLoad = "MainMenu";
        SceneManager.LoadScene("Loading");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitDestroyCheckpoint(){
        if(GameObject.Find("Timer(Clone)") != null)
        {
            AbstractPlayerData.userSession.setFinalTotalTimeLogUser();
        }
        AbstractPlayerData.checkpoint.DeleteDataCheckpoint();
        LoadingData.sceneToLoad = "MainMenu";
        SceneManager.LoadScene("Loading");
    }

    public void StoryScene()
    {
        LoadingData.sceneToLoad = "Story";
        SceneManager.LoadScene("Loading");
    }

    public void CreditScene()
    {
        LoadingData.sceneToLoad = "Credits";
        SceneManager.LoadScene("Loading");
    }

    public void SignUpScene()
    {
        SceneManager.LoadScene("SignUp");
    }

    IEnumerator Notification(string str)
    {
        notification.SetActive(true);
        notification.transform.GetChild(0).gameObject.GetComponent<Text>().text = str;
        yield return new WaitForSecondsRealtime(3f);
        notification.SetActive(false);
    }
}
