using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class Geolocation : MonoBehaviour
{
    public static Geolocation Instance { set; get;}
    public Text GPSStatus;
    public Text longitudeValue;
    public Text latitudeValue;
    public Text altitudeValue;
    public Text horizontalAccuracyValue;
    public Text timestampValue;
    public Text isEnabled;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GPSLoc());
    }

    IEnumerator GPSLoc(){
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation)) {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }

        isEnabled.text = Input.location.isEnabledByUser.ToString();

        // if(!Input.location.isEnabledByUser) {
        //     yield break;
        // }

        Input.location.Start(0.1f, 0.1f);

        GPSStatus.text = Input.location.status.ToString();

        int maxWait = 3;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            GPSStatus.text = "Loading!";
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1) {
            GPSStatus.text = "Time out";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed) {
            GPSStatus.text = "Unable to determine service device location";
            yield break;
        }
        else{
            GPSStatus.text = "Running";
            InvokeRepeating("UpdateGPSData",0.5f, 1f);
        }
    }

    void UpdateGPSData(){
        Debug.Log(Input.location.status);
        if (Input.location.status == LocationServiceStatus.Running)
        {
            GPSStatus.text = Input.location.status.ToString();
            latitudeValue.text = Input.location.lastData.latitude.ToString();
            longitudeValue.text = Input.location.lastData.longitude.ToString();
            altitudeValue.text = Input.location.lastData.altitude.ToString();
            horizontalAccuracyValue.text = Input.location.lastData.horizontalAccuracy.ToString();
            timestampValue.text = Input.location.lastData.timestamp.ToString();
            isEnabled.text = Input.location.isEnabledByUser.ToString();
        }
        else
        {
            GPSStatus.text = Input.location.status.ToString();
            latitudeValue.text = "0";
            longitudeValue.text = "0";
            altitudeValue.text = "0";
            horizontalAccuracyValue.text = "0";
            timestampValue.text = "0";
            isEnabled.text = Input.location.isEnabledByUser.ToString();
        }
    }
}
