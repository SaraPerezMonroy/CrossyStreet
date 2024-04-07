using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textCoin;
    public int coinAmount = 0;

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
}
