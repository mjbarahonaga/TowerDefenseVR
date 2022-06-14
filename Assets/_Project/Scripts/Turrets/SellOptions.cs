using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SellOptions : MonoBehaviour
{
    public UnityEvent EventsToExecuteWhenSell;
    public TextMeshPro PriceText;
    public int Price;
    public void Sell()
    {
        GameManager.Instance.AddCoins(Price);
        EventsToExecuteWhenSell?.Invoke();
    }

    public void UpdatePrice(int howMuchCost)
    {
        Price = howMuchCost / 2;
        PriceText.text = Price.ToString();
    }
}
