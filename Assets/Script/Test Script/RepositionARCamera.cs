using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionARCamera : MonoBehaviour
{
    public Transform _cameraTarget;
    public Transform _playerTarget;
    public Transform _navigatorCamera;

    // Update is called once per frame
    void Update()
    {
        _cameraTarget.position = _playerTarget.position;
        _navigatorCamera.position = _playerTarget.position;
    }
}
