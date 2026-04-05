using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    // Petite classe interne
    class Food
    {
        public int quantity;
        public int month;

        public Food(int q, int m)
        {
            quantity = q;
            month = m;
        }
    }

    List<Food> foods = new List<Food>();

    public void AddFood(int quantity, int month)
    {
        foods.Add(new Food(quantity, month));
    }

    public int GetFoodCount()
    {
        int total = 0;
        foreach (var f in foods)
            total += f.quantity;
        return total;
    }

    public void RemoveExpired(int currentMonth)
    {
        for (int i = foods.Count - 1; i >= 0; i--)
        {
            if (foods[i].month + 3 < currentMonth)
                foods.RemoveAt(i);
        }
    }

    public void ConsumeFood(int amount)
    {
        for (int i = 0; i < foods.Count; i++)
        {
            if (foods[i].quantity >= amount)
            {
                foods[i].quantity -= amount;
                break;
            }
            else
            {
                amount -= foods[i].quantity;
                foods[i].quantity = 0;
            }
        }

        foods.RemoveAll(f => f.quantity == 0);
    }

    public void ClearAll()
    {
        foods.Clear();
    }
}