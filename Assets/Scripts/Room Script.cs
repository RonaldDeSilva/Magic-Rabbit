using UnityEngine;

public class RoomScript : MonoBehaviour
{
    //private GameObject MC;
    //private LightingController LightController;
    public GameObject Barriers;
    private GameObject Enemies;
    private bool playerIsIn;
    private MemoryCard mem;
    private bool CombatRoom = false;

    private void Start()
    {
        Barriers = transform.GetChild(0).gameObject;
        Enemies = transform.GetChild(1).gameObject;
        //MC = GameObject.FindGameObjectWithTag("FakeCamera");
        //LightController = GameObject.Find("LightingController").GetComponent<LightingController>();
        Enemies.SetActive(false);
        Barriers.SetActive(false);
        mem = GameObject.Find("MemoryCard").GetComponent<MemoryCard>();
    }

    private void Update()
    {
        if (playerIsIn)
        {
            var len = Enemies.transform.childCount;
            if (len == 0)
            {
                Barriers.SetActive(false);
                playerIsIn = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            Enemies.SetActive(true);
            if (Enemies.transform.childCount > 0)
            {
                Barriers.SetActive(true);
                CombatRoom = true;
            }
            //MC.transform.position = new Vector3(transform.position.x, transform.position.y, MC.transform.position.z);
            //LightController.RoomTransfer();
            playerIsIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (CombatRoom)
            {
                mem.CurrentRooms += 1;
                CombatRoom = false;
            }
        }
    }
}
