using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gem : MonoBehaviour
{
    private int bounces = 0;
    private MemoryCard mem;
    private GameObject canvas;

    private void Start()
    {
        mem = GameObject.Find("MemoryCard").GetComponent<MemoryCard>();
        canvas = GameObject.Find("Canvas");
    }

    void Update()
    {
        if (bounces >= 3)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponents<BoxCollider2D>()[1].isTrigger = true;
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GroundBreakable") || collision.gameObject.CompareTag("GroundMoveable"))
        {
            bounces++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            mem.gems++;
            canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = mem.gems.ToString();
            Destroy(this.gameObject);
        }
    }
}
