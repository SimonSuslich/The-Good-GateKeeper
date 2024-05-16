//   File: BtnClickAnimation.cs
//   Author: Simon Niklas Suslich
//   Date: 2024-05-16
//   Description: Filen hanterar animationer för GreenButton och RedButton objekten i StampMachine och eventuella ljudeffekter vid knapptrycken

// Inkluderar Unity Engine Library
using UnityEngine;

// Definierar klass BtnClickAnimation av egenskapet Monobehavior
public class BtnClickAnimation : MonoBehaviour
{
    // Definierar en private variabel för AudioManager
    private AudioManager audioManager;

    // Definierar private variablar för sprites när knappen är aktiv och inaktiv
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite activeSprite;

    // Definierar en private run method som körs vid initialisering filen
    // Finner GameObject med taggen "Audio" och tilldelar audioManager det värdet
    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Definierar en public run method som tilldelar detta GameObject sprite värdet activeSprite och kallar på en method från audioManager vid namnet PlaySFX(AudioClip)
    public void BtnActive()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = activeSprite;
        audioManager.PlaySFX(audioManager.stampSound);
    }

    // Definierar en public run method som tilldelar detta GameObject sprite värdet defaultSprite
    public void BntUnactive()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }
}
