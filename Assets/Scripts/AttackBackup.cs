using UnityEngine;
using System.Collections;

public class AttackBackup : MonoBehaviour
{
    #region Attributes

    public int Damage;
    public float seconds;
    public float speed;
    public float stunDuration;
    public float distance;

    public bool Splash;
    public bool Dove;
    public bool WildGrowth;
    public bool StoneForm;
    public bool Zephyr;
    public bool Combust;
    public bool PoisonCloud;
    public bool ConeOfCold;
    public bool Might;
    public bool CardTrick;
    public bool Blink;
    public bool Dash;
    public bool Magic8Ball;

    private Rigidbody2D rb;
    private GameObject Player;
    private Movement MovementScript;
    public GameObject StoneAOE;
    private bool playerTurnedRight;
    private float PlayerStartingSpeed;
    private float PlayerStartingJumpHeight;
    public float FallStartingHeight;
    private float PlayerStartingGravity;
    public bool Phase1 = false;
    public bool Phase2 = false;
    private bool flying = false;
    private bool hasStarted = false;
    public LayerMask groundLayerMask;
    public float initialXSpeed;
    public float initialYSpeed;
    public float maxYSpeed;
    private float timer = 0f;
    public float rotation;
    private GameObject cloudChild;

    #endregion

    #region Start
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        MovementScript = Player.GetComponent<Movement>();
        playerTurnedRight = MovementScript.turnedRight;
        rb = GetComponent<Rigidbody2D>();
        PlayerStartingGravity = Player.GetComponent<Rigidbody2D>().gravityScale;
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
            Player.GetComponent<Rigidbody2D>().gravityScale = PlayerStartingGravity * 3;
            if (MovementScript.canJump)
            {
                seconds = 1f;
                StartCoroutine("Delete");
            }
        }
        else if (Dove && !playerTurnedRight)
        {
            initialXSpeed = -initialXSpeed;
        }
        else if (Zephyr)
        {
            //Zephyr is a cloud which moves up and then back and forth periodically creating lightening which damages enemies and stuns them if they are wet from Splash
            //Im using stoneAOE here just so I don't have to create a new variable but this represents the lightening that spawns from the cloud
            StoneAOE = transform.GetChild(0).gameObject;
            StoneAOE.SetActive(false);
            cloudChild = transform.GetChild(1).gameObject;
        }
        else if (PoisonCloud)
        {
            //Poison cloud has a cloud attached which is set active when hitting the ground or an enemy
            StoneAOE = transform.GetChild(0).gameObject;
            StoneAOE.SetActive(false);
            Phase1 = true;
        }
        else if (Combust)
        {
            timer = 0.4f;
        }
        else if (ConeOfCold && !playerTurnedRight)
        {
            GetComponent<SpriteRenderer>().flipY = true;
        }
        else if (Might)
        {
            StartCoroutine("MightCoroutine");
        }
        else if (CardTrick)
        {
            MovementScript.invulnerable = true;
            Player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.1f);
        }
        else if (Blink)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                var ray = Physics2D.Raycast(new Vector2(Player.transform.position.x, Player.transform.position.y), direction, distance, groundLayerMask);
                if (ray.transform != null)
                {
                    Player.transform.position = ray.point;
                }
                else
                {
                    Player.transform.position = new Vector2(Player.transform.position.x + (direction.x * distance), Player.transform.position.y + (direction.y * distance));
                }
            }
            else
            {

                if (playerTurnedRight)
                {
                    var direction = new Vector2(1, 0);
                    var ray = Physics2D.Raycast(new Vector2(Player.transform.position.x, Player.transform.position.y), direction, distance, groundLayerMask);
                    if (ray.transform != null)
                    {
                        Player.transform.position = ray.point;
                    }
                    else
                    {
                        Player.transform.position = new Vector2(Player.transform.position.x + (1 * distance), Player.transform.position.y);
                    }
                }
                else
                {
                    var direction = new Vector2(-1, 0);
                    var ray = Physics2D.Raycast(new Vector2(Player.transform.position.x, Player.transform.position.y), direction, distance, groundLayerMask);
                    if (ray.transform != null)
                    {
                        Player.transform.position = ray.point;
                    }
                    else
                    {
                        Player.transform.position = new Vector2(Player.transform.position.x + (-1 * distance), Player.transform.position.y);
                    }
                }
            }
            Destroy(this.gameObject);
        }
        else if (Dash)
        {
            MovementScript.dashing = true;
            Player.GetComponent<CapsuleCollider2D>().isTrigger = true;
            MovementScript.rb.linearVelocity = Vector2.zero;
            Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + 0.01f, Player.transform.position.z);
            Player.GetComponent<Rigidbody2D>().gravityScale = 0.0000001f;
        }

        if (rb != null)
        {
            if (rb.bodyType == RigidbodyType2D.Kinematic)
            {
                rb.useFullKinematicContacts = true;
            }
        }

        if (!StoneForm && !Dove && !Combust)
        {
            StartCoroutine("Delete");
        }
    }
    #endregion

    #region Behaviors
    private void FixedUpdate()
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
            if (flying)
            {
                if (playerTurnedRight)
                {
                    rb.linearVelocityY = initialYSpeed;
                    rb.linearVelocityX = initialXSpeed;
                    initialYSpeed += Time.deltaTime;
                    initialXSpeed -= Time.deltaTime;
                    initialYSpeed = Mathf.Clamp(initialYSpeed, initialYSpeed, maxYSpeed);
                    initialXSpeed = Mathf.Clamp(initialXSpeed, 0, initialXSpeed);
                }
                else if (!playerTurnedRight)
                {
                    rb.linearVelocityY = initialYSpeed;
                    rb.linearVelocityX = initialXSpeed;
                    initialYSpeed += Time.deltaTime;
                    initialXSpeed += Time.deltaTime;
                    initialYSpeed = Mathf.Clamp(initialYSpeed, initialYSpeed, maxYSpeed);
                    initialXSpeed = Mathf.Clamp(initialXSpeed, initialXSpeed, 0);
                }
            }
            else
            {
                seconds -= Time.deltaTime;
                if (seconds <= 2f)
                {
                    flying = true;
                }
            }
        }
        else if (WildGrowth)
        {
            this.transform.position = Player.transform.position;
        }
        else if (StoneForm)
        {
            this.transform.position = Player.transform.position;
            if (MovementScript.canJump && !hasStarted)
            {
                Player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                StartCoroutine("StoneFormAttack");
                hasStarted = true;
            }
        }
        else if (Zephyr)
        {
            if (Phase1)
            {
                rb.linearVelocityY = speed;
                timer += Time.deltaTime;
                if (timer >= 4f)
                {
                    Phase1 = false;
                    Phase2 = true;
                    StartCoroutine("ZephyrPhase2");
                }
            }
            else if (Phase2)
            {
                distance = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), -Vector2.up, 500, groundLayerMask).point.y;
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
            if (timer <= 0)
            {
                transform.localScale = new Vector3(2, 1.75f, 1);
                rb.linearVelocity = Vector2.zero;
                if (timer == 0)
                {
                    var list = new Collider2D[10];
                    var filter = new ContactFilter2D().NoFilter();
                    int hitColliders = Physics2D.OverlapCollider(GetComponent<CircleCollider2D>(), filter, list);
                    for (int i = hitColliders - 1; i >= 0; i--)
                    {
                        if (list[i].gameObject.CompareTag("Enemy"))
                        {
                            list[i].gameObject.GetComponent<Enemy>().StopCoroutine("WetCooldown");
                            list[i].gameObject.GetComponent<Enemy>().wet = false;
                            list[i].gameObject.GetComponent<Enemy>().StopCoroutine("DOTCooldown");
                            list[i].gameObject.GetComponent<Enemy>().StopCoroutine("DamageOverTime");
                            list[i].gameObject.GetComponent<Enemy>().DOTDamage = Damage;
                            list[i].gameObject.GetComponent<Enemy>().onFire = true;
                            list[i].gameObject.GetComponent<Enemy>().StartCoroutine("DOTCooldown");
                            if (list[i].gameObject.GetComponent<Enemy>().frozen)
                            {
                                list[i].gameObject.GetComponent<Enemy>().Stunned = false;
                                list[i].gameObject.GetComponent<Enemy>().frozen = false;
                                list[i].gameObject.GetComponent<Enemy>().StopCoroutine("StunCooldown");
                            }
                        }
                    }
                }
            }
            else if (playerTurnedRight && timer > 0)
            {
                rb.linearVelocityX = speed;
            }
            else if (!playerTurnedRight && timer > 0)
            {
                rb.linearVelocityX = -speed;
            }

            if (timer <= -0.1f)
            {
                Destroy(this.gameObject);
            }

            timer -= Time.deltaTime;
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
        else if (ConeOfCold)
        {
            if (playerTurnedRight)
            {
                this.transform.position = new Vector3(Player.transform.position.x + 1.5f, Player.transform.position.y, transform.position.z);
            }
            else if (!playerTurnedRight)
            {
                this.transform.position = new Vector3(Player.transform.position.x - 1.5f, Player.transform.position.y, transform.position.z);
            }
        }
        else if (Might)
        {
            playerTurnedRight = MovementScript.turnedRight;
            if (playerTurnedRight)
            {
                this.transform.position = new Vector3(Player.transform.position.x + 1.5f, Player.transform.position.y, transform.position.z);
            }
            else if (!playerTurnedRight)
            {
                this.transform.position = new Vector3(Player.transform.position.x - 1.5f, Player.transform.position.y, transform.position.z);
            }
        }
        else if (Dash)
        {
            if (MovementScript.dashing)
            {
                if (playerTurnedRight)
                {
                    MovementScript.rb.linearVelocity = new Vector2(speed, 0);
                }
                else if (!playerTurnedRight)
                {
                    MovementScript.rb.linearVelocity = new Vector2(-speed, 0);
                }
            }
            else
            {
                Player.GetComponent<CapsuleCollider2D>().isTrigger = false;
                Destroy(this.gameObject);
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
                collision.gameObject.GetComponent<Enemy>().curHealth -= Damage;
                collision.gameObject.GetComponent<Enemy>().CheckHealth();
                Destroy(this.gameObject);
            }
            else if (Combust)
            {
                timer = 0f;
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
            if (!collision.gameObject.CompareTag("GroundMoveable") && !Splash && !StoneForm && !Zephyr && !PoisonCloud && !Combust)
            {
                Destroy(this.gameObject);
            }
            else if (Zephyr)
            {
                playerTurnedRight = !playerTurnedRight;
            }
            else if (Combust)
            {
                timer = 0f;
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
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            flying = true;
        }
    }
    #endregion

    #region Triggers

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!WildGrowth)
            {
                if (ConeOfCold && collision.gameObject.GetComponent<Enemy>().wet)
                {
                    collision.gameObject.GetComponent<Enemy>().curHealth -= Damage * 4;
                    collision.gameObject.GetComponent<Enemy>().frozen = true;
                    collision.gameObject.GetComponent<Enemy>().CheckHealth();
                    collision.gameObject.GetComponent<Enemy>().Stunned = true;
                    collision.gameObject.GetComponent<Enemy>().StunCooldownTime = stunDuration * 2;
                    collision.gameObject.GetComponent<Enemy>().StartCoroutine("StunCooldown");
                }
                else if (ConeOfCold)
                {
                    collision.gameObject.GetComponent<Enemy>().frozen = true;
                    collision.gameObject.GetComponent<Enemy>().onFire = false;
                    collision.gameObject.GetComponent<Enemy>().curHealth -= Damage;
                    collision.gameObject.GetComponent<Enemy>().CheckHealth();
                    collision.gameObject.GetComponent<Enemy>().Stunned = true;
                    collision.gameObject.GetComponent<Enemy>().StunCooldownTime = stunDuration;
                    collision.gameObject.GetComponent<Enemy>().StartCoroutine("StunCooldown");
                    collision.gameObject.GetComponent<Enemy>().StopCoroutine("DOTCooldown");
                    collision.gameObject.GetComponent<Enemy>().StopCoroutine("DamageOverTime");
                }
                else if (!Might && !ConeOfCold)
                {
                    collision.gameObject.GetComponent<Enemy>().curHealth -= Damage;
                    collision.gameObject.GetComponent<Enemy>().CheckHealth();
                }
            }
        }
        else if (!collision.gameObject.CompareTag("Enemy") && collision.isTrigger == false)
        {
            if (Dash)
            {
                MovementScript.dashing = false;
                Player.GetComponent<CapsuleCollider2D>().isTrigger = false;
                Player.GetComponent<Rigidbody2D>().gravityScale = PlayerStartingGravity;
                Destroy(this.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Ground") | collision.gameObject.CompareTag("GroundBreakable") | collision.gameObject.CompareTag("GroundMoveable"))
        {
            if (Zephyr)
            {
                if (Phase1)
                {
                    Phase1 = false;
                    Phase2 = true;
                    StartCoroutine("ZephyrPhase2");
                }
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
        if (StunTime > 0)
        {
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

    IEnumerator ZephyrPhase2()
    {
        yield return new WaitForSeconds(Random.Range(0.2f, 0.8f));
        StoneAOE.SetActive(true);
        var list = new Collider2D[10];
        var list2 = new Collider2D[10];
        var filter = new ContactFilter2D().NoFilter();
        int hitColliders = Physics2D.OverlapCollider(StoneAOE.GetComponent<CapsuleCollider2D>(), filter, list);
        int hitColliders2 = Physics2D.OverlapCollider(cloudChild.GetComponent<CapsuleCollider2D>(), filter, list2);
        for (int i = hitColliders - 1; i >= 0; i--)
        {
            if (list[i].gameObject.CompareTag("Enemy"))
            {
                if (list[i].gameObject.GetComponent<Enemy>().wet)
                {
                    list[i].gameObject.GetComponent<Enemy>().Stunned = true;
                    list[i].gameObject.GetComponent<Enemy>().StunCooldownTime = stunDuration;
                    list[i].gameObject.GetComponent<Enemy>().StartCoroutine("StunCooldown");
                    list[i].gameObject.GetComponent<Enemy>().curHealth -= Damage * 3;
                    list[i].gameObject.GetComponent<Enemy>().CheckHealth();
                }
                else
                {
                    list[i].gameObject.GetComponent<Enemy>().curHealth -= Damage;
                    list[i].gameObject.GetComponent<Enemy>().CheckHealth();
                }
            }
        }

        for (int i = hitColliders2 - 1; i >= 0; i--)
        {
            if (list2[i].gameObject.CompareTag("Enemy"))
            {
                if (list2[i].gameObject.GetComponent<Enemy>().wet)
                {
                    list2[i].gameObject.GetComponent<Enemy>().Stunned = true;
                    list2[i].gameObject.GetComponent<Enemy>().StunCooldownTime = stunDuration;
                    list2[i].gameObject.GetComponent<Enemy>().StartCoroutine("StunCooldown");
                    list2[i].gameObject.GetComponent<Enemy>().curHealth -= Damage;
                    list2[i].gameObject.GetComponent<Enemy>().CheckHealth();
                }
                else
                {
                    list2[i].gameObject.GetComponent<Enemy>().curHealth -= Damage / 3;
                    list2[i].gameObject.GetComponent<Enemy>().CheckHealth();
                }
            }
        }
        yield return new WaitForSeconds(0.15f);
        StoneAOE.SetActive(false);
        yield return new WaitForSeconds(Random.Range(0.3f, 2f));
        StartCoroutine("ZephyrPhase2");
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
                if (list[i].gameObject.transform.parent != null)
                {
                    Destroy(list[i].gameObject.transform.parent.gameObject);
                }
                else
                {
                    Destroy(list[i].gameObject);
                }
                break;
            }
        }
        if (explode)
        {
            StoneAOE.GetComponent<SpriteRenderer>().color = Color.red;
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
                    if (list[i].gameObject.GetComponent<Enemy>().frozen)
                    {
                        list[i].gameObject.GetComponent<Enemy>().Stunned = false;
                        list[i].gameObject.GetComponent<Enemy>().frozen = false;
                        list[i].gameObject.GetComponent<Enemy>().StopCoroutine("StunCooldown");
                    }
                }
            }
            Destroy(this.gameObject);
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine("PoisonCoroutine");
    }

    IEnumerator MightCoroutine()
    {
        var list = new Collider2D[5];
        var filter = new ContactFilter2D().NoFilter();
        int hitColliders = Physics2D.OverlapCollider(GetComponent<BoxCollider2D>(), filter, list);
        for (int i = hitColliders - 1; i >= 0; i--)
        {
            if (list[i].gameObject.CompareTag("Enemy"))
            {
                list[i].gameObject.GetComponent<Enemy>().curHealth -= Damage;
                list[i].gameObject.GetComponent<Enemy>().CheckHealth();
                list[i].gameObject.GetComponent<Enemy>().Stunned = true;
                list[i].gameObject.GetComponent<Enemy>().StunCooldownTime = stunDuration;
                list[i].gameObject.GetComponent<Enemy>().StopCoroutine("StunCooldown");
                list[i].gameObject.GetComponent<Enemy>().StartCoroutine("StunCooldown");
                if (playerTurnedRight)
                {
                    list[i].gameObject.transform.position = new Vector3(list[i].gameObject.transform.position.x + 0.1f, list[i].gameObject.transform.position.y + 0.1f, list[i].gameObject.transform.position.z);
                }
                else if (!playerTurnedRight)
                {
                    list[i].gameObject.transform.position = new Vector3(list[i].gameObject.transform.position.x - 0.1f, list[i].gameObject.transform.position.y + 0.1f, list[i].gameObject.transform.position.z);
                }
            }
        }
        yield return new WaitForSeconds(0.2f);
        StartCoroutine("MightCoroutine");
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
        else if (CardTrick)
        {
            Player.GetComponent<Movement>().invulnerable = false;
            Player.GetComponent<SpriteRenderer>().color = Player.GetComponent<Movement>().StartingColor;
        }
        else if (Dash)
        {
            MovementScript.dashing = false;
            Player.GetComponent<CapsuleCollider2D>().isTrigger = false;
            Player.GetComponent<Rigidbody2D>().gravityScale = PlayerStartingGravity;
        }
        Destroy(this.gameObject);
    }

    #endregion
}
