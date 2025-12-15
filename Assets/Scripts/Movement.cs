using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    #region Card Numbers and what they mean

    //1 = Splash - stream that makes enemy wet and vulnerable to electricity and freeze ---------------------------------------------------------------------------

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

    //11 = Blink - teleport-----------------------------------------------------------------------------------------------------------------------------------------

    //12 = Dash - Dash----------------------------------------------------------------------------------------------------------------------------------------------

    //13 = Magic 8 Ball - Randomly chooses a card from the deck and does that effect of the card----------------------------------------------------------------------

    //14 = Bee Hive - Spawns bees which swarm and attack enemies----------------------------------------------------------------------------------------------------------------

    #endregion


    #region Attributes

    public float speed;
    public float jumpHeight;
    public int maxHealth;
    public int curHealth;
    private float CooldownTime = 1f;
    public float knockback;
    public float EnemyKnockback;
    public float shuffleTime;

    public Rigidbody2D rb;
    public Color StartingColor;
    private GameObject Hand;
    private GameObject Deck;
    public GameObject[] Attacks;
    public GameObject[] HollowAttacks;
    public GameObject DamageNumbers;
    private GameObject MemoryCard;
    private GameObject hat;

    private bool jumped = false;
    public bool canJump = false;
    private bool UsingCard = false;
    public bool turnedRight = true;
    public bool StoneForm = false;
    public bool invulnerable = false;
    public bool dashing = false;
    public bool justHit = false;
    public bool poisoned = false;
    public bool frozen = false;
    public bool onFire = false;
    public bool wet = false;
    public bool stunned = false;
    private bool shuffling = false;
    public float DOTDamage;
    private int prevHealth;
    public float FrozenTimer;
    private Color DefaultColor;
    private GameObject DeadManMessage;
    public int WildGrowths = 0;
    public float startSpeed;
    public float startJumpHeight;
    private bool discardingCard = false;
    public bool slowed = false;
    private int hurtAnimTimer;
    private GameObject Canvas;
    private int BaseDamage;

    #endregion

    #region Start
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Hand = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).gameObject;
        Deck = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(1).gameObject;
        hat = transform.GetChild(0).GetChild(0).gameObject;
        Canvas = GameObject.Find("Canvas");
        curHealth = maxHealth;
        prevHealth = curHealth;
        StartingColor = GetComponent<SpriteRenderer>().color;
        MemoryCard = GameObject.FindGameObjectWithTag("MemoryCard");
        DefaultColor = GetComponent<SpriteRenderer>().color;
        DeadManMessage = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(2).gameObject;
        DeadManMessage.SetActive(false);
        DealCards();
        startSpeed = speed;
        startJumpHeight = jumpHeight;
        BaseDamage = PlayerPrefs.GetInt("BaseDamage");

        if (PlayerPrefs.GetInt("MaxHealth") != 0)
        {
            maxHealth = maxHealth * (1 + PlayerPrefs.GetInt("MaxHealth"));
        }
    }
    #endregion

    #region Update
    void Update()
    {
        #region Input

        if (!StoneForm && !dashing && !frozen)
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

            if (Input.GetMouseButtonDown(0) && !UsingCard && !shuffling)
            {
                UseCard();
                UsingCard = true;
            }

            if (Input.GetMouseButtonUp(0) && UsingCard)
            {
                UsingCard = false;
                StopCoroutine("Cooldown");
            }

            if (Input.GetMouseButtonDown(1) && !discardingCard && !shuffling)
            {
                DiscardCard();
                discardingCard = true;
            }

            if (Input.GetMouseButtonUp(1) && discardingCard)
            {
                discardingCard = false;
            }

        }
        else if (frozen)
        {
            FrozenTimer -= Time.deltaTime;
            GetComponent<SpriteRenderer>().color = Color.blue;
            if (FrozenTimer <= 0)
            {
                GetComponent<SpriteRenderer>().color = DefaultColor;
                frozen = false;
            }
        }

        if (Input.GetButton("StartButton"))
        {
            SceneManager.LoadScene("Stats Screen");
        }

        if (slowed)
        {
            rb.linearVelocity = rb.linearVelocity * 0.1f;
        }

        if (hurtAnimTimer >= 0)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            hurtAnimTimer--;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = DefaultColor;
        }

        #endregion
    }
    #endregion

    #region Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GroundBreakable") || collision.gameObject.CompareTag("GroundMoveable") || collision.gameObject.CompareTag("Enemy"))
        {
            jumped = false;
            if (StoneForm && collision.gameObject.CompareTag("GroundBreakable"))
            {
                Destroy(collision.gameObject);
            }
        }
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (StoneForm)
            {
                var thing = GameObject.FindGameObjectWithTag("StoneForm").GetComponent<Attack>();
                var dam = thing.Damage;
                collision.gameObject.GetComponent<Enemy>().curHealth -= dam;
                collision.gameObject.GetComponent<Enemy>().CheckHealth();
                collision.gameObject.GetComponent<Enemy>().Knockback(turnedRight, EnemyKnockback);
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
        if (Deck.transform.childCount == 0)
        {
            var len = MemoryCard.transform.childCount;
            for (int i = len - 1; i >= 0; i--)
            {
                var card = MemoryCard.transform.GetChild(i).gameObject;
                card.transform.parent = Deck.transform;
                card.transform.localPosition = Vector3.zero;
            }
        }
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
        if (BaseDamage == 0)
        {
            if (!CurCard.GetComponent<CardEffects>().Hollow)
            {
                if (CurCard.GetComponent<CardEffects>().CardNum == 1) //Splash
                {
                    Instantiate(Attacks[0], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 2) //Dove
                {
                    Instantiate(Attacks[1], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
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
                    Instantiate(Attacks[4], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), Attacks[4].transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 6) //Combust
                {
                    Instantiate(Attacks[5], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 7) // Poison Cloud
                {
                    Instantiate(Attacks[6], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 8) //Cone of Cold
                {
                    Instantiate(Attacks[7], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 9) //Might
                {
                    Instantiate(Attacks[8], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
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
                        Instantiate(Attacks[0], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 2) //Dove
                    {
                        Instantiate(Attacks[1], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
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
                        Instantiate(Attacks[4], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 6) //Combust
                    {
                        Instantiate(Attacks[5], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 7) // Poison Cloud
                    {
                        Instantiate(Attacks[6], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 8) //Cone of Cold
                    {
                        Instantiate(Attacks[7], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 9) //Might
                    {
                        Instantiate(Attacks[8], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
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
                    else if (newCard.GetComponent<CardEffects>().CardNum == 14) //Bee Hive
                    {
                        Instantiate(Attacks[12], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }

                }
                #endregion
                else if (CurCard.GetComponent<CardEffects>().CardNum == 14) //Bee Hive
                {
                    Instantiate(Attacks[12], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                }
            }
            else
            {
                if (CurCard.GetComponent<CardEffects>().CardNum == 1) //Splash
                {
                    Instantiate(HollowAttacks[0], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 2) //Dove
                {
                    Instantiate(HollowAttacks[1], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 3) //Wild Growth
                {
                    Instantiate(HollowAttacks[2], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 4) //Stone Form
                {
                    Instantiate(HollowAttacks[3], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 5) //Zephyr
                {
                    Instantiate(HollowAttacks[4], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), HollowAttacks[4].transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 6) //Combust
                {
                    Instantiate(HollowAttacks[5], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 7) // Poison Cloud
                {
                    Instantiate(HollowAttacks[6], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 8) //Cone of Cold
                {
                    Instantiate(HollowAttacks[7], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 9) //Might
                {
                    Instantiate(HollowAttacks[8], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 10) //Card Trick
                {
                    Instantiate(HollowAttacks[9], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 11) //Blink
                {
                    Instantiate(HollowAttacks[10], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 12) //Dash
                {
                    Instantiate(HollowAttacks[11], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation, transform);
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
                        Instantiate(HollowAttacks[0], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 2) //Dove
                    {
                        Instantiate(HollowAttacks[1], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 3) //Wild Growth
                    {
                        Instantiate(HollowAttacks[2], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 4) //Stone Form
                    {
                        Instantiate(HollowAttacks[3], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 5) //Zephyr
                    {
                        Instantiate(HollowAttacks[4], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 6) //Combust
                    {
                        Instantiate(HollowAttacks[5], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 7) // Poison Cloud
                    {
                        Instantiate(HollowAttacks[6], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 8) //Cone of Cold
                    {
                        Instantiate(HollowAttacks[7], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 9) //Might
                    {
                        Instantiate(HollowAttacks[8], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 10) //Card Trick
                    {
                        Instantiate(HollowAttacks[9], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 11) //Blink
                    {
                        Instantiate(HollowAttacks[10], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 12) //Dash
                    {
                        Instantiate(HollowAttacks[11], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation, transform);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 14) //Bee Hive
                    {
                        Instantiate(HollowAttacks[12], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }

                }
                #endregion
                else if (CurCard.GetComponent<CardEffects>().CardNum == 14) //Bee Hive
                {
                    Instantiate(HollowAttacks[12], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                }
            }
        }
        else
        {
            if (!CurCard.GetComponent<CardEffects>().Hollow)
            {
                if (CurCard.GetComponent<CardEffects>().CardNum == 1) //Splash
                {
                    var Spell = Instantiate(Attacks[0], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 2) //Dove
                {
                    var Spell = Instantiate(Attacks[1], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 3) //Wild Growth
                {
                    var Spell = Instantiate(Attacks[2], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 4) //Stone Form
                {
                    var Spell = Instantiate(Attacks[3], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 5) //Zephyr
                {
                    var Spell = Instantiate(Attacks[4], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), Attacks[4].transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 6) //Combust
                {
                    var Spell = Instantiate(Attacks[5], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 7) // Poison Cloud
                {
                    var Spell = Instantiate(Attacks[6], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 8) //Cone of Cold
                {
                    var Spell = Instantiate(Attacks[7], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 9) //Might
                {
                    var Spell = Instantiate(Attacks[8], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
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
                    var Spell = Instantiate(Attacks[11], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation, transform);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
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
                        Instantiate(Attacks[0], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 2) //Dove
                    {
                        Instantiate(Attacks[1], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
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
                        Instantiate(Attacks[4], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 6) //Combust
                    {
                        Instantiate(Attacks[5], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 7) // Poison Cloud
                    {
                        Instantiate(Attacks[6], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 8) //Cone of Cold
                    {
                        Instantiate(Attacks[7], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 9) //Might
                    {
                        Instantiate(Attacks[8], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
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
                    else if (newCard.GetComponent<CardEffects>().CardNum == 14) //Bee Hive
                    {
                        Instantiate(Attacks[12], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }

                }
                #endregion
                else if (CurCard.GetComponent<CardEffects>().CardNum == 14) //Bee Hive
                {
                    var Spell = Instantiate(Attacks[12], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    Spell.GetComponent<Companion>().damage = Spell.GetComponent<Companion>().damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
            }
            else
            {
                if (CurCard.GetComponent<CardEffects>().CardNum == 1) //Splash
                {
                    var Spell = Instantiate(HollowAttacks[0], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 2) //Dove
                {
                    var Spell = Instantiate(HollowAttacks[1], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 3) //Wild Growth
                {
                    var Spell = Instantiate(HollowAttacks[2], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 4) //Stone Form
                {
                    var Spell = Instantiate(HollowAttacks[3], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 5) //Zephyr
                {
                    var Spell = Instantiate(HollowAttacks[4], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), HollowAttacks[4].transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 6) //Combust
                {
                    var Spell = Instantiate(HollowAttacks[5], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 7) // Poison Cloud
                {
                    var Spell = Instantiate(HollowAttacks[6], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 8) //Cone of Cold
                {
                    var Spell = Instantiate(HollowAttacks[7], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 9) //Might
                {
                    var Spell = Instantiate(HollowAttacks[8], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 10) //Card Trick
                {
                    Instantiate(HollowAttacks[9], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 11) //Blink
                {
                    Instantiate(HollowAttacks[10], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                }
                else if (CurCard.GetComponent<CardEffects>().CardNum == 12) //Dash
                {
                    var Spell = Instantiate(HollowAttacks[11], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation, transform);
                    Spell.GetComponent<Attack>().Damage = Spell.GetComponent<Attack>().Damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
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
                        Instantiate(HollowAttacks[0], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 2) //Dove
                    {
                        Instantiate(HollowAttacks[1], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 3) //Wild Growth
                    {
                        Instantiate(HollowAttacks[2], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 4) //Stone Form
                    {
                        Instantiate(HollowAttacks[3], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 5) //Zephyr
                    {
                        Instantiate(HollowAttacks[4], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 6) //Combust
                    {
                        Instantiate(HollowAttacks[5], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 7) // Poison Cloud
                    {
                        Instantiate(HollowAttacks[6], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 8) //Cone of Cold
                    {
                        Instantiate(HollowAttacks[7], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 9) //Might
                    {
                        Instantiate(HollowAttacks[8], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 10) //Card Trick
                    {
                        Instantiate(HollowAttacks[9], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 11) //Blink
                    {
                        Instantiate(HollowAttacks[10], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 12) //Dash
                    {
                        Instantiate(HollowAttacks[11], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation, transform);
                    }
                    else if (newCard.GetComponent<CardEffects>().CardNum == 14) //Bee Hive
                    {
                        Instantiate(HollowAttacks[12], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    }

                }
                #endregion
                else if (CurCard.GetComponent<CardEffects>().CardNum == 14) //Bee Hive
                {
                    var Spell = Instantiate(HollowAttacks[12], new Vector3(hat.transform.position.x, hat.transform.position.y, transform.position.z), hat.transform.rotation);
                    Spell.GetComponent<Companion>().damage = Spell.GetComponent<Companion>().damage * (1 + PlayerPrefs.GetInt("BaseDamage"));
                }
            }
        }

        CurCard.transform.parent = Deck.transform;
        CurCard.transform.localPosition = Vector3.zero;
        CooldownTime = CurCard.GetComponent<CardEffects>().Cooldown;
        StartCoroutine("Cooldown");

        if (Hand.transform.childCount == 0)
        {
            shuffling = true;
            StartCoroutine("ShufflingCooldown");
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

    public void DiscardCard()
    {
        var CurCard = Hand.transform.GetChild(0);
        CurCard.transform.parent = Deck.transform;
        CurCard.transform.localPosition = Vector3.zero;

        if (Hand.transform.childCount == 0)
        {
            shuffling = true;
            StartCoroutine("ShufflingCooldown");
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

    #endregion


    #region Co-Routines
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.35f);
        UsingCard = false;
    }
    
    public void CheckHealth(GameObject Attacker)
    {
        if (curHealth <= 0)
        {
            StartCoroutine("DeadMan");
        }
        else
        {
            if (!invulnerable && !StoneForm && !justHit)
            {
                justHit = true;
                StartCoroutine("JustHitCoroutine");
                if (Attacker.transform.position.x > transform.position.x)
                {
                    rb.AddForceY(1 * knockback, ForceMode2D.Impulse);
                    rb.AddForceX(-1 * knockback, ForceMode2D.Impulse);
                }
                else if (Attacker.transform.position.x < transform.position.x)
                {
                    rb.AddForce(new Vector2(1 * knockback, 1 * knockback), ForceMode2D.Impulse);
                }
                hurtAnimTimer = 30;
            }

            var numbers = Instantiate(DamageNumbers, transform.position, new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));
            if (onFire)
            {
                numbers.GetComponent<DamageNumbers>().Display(prevHealth - curHealth, Color.red);
            }
            else if (frozen)
            {
                numbers.GetComponent<DamageNumbers>().Display(prevHealth - curHealth, Color.cyan);
            }
            else if (poisoned)
            {
                numbers.GetComponent<DamageNumbers>().Display(prevHealth - curHealth, Color.green);
            }
            else if (wet)
            {
                numbers.GetComponent<DamageNumbers>().Display(prevHealth - curHealth, Color.blue);
            }
            else if (stunned)
            {
                numbers.GetComponent<DamageNumbers>().Display(prevHealth - curHealth, Color.gray);
            }
            else
            {
                numbers.GetComponent<DamageNumbers>().Display(prevHealth - curHealth, Color.white);
            }
            prevHealth = curHealth;
        }
    }

    IEnumerator JustHitCoroutine()
    {
        yield return new WaitForSeconds(1f);
        justHit = false;
    }

    IEnumerator ShufflingCooldown()
    {
        shuffling = true;
        StartCoroutine("DeckCooldownVisual");
        yield return new WaitForSeconds(shuffleTime);
        DealCards();
        yield return new WaitForSeconds(0.2f);
        shuffling = false;
    }

    IEnumerator DeckCooldownVisual()
    {
        Canvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "5";
        yield return new WaitForSeconds(shuffleTime / 5f);
        Canvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "4";
        yield return new WaitForSeconds(shuffleTime / 5);
        Canvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "3";
        yield return new WaitForSeconds(shuffleTime / 5);
        Canvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "2";
        yield return new WaitForSeconds(shuffleTime / 5);
        Canvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "1";
        yield return new WaitForSeconds(shuffleTime / 5);
        Canvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0";
    }


    IEnumerator DeadMan()
    {
        DeadManMessage.SetActive(true);
        stunned = true;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Stats Screen");
    }

    #endregion
}
