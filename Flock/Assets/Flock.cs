using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockingManager myManager;
    float speed;

    void Start()
    {
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    
    void Update()
    {
        transform.Translate(0, 0, Time.deltaTime * speed);
    }
}
