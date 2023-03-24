using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.Android;

public class UserRegis : MonoBehaviour
{
    [SerializeField] private string _userDataPath = "users/";
    [SerializeField] private InputField _inputUserName;
    [SerializeField] private InputField _inputDeviceName;
    [SerializeField] private GameObject notifObject;

    private bool isUsedName = true;
    private bool isRegis = false;
    private bool isPermit = false;
    public void uploadUserData()
    {
        UserData userData = new UserData{
            userName = _inputUserName.text,
            deviceName = _inputDeviceName.text
        };
        var firestore = FirebaseFirestore.DefaultInstance;

        if(userData.userName.Length < 1 || userData.deviceName.Length < 1)
        {
            StartCoroutine(notif("Input tidak boleh kosong!"));
        }
        else if(isUsedName == false)
        {
            
            firestore.Document(_userDataPath + userData.userName).GetSnapshotAsync().ContinueWithOnMainThread(task => {
                if (Permission.HasUserAuthorizedPermission(Permission.FineLocation) && Permission.HasUserAuthorizedPermission(Permission.Camera))
                {
                    if(task.Exception == null)
                    {
                        userData.userQuestTime = new UserQuestTime{
                            questTimeFirst = 0f,
                            questTimeSecond = 0f,
                            questTimeThird = 0f,
                            questTimeFourth = 0f,
                            questTimeFifth = 0f,
                            questTimeSixth = 0f
                        };
                        firestore.Document(_userDataPath + userData.userName).SetAsync(userData);
                        AbstractPlayerData.username = userData.userName;
                        isRegis = true;
                        isPermit = true;
                        StartCoroutine(notif("Akun anda berhasil didaftar!"));
                    }
                }                
                UserData tempUserData = task.Result.ConvertTo<UserData>();
                if(isPermit)
                    StartCoroutine(notif($"Nama {tempUserData.userName} sudah ada!")); 
                else
                    StartCoroutine(notif($"Tidak dapat memainkan game!"));         
            });
        }
        else
        {
            StartCoroutine(notif("Nama belum di Check atau sudah ada!"));
        }
        
    }

    public void checkUserName()
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        
        UserData userData = new UserData{
            userName = _inputUserName.text
        };

        if(userData.userName == "")
        {
            StartCoroutine(notif("Input tidak boleh kosong!"));
        }
        else
        {
            firestore.Document(_userDataPath + userData.userName).GetSnapshotAsync().ContinueWithOnMainThread(task => {
                if(task.Exception == null)
                {
                    StartCoroutine(notif("Nama dapat digunakan"));
                    isUsedName = false;
                }

                UserData userData = task.Result.ConvertTo<UserData>();
               
                StartCoroutine(notif($"Nama {userData.userName} sudah ada!"));
                isUsedName = true;          
            });
        }


        
    }

    IEnumerator notif(string sentence)
    {
        notifObject.SetActive(true);
        notifObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = sentence;
        yield return new WaitForSecondsRealtime(3f);
        notifObject.SetActive(false);

        if(isRegis && isPermit)
        {
            LoadingData.sceneToLoad = "Story";
            SceneManager.LoadScene("Loading");
        }
    }
}
