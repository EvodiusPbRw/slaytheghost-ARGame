using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using static GPSEncoder;
using System.Globalization;

public class Enemy1Location : MonoBehaviour
    {
        public float longitude,
            latitude,
            altitude;
        // Start is called before the first frame update
        void Start()
        {
            // SetLocalOrigin(new Vector2(PlayerPrefs.GetFloat("latitude"),PlayerPrefs.GetFloat("longitude")));
            // Vector2 enemyPos = GPSToUCS(latitude,longitude);
            // transform.position = new Vector3(enemyPos.x, altitude, enemyPos.y);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
