//   File: MainGameManager.cs
//   Author: Simon Niklas Suslich
//   Date: 2024-05-16
//   Description: Filen hanterar spelets huvudsakliga gameplay, dvs alla karaktärer, deras öde, heaven permit och people log

// Inkluderar Unity Engine Library och Text Mesh Pro Library
using UnityEngine;
using TMPro;

// Definierar klass MainGameManager av egenskapet Monobehavior
public class MainGameManager : MonoBehaviour
{
    // Definierar public och private variablar för Character GameObject, Animator, bool när ny Character blir kallad och int för index i listan på Characters
    [Header("--------- Character Stuff ---------")]
    public GameObject CharacterObject;
    private Animator charAnim;
    private bool newCharacter = false;
    private int charIndex;

    // Definierar private bools för vart karaktären skickades till, himelen eller helvetet
    private bool AccessIsGranted = false;
    private bool AccessIsDenied = false;

    // Definierar private float och int för hastighet av karaktärens och papprets rörelse
    private readonly float Speed = 15;
    private readonly float UpSpeed = 10;
    private int paperSpeed = 20;

    // Definierar public och private variablar samtliga Heaven Permit relaterade objekt, bland annat, TextMeshPro objekt, arrayer av string, GameObject paper, bool för att beskriva paper's position, GameObject tape och Animator för tape-animationerna
    [Header("--------- Heaven Permit Variables ---------")]
    public TextMeshPro charNameBox;
    public TextMeshPro charDescriptionBox;
    public TextAsset textAllChars;
    private string[] textArrayOfChars;
    private string[] textCharacter;
    public GameObject paper;
    private bool paperHide = false;
    private bool paperInsert = false;
    private bool paperDesk = false;
    private bool characterPaperClick = false;
    private bool paperIsInserted = false;
    private bool paperIsOnDesk = false;
    private bool paperIsReturned = false;
    private bool paperBlocked = false;
    public GameObject tape;
    private Animator tapeAnim;

    // Definierar public och private variablar som är relaterade med stämpeln, bland annat GameObjectet stamp som tillhör Heaven Permit, bool för att verifiera om stämpeln är på och sprites för de två olika stämplarna
    [Header("--------- Stamp Variables ---------")]
    public GameObject stamp;
    private bool stamped;
    public Sprite heavenStampSprite;
    public Sprite hellStampSprite;

    // Definierar public och private variablar samtliga People Log relaterade objekt, bland annat, TextMeshPro objekt, bool för GameObject's position och bool för att verifiera om karaktärsnamn har lagts till i loggen
    [Header("--------- Log Variables ---------")]
    public GameObject paperLog;
    public TextMeshPro logHeaven;
    public TextMeshPro logHell;
    private bool paperLogDesk;
    private bool nameAddedToLog;

    // Definierar public och private variablar för att beskriva GameObject exitButton och bool som verifierar om det inte finns några karatärer kvar
    public GameObject exitButton;
    private bool noCharacters = false;
    
    // Definierar en private variabel för AudioManager
    private AudioManager audioManager;

    // Definierar en private run method som körs vid initialisering filen
    // Finner GameObject med taggen "Audio" och tilldelar audioManager det värdet
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    void Start()
    {
        exitButton.SetActive(false);
        charAnim = CharacterObject.GetComponent<Animator>();
        tapeAnim = tape.GetComponent<Animator>();
        textArrayOfChars = textAllChars.text.Split('|');
        if (PlayerPrefs.HasKey("CharIndex") && PlayerPrefs.GetInt("CharIndex") < textArrayOfChars.Length)
        {
            charIndex = PlayerPrefs.GetInt("CharIndex");
        }
        else
        {
            charIndex = 0;
        }
        paperLog.SetActive(false);
        ClearStamp();
        ShiftCharacter();
        NewCharacterAppear();
    }

