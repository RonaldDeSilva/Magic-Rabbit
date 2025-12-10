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

    #region Start

    void Start()
    {
        Meme = GameObject.FindGameObjectWithTag("MemoryCard").GetComponent<MemoryCard>();
        var cardshuff = PlayerPrefs.GetInt("CardShuffleTime");
        CardShuffleTime = cardshuff;
        if (cardshuff == 0)
        {
            for (int i = 0; i <= 9; i++)
            {
                CardShuffleTimeArray[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i <= 9; i++)
            {
                if (cardshuff > 0)
                {
                    cardshuff--;
                }
                else
                {
                    CardShuffleTimeArray[i].SetActive(false);
                }
            }
        }

        var MaxHeals = PlayerPrefs.GetInt("MaxHealth");
        MaxHealth = MaxHeals;
        if (MaxHeals == 0)
        {
            for (int i = 0; i <= 9; i++)
            {
                MaxHealthArray[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i <= 9; i++)
            {
                if (MaxHeals > 0)
                {
                    MaxHeals--;
                }
                else
                {
                    MaxHealthArray[i].SetActive(false);
                }
            }
        }

        var BaseDam = PlayerPrefs.GetInt("BaseDamage");
        BaseDamage = BaseDam;
        if (BaseDam == 0)
        {
            for (int i = 0; i <= 9; i++)
            {
                BaseDamageArray[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i <= 9; i++)
            {
                if (BaseDam > 0)
                {
                    BaseDam--;
                }
                else
                {
                    BaseDamageArray[i].SetActive(false);
                }
            }
        }

        var Pluck = PlayerPrefs.GetInt("Luck");
        Luck = Pluck;
        if (Pluck == 0)
        {
            for (int i = 0; i <= 9; i++)
            {
                LuckArray[i].SetActive(false);
            }
            for (int i = 0; i <= 4; i++)
            {
                StartingCardAmountArray[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i <= 9; i++)
            {
                if (Pluck > 0)
                {
                    Pluck--;
                }
                else
                {
                    LuckArray[i].SetActive(false);
                }
            }
        }

        var StartCard = PlayerPrefs.GetInt("StartingCardAmount");
        StartingCardAmount = StartCard;
        if (StartCard == 0)
        {
            for (int i = 0; i <= 4; i++)
            {
                StartingCardAmountArray[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i <= 9; i++)
            {
                if (StartCard > 0)
                {
                    StartCard--;
                }
                else
                {
                    StartingCardAmountArray[i].SetActive(false);
                }
            }
        }
    }

    #endregion

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
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        
    }
    public void CardShuffleTimeButton()
    {
        CardShuffleTime += 1;

        PlayerPrefs.SetInt("CardShuffleTime", CardShuffleTime);

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
        PlayerPrefs.SetInt("MaxHealth", MaxHealth);

        if (MaxHealth >= 10)
        {
            MaxHealth = 10;
            Buttons.SetActive(false);
        }
    }
    public void BaseDamageButton()
    {
        BaseDamage += 1;
        PlayerPrefs.SetInt("BaseDamage", BaseDamage);

        if (BaseDamage >= 10)
        {
            BaseDamage = 10;
            Buttons.SetActive(false);
        }
    }
    public void LuckButton()
    {
        Luck += 1;
        PlayerPrefs.SetInt("Luck", Luck);

        if (Luck >= 10)
        {
            Luck = 10;
            Buttons.SetActive(false);
        }
    }
    public void StartingCardAmountButton()
    {
        StartingCardAmount += 1;
        PlayerPrefs.SetInt("StartingCardAmount", StartingCardAmount);

        if (StartingCardAmount >= 5)
        {
            StartingCardAmount = 5;
            Buttons.SetActive(false);
        }
    }
}
