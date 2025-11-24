using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    public GameObject Shop;
    public GameObject ShopButtons;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Shop.SetActive(true);
            ShopButtons.SetActive(true);
        }
    }
}