    void Update()
    {
        //Character stuff
        if (newCharacter)
        {
            if (CharacterObject.transform.localPosition.y < 0)
            {
                CharacterObject.transform.Translate(new Vector2(0f, 1f) * UpSpeed * Time.deltaTime);
            }
            else
            {
                audioManager.PlaySFX(audioManager.paperSound);
                paperDesk = true;
                newCharacter = false;
            }
        }

        if (AccessIsGranted && !newCharacter && paperIsReturned)
        {
            if (CharacterObject.transform.localPosition.x < 11)
            {
                CharacterObject.transform.Translate(new Vector2(1f, 0f) * Speed * Time.deltaTime);
            }
            else
            {
                paperIsReturned = false;
                AccessIsGranted = false;
                ShiftCharacter();
                NewCharacterAppear();
            }
        }

        if (AccessIsDenied && !newCharacter && paperIsReturned)
        {
            if (CharacterObject.transform.localPosition.x > -11)
            {
                CharacterObject.transform.Translate(new Vector2(-1f, 0f) * Speed * Time.deltaTime);
            }
            else
            {
                paperIsReturned = false;
                AccessIsDenied = false;
                ShiftCharacter();
                NewCharacterAppear();
            }
        }

        if (TapeRemove.tapeFall)
        {
            if (tape.transform.localPosition.y > -35f)
            {
                tape.transform.Translate(new Vector2(0f, -1f) * 80 * Time.deltaTime);
            }
            else
            {
                TapeRemove.tapeFall = false;
            }
        }


        //Paper stuff

        if (Input.GetKeyDown(KeyCode.Space) && !stamped && paperIsOnDesk && !paperBlocked || characterPaperClick && !stamped && paperIsOnDesk && !paperBlocked)
        {
            audioManager.PlaySFX(audioManager.paperSound);
            paperInsert = true;
            characterPaperClick = false;
            paperBlocked = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && stamped && paperIsInserted && !paperBlocked || characterPaperClick && stamped && paperIsInserted && !paperBlocked)
        {
            audioManager.PlaySFX(audioManager.paperSound);
            paperDesk = true;
            characterPaperClick = false;
            paperBlocked = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && stamped && paperIsOnDesk && !paperBlocked || characterPaperClick && stamped && paperIsOnDesk && !paperBlocked)
        {
            audioManager.PlaySFX(audioManager.paperSound);
            paperHide = true;
            characterPaperClick = false;
            paperBlocked = true;
        }




        if (paperInsert && !paperDesk && !paperHide)
        {
            paperBlocked = true;
            if (paper.transform.localPosition.y > -3.8)
            {
                paper.transform.Translate(new Vector2(0f, -1f) * paperSpeed * Time.deltaTime);
            }
            else
            {
                paperInsert = false;
                paperIsInserted = true;
                paperIsOnDesk = false;
                paperBlocked = false;
            }
        }

        if (!paperInsert && !paperDesk && paperHide)
        {
            if (paper.transform.localPosition.y < 14)
            {
                paper.transform.Translate(new Vector2(0f, 1f) * paperSpeed * Time.deltaTime);
            }
            else
            {
                paperHide = false;
                paperIsInserted = false;
                paperIsOnDesk = false;
                paperIsReturned = true;
                paperBlocked = false;
            }
        }

        if (!paperInsert && paperDesk && !paperHide)
        {
            paperBlocked = true;
            if (paper.transform.localPosition.y > 2.2)
            {
                paper.transform.Translate(new Vector2(0f, -1f) * paperSpeed * Time.deltaTime);
            }
            else if (paper.transform.localPosition.y < 1.6)
            {
                paper.transform.Translate(new Vector2(0f, 1f) * paperSpeed * Time.deltaTime);
            }
            else
            {
                paperDesk = false;
                paperIsInserted = false;
                paperIsOnDesk = true;
                paperBlocked = false;

                if (stamped)
                {
                    if (!tapeAnim.GetBool("remove"))
                    {
                        tapeAnim.SetBool("remove", true);
                    }
                }
            }
        }

        if (paperLogDesk)
        {
            if (paperLog.transform.localPosition.y < 1.7)
            {
                paperLog.transform.Translate(new Vector2(0f, 1f) * paperSpeed * Time.deltaTime);
            }
            else
            {
                paperLogDesk = false;
                exitButton.SetActive(true);
            }
        }

        if (noCharacters)
        {
            if (!audioManager.sfxSource.isPlaying)
            {
                noCharacters = false;
                PrintLog();
            }
        }

    }

    //Character
    public void ShiftCharacter()
    {
        PlayerPrefs.SetInt("CharIndex", charIndex);
        if (charIndex < textArrayOfChars.Length)
        {
            nameAddedToLog = false;
            textCharacter = textArrayOfChars[charIndex].Split('*');

            charNameBox.text = textCharacter[0];
            charDescriptionBox.text = textCharacter[1];


            charNameBox.fontSize = 8 - (textCharacter[0].Length - 12) * 0.1f;
            charDescriptionBox.fontSize = 6 - (textCharacter[1].Length - 200) * 0.007f;


            if (textCharacter[2].Trim() == "M")
            {
                charAnim.SetBool("isMale", true);

            }
            else if (textCharacter[2].Trim() == "F")
            {
                charAnim.SetBool("isMale", false);

            }

            charIndex++;
        }
        else
        {
            CharacterObject.SetActive(false);
            paper.SetActive(false);
            audioManager.PlaySFX(audioManager.printerSound);
            noCharacters = true;
        }
    }

    public void PrintLog()
    {
        paperLog.SetActive(true);
        paperLogDesk = true;
    }
    public void AccessGranted()
    {
        if (!nameAddedToLog)
        {
            logHeaven.text += textCharacter[0];
            logHeaven.text += "\n";
            nameAddedToLog = true;
        }

        if (!AccessIsDenied)
        {
            AccessIsGranted = true;
        }
    }
    public void AccessDenied()
    {
        if (!nameAddedToLog)
        {
            logHell.text += textCharacter[0];
            logHell.text += "\n";
            nameAddedToLog = true;
        }

        if (!AccessIsGranted)
        {
            AccessIsDenied = true;
        }
    }

    public void NewCharacterAppear()
    {
        CharacterObject.transform.localPosition = new Vector2(0, -8);
        paper.transform.localPosition = new Vector2(0, 14);
        tapeAnim.SetBool("remove", false);
        tape.transform.localPosition = new Vector2(0f, 5.6f);
        ClearStamp();
        newCharacter = true;
    }



    //Paper

    public void CharacterClicksPaper()
    {
        characterPaperClick = true;
    }



    //Stamp
    public void StampHeaven()
    {
        if (paperIsInserted)
        {
            stamp.GetComponent<SpriteRenderer>().sprite = heavenStampSprite;
            stamped = true;
            AccessGranted();
        }
    }

    public void StampHell()
    {
        if (paperIsInserted)
        {
            stamp.GetComponent<SpriteRenderer>().sprite = hellStampSprite;
            stamped = true;
            AccessDenied();
        }
    }

    public void ClearStamp()
    {
        stamp.GetComponent<SpriteRenderer>().sprite = null;
        stamped = false;
    }
}