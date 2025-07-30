using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Companion : MonoBehaviour
{
    public bool Bee;
    public bool BeeHive;
    private GameObject Target;
    private float timer = 0f;
    private Rigidbody2D rb;
    public int damage;
    public int health;
    public float speed;
    public GameObject BeeGameObject;
    private GameObject Player;
    private GameObject BeeSearchRadius;
    public float teleportRadius;
    public int randRange;
    public float BeeTimer;
    private float beeSpawned = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        if (BeeHive)
        {
            if (Player.transform.GetChild(1).GetChild(0).childCount == 0)
            {
                transform.parent = Player.transform.GetChild(1).GetChild(0);
            }
            else if (Player.transform.GetChild(1).GetChild(1).childCount == 0)
            {
                transform.parent = Player.transform.GetChild(1).GetChild(1);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else if (Bee)
        {
            BeeSearchRadius = transform.GetChild(0).gameObject;
            randRange = transform.parent.gameObject.GetComponent<Companion>().randRange;
        }
    }

    void FixedUpdate()
    {
        if (Bee)
        {
            if (timer % 5f == 0)
            {
                if (Target == null && BeeTimer <= 0f)
                {
                    //rb.AddForce(new Vector2((transform.parent.position.x - transform.position.x) * Time.deltaTime * 0.13f, 0f));
                    //rb.linearVelocity = new Vector2((transform.parent.position.x - transform.position.x) * 3, (transform.parent.position.y - transform.position.y) * 2);
                    rb.linearVelocity = new Vector2((transform.parent.position.x - transform.position.x) * 10 + Random.Range(-randRange, randRange), (transform.parent.position.y - transform.position.y) * 10 + Random.Range(-randRange, randRange));
                    var list = new Collider2D[10];
                    var filter = new ContactFilter2D().NoFilter();
                    int hitColliders = Physics2D.OverlapCollider(BeeSearchRadius.GetComponent<CircleCollider2D>(), filter, list);
                    for (int i = hitColliders - 1; i >= 0; i--)
                    {
                        if (list[i].gameObject.CompareTag("Enemy"))
                        {
                            Target = list[i].gameObject;
                        }
                    }
                }
                else if (BeeTimer >= 0f && Target == null)
                {
                    //rb.linearVelocity = new Vector2(-(transform.parent.position.x - transform.position.x), -(transform.parent.position.y - transform.position.y));
                    rb.linearVelocity = new Vector2((transform.parent.position.x - transform.position.x) * 8 + Random.Range(-randRange, randRange), (transform.parent.position.y - transform.position.y) * 8 + Random.Range(-randRange, randRange));
                    BeeTimer -= Time.deltaTime;
                    var list = new Collider2D[10];
                    var filter = new ContactFilter2D().NoFilter();
                    int hitColliders = Physics2D.OverlapCollider(BeeSearchRadius.GetComponent<CircleCollider2D>(), filter, list);
                    for (int i = hitColliders - 1; i >= 0; i--)
                    {
                        if (list[i].gameObject.CompareTag("Enemy"))
                        {
                            Target = list[i].gameObject;
                        }
                    }
                }
                else
                {
                    rb.linearVelocity = new Vector2((Target.transform.position.x - transform.position.x), (Target.transform.position.y - transform.position.y));
                }
            }

            if (Mathf.Abs(Player.transform.position.x - transform.position.x) > teleportRadius)
            {
                transform.position = transform.parent.position;
                rb.linearVelocity = Vector2.zero;
            }
        }
        else if (BeeHive) 
        {
            if (timer % 2f == 0)
            {
                rb.linearVelocity = new Vector2((transform.parent.position.x - transform.position.x) * speed , (transform.parent.position.y - transform.position.y) * speed);
                if (timer % 10f == 0 && transform.childCount <= 7 && beeSpawned <= 13)
                {
                    Instantiate(BeeGameObject, this.transform);
                    beeSpawned = beeSpawned + 1;
                }
            }

            if (beeSpawned >= 14 && transform.childCount == 0)
            {
                Destroy(this.gameObject);
            }

            if (timer >= 310f)
            {
                timer = 0;
            }

            if (Mathf.Abs(Player.transform.position.x - transform.position.x) > teleportRadius)
            {
                transform.position = transform.parent.position;
                rb.linearVelocity = Vector2.zero;
            }
        }
        timer += 1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && Bee)
        {
            collision.gameObject.GetComponent<Enemy>().curHealth -= damage;
            collision.gameObject.GetComponent<Enemy>().CheckHealth();
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("BeeHive"))
        {
            BeeTimer = 0.05f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BeeHive"))
        {
            BeeTimer = 0.05f;
        }
    }
}
