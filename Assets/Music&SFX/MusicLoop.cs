using UnityEngine;

public class MusicLoop : MonoBehaviour
{
    public AudioClip startSong;
    public AudioClip loopSong;
    private AudioSource music;
    private float timer = 0;
    void Start()
    {
        music = GetComponent<AudioSource>();
        music.clip = startSong;
        music.Play();
        music.loop = false;
    }

    void Update()
    {
        if (timer <= startSong.length)
        {
            timer += Time.deltaTime;
        }
        else if (!music.loop)
        {
            music.Pause();
            music.clip = loopSong;
            music.Play();
            music.loop = true;
        }

        if (Input.GetKey(KeyCode.M) && music.volume == 0.5f)
        {
            music.volume = 0;
        } 
        else if (Input.GetKey(KeyCode.M) && music.volume == 0)
        {
            music.volume = 0.5f;
        }
    }
}
