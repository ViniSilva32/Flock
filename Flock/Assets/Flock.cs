﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    //criação das variaveis
    public FlockingManager myManager;
    float speed;
    bool turning = false;

    void Start()
    {
        //define a velocidade de movimento dos peixes
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    void Update()
    {
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);
        if(!b.Contains(transform.position))
        {
            turning = true;
        }
        else
            turning = false;

        if (turning)
        {
            Vector3 direction = myManager.transform.position - transform.position;
            transform.rotation =Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),
                myManager.rotationspeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            if (Random.Range(0, 100) < 20) 
                ApplyRules();
        }
        //movimenta os peixes
        transform.Translate(0, 0, Time.deltaTime * speed);
    }
    void ApplyRules()
    {
        GameObject[] gos;
        gos = myManager.allFish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        foreach(GameObject go in gos)
        {
            if(go !=this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if(nDistance<= myManager.neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if(nDistance < 1.0)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;

                }
            }
        }
        if(groupSize > 0)
        {
            vcentre = vcentre / groupSize + (myManager.goalpos - this.transform.position);
            speed = gSpeed / groupSize;
            //altera a direção dos peixes
            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction),
                    myManager.rotationspeed * Time.deltaTime);
        }
    }
}
