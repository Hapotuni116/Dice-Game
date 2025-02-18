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

    /// <summary>
    /// This function is called when the script instance is being loaded.
    /// It initializes the game state and UI elements.
    /// </summary>
    void Start()
    {
        resetButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        Turn.text = "Your Turn";
    }

    /// <summary>
    /// Rolls both dice and starts the coroutine to calculate the sum.
    /// </summary>
    public void RollBothDice()
    {
        if(!isEnd){
            dice1.RollDice();
            dice2.RollDice();
            StartCoroutine(CalculateSum());
        }
    }

    /// <summary>
    /// Coroutine that waits for both dice to stop rolling and calculates the sum.
    /// Determines the game outcome based on the sum.
    /// </summary>
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

    /// <summary>
    /// Coroutine that simulates the bot's turn by waiting for a second before rolling the dice.
    /// </summary>
    private IEnumerator BotTurn()
    {
        yield return new WaitForSeconds(1.0f); // Wait for a second before the bot rolls
        RollBothDice();
    }

    /// <summary>
    /// Shows the end game buttons (reset and exit) and hides the roll button.
    /// </summary>
    private void ShowEndButtons()
    {
        Turn.text = "";
        resetButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        rollButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Resets the game state to the initial conditions.
    /// </summary>
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

    /// <summary>
    /// Exits the game application.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
