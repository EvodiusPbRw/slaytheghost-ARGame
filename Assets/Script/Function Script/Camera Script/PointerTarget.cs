using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine;

public class PointerTarget : MonoBehaviour
{

    public GameObject pointerObject;
    public Transform _cameraTarget;
    public Text objective;

    private GameObject instance;

    // Start is called before the first frame update
    void Start()
    {
        this.instance = Instantiate(pointerObject, new Vector3(_cameraTarget.position.x, _cameraTarget.position.y, _cameraTarget.position.z + 1f),Quaternion.Euler(0, 180, 0));

        int LayerNavigator = LayerMask.NameToLayer("Navigator");
        this.instance.layer = LayerNavigator;
        this.instance.name = "Arrows";
        this.instance.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
        this.instance.transform.Rotate(50f, -180f, 0f, Space.World);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject radiusTarget = GameObject.Find(PlayerPrefs.GetString("currentObjectiveLocation"));
        objective.text = PlayerPrefs.GetString("currentObjectiveLocation");
        // Vector3 to = new Vector3(0, -radiusTarget.transform.position.x, 0);
        // this.instance.transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, Time.deltaTime);

        this.instance.transform.LookAt(radiusTarget.transform);
        this.instance.transform.Rotate(this.instance.transform.eulerAngles.x + 50 , this.instance.transform.localEulerAngles.x-180 , this.instance.transform.eulerAngles.z, Space.World);


        this.instance.transform.position = new Vector3(_cameraTarget.position.x, _cameraTarget.position.y-0.175f, _cameraTarget.position.z + 1f);
    }
}
