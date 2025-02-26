using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    #region Card Numbers and what they mean

    //1 = Splash - projectile that makes enemy wet and vulnerable to electricity and freeze
    //2 = Dove - Rising projectile that can be used as a platform
    //3 = WildGrowth - AOE around player which damages enemies and increases players movement speed and jump height
    //4 = StoneForm - causes player to plummet and create an AOE which stuns enemies, also can break through breakable blocks
    //5 = Zephyr - Lightening cloud that moves up and back and forth while striking the ground causing wet enemies to be stunned
    //6 = Combust - fireball which causes DOT on enemy, as well as exploding poison clouds
    //7 = PoisonCloud - ball of poison which turns into AOE poison cloud on impact, causes explosion when interacting with combust or burning enemies

    #endregion


    #region Attributes

    public float speed;
    public float jumpHeight;
    public int maxHealth;
    private int curHealth;
    private float CooldownTime = 1f;

    private Rigidbody2D rb;
    public Color StartingColor;
    private GameObject Hand;
    private GameObject Deck;
    public GameObject[] Attacks;

    private bool jumped = false;
    public bool canJump = false;
    private bool UsingCard = false;
    public bool turnedRight = true;
    public bool StoneForm = false;

    #endregion

    #region Start
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Hand = transform.GetChild(0).gameObject;
        Deck = transform.GetChild(1).gameObject;
        curHealth = maxHealth;
        StartingColor = GetComponent<SpriteRenderer>().color;
        DealCards();
    }
    #endregion

    #region Update
    void Update()
    {
        #region Input

        if (!StoneForm)
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                rb.linearVelocityX = speed * Input.GetAxis("Horizontal");

                if (Input.GetAxis("Horizontal") > 0)
                {
                    turnedRight = true;
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    turnedRight = false;
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

        if (Hand.transform.childCount == 0)
        {
            DealCards();
        }
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
            curHealth -= collision.gameObject.GetComponent<Enemy>().bumpDamage;
            CheckHealth();
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
        while(Hand.transform.childCount < 5)
        {
            var children = Deck.transform.childCount;
            var num = Random.Range(0, children - 1);
            Deck.transform.GetChild(num).parent = Hand.transform;
        }
    }

    private void UseCard()
    {
        var CurCard = Hand.transform.GetChild(0).gameObject;
        
        if (CurCard.GetComponent<CardEffects>().CardNum == 1)
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
        else if (CurCard.GetComponent<CardEffects>().CardNum == 2)
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
        else if (CurCard.GetComponent<CardEffects>().CardNum == 3)
        {
             Instantiate(Attacks[2], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
        }
        else if (CurCard.GetComponent<CardEffects>().CardNum == 4)
        {
            Instantiate(Attacks[3], new Vector3(transform.position.x, transform.position.y, transform.position.z), this.transform.rotation);
        }
        else if (CurCard.GetComponent<CardEffects>().CardNum == 5)
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
        else if (CurCard.GetComponent<CardEffects>().CardNum == 6)
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
        else if (CurCard.GetComponent<CardEffects>().CardNum == 7)
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

        CurCard.transform.parent = Deck.transform;
        CooldownTime = CurCard.GetComponent<CardEffects>().Cooldown;
        StartCoroutine("Cooldown");
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
