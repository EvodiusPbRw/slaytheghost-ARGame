using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        WebCamTexture webCamTexture = new WebCamTexture();
        // GetComponent<Renderer>().material.mainTexture = webCamTexture;
        
        Debug.Log(devices[0].name);

        if (devices.Length > 0)
        {
            webCamTexture.deviceName = devices[0].name;
            webCamTexture.Play();
        }
        Debug.Log(webCamTexture.isPlaying);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
