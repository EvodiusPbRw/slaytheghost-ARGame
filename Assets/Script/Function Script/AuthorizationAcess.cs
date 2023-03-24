using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class AuthorizationAcess : MonoBehaviour
{

    void Awake()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
	    if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
            	Permission.RequestUserPermission(Permission.Camera);
            }
        }

        
    }
}
