using UnityEngine;

public class PlayerFollow : MonoBehaviour
{

    private GameObject Player;
    public bool type1;
    public bool type2;
    private float startingX;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        startingX = transform.position.x;
    }

    void Update()
    {
        if (type1)
        {
            transform.position = new Vector3(Player.transform.position.x, transform.position.y, -10);
        }
        else if (type2)
        {

        }
    }
}
