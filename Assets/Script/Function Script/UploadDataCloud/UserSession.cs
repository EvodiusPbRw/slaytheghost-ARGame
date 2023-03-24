using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Linq;

public class UserSession : MonoBehaviour
{
    [SerializeField] private string _userDataPath = "users/";

    public async void setFinalTotalTimeLogUser()
    {
        var firestore = FirebaseFirestore.DefaultInstance;

        DocumentSnapshot snapshot = await firestore.Document(_userDataPath + AbstractPlayerData.player.Name).GetSnapshotAsync();
        if(snapshot.Exists)
        {
            UserData userData = snapshot.ConvertTo<UserData>();
            userData.totalTime = GameObject.Find("Timer(Clone)").GetComponent<GameTimer>().timer;
            firestore.Document(_userDataPath + userData.userName).SetAsync(userData);
            destroyTimer();
        }
    }

    public async void setUserEnterLogTimeRadius(string location)
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        float time = GameObject.Find("Timer(Clone)").GetComponent<GameTimer>().timer;

        DocumentSnapshot snapshot = await firestore.Document(_userDataPath + AbstractPlayerData.player.Name).GetSnapshotAsync();
        if(snapshot.Exists)
        {
            UserData userData = snapshot.ConvertTo<UserData>();
            List<UserLogTimeRadius> userLogTimeRadius = new List<UserLogTimeRadius>();
            if(userData.userLogTimeRadius == null)
            {
                userLogTimeRadius.Add(new UserLogTimeRadius{
                    userEnterRadius = time,
                    radiusLocation = location
                });
            }
            else
            {
                userLogTimeRadius = userData.userLogTimeRadius.ToList();
                userLogTimeRadius.Add(new UserLogTimeRadius{
                    userEnterRadius = time,
                    radiusLocation = location
                });
            }
            UserLogTimeRadius[] tempUserLogTimeRadius = userLogTimeRadius.ToArray();

            userData.userLogTimeRadius = tempUserLogTimeRadius;
            firestore.Document(_userDataPath + userData.userName).SetAsync(userData);
        }
    }

    public async void setUserExitLogTimeRadius()
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        float time = GameObject.Find("Timer(Clone)").GetComponent<GameTimer>().timer;

        DocumentSnapshot snapshot = await firestore.Document(_userDataPath + AbstractPlayerData.player.Name).GetSnapshotAsync();
        if(snapshot.Exists)
        {
            UserData userData = snapshot.ConvertTo<UserData>();
            List<UserLogTimeRadius> userLogTimeRadius = new List<UserLogTimeRadius>();
            userLogTimeRadius = userData.userLogTimeRadius.ToList();
            userLogTimeRadius[userLogTimeRadius.Count - 1].userExitRadius = time;
            userData.userLogTimeRadius = userLogTimeRadius.ToArray();
            firestore.Document(_userDataPath + userData.userName).SetAsync(userData);
        }
    }

    public async void setUserQuestTime(int idQuest)
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        float time = GameObject.Find("Timer(Clone)").GetComponent<GameTimer>().timer;

        DocumentSnapshot snapshot = await firestore.Document(_userDataPath + AbstractPlayerData.player.Name).GetSnapshotAsync();
        if(snapshot.Exists)
        {
            UserData userData = snapshot.ConvertTo<UserData>();

            if(idQuest == 1)
                userData.userQuestTime.questTimeFirst = time;
            else if(idQuest == 2)
                userData.userQuestTime.questTimeSecond = time;
            else if(idQuest == 3)
                userData.userQuestTime.questTimeThird = time;
            else if(idQuest == 4)
                userData.userQuestTime.questTimeFourth = time;
            else if(idQuest == 5)
                userData.userQuestTime.questTimeFifth = time;
            else if(idQuest == 6)
                userData.userQuestTime.questTimeSixth = time;

            firestore.Document(_userDataPath + userData.userName).SetAsync(userData);
        }
    }

    public async void setContinueUserData()
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        DocumentSnapshot snapshot = await firestore.Document(_userDataPath + AbstractPlayerData.player.Name).GetSnapshotAsync();
        if(snapshot.Exists)
        {
            UserData userData = snapshot.ConvertTo<UserData>();
            GameObject.Find("Timer(Clone)").GetComponent<GameTimer>().timer = userData.totalTime;
        }
    }

    public void destroyTimer()
    {
        Destroy(GameObject.Find("Timer(Clone)"));
    }
}
