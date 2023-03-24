using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
// using static GPSEncoder;

public class GPSLimitation : MonoBehaviour
{   
    public Text latitudeValue;
    public Text longitudeValue;
    private Vector2 gps;

    // void Start()
    // {
    //     // StartCoroutine(CurrentLoc());
    // }
    
    // IEnumerator CurrentLoc()
    // {
    //     // if(!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
    //     // {
    //     //     Permission.RequestUserPermission(Permission.FineLocation);
    //     //     Permission.RequestUserPermission(Permission.CoarseLocation);
    //     // }

    //     // while (Input.location.isEnabledByUser == false) 
    //     // {
    //     //     Debug.Log("Turn on your location!");
    //     // }

    //     // Input.location.Start();

    //     // int maxWait = 3;
    //     // while(Input.location.status == LocationServiceStatus.Initializing)
    //     // {
    //     //     Debug.Log("Loading!");
    //     //     yield return new WaitForSeconds(1);
    //     //     maxWait--;
    //     // }

    //     // if (maxWait < 1) 
    //     // {
    //     //     Debug.Log("Time out");
    //     //     yield break;
    //     // }

    //     // if (Input.location.status == LocationServiceStatus.Failed) 
    //     // {
    //     //     Debug.Log("Gagal terhubung");
    //     //     yield break;
    //     // }
    //     // else 
    //     // {
    //     //     Debug.Log("Success update the GPS Data");
    //     //     this.gps = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
    //     //     PlayerPrefs.SetFloat("latitude", Input.location.lastData.latitude);
    //     //     PlayerPrefs.SetFloat("longitude", Input.location.lastData.longitude);
    //     //     //InvokeRepeating("UpdateLocationUser",0.1f,0.1f);
    //     // }
    // }

    // void UpdateLocationUser(){
    //     PlayerPrefs.SetFloat("latitude", Input.location.lastData.latitude);
    //     PlayerPrefs.SetFloat("longitude", Input.location.lastData.longitude);
    //     PlayerPrefs.Save();
    //     latitudeValue.text = PlayerPrefs.GetFloat("latitude").ToString();
    //     longitudeValue.text = PlayerPrefs.GetFloat("longitude").ToString();
    // }
}
