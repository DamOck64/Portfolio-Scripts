using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMaker : MonoBehaviour
{
    [SerializeField] private GameObject[] theNote;
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private GameObject theChart;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(theNote[0], spawnPoint[0]);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Instantiate(theNote[1], spawnPoint[1]);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Instantiate(theNote[2], spawnPoint[2]);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Instantiate(theNote[3], spawnPoint[3]);
        }
    }
}
