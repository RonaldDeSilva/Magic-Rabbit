using UnityEngine;
using UnityEngine.SceneManagement;

public class CardMenu : MonoBehaviour
{
    private GameObject SelectedCard;
    public GameObject Deck;
    private GameObject MemoryCard;
    public GameObject Selector;
    private float delay;
    private int position = 0;
    private int numCards = 0;
    public Transform[] deckPositions;

    private void Start()
    {
        MemoryCard = GameObject.FindGameObjectWithTag("MemoryCard");
    }

    void Update()
    {
        if (SelectedCard != Deck.transform.GetChild(position).gameObject)
        {
            SelectedCard = Deck.transform.GetChild(position).gameObject;
            Selector.transform.position = SelectedCard.transform.position;
        }

        if (delay < 0 && numCards < 14)
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                if (Input.GetAxis("Horizontal") > 0)
                {
                    position++;
                    delay = 0.2f;
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    position--;
                    delay = 0.2f;
                }
            }

            if (Input.GetAxis("Jump") != 0)
            {
                var newCard = Instantiate(SelectedCard, MemoryCard.transform, true);
                newCard.transform.position = deckPositions[numCards].position;
                numCards++;
                delay = 0.2f;
            }

            if (numCards >= 4)
            {
                if (Input.GetAxis("Fire1") > 0)
                {
                    var len = MemoryCard.transform.childCount;
                    for (int i = len - 1; i >= 0; i--)
                    {
                        MemoryCard.transform.GetChild(i).transform.localPosition = Vector3.zero;
                    }
                    SceneManager.LoadScene("New Demo");
                }
            }
        }
        delay -= Time.deltaTime;

        if (position > Deck.transform.childCount - 1)
        {
            position = 0;
        }
        else if (position < 0)
        {
            position = 12;
        }
    }
}
