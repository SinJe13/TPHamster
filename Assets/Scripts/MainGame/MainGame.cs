using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject questionPanel;
    public GameObject defeatPanel;

    public TMP_Text summaryText;
    public TMP_Text questionText;
    public TMP_InputField inputField;
    public TMP_Text defeatText;

    public UIAnimator animator;

    GameLogic game = new GameLogic();
    FoodManager food = new FoodManager();

    int step = 0;
    bool isGameOver = false;

    int totalMoneyEarned = 0;
    int totalHamstersBorn = 0;
    int totalDeaths = 0;

    void Start()
    {
        food.AddFood(5, 0);
        ShowMain();
    }

    public void StartQuestions()
    {
        if (isGameOver) return;

        //mainPanel.SetActive(false);
        StartCoroutine(animator.Show(questionPanel));

        step = 0;
        UpdateQuestion();
    }

    public void SubmitAnswer()
    {
        if (isGameOver) return;

        if (!int.TryParse(inputField.text, out int value)) return;

        ProcessStep(value);

        inputField.text = "";
        step++;

        if (step > 3)
        {
            EndTurn();
            ShowMain();
        }
        else
        {
            UpdateQuestion();
        }
    }

    void ProcessStep(int value)
    {
        switch (step)
        {
            case 0:
                int beforeM = game.money;
                game.SellMales(value);
                totalMoneyEarned += game.money - beforeM;
                break;

            case 1:
                int beforeF = game.money;
                game.SellFemales(value);
                totalMoneyEarned += game.money - beforeF;
                break;

            case 2:
                int maxFood = game.money / 2;
                value = Mathf.Min(value, maxFood);
                food.AddFood(value, game.month);
                game.money -= value * 2;
                break;

            case 3:
                int before = game.newMales + game.newFemales;
                game.Coupling(value);
                int after = game.newMales + game.newFemales;
                totalHamstersBorn += (after);

                HandleFood();
                break;
        }
    }

    void HandleFood()
    {
        food.RemoveExpired(game.month);

        int availableFood = food.GetFoodCount();

        foreach (var h in game.maleAdults) h.hasEaten = false;
        foreach (var h in game.femaleAdults) h.hasEaten = false;

        foreach (var h in game.maleAdults)
        {
            if (availableFood > 0)
            {
                h.hasEaten = true;
                availableFood--;
            }
        }

        foreach (var h in game.femaleAdults)
        {
            if (availableFood > 0)
            {
                h.hasEaten = true;
                availableFood--;
            }
        }

        for (int i = game.maleAdults.Count - 1; i >= 0; i--)
        {
            if (!game.maleAdults[i].hasEaten)
            {
                game.maleAdults.RemoveAt(i);
                totalDeaths++;
            }
        }

        for (int i = game.femaleAdults.Count - 1; i >= 0; i--)
        {
            if (!game.femaleAdults[i].hasEaten)
            {
                game.femaleAdults.RemoveAt(i);
                totalDeaths++;
            }
        }

        if (availableFood == 0)
            food.ClearAll();
        else
            food.ConsumeFood(food.GetFoodCount() - availableFood);
    }

    void EndTurn()
    {
        totalDeaths += game.HandleAgingAndDeaths();
        CheckDefeat();
    }

    void CheckDefeat()
    {
        int total =
            game.maleAdults.Count +
            game.femaleAdults.Count +
            game.maleBabies.Count +
            game.femaleBabies.Count;

        if (total <= 0)
            ShowDefeat();
    }

    void ShowDefeat()
    {
        isGameOver = true;

        //mainPanel.SetActive(false);
        StartCoroutine(animator.Show(defeatPanel));

        defeatText.text =
            "GAME OVER\n\n" +
            "Months : " + game.month + "\n" +
            "Money : " + totalMoneyEarned + "$\n" +
            "Births : " + totalHamstersBorn + "\n" +
            "Deaths : " + totalDeaths;
    }

    void ShowMain()
    {
        StartCoroutine(animator.Hide(questionPanel));
        //mainPanel.SetActive(true);
        UpdateSummary();
    }

    void UpdateQuestion()
    {
        switch (step)
        {
            case 0: questionText.text = "How many males do you want to sell ?"; break;
            case 1: questionText.text = "How many females do you want to sell ?"; break;
            case 2: questionText.text = "How much food do you want to buy ?"; break;
            case 3: questionText.text = "How many hamsters do you want to breed ?"; break;
        }
    }

    void UpdateSummary()
    {
        summaryText.text =
            "MONTH : " + game.month + "\n\n" +
            "MALES : " + game.maleAdults.Count + "\n" +
            "FEMALES : " + game.femaleAdults.Count + "\n\n" +
            "BABIES : " + "\n" +
            "MALES : "+ game.maleBabies.Count + " | FEMALES : " + game.femaleBabies.Count + "\n\n" +
            "FOOD : " + food.GetFoodCount() + "\n" +
            "MONEY : " + game.money + "$";
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(0);
    }
}