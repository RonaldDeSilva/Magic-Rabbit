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

    }
    public void MaxHealthButton()
    {
        MaxHealth += 1;
    }
    public void BaseDamageButton()
    {
        BaseDamage += 1;
    }
    public void LuckButton()
    {
        Luck += 1;
    }
    public void StartingCardAmountButton()
    {
        StartingCardAmount += 1;
    }
}
