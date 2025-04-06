using UnityEngine;

public class StrikePoint : MonoBehaviour
{
    private Attack Father;
    private float RaycastPoint;
    private CapsuleCollider2D theCollider;
    private SpriteRenderer sprite;
    private float rotation;

    private void Start()
    {
        Father = transform.parent.gameObject.GetComponent<Attack>();
        sprite = GetComponent<SpriteRenderer>();
        theCollider = GetComponent<CapsuleCollider2D>();
        sprite.enabled = false;
        theCollider.enabled = false;
    }
    void FixedUpdate()
    {
        RaycastPoint = Father.distance;
        rotation = Father.rotation;
        if (Father.StoneAOE.activeSelf)
        {
            sprite.enabled = true;
            theCollider.enabled = true;
        }
        else if (!Father.StoneAOE.activeSelf)
        {
            sprite.enabled = false;
            theCollider.enabled = false;
        }

        transform.position = new Vector3(Father.gameObject.transform.position.x, RaycastPoint, transform.position.z);
    }
}
