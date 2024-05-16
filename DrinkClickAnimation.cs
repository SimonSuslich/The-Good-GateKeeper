//   File: DrinkClickAnimation.cs
//   Author: Simon Niklas Suslich
//   Date: 2024-05-16
//   Description: Filen hanterar animationer för HolyDrink objektet och eventuella ljudeffekter vid klick

// Inkluderar Unity Engine Library
using UnityEngine;

// Definierar klass DrinkClickAnimation av egenskapet Monobehavior
public class DrinkClickAnimation : MonoBehaviour
{
    // Definierar en private variabel för AudioManager
    private AudioManager audioManager;

    // Definierar private variablar för sprites när HolyDrink är tryckt och inte tryckt på
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite activeSprite;

    // Definierar en private run method som körs vid initialisering filen
    // Finner GameObject med taggen "Audio" och tilldelar audioManager det värdet
    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Definierar en public run method som tilldelar detta GameObject sprite värdet activeSprite och kallar på en method från audioManager vid namnet PlaySFX(AudioClip)
    public void DrinkActive()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = activeSprite;
        audioManager.PlaySFX(audioManager.canPressSound);
    }

    // Definierar en public run method som tilldelar detta GameObject sprite värdet defaultSprite och kallar på en method från audioManager vid namnet PlaySFX(AudioClip)
    public void DrinkUnactive()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 75);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;
        audioManager.PlaySFX(audioManager.canReleaseSound);
    }
}
