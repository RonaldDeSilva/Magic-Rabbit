using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Movement PlayerScript;
    public Sprite[] sprites;

    void Start()
    {
        PlayerScript = transform.parent.GetComponent<Movement>();
    }


    void Update()
    {
        if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.05f * PlayerScript.maxHealth)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[1];
            if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.1f * PlayerScript.maxHealth)
            {
                GetComponent<SpriteRenderer>().sprite = sprites[2];
                if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.15f * PlayerScript.maxHealth)
                {
                    GetComponent<SpriteRenderer>().sprite = sprites[3];
                    if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.2f * PlayerScript.maxHealth)
                    {
                        GetComponent<SpriteRenderer>().sprite = sprites[4];
                        if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.25f * PlayerScript.maxHealth)
                        {
                            GetComponent<SpriteRenderer>().sprite = sprites[5];
                            if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.3f * PlayerScript.maxHealth)
                            {
                                GetComponent<SpriteRenderer>().sprite = sprites[6];
                                if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.35f * PlayerScript.maxHealth)
                                {
                                    GetComponent<SpriteRenderer>().sprite = sprites[7];
                                    if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.4f * PlayerScript.maxHealth)
                                    {
                                        GetComponent<SpriteRenderer>().sprite = sprites[8];
                                        if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.45f * PlayerScript.maxHealth)
                                        {
                                            GetComponent<SpriteRenderer>().sprite = sprites[9];
                                            if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.5f * PlayerScript.maxHealth)
                                            {
                                                GetComponent<SpriteRenderer>().sprite = sprites[10];
                                                if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.55f * PlayerScript.maxHealth)
                                                {
                                                    GetComponent<SpriteRenderer>().sprite = sprites[11];
                                                    if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.6f * PlayerScript.maxHealth)
                                                    {
                                                        GetComponent<SpriteRenderer>().sprite = sprites[12];
                                                        if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.65f * PlayerScript.maxHealth)
                                                        {
                                                            GetComponent<SpriteRenderer>().sprite = sprites[13];
                                                            if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.7f * PlayerScript.maxHealth)
                                                            {
                                                                GetComponent<SpriteRenderer>().sprite = sprites[14];
                                                                if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.75f * PlayerScript.maxHealth)
                                                                {
                                                                    GetComponent<SpriteRenderer>().sprite = sprites[15];
                                                                    if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.8f * PlayerScript.maxHealth)
                                                                    {
                                                                        GetComponent<SpriteRenderer>().sprite = sprites[16];
                                                                        if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.85f * PlayerScript.maxHealth)
                                                                        {
                                                                            GetComponent<SpriteRenderer>().sprite = sprites[17];
                                                                            if (PlayerScript.maxHealth - PlayerScript.curHealth >= 0.9f * PlayerScript.maxHealth)
                                                                            {
                                                                                GetComponent<SpriteRenderer>().sprite = sprites[18];
                                                                                if (PlayerScript.maxHealth - PlayerScript.curHealth >= PlayerScript.maxHealth)
                                                                                {
                                                                                    GetComponent<SpriteRenderer>().sprite = sprites[19];                                                                                    
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
