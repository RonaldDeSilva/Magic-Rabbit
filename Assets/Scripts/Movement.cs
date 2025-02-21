using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
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
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GroundBreakable"))
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
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GroundBreakable"))
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GroundBreakable"))
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
                //thing.transform.rotation = new Quaternion(0, 0, 90, this.transform.rotation.w);
            }
            else if (!turnedRight)
            {
                Instantiate(Attacks[4], new Vector3(transform.position.x - 2.5f, transform.position.y + 1, transform.position.z), Attacks[4].transform.rotation);
                //thing.transform.rotation = new Quaternion(0, 0, 90, this.transform.rotation.w);
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
