using UnityEngine;

public class HatScript : MonoBehaviour
{
    public GameObject Position1;
    public GameObject Position2;
    public GameObject Position3;
    public GameObject Position4;
    public GameObject Position5;
    public GameObject Position6;
    public GameObject Position7;
    public GameObject Position8;
    public float tolerance;
    private GameObject Player;
    private Camera Cam;

    private void Start()
    {
        //Player = transform.parent.parent.gameObject;
        Player = transform.parent.gameObject;
        Cam = GameObject.FindGameObjectWithTag("FakeCamera").GetComponent<Camera>();
    }

    void Update()
    {
        //New Way Mouse Way

        Vector2 dir = Cam.ScreenToWorldPoint(Input.mousePosition) - Player.transform.position;
        float angleInRadians = Mathf.Atan2(dir.y, dir.x);
        var angleInDegrees = angleInRadians * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, angleInDegrees);
        
        /*
        if (angleInDegrees < 0)
        {
            angleInDegrees = angleInDegrees + 360f;
        }

        if (angleInDegrees > 335 || angleInDegrees < 25)
        {

        }
        */

        //Old Arrow keys way
        /*
        if (Input.GetAxis("HorizontalAK") != 0)
        {
            if (Input.GetAxis("VerticalAK") != 0)
            {
                if (Input.GetAxis("VerticalAK") > tolerance && Input.GetAxis("HorizontalAK") > tolerance)
                {
                    transform.localPosition = Position2.transform.localPosition;
                    transform.rotation = Position2.transform.rotation;
                }
                else if (Input.GetAxis("VerticalAK") > tolerance && Input.GetAxis("HorizontalAK") < -tolerance)
                {
                    transform.localPosition = Position4.transform.localPosition;
                    transform.rotation = Position4.transform.rotation;
                }
                else if (Input.GetAxis("VerticalAK") < -tolerance && Input.GetAxis("HorizontalAK") < -tolerance)
                {
                    transform.localPosition = Position6.transform.localPosition;
                    transform.rotation = Position6.transform.rotation;
                }
                else if (Input.GetAxis("VerticalAK") < -tolerance && Input.GetAxis("HorizontalAK") > tolerance)
                {
                    transform.localPosition = Position8.transform.localPosition;
                    transform.rotation = Position8.transform.rotation;
                }
            }
            else
            {
                if (Input.GetAxis("HorizontalAK") > tolerance)
                {
                    transform.localPosition = Position1.transform.localPosition;
                    transform.rotation = Position1.transform.rotation;
                }
                else if (Input.GetAxis("HorizontalAK") < -tolerance)
                {
                    transform.localPosition = Position5.transform.localPosition;
                    transform.rotation = Position5.transform.rotation;
                }
            }
        }
        else if (Input.GetAxis("VerticalAK") != 0)
        {
            if (Input.GetAxis("VerticalAK") > tolerance)
            {
                transform.localPosition = Position3.transform.localPosition;
                transform.rotation = Position3.transform.rotation;
            }
            else if (Input.GetAxis("VerticalAK") < -tolerance)
            {
                transform.localPosition = Position7.transform.localPosition;
                transform.rotation = Position7.transform.rotation;
            }
        }*/
    }
}
