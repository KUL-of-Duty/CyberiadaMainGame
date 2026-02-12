using UnityEngine;

// Enum pozostaje bez zmian, upewnij się że SPRINT tam jest
public enum SoundType { WALK, JUMP, SPRINT, PICKUP, DROP, DOOROPENING, RELOAD, PISTOLSHOT, RIFLESHOT, WEAPONCHANGE, AXESWING, TAKINGDAMAGE, SMOKING, ENEMYROAR, ENEMYWALK, ENEMYATTACK }

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;
    private AudioSource feetAudioSource; 

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
       
        feetAudioSource = gameObject.AddComponent<AudioSource>();
        feetAudioSource.spatialBlend = audioSource.spatialBlend;
    }

    public static void PlaySound(SoundType sound, float volume = 1f)
{
    //zabezpieczenie bo muszę ogarnąć gdzie jest error
    
    if (instance == null)
    {
        Debug.LogWarning("SoundManager: Brak instancji na scenie!");
        return;
    }

    
    if (instance.soundList == null)
    {
        Debug.LogError("SoundManager: Tablica soundList nie jest przypisana w Inspektorze!");
        return;
    }

    int index = (int)sound;

    
    if (index < instance.soundList.Length && instance.soundList[index] != null)
    {
        instance.audioSource.PlayOneShot(instance.soundList[index], volume);
    }
    else
    {
        Debug.LogWarning($"SoundManager: Brak klipu audio dla typu {sound} na indeksie {index}");
    }
}

    public static void PlayFootstep(SoundType sound, float volume = 1f)
    {
        if (instance == null || (int)sound >= instance.soundList.Length) return;

       
        if (!instance.feetAudioSource.isPlaying)
        {
            instance.feetAudioSource.clip = instance.soundList[(int)sound];
            instance.feetAudioSource.volume = volume;
            instance.feetAudioSource.Play();
        }
    }
}