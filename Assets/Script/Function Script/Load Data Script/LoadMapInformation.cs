using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using Mapbox.Unity.Location;
using static MapModel;

public class LoadMapInformation :  MonoBehaviour
{
    [SerializeField]
    AbstractMap _map;

    public GameObject SpawnRadius;
    public GameObject PointerTarget;
    void Update()
    {
        _map.MapVisualizer.OnMapVisualizerStateChanged += (s) =>
        {
            if (s == ModuleState.Working)
            {
                SpawnRadius.SetActive(true);
                //PointerTarget.SetActive(true);
            }
        };
    }
}
