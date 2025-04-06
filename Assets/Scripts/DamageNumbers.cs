using UnityEngine;

public class DamageNumbers : MonoBehaviour
{
    public Transform[] positions;
    public GameObject[] numbers;
    private float initialXSpeed;
    private float initialYSpeed;
    private float initialRotation;
    private float targetRotation;
    private float rotationRate;
    public float duration;
    public int min;
    public int max;
    private float xRate;
    private float yRate;
    private Rigidbody2D rb;
    private float Size;


    private void Start()
    {
        var coinFlip1 = Random.Range(0, 2);
        if (coinFlip1 == 0)
        {
            initialXSpeed = -Random.Range(min, max);
        }
        else if (coinFlip1 == 1)
        {
            initialXSpeed = Random.Range(min, max);
        }

        var coinFlip2 = Random.Range(0, 2);
        if (coinFlip2 == 0)
        {
            initialYSpeed = -Random.Range(min, max);
        }
        else if (coinFlip2 == 1)
        {
            initialYSpeed = Random.Range(min, max);
        }

        var coinFlip3 = Random.Range(0, 2);
        if (coinFlip3 == 0)
        {
            initialRotation = -Random.Range(min, max);
        }
        else if (coinFlip3 == 1)
        {
            initialRotation = Random.Range(min, max);
        }
        targetRotation = -initialRotation;

        rotationRate = Mathf.Abs(initialRotation / 15);
        xRate = Mathf.Abs(initialXSpeed / 30);
        yRate = Mathf.Abs(initialYSpeed / 30);

        rb = GetComponent<Rigidbody2D>();
        rb.rotation = initialRotation;
    }

