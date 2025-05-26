using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    public int maxHealth;
    public float speed;
    public float walkTime;
    public int bumpDamage;
    public int attackDamage;
    public bool Stunned = false;
    public float StunCooldownTime;
    public bool wet = false;
    public int DOTDamage;
    public bool onFire = false;
    public bool poisoned = false;
    public bool frozen = false;
    public Rigidbody2D rb;
    public int curHealth;
    private Color startColor;
    private bool isTurnedRight;
    private float theMagnitude;
    public bool bounce = false;
    private int bounces = 0;
    private float CurrentTime = 1;
    private GameObject Player;
    public GameObject damageNumber;
    private int prevHealth;
    public GameObject Zombie;
    public GameObject IceBat;
    private float necroTimer = 0;
    private float jumpTimer = 0;

    public bool isNecromancer;
    public bool isZombie;
    public bool isIceBat;
    private float Acceleration = 0.01f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        curHealth = maxHealth;
        prevHealth = curHealth;
        startColor = GetComponent<SpriteRenderer>().color;
        Player = GameObject.FindGameObjectWithTag("Player");
    }


    void FixedUpdate()
    {
        if (!bounce && !Stunned)
        {
            if (isZombie)
            {
                Acceleration = Mathf.Clamp(Acceleration, 0, 1);
                rb.linearVelocityX = (Player.transform.position.x - transform.position.x) * speed * Acceleration;
                if (Acceleration != 1)
                {
                    Acceleration += Time.deltaTime;
                }

                
                if (jumpTimer < 2)
                {
                    jumpTimer += Time.deltaTime;
                }
                else
                {
                    var jumpValue = Random.Range(0, 2);
                    if (jumpValue == 1)
                    {
                        rb.AddForceY(4000);
                        jumpTimer = 0;
                    }
                }


            }
            else if (isNecromancer)
            {
                necroTimer += 1f;
                if (necroTimer % 250f == 0)
                {
                    var coinFlip = Random.Range(0, 2);
                    if (coinFlip == 0)
                    {
                        var child = Instantiate(Zombie, new Vector3(transform.position.x - 1f, transform.position.y + 0.25f, 0f), new Quaternion(0, 0, 0, 0), transform);
                    }
                    else if (coinFlip == 1)
                    {
                        var child = Instantiate(IceBat, new Vector3(transform.position.x - 1f, transform.position.y + 2.5f, 0f), new Quaternion(0, 0, 0, 0), transform);
                    }
                }

                if (necroTimer > 501)
                {
                    necroTimer = 0;
                }
            }
            else if (isIceBat)
            {
                if (jumpTimer < 2)
                {
                    jumpTimer += Time.deltaTime;
                    Acceleration = Mathf.Clamp(Acceleration, 0, 1);
                    if (jumpTimer > 0.6f)
                    {
                        rb.linearVelocity = new Vector2((Player.transform.position.x - transform.position.x) * speed * Acceleration, (Player.transform.position.y - transform.position.y) * speed * Acceleration);
                    }

                    if (Acceleration != 1)
                    {
                        Acceleration += Time.deltaTime;
                    }
                }
                else
                {
                    var jumpValue = Random.Range(0, 2);
                    if (jumpValue == 1)
                    {
                        if (Player.transform.position.x < transform.position.x)
                        {
                            rb.AddForceX(-450);
                        }
                        else if (Player.transform.position.x > transform.position.x)
                        {
                            rb.AddForceX(450);
                        }
                        jumpTimer = 0;
                    }
                }
            }

        }
        else if (CurrentTime >= 0f && bounce)
        {
            if (CurrentTime >= 1)
            {
                if (Player.transform.position.x < transform.position.x)
                {
                    rb.AddForce(new Vector2(1 * theMagnitude, 1f * theMagnitude), ForceMode2D.Impulse);
                }
                else
                {
                    rb.AddForce(new Vector2(-1 * theMagnitude, 1f * theMagnitude), ForceMode2D.Impulse);
                }
            }

            CurrentTime -= Time.deltaTime;
            if (CurrentTime <= 0f)
            {
                bounce = false;
                bounces = 0;
            }
            else if (CurrentTime <= 0.75f)
            {
                GetComponent<CapsuleCollider2D>().isTrigger = false;
            }
        }
        else if (Stunned)
        {
            rb.linearVelocityX = 0;
        }
        else if (CurrentTime <= 0f)
        {
            bounce = false;
            bounces = 0;
        }
        rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -speed * 3, speed * 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bounce && bounces <= 1)
        {
            if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GroundMoveable") || collision.gameObject.CompareTag("GroundBreakable"))
            {
                rb.linearVelocity = new Vector2(-rb.linearVelocityX, -rb.linearVelocityY);
                bounces += 1;
            }
        }
        else
        {
            GetComponent<CapsuleCollider2D>().isTrigger = false;
            bounces = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (bounce)
        {
            if (rb.linearVelocityX > 1 | rb.linearVelocityX < -1 && rb.linearVelocityY > 1 | rb.linearVelocityY < -1)
            {
                rb.linearVelocity = new Vector2(-rb.linearVelocityX, -rb.linearVelocityY);
            }
            else if (isTurnedRight)
            {
                rb.AddForce(new Vector2(3 * theMagnitude, 5f * theMagnitude), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(-3 * theMagnitude, 5f * theMagnitude), ForceMode2D.Impulse);
            }
            CurrentTime -= 0.1f;
        }
        if (isIceBat)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (!Player.GetComponent<Movement>().invulnerable && !Player.GetComponent<Movement>().justHit && !Player.GetComponent<Movement>().StoneForm)
                {
                    Player.GetComponent<Movement>().curHealth -= bumpDamage;
                    CheckHealth();
                    var rando = Random.Range(0, 4);
                    if (rando == 3)
                    {
                        if (!Player.GetComponent<Movement>().frozen)
                        {
                            Player.GetComponent<Movement>().frozen = true;
                            Player.GetComponent<Movement>().FrozenTimer = 2f;
                        }
                    }
                }
            }
        }
    }


    public void CheckHealth()
    {
        StartCoroutine("FlashRed");
        if (curHealth <= 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            var numbers = Instantiate(damageNumber, transform.position, new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));
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
            else if (Stunned)
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
    public void Knockback(bool turnedRight, float magnitude)
    {
        isTurnedRight = turnedRight;
        theMagnitude = magnitude;
        bounce = true;
        CurrentTime = 1;
        GetComponent<CapsuleCollider2D>().isTrigger = true; 
    }

    IEnumerator FlashRed()
    {
        GetComponent<SpriteRenderer>().color = Color.black;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = startColor;
    }

    public IEnumerator StunCooldown()
    {
        GetComponent<SpriteRenderer>().color = Color.gray;
        yield return new WaitForSeconds(StunCooldownTime);
        GetComponent<SpriteRenderer>().color = startColor;
        Stunned = false;
        frozen = false;
    }

    public IEnumerator WetCooldown()
    {
        GetComponent<SpriteRenderer>().color = Color.blue;
        yield return new WaitForSeconds(15f);
        GetComponent<SpriteRenderer>().color = startColor;
        wet = false;
    }

    public IEnumerator DamageOverTime()
    {
        if (onFire)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (poisoned)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        curHealth -= DOTDamage;
        CheckHealth();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("DamageOverTime");

    }
    
    public IEnumerator DOTCooldown()
    {
        StartCoroutine("DamageOverTime");
        yield return new WaitForSeconds(4);
        StopCoroutine("DamageOverTime");
        onFire = false;
        poisoned = false;
    }
}
