using UnityEngine;

public class lightning : MonoBehaviour
{
    private Attack father;

    private void Start()
    {
        father = transform.parent.gameObject.GetComponent<Attack>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //father.SwitchDirections
        if (collision.gameObject.CompareTag("Ground"))
        {
            father = transform.parent.gameObject.GetComponent<Attack>();
            father.Phase1 = false;
            father.Phase2 = true;
            father.StartCoroutine("ZephyrPhase2");
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            father = transform.parent.gameObject.GetComponent<Attack>();
            father.Phase1 = false;
            father.Phase2 = true;
            father.StartCoroutine("ZephyrPhase2");
            Destroy(this.gameObject);
        }
    }
}
