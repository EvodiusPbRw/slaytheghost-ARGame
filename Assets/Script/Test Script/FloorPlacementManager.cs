using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]

public class FloorPlacementManager : MonoBehaviour
{
    private GameObject spawnEnemy;

    private ARRaycastManager arRaycastManager;
    private ARPlaneManager arPlaneManager;

    [SerializeField]
    public GameObject objectToSpawn;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void awake(){
        arRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition){
        if (Input.touchCount > 0){
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TryGetTouchPosition(out Vector2 touchPosition)) return;
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began){
            if(arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes)){
                var hitPose = hits[0].pose;
                foreach (var plane in arPlaneManager.trackables)
                {
                    plane.gameObject.SetActive(false);
                }
                arPlaneManager.enabled = false;
                spawnEnemy = Instantiate(objectToSpawn, hitPose.position, hitPose.rotation);
            }
        }        
    }
}
