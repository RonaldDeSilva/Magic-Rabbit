using UnityEngine;

public class PlayerFollow : MonoBehaviour
{

    private GameObject Player;
    public bool type1;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (type1)
        {
            transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.z);
        }
    }
}
