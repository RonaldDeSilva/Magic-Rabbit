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
    public class theCollider { };

    public Rigidbody2D rb;
    public int curHealth;
    private bool right = true;
    private Color startColor;
    private bool isTurnedRight;
    private float theMagnitude;
    public bool bounce = false;
    private int bounces = 0;
    private float CurrentTime = 1;
    private GameObject Player;
    public GameObject damageNumber;
    private int prevHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        curHealth = maxHealth;
        prevHealth = curHealth;
        startColor = GetComponent<SpriteRenderer>().color;
        Player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine("WalkCycle");
    }


    void FixedUpdate()
    {
        if (!bounce)
        {
            if (right)
            {
                rb.linearVelocityX += speed/10;
            }
            else if (!right)
            {
                rb.linearVelocityX -= speed/10;
            }

            if (Stunned)
            {
                rb.linearVelocityX = 0;
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
        if (CurrentTime <= 0f)
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
                numbers.GetComponent<DamageNumbers>().Display(prevHealth - curHealth, Color.blue);
            }
            else if (poisoned)
            {
                numbers.GetComponent<DamageNumbers>().Display(prevHealth - curHealth, Color.green);
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

    IEnumerator WalkCycle()
    {
        right = true;
        yield return new WaitForSeconds(walkTime);
        right = false;
        yield return new WaitForSeconds(walkTime);
        StartCoroutine("WalkCycle");
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
