using Firebase.Firestore;

[FirestoreData]
public class UserLogTimeRadius
{
    [FirestoreProperty]
    public float userEnterRadius{get;set;}

    [FirestoreProperty]
    public float userExitRadius{get;set;}

    [FirestoreProperty]
    public string radiusLocation{get;set;}
}
