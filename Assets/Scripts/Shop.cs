using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject[] Trinkets;
    public GameObject[] Cards;
    public GameObject[] HoloCards;
    public Transform[] TrinketLocations;
    public Transform[] CardLocations;
    public Sprite[] messages;
    public GameObject ShopButtons;
    private GameObject MainCam;
    private MemoryCard Mem;
    private GameObject Canvas;

    void Start()
    {
        MainCam = GameObject.FindGameObjectWithTag("MainCamera");
        Mem = GameObject.Find("MemoryCard").GetComponent<MemoryCard>();
        Canvas = GameObject.Find("Canvas");
        int rando = Random.Range(0, messages.Length);
        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = messages[rando];
        //Trinkets



        //Cards

        var cardsLength = Cards.Length;
        var HoloCardsLength = HoloCards.Length;

        for (int i = 0; i < CardLocations.Length; i++)
        {
            int diceRoll = Random.Range(0, 50);
            if (diceRoll == 0f)
            {
                int secondDiceRoll = Random.Range(0, HoloCardsLength);
                var holoCardInstance = Instantiate(HoloCards[secondDiceRoll], CardLocations[i]);
                holoCardInstance.transform.localPosition = Vector3.zero;
                if (i <= 5)
                {
                    var button = ShopButtons.transform.GetChild(i).GetChild(0);
                    button.gameObject.GetComponent<TextMeshProUGUI>().text = "$" + holoCardInstance.GetComponent<CardEffects>().Cost.ToString();
                }
                else if (i == 6)
                {
                    var button = ShopButtons.transform.GetChild(9).GetChild(0);
                    holoCardInstance.GetComponent<CardEffects>().Cost = holoCardInstance.GetComponent<CardEffects>().Cost / 2;
                    button.gameObject.GetComponent<TextMeshProUGUI>().text = "$" + holoCardInstance.GetComponent<CardEffects>().Cost.ToString();
                }
            }
            else
            {
                int secondDiceRoll = Random.Range(0, cardsLength);
                var CardInstance = Instantiate(Cards[secondDiceRoll], CardLocations[i]);
                CardInstance.transform.localPosition = Vector3.zero;
                if (i <= 5)
                {
                    var button = ShopButtons.transform.GetChild(i).GetChild(0);
                    button.gameObject.GetComponent<TextMeshProUGUI>().text = "$" + CardInstance.GetComponent<CardEffects>().Cost.ToString();
                }
                else if (i == 6)
                {
                    var button = ShopButtons.transform.GetChild(9).GetChild(0);
                    CardInstance.GetComponent<CardEffects>().Cost = CardInstance.GetComponent<CardEffects>().Cost / 2;
                    button.gameObject.GetComponent<TextMeshProUGUI>().text = "$" + CardInstance.GetComponent<CardEffects>().Cost.ToString();
                }
            }
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().justHit = false;
            ShopButtons.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        } 
    }

    public void CardButton1()
    {
        var card = CardLocations[0].GetChild(0).gameObject;
        if (Mem.gems >= card.GetComponent<CardEffects>().Cost)
        {
            card.transform.parent = MainCam.transform.GetChild(1);
            card.transform.localPosition = Vector3.zero;
            var button = ShopButtons.transform.GetChild(0).GetChild(0);
            button.gameObject.GetComponent<TextMeshProUGUI>().text = "Sold American";
            Mem.gems -= card.GetComponent<CardEffects>().Cost;
            Canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Mem.gems.ToString();
        }
    }

    public void CardButton2()
    {
        var card = CardLocations[1].GetChild(0).gameObject;
        if (Mem.gems >= card.GetComponent<CardEffects>().Cost)
        {
            card.transform.parent = MainCam.transform.GetChild(1);
            card.transform.localPosition = Vector3.zero;
            var button = ShopButtons.transform.GetChild(1).GetChild(0);
            button.gameObject.GetComponent<TextMeshProUGUI>().text = "Sold American";
            Mem.gems -= card.GetComponent<CardEffects>().Cost;
            Canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Mem.gems.ToString();
        }
    }

    public void CardButton3()
    {
        var card = CardLocations[2].GetChild(0).gameObject;
        if (Mem.gems >= card.GetComponent<CardEffects>().Cost)
        {
            card.transform.parent = MainCam.transform.GetChild(1);
            card.transform.localPosition = Vector3.zero;
            var button = ShopButtons.transform.GetChild(2).GetChild(0);
            button.gameObject.GetComponent<TextMeshProUGUI>().text = "Sold American";
            Mem.gems -= card.GetComponent<CardEffects>().Cost;
            Canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Mem.gems.ToString();
        }
    }

    public void CardButton4()
    {
        var card = CardLocations[3].GetChild(0).gameObject;
        if (Mem.gems >= card.GetComponent<CardEffects>().Cost)
        {
            card.transform.parent = MainCam.transform.GetChild(1);
            card.transform.localPosition = Vector3.zero;
            var button = ShopButtons.transform.GetChild(3).GetChild(0);
            button.gameObject.GetComponent<TextMeshProUGUI>().text = "Sold American";
            Mem.gems -= card.GetComponent<CardEffects>().Cost;
            Canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Mem.gems.ToString();
        }
    }

    public void CardButton5()
    {
        var card = CardLocations[4].GetChild(0).gameObject;
        if (Mem.gems >= card.GetComponent<CardEffects>().Cost)
        {
            card.transform.parent = MainCam.transform.GetChild(1);
            card.transform.localPosition = Vector3.zero;
            var button = ShopButtons.transform.GetChild(4).GetChild(0);
            button.gameObject.GetComponent<TextMeshProUGUI>().text = "Sold American";
            Mem.gems -= card.GetComponent<CardEffects>().Cost;
            Canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Mem.gems.ToString();
        }
    }

    public void CardButton6()
    {
        var card = CardLocations[5].GetChild(0).gameObject;
        if (Mem.gems >= card.GetComponent<CardEffects>().Cost)
        {
            card.transform.parent = MainCam.transform.GetChild(1);
            card.transform.localPosition = Vector3.zero;
            var button = ShopButtons.transform.GetChild(5).GetChild(0);
            button.gameObject.GetComponent<TextMeshProUGUI>().text = "Sold American";
            Mem.gems -= card.GetComponent<CardEffects>().Cost;
            Canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Mem.gems.ToString();
        }
    }

    public void CardButtonDiscount()
    {
        var card = CardLocations[6].GetChild(0).gameObject;
        if (Mem.gems >= card.GetComponent<CardEffects>().Cost)
        {
            card.transform.parent = MainCam.transform.GetChild(1);
            card.transform.localPosition = Vector3.zero;
            var button = ShopButtons.transform.GetChild(9).GetChild(0);
            button.gameObject.GetComponent<TextMeshProUGUI>().text = "Sold American";
            Mem.gems -= card.GetComponent<CardEffects>().Cost;
            Canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Mem.gems.ToString();
        }
    }
}
