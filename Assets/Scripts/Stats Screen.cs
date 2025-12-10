using UnityEngine;
using System.Collections;


public class StatsScreen : MonoBehaviour

{
    public int CardShuffleTime;
    public int MaxHealth;
    public int BaseDamage;
    public int Luck;
    public int StartingCardAmount;
    public MemoryCard Meme;
    public GameObject Buttons;
    public GameObject[] CardShuffleTimeArray;
    public GameObject[] MaxHealthArray;
    public GameObject[] BaseDamageArray;
    public GameObject[] LuckArray;
    public GameObject[] StartingCardAmountArray;
    



    void Start()
    {
        Meme = GameObject.FindGameObjectWithTag("MemoryCard").GetComponent<MemoryCard>();
    }


    void Update()
    {
        if (Meme.rabbitFeet >= 2)
        {
            Buttons.SetActive(true);
        }
        else
        {
            Buttons.SetActive(false);
        }
        
        
    }
    public void CardShuffleTimeButton()
    {
        CardShuffleTime += 1;

        //If CardShuffleTime is greater than 10, set it to 10 and disable the buttons
        if (CardShuffleTime >= 10)
        {
            CardShuffleTime = 10;
            Buttons.SetActive(false);
        }

    }
    public void MaxHealthButton()
    {
        MaxHealth += 1;

        if (MaxHealth >= 10)
        {
            MaxHealth = 10;
            Buttons.SetActive(false);
        }
    }
    public void BaseDamageButton()
    {
        BaseDamage += 1;

        if (BaseDamage >= 10)
        {
            BaseDamage = 10;
            Buttons.SetActive(false);
        }
    }
    public void LuckButton()
    {
        Luck += 1;

        if (Luck >= 10)
        {
            Luck = 10;
            Buttons.SetActive(false);
        }
    }
    public void StartingCardAmountButton()
    {
        StartingCardAmount += 1;

        if (StartingCardAmount >= 5)
        {
            StartingCardAmount = 5;
            Buttons.SetActive(false);
        }
    }
}
