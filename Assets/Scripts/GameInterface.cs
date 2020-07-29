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
    [SerializeField] GameObject finishPanel;
    [SerializeField] Text finishText;

    [Header("Screen Flash")]
    [SerializeField] Image screenFlashImage;
    private int screenFlashAlpha = 0;
    private bool isScreenFlashing = false;
    

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
        GameManager.Instance.GameHasStarted = true;
        welcomePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    private void Update()
    {
        SwingText();

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

        
    }

    private void SwingText()
    {
        if (GameManager.Instance.GameHasStarted && !GameManager.Instance.GameFinished)
        {
            swingText.SetActive(true);
            swingText.GetComponent<Text>().text = "Swings: " + GameManager.Instance.RoundSwings;
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
}
