using UnityEngine;
using System.Collections;

public class LightingController : MonoBehaviour
{
    private GameObject Player;
    private float PlayerX;
    private float PlayerY;
    public float Scale;
    private float StartingX;
    private GameObject PlayerLight;
    private GameObject IntroLighting;
    private GameObject LoopingLighting;
    private GameObject BossRoomLighting;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerX = Player.transform.position.x;
        StartingX = PlayerX;
        PlayerY = Player.transform.position.y;
        PlayerLight = transform.GetChild(0).gameObject;
        StartCoroutine("LightFollow");
    }

    IEnumerator LightFollow()
    {
        yield return new WaitForSeconds(0.1f);
        if (Player.transform.position.x != PlayerX || Player.transform.position.y != PlayerY)
        {
            PlayerLight.transform.position = new Vector3(PlayerLight.transform.position.x + ((Player.transform.position.x - PlayerX) * Scale), PlayerLight.transform.position.y + ((Player.transform.position.y - PlayerY) * Scale), PlayerLight.transform.position.z);
            PlayerX = Player.transform.position.x;
            PlayerY = Player.transform.position.y;
        }
        StartCoroutine("LightFollow");
    }
}
