using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    public int rabbitFeet;
    public int gems;
    public int RoomsPerFoot;
    public int CurrentRooms;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (CurrentRooms == RoomsPerFoot)
        {
            rabbitFeet += 1;
            //Animation to show new rabbit foot gained
            CurrentRooms = 0;
        }
    }

}
