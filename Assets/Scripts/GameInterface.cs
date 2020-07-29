using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInterface : MonoBehaviour
{
    public static GameInterface Instance;

    [Header("Ball UI")]
    [SerializeField] GameObject powerBar;
    [SerializeField] Image powerBarImage;
    [SerializeField] GameObject swingText;

    [Header("Game Interface Panels")]
    [SerializeField] GameObject welcomePanel;
    public GameObject escapeMenu;
    [SerializeField] GameObject finishPanel;
    [SerializeField] Text finishText;

    [Header("Screen Flash")]
    [SerializeField] Image screenFlashImage;
    private int screenFlashAlpha = 0;
    private bool isScreenFlashing = false;
    
    //Sound
    [Header("Sound")]
    [SerializeField] Slider soundSlider;
    [SerializeField] Text soundValueText;


    // Properties
    public GameObject PowerBar { get => powerBar; set => powerBar = value; }
    public Image PowerBarImage { get => powerBarImage; set => powerBarImage = value; }
    public GameObject WelcomePanel1 { get => welcomePanel; set => welcomePanel = value; }
    public GameObject FinishPanel { get => finishPanel; set => finishPanel = value; }
    public Text FinishText { get => finishText; set => finishText = value; }

    private void Start()
    {
        if (Instance == null || Instance != this)
            Instance = this;

        welcomePanel.SetActive(true);
    }

    public void StartGame()
    {
        GameManager.GameHasStarted = true;
        welcomePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        SwingText();

        GameManager.SoundVolume = soundSlider.value;
        soundValueText.text = GameManager.SoundVolume.ToString() + "%";

        // Screen flashing
        if (isScreenFlashing)
        {
            if (screenFlashAlpha > 0f)
                screenFlashAlpha -= 3;

            if (screenFlashAlpha < 10)
            {
                screenFlashAlpha = 0;
                isScreenFlashing = false;
            }

            screenFlashImage.GetComponent<Image>().color = new Color32(255, 255, 225, (byte)screenFlashAlpha);
        }

        //Escape Menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeMenu();

            GameManager.InEscapeMenu = !GameManager.InEscapeMenu;

            if (GameManager.InEscapeMenu)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
                
        }
            
    }

    private void SwingText()
    {
        if (GameManager.GameHasStarted && !GameManager.GameFinished)
        {
            swingText.SetActive(true);
            swingText.GetComponent<Text>().text = "Swings: " + GameManager.RoundSwings;
        }
        else
        {
            swingText.SetActive(false);
        }
    }

    public void ScreenFlash()
    {
        isScreenFlashing = true;
        screenFlashAlpha = 255;
    }

    public void EscapeMenu()
    {
        escapeMenu.SetActive(!escapeMenu.activeSelf);
    }
}
