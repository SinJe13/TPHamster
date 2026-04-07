using System.Collections.Generic;

public class FoodManager
{
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

    public int ConsumeFood(int amount)
    {
        int consumed = 0;

        for (int i = 0; i < foods.Count; i++)
        {
            if (foods[i].quantity >= amount)
            {
                foods[i].quantity -= amount;
                consumed += amount;
                break;
            }
            else
            {
                amount -= foods[i].quantity;
                consumed += foods[i].quantity;
                foods[i].quantity = 0;
            }
        }

        foods.RemoveAll(f => f.quantity == 0);
        return consumed;
    }

    public void ClearAll()
    {
        foods.Clear();
    }
}