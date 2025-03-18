using UnityEngine;

public class PlayerFollow : MonoBehaviour
{

    private GameObject Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(Player.transform.position.x, 0, 9999999), transform.position.y, -10);
    }
}
