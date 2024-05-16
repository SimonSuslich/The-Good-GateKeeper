//   File: AudioManager.cs
//   Author: Simon Niklas Suslich
//   Date: 2024-05-16
//   Description: Filen hanterar AudioMixer för produkten. Den hanterar uppspelandet av backgrundsmusik och särskillda ljudeffekter.

// Inkluderar Unity Engine Library
using UnityEngine;

// Definierar klass AudioManager av egenskapet Monobehavior
public class AudioManager : MonoBehaviour
{
    //Defiierar public variablar för ljudkälla och ljudklip

    [Header("--------- Audio Source ---------")]    
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("--------- Audio Clip ---------")]  
    public AudioClip backgroundMusic;
    public AudioClip paperSound;
    public AudioClip stampSound;
    public AudioClip printerSound;
    public AudioClip canPressSound;
    public AudioClip canReleaseSound;

    // Definierar en public instance av klassen AudioManager
    public static AudioManager instance;

    // Definierar en private run method som körs vid initialisering filen
    // Den verifierar om det existerar en instance av klassen AudioMixer
    // Om den inte existerar så kommer just den specifika AudioMixer:n inte raderas när man skiftar mellan olika scener
    // Den existerar så kommer detta GameObject att raderas.
    private void Awake()
    {
        // Kollar på värdet av instance och genomför ovanstående funktioner
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Definierar en private run method som körs när relevant GameObject aktiveras
    // Tilldelar musicSource clip värdet backgroundMusic, dvs att klippet blir till bakgrundsmusiken, och sedan börjar spela musicSource
    private void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    // Definierar en public run method som tar emot AudioCLip som argument
    // Låter sfxSource att spela AudioClip en gång
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
