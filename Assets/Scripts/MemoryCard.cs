using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryCard : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
