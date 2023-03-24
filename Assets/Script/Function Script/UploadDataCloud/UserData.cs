using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class UserData
{
    [FirestoreProperty]
    public string userName{get; set;}

    [FirestoreProperty]
    public string deviceName {get; set;}

    [FirestoreProperty]
    public float totalTime{get; set;}

    [FirestoreProperty]
    public UserQuestTime userQuestTime{get; set;}

    [FirestoreProperty]
    public UserLogTimeRadius[] userLogTimeRadius{get; set;}
}
