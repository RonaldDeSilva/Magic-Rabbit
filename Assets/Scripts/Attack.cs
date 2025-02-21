using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{
    #region Attributes

    public int Damage;
    public float seconds;
    public float speed;

    public bool Splash;
    public bool Dove;
    public bool WildGrowth;
    public bool StoneForm;
    public bool Zephyr;

    private Rigidbody2D rb;
    private GameObject Player;
    private Movement MovementScript;
    private GameObject StoneAOE;
    private bool playerTurnedRight;
    private float PlayerStartingSpeed;
    private float PlayerStartingJumpHeight;
    private float FallStartingHeight;
    private float PlayerStartingGravity;
    private bool Phase1 = false;
    private bool Phase2 = false;

    #endregion

    #region Start
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        MovementScript = Player.GetComponent<Movement>();
        playerTurnedRight = MovementScript.turnedRight;
        rb = GetComponent<Rigidbody2D>();
        if (WildGrowth)
        {
            //Wild Growth is an aoe around the player which increases their speed and jump height and also damages enemies periodically
            PlayerStartingSpeed = MovementScript.speed;
            PlayerStartingJumpHeight = MovementScript.jumpHeight;
            MovementScript.speed = MovementScript.speed * 1.5f;
            MovementScript.jumpHeight = MovementScript.jumpHeight * 1.5f;
            StartCoroutine("WildGrowthDamage");
        }
        else if (StoneForm)
        {
            //Stone form causes the player to not be able to move until they hit the ground causing a AOE explosion which stuns all around based on the distance fell
            MovementScript.StoneForm = true;
            StoneAOE = transform.GetChild(0).gameObject;
            StoneAOE.SetActive(false);
            Player.GetComponent<SpriteRenderer>().color = Color.gray;
            FallStartingHeight = Player.transform.position.y;
            PlayerStartingGravity = Player.GetComponent<Rigidbody2D>().gravityScale;
            Player.GetComponent<Rigidbody2D>().gravityScale = PlayerStartingGravity * 3;
            if (MovementScript.canJump)
            {
                seconds = 3f;
                StartCoroutine("Delete");
            }
        }
        else if (Zephyr)
        {
            //Zephyr is a cloud which moves up and then back and forth periodically creating lightening which damages enemies and stuns them if they are wet from Splash
            //Im using stoneAOE here just so I don't have to create a new variable but this represents the lightening that spawns from the cloud
            StoneAOE = transform.GetChild(0).gameObject;
            StoneAOE.SetActive(false);
            Phase1 = true;
            StartCoroutine("ZephyrCoroutine");
        }

        if (!StoneForm)
        {
            StartCoroutine("Delete");
        }
    }
    #endregion

    #region Behaviors
    private void Update()
    {
        if (Splash)
        {
            if (playerTurnedRight)
            {
                rb.linearVelocityX = speed;
            }
            else if (!playerTurnedRight)
            {
                rb.linearVelocityX = -speed;
            }
        }
        else if (Dove)
        {
            rb.linearVelocityY = speed;
        }
        else if (WildGrowth)
        {
            this.transform.position = Player.transform.position;
        }
        else if (StoneForm)
        {
            this.transform.position = Player.transform.position;
            if (MovementScript.canJump)
            {
                Player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                StartCoroutine("StoneFormAttack");
            }
        }
        else if (Zephyr)
        {
            if (Phase1)
            {
                rb.linearVelocityY = speed;
            }
            else if (Phase2)
            {
                rb.linearVelocityY = 0;
                if (playerTurnedRight)
                {
                    rb.linearVelocityX = speed;
                }
                else if (!playerTurnedRight)
                {
                    rb.linearVelocityX = -speed;
                }
            }
        }
    }
    #endregion

    #region Collisions and Coroutines

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (Splash)
            {
                if (collision.gameObject.GetComponent<Enemy>().wet)
                {
                    collision.gameObject.GetComponent<Enemy>().StopCoroutine("WetCooldown");
                    collision.gameObject.GetComponent<Enemy>().StartCoroutine("WetCooldown");
                }
                else
                {
                    collision.gameObject.GetComponent<Enemy>().wet = true;
                    collision.gameObject.GetComponent<Enemy>().StartCoroutine("WetCooldown");
                }
                Destroy(this.gameObject);
            }
            else
            {
                collision.gameObject.GetComponent<Enemy>().curHealth -= Damage;
                collision.gameObject.GetComponent<Enemy>().CheckHealth();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!WildGrowth)
            {
                collision.gameObject.GetComponent<Enemy>().curHealth -= Damage;
                collision.gameObject.GetComponent<Enemy>().CheckHealth();
            }
        }
    }

    //Wild Growth uses Overlap Collider to find enemies and then damages them for every half second they remain in the AOE
    IEnumerator WildGrowthDamage()
    {
        var list = new Collider2D[10];
        var filter = new ContactFilter2D().NoFilter();
        int hitColliders = Physics2D.OverlapCollider(GetComponent<CircleCollider2D>(), filter, list);
        for (int i = hitColliders - 1; i >= 0; i--)
        {
            if (list[i].gameObject.CompareTag("Enemy"))
            {
                list[i].gameObject.GetComponent<Enemy>().curHealth -= Damage;
                list[i].gameObject.GetComponent<Enemy>().CheckHealth();
            }
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("WildGrowthDamage");
    }

    //Stone Form attack coroutine finds the stun time based on how far the player fell, and then stuns the enemy for that amount of time using overlap collider
    IEnumerator StoneFormAttack()
    {
        var StunTime = Mathf.Clamp(FallStartingHeight - Player.transform.position.y, 0, 10);
        if (StunTime > 0) {
            StoneAOE.SetActive(true);
            var list = new Collider2D[10];
            var filter = new ContactFilter2D().NoFilter();
            int hitColliders = Physics2D.OverlapCollider(StoneAOE.GetComponent<CircleCollider2D>(), filter, list);
            for (int i = hitColliders - 1; i >= 0; i--)
            {
                if (list[i].gameObject.CompareTag("Enemy"))
                {
                    //list[i].gameObject.GetComponent<Enemy>().curHealth -= Damage;
                    //list[i].gameObject.GetComponent<Enemy>().CheckHealth();
                    list[i].gameObject.GetComponent<Enemy>().Stunned = true;
                    list[i].gameObject.GetComponent<Enemy>().StunCooldownTime = StunTime;
                    list[i].gameObject.GetComponent<Enemy>().StartCoroutine("StunCooldown");
                }
            }
        }
        yield return new WaitForSeconds(seconds);
        StartCoroutine("Delete");
    }
    #region Zephyr Coroutines
    //Zephyr coroutines move the cloud where it needs to go and then move back and forth randomly casting lighening
    IEnumerator ZephyrCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        Phase1 = false;
        Phase2 = true;
        StartCoroutine("ZephyrPhase2");
        StartCoroutine("ZephyrMovement");
    }

    IEnumerator ZephyrPhase2()
    {
        yield return new WaitForSeconds(Random.Range(0.2f, 0.8f));
        StoneAOE.SetActive(true);
        var list = new Collider2D[10];
        var filter = new ContactFilter2D().NoFilter();
        int hitColliders = Physics2D.OverlapCollider(StoneAOE.GetComponent<CapsuleCollider2D>(), filter, list);
        for (int i = hitColliders - 1; i >= 0; i--)
        {
            if (list[i].gameObject.CompareTag("Enemy"))
            {
                list[i].gameObject.GetComponent<Enemy>().curHealth -= Damage;
                list[i].gameObject.GetComponent<Enemy>().CheckHealth();

                if (list[i].gameObject.GetComponent<Enemy>().wet)
                {
                    list[i].gameObject.GetComponent<Enemy>().Stunned = true;
                    list[i].gameObject.GetComponent<Enemy>().StunCooldownTime = 3f;
                    list[i].gameObject.GetComponent<Enemy>().StartCoroutine("StunCooldown");
                }
            }
        }
        yield return new WaitForSeconds(0.15f);
        StoneAOE.SetActive(false);
        yield return new WaitForSeconds(Random.Range(0.3f, 2f));
        StartCoroutine("ZephyrPhase2");
    }

    IEnumerator ZephyrMovement()
    {
        yield return new WaitForSeconds(4f);
        playerTurnedRight = !playerTurnedRight;
        StartCoroutine("ZephyrMovement");
    }
    #endregion
    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(seconds);
        if (WildGrowth)
        {
            MovementScript.speed = PlayerStartingSpeed;
            MovementScript.jumpHeight = PlayerStartingJumpHeight;
        }
        else if (StoneForm)
        {
            MovementScript.StoneForm = false;
            Player.GetComponent<SpriteRenderer>().color = MovementScript.StartingColor;
            Player.GetComponent<Rigidbody2D>().gravityScale = PlayerStartingGravity;
            Player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        Destroy(this.gameObject);
    }

    #endregion
}