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

    void Update()
    {
        if (Input.GetAxis("HorizontalAK") != 0)
        {
            if (Input.GetAxis("VerticalAK") != 0)
            {
                if (Input.GetAxis("VerticalAK") > tolerance && Input.GetAxis("HorizontalAK") > tolerance)
                {
                    transform.parent = Position2.transform;
                    transform.localPosition = Vector3.zero;
                    transform.rotation = Position2.transform.rotation;
                }
                else if (Input.GetAxis("VerticalAK") > tolerance && Input.GetAxis("HorizontalAK") < -tolerance)
                {
                    transform.parent = Position4.transform;
                    transform.localPosition = Vector3.zero;
                    transform.rotation = Position4.transform.rotation;
                }
                else if (Input.GetAxis("VerticalAK") < -tolerance && Input.GetAxis("HorizontalAK") < -tolerance)
                {
                    transform.parent = Position6.transform;
                    transform.localPosition = Vector3.zero;
                    transform.rotation = Position6.transform.rotation;
                }
                else if (Input.GetAxis("VerticalAK") < -tolerance && Input.GetAxis("HorizontalAK") > tolerance)
                {
                    transform.parent = Position8.transform;
                    transform.localPosition = Vector3.zero;
                    transform.rotation = Position8.transform.rotation;
                }
            }
            else
            {
                if (Input.GetAxis("HorizontalAK") > tolerance)
                {
                    transform.parent = Position1.transform;
                    transform.localPosition = Vector3.zero;
                    transform.rotation = Position1.transform.rotation;
                }
                else if (Input.GetAxis("HorizontalAK") < -tolerance)
                {
                    transform.parent = Position5.transform;
                    transform.localPosition = Vector3.zero;
                    transform.rotation = Position5.transform.rotation;
                }
            }
        }
        else if (Input.GetAxis("VerticalAK") != 0)
        {
            if (Input.GetAxis("VerticalAK") > tolerance)
            {
                transform.parent = Position3.transform;
                transform.localPosition = Vector3.zero;
                transform.rotation = Position3.transform.rotation;
            }
            else if (Input.GetAxis("VerticalAK") < -tolerance)
            {
                transform.parent = Position7.transform;
                transform.localPosition = Vector3.zero;
                transform.rotation = Position7.transform.rotation;
            }
        }
    }
}
