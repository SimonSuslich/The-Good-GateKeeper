using UnityEngine;
using TMPro;

public class TutorialCharManager : MonoBehaviour
{
    //Character
    public GameObject CharacterObject;
    private Animator charAnim;
    private bool newCharacter = false;
    private int charIndex = 0;

    //Character Fate
    private bool AccessIsGranted = false;
    private bool AccessIsDenied = false;

    //Variables for movement speed
    private readonly float Speed = 15;
    private readonly float UpSpeed = 10;
    private int paperSpeed = 20;


    //Heaven Permit
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

    //Stamp
    public GameObject stamp;
    private bool stamped;
    public Sprite heavenStampSprite;
    public Sprite hellStampSprite;

    public GameObject textBox;
    public TextMeshProUGUI dialogTextBox;
    private bool firstRound = true;
    public GameObject dialogBoxManager;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogBoxManager.SetActive(false);
        charAnim = CharacterObject.GetComponent<Animator>();
        tapeAnim = tape.GetComponent<Animator>();
        textArrayOfChars = textAllChars.text.Split('|');
        textBox.SetActive(true);
        ClearStamp();
        ShiftCharacter();
        NewCharacterAppear();
    }

    // Update is called once per frame
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
            firstRound = false;
        }


        if (firstRound)
        {
            if (paperIsOnDesk && !stamped)
            {
                dialogTextBox.text = "Press [SPACE] or click on the Heaven Permit to insert it into the Stamp Machine.";
            }
            if (paperIsInserted && stamped)
            {
                dialogTextBox.text = "Press [SPACE] or click on the Heaven Permit to place the paper on the table and review the person's name.";
            }
            if (paperIsInserted && !stamped)
            {
                dialogTextBox.text = "Press the stamp to decide the person's fate.";
            }
            if (paperIsOnDesk && stamped)
            {
                dialogTextBox.text = "Press [SPACE] or click on the Heaven Permit to return it to the person and send him to his destination.";
            }
        }
        else
        {
            dialogTextBox.text = "Well done! Now try doind it by yourself.";
        }



        if (paperInsert && !paperDesk && !paperHide)
        {
            paperBlocked = true;
            if (paper.transform.localPosition.y > -4)
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

    }

    //Character
    public void ShiftCharacter()
    {
        if (charIndex < textArrayOfChars.Length)
        {
            if (charIndex > 0)
            {
                firstRound = false;
            }

            textCharacter = textArrayOfChars[charIndex].Split('*');

            charNameBox.text = textCharacter[0];
            charDescriptionBox.text = textCharacter[1];


            charNameBox.fontSize = 8 - (textCharacter[0].Length - 12) * 0.1f;
            charDescriptionBox.fontSize = 6 - (textCharacter[1].Length - 200) * 0.009f;


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
            dialogBoxManager.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    public void AccessGranted()
    {
        if (!AccessIsDenied)
        {
            AccessIsGranted = true;
        }
    }
    public void AccessDenied()
    {
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