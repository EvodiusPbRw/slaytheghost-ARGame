using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusPosition : MonoBehaviour
{
    private Transform _radiusTarget;
    public Transform _referenceCamera;

    private bool _isInitialized = false;

    // Start is called before the first frame update
    void Awake()
    {
        if(_referenceCamera == null) {
            Debug.Log("There is no camera you attach!");
        } else {
            _isInitialized = true;
        }
    }

    // Update is called once per frame
    public void positionRadius(string name)
    {
        if(!_isInitialized) return;
        _radiusTarget = GameObject.Find(name).transform;
        Vector3 radiusPosition = _radiusTarget.position;
        radiusPosition.z -= 45;
        radiusPosition.y = _referenceCamera.transform.position.y;
        _referenceCamera.position = radiusPosition;
    }
}
