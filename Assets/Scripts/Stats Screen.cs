using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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
    public int CardSelectionScreen;

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
            for (int i = 0; i <= 4; i++)
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
        if (Meme.rabbitFeet <= 0)
        {
            for (int i = Buttons.transform.childCount - 1; i >= 0; i--)
            {
                Buttons.transform.GetChild(i).GetComponent<Button>().enabled = false;
            }
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
        var cardshuff = PlayerPrefs.GetInt("CardShuffleTime");
        for (int i = 0; i <= 9; i++)
        {
            if (cardshuff > 0)
            {
                CardShuffleTimeArray[i].SetActive(true);
                cardshuff--;
            }
            else
            {
                CardShuffleTimeArray[i].SetActive(false);
            }
        }

        Meme.rabbitFeet -= 1;
    }
    public void MaxHealthButton()
    {
        MaxHealth += 1;
        PlayerPrefs.SetInt("MaxHealth", MaxHealth);
        var cardshuff = PlayerPrefs.GetInt("MaxHealth");
        for (int i = 0; i <= 9; i++)
        {
            if (cardshuff > 0)
            {
                MaxHealthArray[i].SetActive(true);
                cardshuff--;
            }
            else
            {
                MaxHealthArray[i].SetActive(false);
            }
        }

        Meme.rabbitFeet -= 1;
    }
    public void BaseDamageButton()
    {
        BaseDamage += 1;
        PlayerPrefs.SetInt("BaseDamage", BaseDamage);
        var cardshuff = PlayerPrefs.GetInt("BaseDamage");
        for (int i = 0; i <= 9; i++)
        {
            if (cardshuff > 0)
            {
                BaseDamageArray[i].SetActive(true);
                cardshuff--;
            }
            else
            {
                BaseDamageArray[i].SetActive(false);
            }
        }

        Meme.rabbitFeet -= 1;
    }
    public void LuckButton()
    {
        Luck += 1;
        PlayerPrefs.SetInt("Luck", Luck);
        var cardshuff = PlayerPrefs.GetInt("Luck");
        for (int i = 0; i <= 9; i++)
        {
            if (cardshuff > 0)
            {
                LuckArray[i].SetActive(true);
                cardshuff--;
            }
            else
            {
                LuckArray[i].SetActive(false);
            }
        }

        Meme.rabbitFeet -= 1;
    }
    public void StartingCardAmountButton()
    {
        StartingCardAmount += 1;
        PlayerPrefs.SetInt("StartingCardAmount", StartingCardAmount);
        var cardshuff = PlayerPrefs.GetInt("StartingCardAmount");
        for (int i = 0; i <= 4; i++)
        {
            if (cardshuff > 0)
            {
                StartingCardAmountArray[i].SetActive(true);
                cardshuff--;
            }
            else
            {
                StartingCardAmountArray[i].SetActive(false);
            }
        }

        Meme.rabbitFeet -= 1;
    }

    public void CardScreen()
    {
        SceneManager.LoadScene("Card Menu 2");
       
    }


}
