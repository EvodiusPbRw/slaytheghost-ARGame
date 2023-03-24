using Firebase.Firestore;

[FirestoreData]
public class UserQuestTime
{
    [FirestoreProperty]
    public float questTimeFirst{get; set;}

    [FirestoreProperty]
    public float questTimeSecond{get; set;}

    [FirestoreProperty]
    public float questTimeThird{get; set;}

    [FirestoreProperty]
    public float questTimeFourth{get; set;}

    [FirestoreProperty]
    public float questTimeFifth{get; set;}

    [FirestoreProperty]
    public float questTimeSixth{get; set;}
}
