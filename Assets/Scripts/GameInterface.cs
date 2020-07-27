using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInterface : MonoBehaviour
{
    public static GameInterface Instance;

    [Header("Ball Power UI")]
    [SerializeField] GameObject powerBar;
    [SerializeField] Image powerBarImage;

    [Header("Game Interface Panels")]
    [SerializeField] GameObject welcomePanel;
    [SerializeField] GameObject finishPanel;

    public GameObject PowerBar { get => powerBar; set => powerBar = value; }
    public Image PowerBarImage { get => powerBarImage; set => powerBarImage = value; }
    public GameObject WelcomePanel1 { get => welcomePanel; set => welcomePanel = value; }
    public GameObject FinishPanel { get => finishPanel; set => finishPanel = value; }

    private void Start()
    {
        if (Instance == null || Instance != this)
            Instance = this;
    }

    public void StartGame()
    {
        GameManager.Instance.GameHasStarted = true;
        welcomePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
