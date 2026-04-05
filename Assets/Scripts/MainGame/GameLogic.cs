using System;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public int hamsterMaleAdult = 2;
    public int hamsterFemaleAdult = 2;
    public int hamsterMaleBaby = 0;
    public int hamsterFemaleBaby = 0;
    public int money = 10;
    public int month = 0;

    public int newMales = 0;
    public int newFemales = 0;

    public int maleDead = 0;
    public int femaleDead = 0;

    System.Random rand = new System.Random();

    const int HamsterPriceSell = 3;

    public void SellMales(int value)
    {
        value = Mathf.Min(value, hamsterMaleAdult);
        hamsterMaleAdult -= value;
        money += value * HamsterPriceSell;
    }

    public void SellFemales(int value)
    {
        value = Mathf.Min(value, hamsterFemaleAdult);
        hamsterFemaleAdult -= value;
        money += value * HamsterPriceSell;
    }

    public void Coupling(int value)
    {
        int babies = 0;

        for (int i = 0; i < value; i++)
            babies += rand.Next(1, 4);

        newMales = 0;
        newFemales = 0;

        for (int i = 0; i < babies; i++)
        {
            if (rand.Next(0, 100) < 50)
                newMales++;
            else
                newFemales++;
        }
    }

    public void NextMonth()
    {
        hamsterMaleAdult += hamsterMaleBaby;
        hamsterFemaleAdult += hamsterFemaleBaby;

        hamsterMaleBaby = newMales;
        hamsterFemaleBaby = newFemales;

        month++;
    }
}