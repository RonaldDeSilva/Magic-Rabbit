using System.Collections;
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
    public GameObject BeeGameObject;
    private GameObject Player;
    private GameObject BeeSearchRadius;
    public float teleportRadius;

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
            else if (Player.transform.GetChild(1).GetChild(2).childCount == 0)
            {
                transform.parent = Player.transform.GetChild(1).GetChild(2);
            }
            else if (Player.transform.GetChild(1).GetChild(3).childCount == 0)
            {
                transform.parent = Player.transform.GetChild(1).GetChild(3);
            }
            else
            {
                transform.parent = Player.transform;
            }
        }
        else if (Bee)
        {
            BeeSearchRadius = transform.GetChild(0).gameObject;
        }
    }

    void FixedUpdate()
    {
        if (Bee)
        {
            if (timer % 5f == 0)
            {
                if (Target == null)
                {
                    rb.AddForce(new Vector2((transform.parent.position.x - transform.position.x) * Time.deltaTime * 0.13f, (transform.parent.position.y - transform.position.y) * Time.deltaTime * 0.1f));
                    StartCoroutine("SearchForEnemies");
                }
                else
                {
                    rb.AddForce(new Vector2((Target.transform.position.x - transform.position.x) * Time.deltaTime * 0.11f, (Target.transform.position.y - transform.position.y) * Time.deltaTime * 0.2f));
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
                rb.AddForce(new Vector2((transform.parent.position.x - transform.position.x) * Time.deltaTime * 0.03f, (transform.parent.position.y - transform.position.y) * Time.deltaTime * 0.03f));
                if (timer % 10f == 0 && transform.childCount <= 10)
                {
                    Instantiate(BeeGameObject, this.transform);
                }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && Bee)
        {
            collision.gameObject.GetComponent<Enemy>().curHealth -= damage;
            collision.gameObject.GetComponent<Enemy>().CheckHealth();
            Destroy(this.gameObject);
        }
    }

    IEnumerator SearchForEnemies()
    {
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
        yield return new WaitForEndOfFrame();
    }
}
