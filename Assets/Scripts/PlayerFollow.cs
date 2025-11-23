using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    
    private GameObject Player;
    public bool type1;
    public GameObject ShopScreen;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        ShopScreen = transform.GetChild(3).gameObject;
        ShopScreen.SetActive(false);
    }

    void Update()
    {
        if (type1)
        {
            transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.z);
        }
    }
}
