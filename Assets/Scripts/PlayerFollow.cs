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
            transform.position = new Vector3(Mathf.Clamp(Player.transform.position.x, startingX, 9999999), transform.position.y, -10);
        }
        else if (type2)
        {

        }
    }
}
