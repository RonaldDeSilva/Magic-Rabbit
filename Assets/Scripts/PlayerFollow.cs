using UnityEngine;

public class PlayerFollow : MonoBehaviour
{

    private GameObject Player;
    public bool type1;
    public bool type2;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (type1)
        {
            transform.position = new Vector3(Mathf.Clamp(Player.transform.position.x, 0, 9999999), transform.position.y, -10);
        }
        else if (type2)
        {

        }
    }
}
