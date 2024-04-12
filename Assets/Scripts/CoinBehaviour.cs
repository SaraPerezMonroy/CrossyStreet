using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI textCoin;
    public int coinAmount = 0;

    [SerializeField]
    private CanvasGroup coinCanvasGroup;

    public float fadeDuration = 1f;
    public float displayDuration = 2f;

    private void Start()
    {
        coinAmount = PlayerPrefs.GetInt("Coin", 0);
        UpdateCoinText();
    }

    private void Update()
    {
        PlayerPrefs.SetInt("Coins", coinAmount);
        PlayerPrefs.Save();
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        textCoin.text = "Coins: " + coinAmount.ToString();
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
}
