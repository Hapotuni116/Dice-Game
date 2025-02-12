using UnityEngine;


public class Game : MonoBehaviour
{
    [SerializeField] private Dice dice1;
    [SerializeField] private Dice dice2;

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

        bool DiceHaveStoppedRolling()
        {
            return dice1.GetNum() != 0 && dice2.GetNum() != 0;
        }

        int sum = dice1.GetNum() + dice2.GetNum();
        Debug.Log("Sum of both dice: " + sum);
    }
}
