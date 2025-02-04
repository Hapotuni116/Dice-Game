using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private GameObject dice;
    [SerializeField] private GameObject table;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dice.SetActive(false);
        table.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
