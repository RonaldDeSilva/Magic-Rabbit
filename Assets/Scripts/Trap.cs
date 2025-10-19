using UnityEngine;

public class Trap : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!collision.gameObject.GetComponent<Movement>().invulnerable && !collision.gameObject.GetComponent<Movement>().justHit && !collision.gameObject.GetComponent<Movement>().StoneForm)
            {
                collision.gameObject.GetComponent<Movement>().curHealth -= damage;
                collision.gameObject.GetComponent<Movement>().CheckHealth(this.gameObject);
            }
        }
    }
}
