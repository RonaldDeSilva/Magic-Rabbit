using UnityEngine;

public class RoomScript : MonoBehaviour
{
    private GameObject MC;

    private void Start()
    {
        MC = GameObject.FindGameObjectWithTag("FakeCamera");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MC.transform.position = new Vector3(transform.position.x, transform.position.y, MC.transform.position.z);
        }
    }
}
