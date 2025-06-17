using UnityEngine;

public class HatScript : MonoBehaviour
{
    private GameObject Player;
    private Camera Cam;

    private void Start()
    {
        //Player = transform.parent.parent.gameObject;
        Player = transform.parent.gameObject;
        Cam = GameObject.FindGameObjectWithTag("FakeCamera").GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        //New Way Mouse Way

        Vector2 dir = Cam.ScreenToWorldPoint(Input.mousePosition) - Player.transform.position;
        float angleInRadians = Mathf.Atan2(dir.y, dir.x);
        var angleInDegrees = angleInRadians * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, angleInDegrees);
        

    }
}
