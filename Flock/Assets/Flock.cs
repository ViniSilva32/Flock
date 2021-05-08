using System.Collections;
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

        RaycastHit hit = new RaycastHit(); // local para onde os peixes vão mirar
        Vector3 direction = myManager.transform.position - transform.position;

        if(!b.Contains(transform.position)) 
        {
            turning = true;
            direction = myManager.transform.position - transform.position; 
        }
        else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit)) 
        {
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal); // impede que os peixes toquem ou vão em uma direção
        }
        else
            turning = false;

        if (turning)
        {
                transform.rotation =Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),                         //local que os peixes olham para virar
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
        GameObject[] gos;                    //criação do objeto
        gos = myManager.allFish;            

        Vector3 vcentre = Vector3.zero;     
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;                        // variaveis para definir velocidade, distancia e quanto os peixes irão se agrupar
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

                    if(nDistance < 1.0)                                                         // faz com que os peixes não se toquem e respeitem o espaço um do outro
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
           
            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)                                           //altera a direção dos peixes
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction),
                    myManager.rotationspeed * Time.deltaTime);
        }
    }
}
