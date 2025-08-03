using UnityEngine;

public class DeathMessages : MonoBehaviour
{
    public Sprite[] sprites;

    void Start()
    {
        var num = Random.Range(0, sprites.Length);
        GetComponent<SpriteRenderer>().sprite = sprites[num];
    }

}
