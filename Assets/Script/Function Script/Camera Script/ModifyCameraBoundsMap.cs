using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyCameraBoundsMap : MonoBehaviour
{
    public Transform _mapCamera;
    public Transform _minimapCamera;
    public RenderTexture rt_map;
    public RenderTexture rt_minimap;

    public GameObject _mapPanel;
    private Camera _cameraBoundMap;

    void Start()
    {
        
        int LayerNavigator = LayerMask.NameToLayer("Minimap");
        gameObject.layer = LayerNavigator;
        _cameraBoundMap = gameObject.GetComponent(typeof(Camera)) as Camera;
        _cameraBoundMap.targetTexture = rt_map;
        _cameraBoundMap.depth = -1;
    }

    void Update()
    {
        StartCoroutine(ModifyComponentCamera());
    }

    IEnumerator ModifyComponentCamera(){
        if(!_mapPanel.activeSelf) 
        {
            gameObject.transform.position = new Vector3(_minimapCamera.position.x, _minimapCamera.position.y, _minimapCamera.position.z);
            gameObject.transform.rotation = Quaternion.Euler(90f, 0, 0);
            _cameraBoundMap.targetTexture = rt_minimap;
            _cameraBoundMap.fieldOfView = 40f;
        }
        else
        {
            gameObject.transform.position = new Vector3(_mapCamera.position.x, _mapCamera.position.y, _mapCamera.position.z);
            gameObject.transform.Rotate((-(gameObject.transform.eulerAngles.x)+60),0,0,Space.World);
            _cameraBoundMap.targetTexture = rt_map;
            _cameraBoundMap.fieldOfView = 60f;
        }
        yield return null;
    }
}
