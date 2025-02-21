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

    IEnumerator WetCooldown()
    {
        GetComponent<SpriteRenderer>().color = Color.blue;
        yield return new WaitForSeconds(4f);
        GetComponent<SpriteRenderer>().color = startColor;
        wet = false;
    }
}
