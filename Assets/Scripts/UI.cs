using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public AudioSource deathSound;
    public AudioSource bgMusic;

    [SerializeField]
    public TextMeshProUGUI textSteps;

    public int record = 0;
    public bool newRecord = false;

    public AudioListener audioListener;

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
        playerBehaviour.steps = PlayerPrefs.GetInt("Score", 0); // Almacena los pasitos como el score, si no tenemos nada, lo ponemos como 0
        record = PlayerPrefs.GetInt("Record", 0); // Almacena el record 
        coinAmount = PlayerPrefs.GetInt("Coin", 0); // Almacenamos los coins
        UpdateCoinText();
    }

    private void Update()
    {
        PlayerPrefs.SetInt("Coins", coinAmount); // Vamos actualizando la cantidad de monedas conseguidas
        PlayerPrefs.Save(); // Lo guardamos
        UpdateCoinText(); // Actualizamos el texto 
        PlayerPrefs.GetInt("Steps", playerBehaviour.steps); // Actualizamos cantidad de pasos 
        PlayerPrefs.Save(); // Guardamos en el almacenamiento del jugador

        if (playerBehaviour.steps > record) // Si los pasos dados en este momento, son mayores a los guardados 
        {
            record = playerBehaviour.steps; // El record es ahora los pasos dados en el juego actual
            PlayerPrefs.SetInt("Record", record);
            PlayerPrefs.Save();
            newRecord = true; // Activamos para que sea nuevo record
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
        playerBehaviour.steps = steps;
        textSteps.text = "Score: " + steps;
    }

    public void GameEnding(AudioSource deathEffect)
    {
        bgMusic.Pause();
        deathEffect.Play();
        deathSound.Play();
        SwipeController.instance.enabled = false;
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

        StartCoroutine(WaitAndDisableAudioListener());
    }

    IEnumerator WaitAndDisableAudioListener() // Para dejar de oír los coches y trenes
    {
        yield return new WaitForSeconds(3f);
        audioListener.enabled = false;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}