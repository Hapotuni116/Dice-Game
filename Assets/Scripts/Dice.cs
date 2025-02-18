using UnityEngine;
using System.Collections;

/// <summary>
/// The Dice class is responsible for simulating the behavior of a dice roll in the game.
/// It applies forces and torques to the dice to simulate a realistic roll and determines the result.
/// </summary>
public class Dice : MonoBehaviour
{
    [Header("Die Forces")]
    [SerializeField] private float torqueMinimum = 0.1f;
    [SerializeField] private float torqueMaximum = 10f;
    [SerializeField] private float throwStrength = 10f;
    
    private Transform _trans;
    private Rigidbody _rb;
    private int _num;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// It initializes the transform and rigidbody components.
    /// </summary>
    private void OnEnable()
    {
        _trans = transform;
        _rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Rolls the dice by applying an upward force and random torques.
    /// Starts a coroutine to wait for the dice to stop rolling.
    /// </summary>
    public void RollDice()
    {
        _num = 0;
        _rb.AddForce(Vector3.up * throwStrength, ForceMode.Impulse);
        _rb.AddTorque(_trans.forward * Random.Range(torqueMinimum, torqueMaximum) + 
                      _trans.up * Random.Range(torqueMinimum, torqueMaximum) + 
                      _trans.right * Random.Range(torqueMinimum, torqueMaximum));
        StartCoroutine(WaitForStop());
    }

    /// <summary>
    /// Coroutine that waits for the dice to stop rolling.
    /// Once the dice stops, it checks the result.
    /// </summary>
    private IEnumerator WaitForStop()
    {
        yield return new WaitForFixedUpdate();
        while (_rb.angularVelocity.sqrMagnitude > 0.1)
        {
            yield return new WaitForFixedUpdate();
        }
        CheckDice();
    }

    /// <summary>
    /// Checks the orientation of the dice to determine the result.
    /// Sets the _num variable based on the dice's final orientation.
    /// </summary>
    private void CheckDice()
    {
        float frontBack = Mathf.Round(Vector3.Dot(_trans.up.normalized, Vector3.up));
        float upDown = Mathf.Round(Vector3.Dot(_trans.forward.normalized, Vector3.up));
        float LeftRight = Mathf.Round(Vector3.Dot(_trans.right.normalized, Vector3.up));

        switch (upDown)
        {
            case 1:
                _num = 1;
                break;
            case -1:
                _num = 6;
                break;
        }

        switch (LeftRight)
        {
            case 1:
                _num = 4;
                break;
            case -1:
                _num = 3;
                break;
        }

        switch (frontBack)
        {
            case 1:
                _num = 5;
                break;
            case -1:
                _num = 2;
                break;
        }
    }

    /// <summary>
    /// Returns the result of the dice roll.
    /// </summary>
    /// <returns>The number on the top face of the dice.</returns>
    public int GetNum()
    {
        return _num;
    }
}
