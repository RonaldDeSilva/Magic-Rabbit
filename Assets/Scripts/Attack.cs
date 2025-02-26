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
    public bool Combust;
    public bool PoisonCloud;

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
        else if (PoisonCloud)
        {
            //Poison cloud has a cloud attached which is set active when hitting the ground or an enemy
            StoneAOE = transform.GetChild(0).gameObject;
            StoneAOE.SetActive(false);
            Phase1 = true;
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
            //Splash moves whichever way the player is facing until it hits the ground
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
        else if (Combust)
        {
            //Combust moves whichever way the player is facing until it hits the ground
            if (playerTurnedRight)
            {
                rb.linearVelocityX = speed;
            }
            else if (!playerTurnedRight)
            {
                rb.linearVelocityX = -speed;
            }
        }
        else if (PoisonCloud)
        {
            if (Phase1)
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
            else if (Phase2)
            {
                rb.linearVelocityX = 0;
            }
        }

    }
    #endregion

    #region Collision

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
                    //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(rb.linearVelocity * 5);
                }
                else
                {
                    collision.gameObject.GetComponent<Enemy>().StopCoroutine("DOTCooldown");
                    collision.gameObject.GetComponent<Enemy>().StopCoroutine("DamageOverTime");
                    collision.gameObject.GetComponent<Enemy>().onFire = false;
                    collision.gameObject.GetComponent<Enemy>().wet = true;
                    collision.gameObject.GetComponent<Enemy>().StartCoroutine("WetCooldown");
                    //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(rb.linearVelocity * 5);
                }
                Destroy(this.gameObject);
            }
            else if (Combust)
            {
                collision.gameObject.GetComponent<Enemy>().StopCoroutine("WetCooldown");
                collision.gameObject.GetComponent<Enemy>().wet = false;
                collision.gameObject.GetComponent<Enemy>().StopCoroutine("DOTCooldown");
                collision.gameObject.GetComponent<Enemy>().StopCoroutine("DamageOverTime");
                collision.gameObject.GetComponent<Enemy>().DOTDamage = Damage;
                collision.gameObject.GetComponent<Enemy>().onFire = true;
                collision.gameObject.GetComponent<Enemy>().StartCoroutine("DOTCooldown");
                Destroy(this.gameObject);
            }
            else if (PoisonCloud)
            {
                Phase1 = false;
                Phase2 = true;
                GetComponent<CircleCollider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                StoneAOE.SetActive(true);
                StartCoroutine("PoisonCoroutine");
            }
            else
            {
                collision.gameObject.GetComponent<Enemy>().curHealth -= Damage;
                collision.gameObject.GetComponent<Enemy>().CheckHealth();
            }
        }
        else if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GroundBreakable") || collision.gameObject.CompareTag("GroundMoveable"))
        {
            if (!collision.gameObject.CompareTag("GroundMoveable") && !Splash)
            {
                Destroy(this.gameObject);
            }
        }
        Debug.Log(collision.gameObject.tag);
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

    #endregion

    #region Coroutines

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

    IEnumerator PoisonCoroutine()
    {
        var list = new Collider2D[10];
        var filter = new ContactFilter2D().NoFilter();
        int hitColliders = Physics2D.OverlapCollider(StoneAOE.GetComponent<CircleCollider2D>(), filter, list);
        var explode = false;
        for (int i = hitColliders - 1; i >= 0; i--)
        {
            if (list[i].gameObject.CompareTag("Enemy"))
            {
                if (!list[i].gameObject.GetComponent<Enemy>().poisoned && !list[i].gameObject.GetComponent<Enemy>().onFire)
                {
                    list[i].gameObject.GetComponent<Enemy>().StopCoroutine("DOTCooldown");
                    list[i].gameObject.GetComponent<Enemy>().StopCoroutine("DamageOverTime");
                    list[i].gameObject.GetComponent<Enemy>().DOTDamage = Damage;
                    list[i].gameObject.GetComponent<Enemy>().poisoned = true;
                    list[i].gameObject.GetComponent<Enemy>().StartCoroutine("DOTCooldown");
                }
                else if (list[i].gameObject.GetComponent<Enemy>().onFire)
                {
                    explode = true;
                    break;
                }
            }
            else if (list[i].gameObject.CompareTag("Combust"))
            {
                explode = true;
                Destroy(list[i].gameObject);
                break;
            }
        }
        if (explode)
        {
            for (int i = hitColliders - 1; i >= 0; i--)
            {
                if (list[i].gameObject.CompareTag("Enemy"))
                {
                    list[i].gameObject.GetComponent<Enemy>().curHealth -= Damage * 5;
                    list[i].gameObject.GetComponent<Enemy>().CheckHealth();
                    list[i].gameObject.GetComponent<Enemy>().StopCoroutine("DOTCooldown");
                    list[i].gameObject.GetComponent<Enemy>().StopCoroutine("DamageOverTime");
                    list[i].gameObject.GetComponent<Enemy>().DOTDamage = Damage * 2;
                    list[i].gameObject.GetComponent<Enemy>().onFire = true;
                    list[i].gameObject.GetComponent<Enemy>().StartCoroutine("DOTCooldown");
                    list[i].gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, speed * 600));
                }
            }
            Destroy(this.gameObject);
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine("PoisonCoroutine");
    }

    #endregion
}