using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryCard : MonoBehaviour
{
    public int rabbitFeet;
    public int gems;
    public int RoomsPerFoot;
    public int CurrentRooms;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        //rabbitFeet = PlayerPrefs.GetInt("RabbitFeet");
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "New Demo")
        {
            if (CurrentRooms == RoomsPerFoot)
            {
                rabbitFeet += 5;
                GameObject.Find("Canvas").transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = rabbitFeet.ToString();
                CurrentRooms = 0;
                PlayerPrefs.SetInt("RabbitFeet", rabbitFeet);
            }
        }
    }

}
