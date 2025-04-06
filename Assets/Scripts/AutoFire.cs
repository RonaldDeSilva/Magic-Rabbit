using UnityEngine;

public class AutoFire : MonoBehaviour
{
    private GameObject Deck;
    private GameObject Hand;

    private void Start()
    {
        Hand = GameObject.FindGameObjectWithTag("FakeCamera").transform.GetChild(0).gameObject;
        Deck = GameObject.FindGameObjectWithTag("FakeCamera").transform.GetChild(1).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var deckLength = Deck.transform.childCount;
            for (int i = deckLength - 1; i >= 0; i--)
            {
                Deck.transform.GetChild(i).GetComponent<CardEffects>().Cooldown = 0;
            }

            var HandLength = Deck.transform.childCount;
            for (int i = HandLength - 1; i >= 0; i--)
            {
                Hand.transform.GetChild(i).GetComponent<CardEffects>().Cooldown = 0;
            }

            collision.gameObject.GetComponent<Movement>().RapidFire = true;
        }
    }
}
