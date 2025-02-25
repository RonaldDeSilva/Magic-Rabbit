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

    private Rigidbody2D rb;
    public int curHealth;
    private bool right = true;
    private Color startColor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        curHealth = maxHealth;
        startColor = GetComponent<SpriteRenderer>().color;
        StartCoroutine("WalkCycle");
    }


    void Update()
    {
        if (!Stunned)
        {
            if (right)
            {
                rb.linearVelocityX = speed;
            }
            else if (!right)
            {
                rb.linearVelocityX = -speed;
            }
        }
        else if (Stunned)
        {
            rb.linearVelocityX = 0;
        }
    }

    public void CheckHealth()
    {
        StartCoroutine("FlashRed");
        if (curHealth <= 0)
        {
            Destroy(this.gameObject);
        }
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
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = startColor;
        yield return new WaitForSeconds(0.4f);
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
