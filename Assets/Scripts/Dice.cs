using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour
{
    [Header("Die Forces")]
    [SerializeField] private float torqueMinimum = 0.1f;
    [SerializeField] private float torqueMaximum = 21f;
    [SerializeField] private float throwStrength = 10f;

    private Transform _trans;
    private Rigidbody _rb;
    private int  _num;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        _trans = transform;
        _rb = GetComponent<Rigidbody>();

    }

    public void RollDice(){
        _rb.AddForce(Vector3.up *  throwStrength, ForceMode.Impulse);

        _rb.AddTorque(_trans.forward * Random.Range(torqueMinimum, torqueMaximum) + _trans.up * Random.Range(torqueMinimum, torqueMaximum)+ _trans.right * Random.Range(torqueMinimum, torqueMaximum));
        StartCoroutine(WaitForStop());
    }

    private IEnumerator WaitForStop(){

        yield return new WaitForFixedUpdate();
        while(_rb.angularVelocity.sqrMagnitude > 0.1){
            yield return new WaitForFixedUpdate();
        }
        CheckDice();
    }

    private void CheckDice(){
        float frontBack = Mathf.Round(Vector3.Dot(_trans.up.normalized, Vector3.up));
        float upDown = Mathf.Round(Vector3.Dot(_trans.forward.normalized, Vector3.up));
        float LeftRight = Mathf.Round(Vector3.Dot(_trans.right.normalized, Vector3.up));

        switch(upDown){
            case 1:
                Debug.Log("1");
                break;
            case -1:
                Debug.Log("6");
                break;
        }

        switch(LeftRight){
            case 1:
                Debug.Log("4");
                break;
            case -1:
                Debug.Log("3");
                break;
        }

        switch(frontBack){
            case 1:
                Debug.Log("5");
                break;
            case -1:
                Debug.Log("2");
                break;
        }
    }

}
