using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    [SerializeField]
    public TextMeshProUGUI textCoin;
    public int coinAmount = 0;

    [SerializeField]
    private CanvasGroup coinCanvasGroup;

    public float fadeDuration = 1f;
    public float displayDuration = 2f;

    public LevelBehaviour levelBehaviour;
    [SerializeField]
    public PlayerBehaviour playerBehaviour;

    [SerializeField]
    public GameObject gameEndingScreen;
    [SerializeField]
    public GameObject gameUI;
    [SerializeField]
    public TextMeshProUGUI textEnding;
    [SerializeField]
    public TextMeshProUGUI newRecordLabel;

    public GameObject crownImage;

    [SerializeField]
    public TextMeshProUGUI textSteps;

    public int record = 0;
    public bool newRecord = false;

    private void Awake()
    {
        if (UI.instance == null)
        {
            UI.instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        playerBehaviour.steps = PlayerPrefs.GetInt("Score", 0);
        record = PlayerPrefs.GetInt("Record", 0);
        coinAmount = PlayerPrefs.GetInt("Coin", 0);
        UpdateCoinText();
    }

    private void Update()
    {
        PlayerPrefs.SetInt("Coins", coinAmount);
        PlayerPrefs.Save();
        UpdateCoinText();
        PlayerPrefs.GetInt("Steps", playerBehaviour.steps);
        PlayerPrefs.Save();

        if (playerBehaviour.steps > record)
        {
            record = playerBehaviour.steps;
            PlayerPrefs.SetInt("Record", record);
            PlayerPrefs.Save();
            newRecord = true;
        }
    }

    private void UpdateCoinText()
    {
        textCoin.text = "Coins: " + coinAmount;
    }

    public void DisplayText()
    {
        LeanTween.cancel(coinCanvasGroup.gameObject); // Para que no se repita la animación si se está haciendo ya 
        LeanTween.alphaCanvas(coinCanvasGroup, 1f, fadeDuration / 2).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            LeanTween.alphaCanvas(coinCanvasGroup, 0f, fadeDuration / 2).setEase(LeanTweenType.easeInOutQuad).setDelay(displayDuration).setOnComplete(() =>
            {
                coinCanvasGroup.alpha = 0f;
            });
        });
    }
    public void UpdateTextSteps(int steps)
    {
        textSteps.text = "Score: " + steps;
    }

    public void GameEnding()
    {
        gameEndingScreen.SetActive(true);
        textEnding.text = "Total coins: " + coinAmount + "\nTotal steps: " + playerBehaviour.steps;
        gameUI.SetActive(false);
        if (newRecord)
        {
            newRecordLabel.text = "New record!";
            crownImage.SetActive(true);
        }
        else
        {
            newRecordLabel.text = "Record: " + record;
        }
    }
}