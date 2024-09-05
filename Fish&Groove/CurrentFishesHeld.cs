using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentFishesHeld : MonoBehaviour
{
    //Fish quantities
    //public int S1, S2, S3, M1, M2, M3, M4, M5, M6, H1, H2, H3, H4, H5, H6;
    //Fish Prices IN CENTS
    //public float S1P, S2P, S3P, M1P, M2P, M3P, M4P, M5P, M6P, H1P, H2P, H3P, H4P, H5P, H6P;

    [HideInInspector] public float[] BasePrices = new float[15];
    public float[] FishPrices = new float[15];
    public int[] Fish = new int[15]; 

    private IEnumerator activeCoroutine;
    private FishValueIndicator costChanger;

    private void Start()
    {
        costChanger = GetComponent<FishValueIndicator>();
        for (int i = 0; i < FishPrices.Length; i++) 
        {
            BasePrices[i] = FishPrices[i];
        }
    }

    private void Update()
    {
        if (activeCoroutine == null)
        {
            activeCoroutine = NeedDecay();
            StartCoroutine(activeCoroutine);
        }
    }

    private IEnumerator NeedDecay()
    {
        yield return new WaitForSeconds(60f);
        
        Fish[0] += Random.Range(-5, 10);
        Fish[1] += Random.Range(-5, 10);
        Fish[2] += Random.Range(-5, 10);
        Fish[3] += Random.Range(-10, 10);
        Fish[4] += Random.Range(-10, 10);
        Fish[5] += Random.Range(-10, 10);
        Fish[6] += Random.Range(-10, 10);
        Fish[7] += Random.Range(-10, 10);
        Fish[8] += Random.Range(-10, 10);
        Fish[9] += Random.Range(-10, 5);
        Fish[10] += Random.Range(-10, 5);
        Fish[11] += Random.Range(-10, 5);
        Fish[12] += Random.Range(-10, 5);
        Fish[13] += Random.Range(-10, 5);
        Fish[14] += Random.Range(-10, 5);
        

        costChanger.CalculatePrices();
        activeCoroutine = null;
    }
}
