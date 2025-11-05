using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    
    private string musicName = "XD";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound()
    {
        Debug.Log(musicName);
    }
}
