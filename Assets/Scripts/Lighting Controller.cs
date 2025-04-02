using UnityEngine;
using System.Collections;

public class LightingController : MonoBehaviour
{
    private GameObject Player;
    private float PlayerX;
    private float PlayerY;
    public float Scale;
    private float StartingX;
    private float LightStartingX;
    public float LightEndingX;
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
        LightStartingX = PlayerLight.transform.position.x;
        StartCoroutine("LightFollow");
    }

    public void RoomTransfer()
    {
        if (Player.GetComponent<Movement>().turnedRight)
        {
            PlayerLight.transform.position = new Vector3(LightStartingX, PlayerLight.transform.position.y, PlayerLight.transform.position.z);
        }
        else
        {
            PlayerLight.transform.localPosition = new Vector3(LightEndingX, PlayerLight.transform.localPosition.y, PlayerLight.transform.localPosition.z);
        }
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
