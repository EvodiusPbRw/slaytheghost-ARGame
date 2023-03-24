using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

public class AnchorManager : MonoBehaviour
{
    // public GameObject enemy;
    // private ARAnchor anchor;

    // private Vector3 LastPosition;
    // private Quaternion LastRotation;

    // void Start() 
    // {
        
    // }

    // void Update()
    // {
    //     if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {
    //         anchor = Session.CreateAnchor(transform.position, transform.rotation);
    //         GameObject.Instantiate(enemy, anchor.transform.position, anchor.transform.rotation,anchor.transform);
    //     }
    //     if(anchor == null) 
    //         return;
    //     if(anchor.transform.position != LastPosition){
    //         LastPosition = anchor.transform.position;
    //     }
    //     if(anchor.transform.rotation != LastRotation){
    //         LastRotation = anchor.transform.rotation;
    //     }
    // }
}
