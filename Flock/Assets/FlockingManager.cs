using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    //criação das variaveis e objetos
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    public Vector3 goalpos;

    //adiciona as barras para modificação direta na unity
    [Header("Configurações do Cardume")]
    [Range(0.0f, 5.0f)]
    public float minSpeed; // define a velocidade minima de movimento dos peixes
    [Range(0.0f, 5.0f)]
    public float maxSpeed; // define a velocidade maxima de movimento dos peixes
    [Range(1.0f, 10.0f)]
    public float neighbourDistance; //define a distancia de um peixe para o outro
    [Range(0.0f, 5.0f)]
    public float rotationspeed; // define a velocidade maxima de rotação dos peixes
    void Start()
    {
        //instancia os peixes na cena
        allFish = new GameObject[numFish];
        for(int i = 0; i < numFish; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), Random.Range(-swinLimits.y, swinLimits.y),Random.Range(-swinLimits.z, swinLimits.z));
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().myManager = this;
        }
        goalpos = this.transform.position;
    }


    void Update()
    {
        goalpos = this.transform.position;
        //altera a direção em que os peixes se movem
        if (Random.Range(0, 100) < 10)
        {
            
            goalpos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                                                Random.Range(-swinLimits.y, swinLimits.y),
                                                Random.Range(-swinLimits.z, swinLimits.z));

        }
    }
}
