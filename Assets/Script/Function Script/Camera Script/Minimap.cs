using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform player;

    public void LateUpdate()
    {

        Vector3 playerPosition = player.position;
        playerPosition.y = transform.position.y;
        transform.position = playerPosition;

        // transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
