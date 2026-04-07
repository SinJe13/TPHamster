using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject questionPanel;
    public GameObject defeatPanel;

    [Header("UI Main")]
    public TMP_Text summaryText;

    [Header("UI Question")]
    public TMP_Text questionText;
    public TMP_InputField inputField;

    [Header("UI Defeat")]
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

        mainPanel.SetActive(false);
        StartCoroutine(animator.Show(questionPanel));

        step = 0;
        UpdateQuestion();
    }

    public void SubmitAnswer()
    {
        if (isGameOver) return;

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

    void ShowMain()
    {
        StartCoroutine(animator.Hide(questionPanel));
        mainPanel.SetActive(true);
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

    void ProcessStep(int value)
    {
        switch (step)
        {
            case 0:
                int beforeMoneyM = game.money;
                game.SellMales(value);
                totalMoneyEarned += (game.money - beforeMoneyM);
                break;

            case 1:
                int beforeMoneyF = game.money;
                game.SellFemales(value);
                totalMoneyEarned += (game.money - beforeMoneyF);
                break;

            case 2:
                int maxFood = game.money / 2;
                value = Mathf.Min(value, maxFood);
                food.AddFood(value, game.month);
                game.money -= value * 2;
                break;

            case 3:
                int beforeBabies = game.newMales + game.newFemales;
                game.Coupling(value);
                int afterBabies = game.newMales + game.newFemales;

                totalHamstersBorn += (afterBabies - beforeBabies);

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
            totalDeaths += deaths;

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
        CheckDefeat();
    }

    void CheckDefeat()
    {
        int total =
            game.hamsterMaleAdult +
            game.hamsterFemaleAdult +
            game.hamsterMaleBaby +
            game.hamsterFemaleBaby;

        if (total <= 0)
        {
            ShowDefeat();
        }
    }

    void ShowDefeat()
    {
        isGameOver = true;

        mainPanel.SetActive(false);
        StartCoroutine(animator.Show(defeatPanel));

        defeatText.text =
            "GAME OVER\n\n" +
            "Reached months : " + game.month + "\n" +
            "Total money earned : " + totalMoneyEarned + "$\n" +
            "Births : " + totalHamstersBorn + "\n" +
            "Deaths : " + totalDeaths;
    }

    void UpdateSummary()
    {
        summaryText.text =
            "MONTH : " + game.month + "\n\n" +
            "MALES : " + game.hamsterMaleAdult + "\n" +
            "FEMALES : " + game.hamsterFemaleAdult + "\n" +
            "BABIES : " + "\n" +
            "MALES : " + game.hamsterMaleBaby + "FEMALES : " + game.hamsterFemaleBaby + "\n\n" +
            "FOOD : " + food.GetFoodCount() + "\n" +
            "MONEY : " + game.money + "$";
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(0);
    }
}