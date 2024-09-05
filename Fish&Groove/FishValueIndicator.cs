using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FishValueIndicator : MonoBehaviour
{
    private CurrentFishesHeld stockChecker;
    private Inventory inventory;



    void Start()
    {
        stockChecker = GetComponent<CurrentFishesHeld>();
        inventory = FindObjectOfType<Inventory>();
    }

    public void CalculatePrices()
    {
        //Technically the 3 ID ints could be condensed down into one since they are all the same, but to keep it understandable il keep all 3.
        //For refrence CalculationBase(fishID, fishIDPrice, basePriceID, multiplier, factor);
        CalculationBase(0, 0, 0, 0, 1);
        CalculationBase(1, 1, 1, 0, 1);
        CalculationBase(2, 2, 2, 0, 1);
        CalculationBase(3, 3, 3, 0, 1);
        CalculationBase(4, 4, 4, 0, 1);
        CalculationBase(5, 5, 5, 0, 1);
        CalculationBase(6, 6, 6, 0, 1);
        CalculationBase(7, 7, 7, 0, 1);
        CalculationBase(8, 8, 8, 0, 1);
        CalculationBase(9, 9, 9, 0, 1);
        CalculationBase(10, 10, 10, 0, 1);
        CalculationBase(11, 11, 11, 0, 1);
        CalculationBase(12, 12, 12, 0, 1);
        CalculationBase(13, 13, 13, 0, 1);
        CalculationBase(14, 14, 14, 0, 1);
    }

    
    public void CalculationBase(int fishID, int fishIDPrice, int basePriceID, int multiplier, float factor)
    {
        //Increase in demand.
        if (stockChecker.Fish[fishID] <= 0)
        {
            for (int i = 0; i > stockChecker.Fish[fishID]; i--)
            {
                multiplier = i / -5;
                if (i == 0)
                {
                    factor += 0.05F;
                }
            }
            factor += 0.05f * multiplier;
            
            stockChecker.FishPrices[fishIDPrice] = stockChecker.BasePrices[basePriceID] * factor;
        }
        //Decrease in demand.
        else if (stockChecker.Fish[fishID] >= 1)
        {
            for (int i = 0; i < stockChecker.Fish[fishID]; i++)
            {
                multiplier = i / 5;
                if (i == 0)
                {
                    factor -= 0.05F;
                }
            }
            factor -= 0.05f * multiplier;
            if (factor <= 0.5f)
            {
                factor = 0.5f;
            }

            stockChecker.FishPrices[fishIDPrice] = stockChecker.BasePrices[basePriceID] * factor;
        }
    }


}