    private void FixedUpdate()
    {
        if (duration >= 0)
        {
            rb.linearVelocity = new Vector2(initialXSpeed, initialYSpeed);
            rb.rotation = initialRotation;
            if (initialXSpeed < 0f)
            {
                initialXSpeed += xRate;
                initialXSpeed = Mathf.Clamp(initialXSpeed, initialXSpeed, 0f);
            }
            else if (initialXSpeed > 0f)
            {
                initialXSpeed -= xRate;
                initialXSpeed = Mathf.Clamp(initialXSpeed, 0f, initialXSpeed);
            }

            if (initialYSpeed < 0f)
            {
                initialYSpeed += yRate;
                initialYSpeed = Mathf.Clamp(initialYSpeed, initialYSpeed, 0f);
            }
            else if (initialYSpeed > 0f)
            {
                initialYSpeed -= yRate;
                initialYSpeed = Mathf.Clamp(initialYSpeed, 0f, initialYSpeed);
            }

            if (initialRotation < targetRotation)
            {
                initialRotation += rotationRate;
            }
            else if (initialRotation > targetRotation)
            {
                initialRotation -= rotationRate;
            }

            duration -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Display(int Damage, Color numberColor)
    {
        var len = Damage.ToString().Length;
        Size = Damage / 100;
        transform.localScale = new Vector3(1 * (1 + Size), 1 * (1 + Size), 1);
        if (len == 1)
        {
            var damageText = Damage.ToString();
            var damageTextList = damageText.ToCharArray();
            if (damageTextList[0] == '0')
            {
                var num = Instantiate(numbers[0], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;

            }
            else if (damageTextList[0] == '1')
            {
                var num = Instantiate(numbers[1], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '2')
            {
                var num = Instantiate(numbers[2], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '3')
            {
                var num = Instantiate(numbers[3], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '4')
            {
                var num = Instantiate(numbers[4], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '5')
            {
                var num = Instantiate(numbers[5], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '6')
            {
                var num = Instantiate(numbers[6], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '7')
            {
                var num = Instantiate(numbers[7], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '8')
            {
                var num = Instantiate(numbers[8], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '9')
            {
                var num = Instantiate(numbers[9], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
        }
        else if (len == 2)
        {
            var damageText = Damage.ToString();
            var damageTextList = damageText.ToCharArray();
            //First Character
            if (damageTextList[0] == '0')
            {
                var num = Instantiate(numbers[0], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '1')
            {
                var num = Instantiate(numbers[1], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '2')
            {
                var num = Instantiate(numbers[2], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '3')
            {
                var num = Instantiate(numbers[3], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '4')
            {
                var num = Instantiate(numbers[4], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '5')
            {
                var num = Instantiate(numbers[5], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '6')
            {
                var num = Instantiate(numbers[6], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '7')
            {
                var num = Instantiate(numbers[7], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '8')
            {
                var num = Instantiate(numbers[8], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '9')
            {
                var num = Instantiate(numbers[9], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            //Second Character
            if (damageTextList[1] == '0')
            {
                var num = Instantiate(numbers[0], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '1')
            {
                var num = Instantiate(numbers[1], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '2')
            {
                var num = Instantiate(numbers[2], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '3')
            {
                var num = Instantiate(numbers[3], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '4')
            {
                var num = Instantiate(numbers[4], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '5')
            {
                var num = Instantiate(numbers[5], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '6')
            {
                var num = Instantiate(numbers[6], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '7')
            {
                var num = Instantiate(numbers[7], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '8')
            {
                var num = Instantiate(numbers[8], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '9')
            {
                var num = Instantiate(numbers[9], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
        }
        else if (len == 3)
        {
            var damageText = Damage.ToString();
            var damageTextList = damageText.ToCharArray();
            //First Character
            if (damageTextList[0] == '0')
            {
                var num = Instantiate(numbers[0], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '1')
            {
                var num = Instantiate(numbers[1], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '2')
            {
                var num = Instantiate(numbers[2], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '3')
            {
                var num = Instantiate(numbers[3], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '4')
            {
                var num = Instantiate(numbers[4], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '5')
            {
                var num = Instantiate(numbers[5], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '6')
            {
                var num = Instantiate(numbers[6], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '7')
            {
                var num = Instantiate(numbers[7], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '8')
            {
                var num = Instantiate(numbers[8], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[0] == '9')
            {
                var num = Instantiate(numbers[9], positions[0], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            //Second Character
            if (damageTextList[1] == '0')
            {
                var num = Instantiate(numbers[0], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '1')
            {
                var num = Instantiate(numbers[1], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '2')
            {
                var num = Instantiate(numbers[2], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '3')
            {
                var num = Instantiate(numbers[3], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '4')
            {
                var num = Instantiate(numbers[4], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '5')
            {
                var num = Instantiate(numbers[5], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '6')
            {
                var num = Instantiate(numbers[6], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '7')
            {
                var num = Instantiate(numbers[7], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '8')
            {
                var num = Instantiate(numbers[8], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[1] == '9')
            {
                var num = Instantiate(numbers[9], positions[1], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            //Third Character
            if (damageTextList[2] == '0')
            {
                var num = Instantiate(numbers[0], positions[2], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[2] == '1')
            {
                var num = Instantiate(numbers[1], positions[2], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[2] == '2')
            {
                var num = Instantiate(numbers[2], positions[2], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[2] == '3')
            {
                var num = Instantiate(numbers[3], positions[2], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[2] == '4')
            {
                var num = Instantiate(numbers[4], positions[2], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[2] == '5')
            {
                var num = Instantiate(numbers[5], positions[2], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[2] == '6')
            {
                var num = Instantiate(numbers[6], positions[2], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[2] == '7')
            {
                var num = Instantiate(numbers[7], positions[2], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[2] == '8')
            {
                var num = Instantiate(numbers[8], positions[2], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
            else if (damageTextList[2] == '9')
            {
                var num = Instantiate(numbers[9], positions[2], false);
                num.transform.localPosition = Vector3.zero;
                num.GetComponent<SpriteRenderer>().color = numberColor;
            }
        }
    }
}
