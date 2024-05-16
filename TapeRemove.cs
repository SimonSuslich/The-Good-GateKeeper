//   File: TapeRemove.cs
//   Author: Simon Niklas Suslich
//   Date: 2024-05-16
//   Description: Filen hanterar tape animationen och notificerar andra filer när animationen börjar spelas

// Inkluderar Unity Engine Library
using UnityEngine;

// Definierar klass TapeRemove av egenskapet Monobehavior
public class TapeRemove : MonoBehaviour
{
    // Definierar en global variabel bool tapeFall som används i MainGameManager.cs
    static public bool tapeFall = false;

    // Definierar en public run method som tilldelar variabeln tapeFall värdet true när tape-animationen i Animatorn börjar spelas
    public void TapeFallTrigger()
    {
        tapeFall = true;
    }
}
