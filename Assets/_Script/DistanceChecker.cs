using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceChecker : MonoBehaviour
{

    public Transform target;
    public float distance;
    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, target.position);
    }
}
