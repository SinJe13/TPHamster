using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class MainGame : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject questionPanel;

    public TMP_Text summaryText;
    public TMP_Text questionText;
    public TMP_InputField inputField;

    public UIAnimator animator;

    GameLogic game = new GameLogic();
    FoodManager food = new FoodManager();

    int step = 0;

    void Start()
    {
        food.AddFood(5, 0);
        ShowMain();
    }

    public void StartQuestions()
    {
        //mainPanel.SetActive(false);
        StartCoroutine(animator.Show(questionPanel));

        step = 0;
        UpdateQuestion();
    }

    public void SubmitAnswer()
    {
        int value;
        if (!int.TryParse(inputField.text, out value)) return;

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
            case 0: game.SellMales(value); break;
            case 1: game.SellFemales(value); break;

            case 2:
                int maxFood = game.money / 2;
                value = Mathf.Min(value, maxFood);
                food.AddFood(value, game.month);
                game.money -= value * 2;
                break;

            case 3:
                game.Coupling(value);
                HandleFood();
                break;
        }
    }

    void HandleFood()
    {
        food.RemoveExpired(game.month);

        int required = game.hamsterMaleAdult + game.hamsterFemaleAdult;
        int available = food.GetFoodCount();

        if (required > available)
        {
            int deaths = required - available;

            for (int i = 0; i < deaths; i++)
            {
                if (game.hamsterMaleAdult > 0)
                    game.hamsterMaleAdult--;
                else if (game.hamsterFemaleAdult > 0)
                    game.hamsterFemaleAdult--;
            }

            food.ClearAll();
        }
        else
        {
            food.ConsumeFood(required);
        }
    }

    void EndTurn()
    {
        game.NextMonth();
    }

    void ShowMain()
    {
        StartCoroutine(animator.Hide(questionPanel));
        //mainPanel.SetActive(true);
        UpdateSummary();
    }

    void UpdateSummary()
    {
        summaryText.text =
            "MONTH : " + game.month + "\n\n" +
            "MALES : " + game.hamsterMaleAdult + "\n" +
            "FEMALES : " + game.hamsterFemaleAdult + "\n\n" +
            "FOOD : " + food.GetFoodCount() + "\n" +
            "MONEY : " + game.money + "$";
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
}