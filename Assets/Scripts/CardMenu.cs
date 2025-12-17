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
    private int numOfCardSlots;
    public GameObject ToolTip;
    public Sprite[] sprites;

    private void Start()
    {
        MemoryCard = GameObject.FindGameObjectWithTag("MemoryCard");
        numOfCardSlots = 5 + PlayerPrefs.GetInt("StartingCardAmount");
        for (int i = MemoryCard.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(MemoryCard.transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        
        if (SelectedCard != Deck.transform.GetChild(position).gameObject)
        {
            SelectedCard = Deck.transform.GetChild(position).gameObject;
            Selector.transform.position = SelectedCard.transform.position;
        }

        if (delay < 0 && numCards < numOfCardSlots)
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

            if (Input.GetAxis("Vertical") != 0 && delay < 0f)
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    position = position - 7;
                    delay = 0.2f;
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    position = position + 7;
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
        }

        if (numCards >= 5)
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
        delay -= Time.deltaTime;

        if (position > Deck.transform.childCount - 1)
        {
            position = position - (Deck.transform.childCount - 1);
        }
        else if (position < 0)
        {
            position = (Deck.transform.childCount - 1) + position;
        }

        ToolTip.GetComponent<SpriteRenderer>().sprite = sprites[position];

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }

        if (Input.GetButton("StartButton"))
        {
            SceneManager.LoadScene("Card Menu 2");
        }
    }
}
