using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    #region Card Numbers and what they mean

    //1 = Splash - projectile that makes enemy wet and vulnerable to electricity and freeze ---------------------------------------------------------------------------

    //2 = Dove - Rising projectile that can be used as a platform-------------------------------------------------------------------------------------------------------

    //3 = Wild Growth - AOE around player which damages enemies and increases players movement speed and jump height----------------------------------------------------

    //4 = Stone Form - causes player to plummet and create an AOE which stuns enemies, also can break through breakable blocks-----------------------------------------

    //5 = Zephyr - Lightening cloud that moves up and back and forth while striking the ground causing wet enemies to be stunned---------------------------------------

    //6 = Combust - fireball which causes DOT on enemy, as well as exploding poison clouds, cancels out wet and frozen------------------------------------------------

    //7 = Poison Cloud - ball of poison which turns into AOE poison cloud on impact, causes explosion when interacting-----------------------------------------------
    //with combust or burning enemies cancelling out wet and frozen

    //8 = Cone Of Cold - Freezing cone that freezes and damages enemies, doing more damage and freezing longer when they are wet---------------------------------------

    //9 = Might - A flurry of blows which stuns the enemy shortly and slightly pushes them away and up---------------------------------------------------------------

    //10 = Card Trick - Makes character invulnerable for 1.5 seconds------------------------------------------------------------------------------------------------

    //11 = Blink - teleport------------------------------------------------------------------------------------------------

    #endregion


    #region Attributes

    public float speed;
    public float jumpHeight;
    public int maxHealth;
    private int curHealth;
    private float CooldownTime = 1f;

    public Rigidbody2D rb;
    public Color StartingColor;
    private GameObject Hand;
    private GameObject Deck;
    public GameObject[] Attacks;

    private bool jumped = false;
    public bool canJump = false;
    private bool UsingCard = false;
    public bool turnedRight = true;
    public bool StoneForm = false;
    public bool invulnerable = false;
    public bool dashing = false;

    #endregion

    #region Start
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Hand = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).gameObject;
        Deck = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(1).gameObject;
        curHealth = maxHealth;
        StartingColor = GetComponent<SpriteRenderer>().color;
        DealCards();
    }
    #endregion

    #region Update
    void Update()
    {
        #region Input

        if (!StoneForm & !dashing)
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                rb.linearVelocityX = speed * Input.GetAxis("Horizontal");

                if (Input.GetAxis("Horizontal") > 0)
                {
                    turnedRight = true;
                    GetComponent<SpriteRenderer>().flipX = false;
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    turnedRight = false;
                    GetComponent<SpriteRenderer>().flipX = true;
                }
            }
            else if (Input.GetAxis("Horizontal") == 0)
            {
                rb.linearVelocityX = 0;
            }

            if (Input.GetAxis("Jump") > 0)
            {
                if (!jumped && canJump)
                {
                    rb.AddForceY(jumpHeight);
                    jumped = true;
                }
            }

            if (Input.GetAxis("Fire1") > 0 && !UsingCard)
            {
                UseCard();
                UsingCard = true;
            }
        }

        #endregion
    }
    #endregion

    #region Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GroundBreakable") || collision.gameObject.CompareTag("GroundMoveable"))
        {
            jumped = false;
            if (StoneForm && collision.gameObject.CompareTag("GroundBreakable"))
            {
                Destroy(collision.gameObject);
            }
        }
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!invulnerable)
            {
                curHealth -= collision.gameObject.GetComponent<Enemy>().bumpDamage;
                CheckHealth();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GroundBreakable") || collision.gameObject.CompareTag("GroundMoveable"))
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GroundBreakable") || collision.gameObject.CompareTag("GroundMoveable"))
        {
            canJump = false;
        }
    }
    #endregion

    #region Card Stuff
    private void DealCards()
    {
        while (Deck.transform.childCount != 0)
        {
            var children = Deck.transform.childCount;
            var num = Random.Range(0, children);
            Deck.transform.GetChild(num).parent = Hand.transform;
        }
        Hand.transform.GetChild(0).localPosition = new Vector3(6.74f, -3.73f, 0);
        Hand.transform.GetChild(1).localPosition = new Vector3(8.11f, -3.73f, 0);
    }

    private void UseCard()
    {
        var CurCard = Hand.transform.GetChild(0);
        
        if (CurCard.GetComponent<CardEffects>().CardNum == 1) //Splash
        {
            if (turnedRight)
            {
                Instantiate(Attacks[0], new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), this.transform.rotation);
            }
            else if (!turnedRight)
            {
                Instantiate(Attacks[0], new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), this.transform.rotation);
            }
        }
        else if (CurCard.GetComponent<CardEffects>().CardNum == 2) //Dove
        {
            if (turnedRight)
            {
                Instantiate(Attacks[1], new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z), this.transform.rotation);
            }
            else if (!turnedRight)
            {
                Instantiate(Attacks[1], new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z), this.transform.rotation);
            }
        }
        else if (CurCard.GetComponent<CardEffects>().CardNum == 3) //Wild Growth
        {
             Instantiate(Attacks[2], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
        }
        else if (CurCard.GetComponent<CardEffects>().CardNum == 4) //Stone Form
        {
            Instantiate(Attacks[3], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
        }
        else if (CurCard.GetComponent<CardEffects>().CardNum == 5) //Zephyr
        {
            if (turnedRight)
            {
                Instantiate(Attacks[4], new Vector3(transform.position.x + 2.5f, transform.position.y + 1, transform.position.z), Attacks[4].transform.rotation);
            }
            else if (!turnedRight)
            {
                Instantiate(Attacks[4], new Vector3(transform.position.x - 2.5f, transform.position.y + 1, transform.position.z), Attacks[4].transform.rotation);
            }
        }
        else if (CurCard.GetComponent<CardEffects>().CardNum == 6) //Combust
        {
            if (turnedRight)
            {
                Instantiate(Attacks[5], new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), this.transform.rotation);
            }
            else if (!turnedRight)
            {
                Instantiate(Attacks[5], new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), this.transform.rotation);
            }
        }
        else if (CurCard.GetComponent<CardEffects>().CardNum == 7) // Poison Cloud
        {
            if (turnedRight)
            {
                Instantiate(Attacks[6], new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), this.transform.rotation);
            }
            else if (!turnedRight)
            {
                Instantiate(Attacks[6], new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), this.transform.rotation);
            }
        }
        else if (CurCard.GetComponent<CardEffects>().CardNum == 8) //Cone of Cold
        {
            if (turnedRight)
            {
                Instantiate(Attacks[7], new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z), Attacks[7].transform.rotation);
            }
            else if (!turnedRight)
            {
                Instantiate(Attacks[7], new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z), Attacks[7].transform.rotation);
            }
        }
        else if (CurCard.GetComponent<CardEffects>().CardNum == 9) //Might
        {
            if (turnedRight)
            {
                Instantiate(Attacks[8], new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z), Attacks[7].transform.rotation);
            }
            else if (!turnedRight)
            {
                Instantiate(Attacks[8], new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z), Attacks[7].transform.rotation);
            }
        }
        else if (CurCard.GetComponent<CardEffects>().CardNum == 10) //Card Trick
        {
            Instantiate(Attacks[9], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
        }
        else if (CurCard.GetComponent<CardEffects>().CardNum == 11) //Blink
        {
            Instantiate(Attacks[10], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
        }
        else if (CurCard.GetComponent<CardEffects>().CardNum == 12) //Dash
        {
            Instantiate(Attacks[11], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation, transform);
        }

        #region Magic 8 Ball lol

        else if (CurCard.GetComponent<CardEffects>().CardNum == 13) //Magic 8 Ball
        {
            var newCard = new GameObject();
            if (Hand.transform.childCount > 1 && Deck.transform.childCount > 0)
            {
                var totalCards = Deck.transform.childCount + Hand.transform.childCount;
                var num = Random.Range(1, totalCards);
                if (num < Hand.transform.childCount)
                {
                    newCard = Hand.transform.GetChild(num).gameObject;
                    if (newCard.GetComponent<CardEffects>().CardNum == 13)
                    {
                        while (newCard.GetComponent<CardEffects>().CardNum == 13)
                        {
                            num = Random.Range(1, totalCards);
                            if (num < Hand.transform.childCount)
                            {
                                newCard = Hand.transform.GetChild(num).gameObject;
                            }
                            else
                            {
                                newCard = Deck.transform.GetChild((num - Hand.transform.childCount)).gameObject;
                            }  
                        }
                    }
                }
                else
                {
                    newCard = Deck.transform.GetChild((num - Hand.transform.childCount)).gameObject;
                    if (newCard.GetComponent<CardEffects>().CardNum == 13)
                    {
                        while (newCard.GetComponent<CardEffects>().CardNum == 13)
                        {
                            num = Random.Range(1, totalCards);
                            if (num < Hand.transform.childCount)
                            {
                                newCard = Hand.transform.GetChild(num).gameObject;
                            }
                            else
                            {
                                newCard = Deck.transform.GetChild((num - Hand.transform.childCount)).gameObject;
                            }
                        }
                    }
                }
            }
            else if (Hand.transform.childCount == 1 && Deck.transform.childCount > 0)
            {
                var totalCards = Deck.transform.childCount;
                var num = Random.Range(0, totalCards);
                newCard = Deck.transform.GetChild(num).gameObject;
                if (newCard.GetComponent<CardEffects>().CardNum == 13)
                {
                    while (newCard.GetComponent<CardEffects>().CardNum == 13)
                    {
                        num = Random.Range(0, totalCards);
                        newCard = Deck.transform.GetChild(num).gameObject;
                    }
                }
            }
            else if (Hand.transform.childCount > 1 && Deck.transform.childCount == 0)
            {
                var totalCards = Hand.transform.childCount;
                var num = Random.Range(1, totalCards);
                newCard = Hand.transform.GetChild(num).gameObject;
                if (newCard.GetComponent<CardEffects>().CardNum == 13)
                {
                    while (newCard.GetComponent<CardEffects>().CardNum == 13)
                    {
                        num = Random.Range(1, totalCards);
                        newCard = Hand.transform.GetChild(num).gameObject;
                    }
                }
            }

            if (newCard.GetComponent<CardEffects>().CardNum == 1) //Splash
            {
                if (turnedRight)
                {
                    Instantiate(Attacks[0], new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), this.transform.rotation);
                }
                else if (!turnedRight)
                {
                    Instantiate(Attacks[0], new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), this.transform.rotation);
                }
            }
            else if (newCard.GetComponent<CardEffects>().CardNum == 2) //Dove
            {
                if (turnedRight)
                {
                    Instantiate(Attacks[1], new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z), this.transform.rotation);
                }
                else if (!turnedRight)
                {
                    Instantiate(Attacks[1], new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z), this.transform.rotation);
                }
            }
            else if (newCard.GetComponent<CardEffects>().CardNum == 3) //Wild Growth
            {
                Instantiate(Attacks[2], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
            }
            else if (newCard.GetComponent<CardEffects>().CardNum == 4) //Stone Form
            {
                Instantiate(Attacks[3], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
            }
            else if (newCard.GetComponent<CardEffects>().CardNum == 5) //Zephyr
            {
                if (turnedRight)
                {
                    Instantiate(Attacks[4], new Vector3(transform.position.x + 2.5f, transform.position.y + 1, transform.position.z), Attacks[4].transform.rotation);
                }
                else if (!turnedRight)
                {
                    Instantiate(Attacks[4], new Vector3(transform.position.x - 2.5f, transform.position.y + 1, transform.position.z), Attacks[4].transform.rotation);
                }
            }
            else if (newCard.GetComponent<CardEffects>().CardNum == 6) //Combust
            {
                if (turnedRight)
                {
                    Instantiate(Attacks[5], new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), this.transform.rotation);
                }
                else if (!turnedRight)
                {
                    Instantiate(Attacks[5], new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), this.transform.rotation);
                }
            }
            else if (newCard.GetComponent<CardEffects>().CardNum == 7) // Poison Cloud
            {
                if (turnedRight)
                {
                    Instantiate(Attacks[6], new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), this.transform.rotation);
                }
                else if (!turnedRight)
                {
                    Instantiate(Attacks[6], new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), this.transform.rotation);
                }
            }
            else if (newCard.GetComponent<CardEffects>().CardNum == 8) //Cone of Cold
            {
                if (turnedRight)
                {
                    Instantiate(Attacks[7], new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z), Attacks[7].transform.rotation);
                }
                else if (!turnedRight)
                {
                    Instantiate(Attacks[7], new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z), Attacks[7].transform.rotation);
                }
            }
            else if (newCard.GetComponent<CardEffects>().CardNum == 9) //Might
            {
                if (turnedRight)
                {
                    Instantiate(Attacks[8], new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z), Attacks[7].transform.rotation);
                }
                else if (!turnedRight)
                {
                    Instantiate(Attacks[8], new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z), Attacks[7].transform.rotation);
                }
            }
            else if (newCard.GetComponent<CardEffects>().CardNum == 10) //Card Trick
            {
                Instantiate(Attacks[9], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
            }
            else if (newCard.GetComponent<CardEffects>().CardNum == 11) //Blink
            {
                Instantiate(Attacks[10], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
            }
            else if (newCard.GetComponent<CardEffects>().CardNum == 12) //Dash
            {
                Instantiate(Attacks[11], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation, transform);
            }
        }
        #endregion
        CurCard.transform.parent = Deck.transform;
        CurCard.transform.localPosition = Vector3.zero;
        CooldownTime = CurCard.GetComponent<CardEffects>().Cooldown;
        StartCoroutine("Cooldown");

        if (Hand.transform.childCount == 0)
        {
            DealCards();
        }
        else if (Hand.transform.childCount == 1)
        {
            Hand.transform.GetChild(0).localPosition = new Vector3(6.74f, -3.73f, 0);
        }
        else if (Hand.transform.childCount >= 2)
        {
            Hand.transform.GetChild(0).localPosition = new Vector3(6.74f, -3.73f, 0);
            Hand.transform.GetChild(1).localPosition = new Vector3(8.11f, -3.73f, 0);
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(CooldownTime);
        UsingCard = false;
    }
    #endregion

    private void CheckHealth()
    {
        if (curHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
