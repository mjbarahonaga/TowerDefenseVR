using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PosterInfo : MonoBehaviour
{
    public TextMeshPro Coins;
    public TextMeshPro Horde;
    public TextMeshPro Killed;
    public TextMeshPro Lives;

    private void Start()
    {
        GameManager.OnUpdateCoins += UpdateValueCoins;
        GameManager.OnUpdateCurrentHorde += UpdateValueHorde;
        GameManager.OnUpdateEnemiesKilled += UpdateValueKilled;
        GameManager.OnUpdateCurrentLives += UpdateValueLives;
    }

    private void OnDestroy()
    {
        GameManager.OnUpdateCoins -= UpdateValueCoins;
        GameManager.OnUpdateCurrentHorde -= UpdateValueHorde;
        GameManager.OnUpdateEnemiesKilled -= UpdateValueKilled;
        GameManager.OnUpdateCurrentLives -= UpdateValueLives;
    }

    public void UpdateValueCoins(int coins) => Coins.text = coins.ToString();

    public void UpdateValueHorde(int horde) => Horde.text = horde.ToString();

    public void UpdateValueKilled(int killed) => Killed.text = killed.ToString();   

    public void UpdateValueLives(int lives) => Lives.text = lives.ToString();
}
