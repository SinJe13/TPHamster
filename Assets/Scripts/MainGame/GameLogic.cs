using System.Collections.Generic;
using UnityEngine;

public class GameLogic
{
    public List<Hamster> maleAdults = new List<Hamster>();
    public List<Hamster> femaleAdults = new List<Hamster>();
    public List<Hamster> maleBabies = new List<Hamster>();
    public List<Hamster> femaleBabies = new List<Hamster>();

    public int money = 10;
    public int month = 0;

    public int newMales = 0;
    public int newFemales = 0;

    System.Random rand = new System.Random();

    const int SellPrice = 3;

    public GameLogic()
    {
        // départ
        maleAdults.Add(new Hamster(0));
        maleAdults.Add(new Hamster(0));
        femaleAdults.Add(new Hamster(0));
        femaleAdults.Add(new Hamster(0));
    }

    public void SellMales(int value)
    {
        value = Mathf.Min(value, maleAdults.Count);
        maleAdults.RemoveRange(0, value);
        money += value * SellPrice;
    }

    public void SellFemales(int value)
    {
        value = Mathf.Min(value, femaleAdults.Count);
        femaleAdults.RemoveRange(0, value);
        money += value * SellPrice;
    }

    public void Coupling(int value)
    {
        int maxCouple = Mathf.Min(maleAdults.Count, femaleAdults.Count);
        value = Mathf.Min(value, maxCouple);

        int babies = 0;

        for (int i = 0; i < value; i++)
            babies += rand.Next(1, 4);

        newMales = 0;
        newFemales = 0;

        for (int i = 0; i < babies; i++)
        {
            if (rand.Next(0, 100) < 50)
            {
                maleBabies.Add(new Hamster(0));
                newMales++;
            }
            else
            {
                femaleBabies.Add(new Hamster(0));
                newFemales++;
            }
        }
    }

    public int HandleAgingAndDeaths()
    {
        int deaths = 0;

        // Vieillir tout le monde
        foreach (var h in maleAdults) h.age++;
        foreach (var h in femaleAdults) h.age++;
        foreach (var h in maleBabies) h.age++;
        foreach (var h in femaleBabies) h.age++;

        // bébés -> adultes (>=2 mois)
        for (int i = maleBabies.Count - 1; i >= 0; i--)
        {
            if (maleBabies[i].age >= 2)
            {
                maleAdults.Add(maleBabies[i]);
                maleBabies.RemoveAt(i);
            }
        }

        for (int i = femaleBabies.Count - 1; i >= 0; i--)
        {
            if (femaleBabies[i].age >= 2)
            {
                femaleAdults.Add(femaleBabies[i]);
                femaleBabies.RemoveAt(i);
            }
        }

        // morts vieillesse (>24 mois → 60%)
        for (int i = maleAdults.Count - 1; i >= 0; i--)
        {
            if (maleAdults[i].age > 24 && rand.Next(0, 100) < 60)
            {
                maleAdults.RemoveAt(i);
                deaths++;
            }
        }

        for (int i = femaleAdults.Count - 1; i >= 0; i--)
        {
            if (femaleAdults[i].age > 24 && rand.Next(0, 100) < 60)
            {
                femaleAdults.RemoveAt(i);
                deaths++;
            }
        }

        month++;
        return deaths;
    }
}