using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    public Transform _playerTarget;
    public Transform _referenceCamera;
    
    private bool _isInitialized = false;
    
    void Awake()
    {
        if(_referenceCamera == null) {
            Debug.Log("There is no camera you attach!");
        } else {
            _isInitialized = true;
        }
    }

    public void positionPlayer() {
        if (!_isInitialized) return;
        Vector3 playerPosition = _playerTarget.position;
        playerPosition.z = playerPosition.z - 45;
        playerPosition.y = _referenceCamera.transform.position.y;
        _referenceCamera.position = playerPosition;
    }
}
