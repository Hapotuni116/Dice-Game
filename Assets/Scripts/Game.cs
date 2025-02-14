using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game : MonoBehaviour
{
    [SerializeField] private Dice dice1;
    [SerializeField] private Dice dice2;
    [SerializeField] private Text Point;
    [SerializeField] private Text End;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button rollButton;
    [SerializeField] private Text Turn;

    private int point = 0;
    private bool isFirstRoll = true;
    private int currentPlayer = 0; // 0 = player, 1-3 = bots
    private bool isEnd = false;

    void Start()
    {
        resetButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        Turn.text = "Your Turn";
    }
    public void RollBothDice()
    {
        if(!isEnd){
            dice1.RollDice();
            dice2.RollDice();
            StartCoroutine(CalculateSum());
        }
    }

    private IEnumerator CalculateSum()
    {
        // Wait until both dice have stopped rolling
        yield return new WaitUntil(DiceHaveStoppedRolling);
        yield return new WaitForSeconds(0.5f);
        bool DiceHaveStoppedRolling()
        {
            return dice1.GetNum() != 0 && dice2.GetNum() != 0;
        }

        int sum = dice1.GetNum() + dice2.GetNum();
        Debug.Log("Sum of both dice: " + sum);

        if (isFirstRoll)
        {
            if (sum == 7 || sum == 11)
            {
                End.text = currentPlayer == 0 ? "You win!" : "Bot " + currentPlayer + " wins!";
                Debug.Log(currentPlayer == 0 ? "You win!" : "Bot " + currentPlayer + " wins!");
                isEnd = true;
                ShowEndButtons();
            }
            else if (sum == 2 || sum == 3 || sum == 12)
            {
                End.text = currentPlayer == 0 ? "You lose!" : "Bot " + currentPlayer + " loses!";
                Debug.Log(currentPlayer == 0 ? "You lose!" : "Bot " + currentPlayer + " loses!");
                isEnd = true;
                ShowEndButtons();
            }
            else
            {
                point = sum;
                Point.text = "Point: " + point + "\nCurrent Roll: " + sum;
                isFirstRoll = false;
                Debug.Log("Point is set to: " + point);
            }
        }
        else
        {
            Point.text = "Point: " + point + "\nCurrent Roll: " + sum;
            if (sum == point)
            {
                End.text = currentPlayer == 0 ? "You win!" : "Bot " + currentPlayer + " wins!";
                Debug.Log(currentPlayer == 0 ? "You win!" : "Bot " + currentPlayer + " wins!");
                isEnd = true;
                ShowEndButtons();
            }
            else if (sum == 7)
            {
                End.text = currentPlayer == 0 ? "You lose!" : "Bot " + currentPlayer + " loses!";
                Debug.Log(currentPlayer == 0 ? "You lose!" : "Bot " + currentPlayer + " loses!");
                isEnd = true;
                ShowEndButtons();
            }
        }


        if(!isEnd){
            // Move to the next player
            currentPlayer = (currentPlayer + 1) % 4;
            if (currentPlayer != 0)
            {
                Turn.text = currentPlayer == 0 ? "Your Turn" : "Bot " + currentPlayer + "'s Turn";
                rollButton.gameObject.SetActive(false);
                StartCoroutine(BotTurn());
            }
            else
            {
                Turn.text = "Your Turn";
                rollButton.gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator BotTurn()
    {
        yield return new WaitForSeconds(1.0f); // Wait for a second before the bot rolls
        RollBothDice();
    }

    private void ShowEndButtons()
    {
        Turn.text = "";
        resetButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        rollButton.gameObject.SetActive(false);
    }

    public void ResetGame()
    {
        point = 0;
        isFirstRoll = true;
        Point.text = "Target: 7 or 11";
        End.text = "";
        resetButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        rollButton.gameObject.SetActive(true);
        currentPlayer = 0;
        isEnd = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
