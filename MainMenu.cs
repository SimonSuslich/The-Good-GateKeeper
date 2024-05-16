//   File: MainMenu.cs
//   Author: Simon Niklas Suslich
//   Date: 2024-05-16
//   Description: Filen hanterar menyn och relaterade metoder

// Inkluderar Unity Engine Library och Scene Management
using UnityEngine;
using UnityEngine.SceneManagement;

// Definierar klass MainMenu av egenskapet Monobehavior
public class MainMenu : MonoBehaviour
{
    // Definierar två public GameObject för de två panelerna Menu och SoundSettings
    public GameObject menuPanel;
    public GameObject soundSettingsPanel;

    // Definierar en private run method som körs vid initialisering filen
    // Inaktiverar (döljer) menuPanel och soundSettingsPanel vid programmets start
    public void Awake()
    {
        menuPanel.SetActive(false);
        soundSettingsPanel.SetActive(false);
    }

    // Definierar en public run method som laddar up MainMenu scenen
    public void EnterMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    // Definierar en public run method som laddar upp MainGame scenen
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    // Definierar en public run method som laddar upp Tutorial scenen om man inte har spelat spelet förut
    public void PlayTutorial()
    {
        if (PlayerPrefs.HasKey("CharIndex")) {
            PlayerPrefs.SetInt("CharIndex", 0);
        }
        SceneManager.LoadSceneAsync(2);
    }

    // Definierar public run method som togglar menuPanel's aktivitet
    public void ToggleMenuPanel()
    {
        menuPanel.SetActive(!menuPanel.active);
    }

    // Definierar public run method som togglar soundSettinsPanel's aktivitet
    public void ToggleSettingsPanel()
    {
        soundSettingsPanel.SetActive(!soundSettingsPanel.active);
    }

    // Definierar en public run method som stänger av programmet (funkat ej i Unity Editor)
    public void QuitGame()
    {
        Application.Quit();
    }
}
