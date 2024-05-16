//   File: TutorialDialog.cs
//   Author: Simon Niklas Suslich
//   Date: 2024-05-16
//   Description: Filen hanterar dialogerna i Tutorial scenen

// Inkluderar Unity Engine Library, Scene Management och Text Mesh Pro Library och 
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Definierar klass TutorialDialog av egenskapet Monobehavior
public class TutorialDialog : MonoBehaviour
{
    // Definierar public och private variablar som är relaterade till dialogBoxen i Tutorial scenen
    public GameObject CharacterManager;
    public GameObject textBox;
    public TextMeshProUGUI dialogTextBox;
    private string[] dialogPart;
    private string[] dialogArray;
    public TextAsset dialogDoc;
    private int dialogI = 0;
    private int dialogJ = 0;

    // Definierar en run method som körs när relevant GameObject aktiveras
    // Methoden delar string informationen från dialogDoc i två arrayer, dialogPart (de två delarna av dialogen, början och slutet) och dialogArray (varje enskild bit i dialogen).
    void Start()
    {
        dialogPart = dialogDoc.text.Split('|');
        dialogArray = dialogPart[dialogI].Split('\n');
    }

    // Definierar en loopande run method som tar emot användarinput och hanterar dialogBoxen utifrån det.
    void Update()
    {
        // If sats som tar emot användar input och ökar värdet på dialogJ vilket itterar dialogArray[]
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogJ++;
        }

        // If sats som verifierar att dialogJ inte är utanför spannet av dialogArray[]
        // Om true, då blir texten i dialogTextBox en tom string, textBox döljs, CharacterManager aktiveras, dialogI ittereras
        // Om false, då tilldelas dialogBoxens text värdet av dialogArray[dialogJ]
        if (dialogJ > dialogArray.Length - 1)
        {
            dialogTextBox.text = "";
            textBox.SetActive(false);
            CharacterManager.SetActive(true);
            dialogI++;
            // If sats som ger true om dialogI är mindre än eller lika med längden på arrayen dialogPart
            // Om true, arrayen dialogArrya tilldelas värdet av dialogPart av index dialogI splitat vid varje radbrytning
            if (dialogI <= dialogPart.Length - 1) {
                dialogArray = dialogPart[dialogI].Split('\n');
                dialogJ = 0;
            }
        }
        else
        {
            dialogTextBox.text = dialogArray[dialogJ];
        }

        // Embeded if sats som kollar om dialogPart och dialogArray har ittererats till fullo

        // If sats som ger true om dialogI är utanför spannet på dialogPart
        if (dialogI > dialogPart.Length - 1) {
            // If sats som ger true om dialogJ är utanför spannet på dialogArray
            // Om true, SceneManager laddar upp MainGame scenen
            if (dialogJ > dialogArray.Length - 1) {
                SceneManager.LoadSceneAsync(1);
            }
        }
    }

    // Definierar en public run method som ökar värdet av indexen dialogJ med 1, vilket gör att dialogArray[] ittereras
    public void Nextdialog()
    {
        dialogJ++;
    }

    // Definierar en public run method som låter SceneManager att ladda Main Game scenen
    public void SkipTutorial() {
        SceneManager.LoadSceneAsync(1);
    }
}
