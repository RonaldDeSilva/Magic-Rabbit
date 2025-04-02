using UnityEngine;

public class RoomScript : MonoBehaviour
{
    private GameObject MC;
    private LightingController LightController;

    private void Start()
    {
        MC = GameObject.FindGameObjectWithTag("FakeCamera");
        LightController = GameObject.Find("LightingController").GetComponent<LightingController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MC.transform.position = new Vector3(transform.position.x, transform.position.y, MC.transform.position.z);
            LightController.RoomTransfer();
        }
    }
}
