using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOverlapSphere : MonoBehaviour
{
    // Start is called before the first frame update

    void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.SendMessage("AddDamage");
        }
    }

    public Vector3 center;
    public float radius;
    void Start()
    {
        ExplosionDamage(center, radius);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
