using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game : MonoBehaviour
{
    [SerializeField] private Dice dice1;
    [SerializeField] private Dice dice2;
    [SerializeField] private Text Point;

    public void RollBothDice()
    {
        dice1.RollDice();
        dice2.RollDice();
        StartCoroutine(CalculateSum());
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
        Point.text = "Target = " + sum;
    }
}

//7 or 11 equals win on first roll
//2, 3, 12 equals lose on first roll
//after first roll, that becomes the point
//roll the point to win
//roll a 7 to lose
