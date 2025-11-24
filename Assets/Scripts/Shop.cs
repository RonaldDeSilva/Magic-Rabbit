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

    void Start()
    {
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
            }
            else
            {
                int secondDiceRoll = Random.Range(0, cardsLength);
                var CardInstance = Instantiate(Cards[secondDiceRoll], CardLocations[i]);
                CardInstance.transform.localPosition = Vector3.zero;
            }
        }

    }

}
