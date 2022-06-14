using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class BuyOptions : MonoBehaviour
{
    public DataLevelTurret dataLevel; //< To get the price of lvl 1
    public UnityEvent EventsToExecuteWhenBuy;
    public SellOptions RefSellOptions;
    public TextMeshPro PriceText;

    private void Start()
    {
        
        if(dataLevel == null) //< If we can't upgrade it more
        {
            GetComponent<Renderer>().enabled = false;
            GetComponent<XRSimpleInteractable>().enabled = false;
            return;
        }
        else
        {
            GetComponent<Renderer>().enabled = true;
            GetComponent<XRSimpleInteractable>().enabled = true;
            UpdatePrice(dataLevel);
        }
        
    }

    public void TryToBuy()
    {
        if (dataLevel == null) return;
        if(GameManager.Instance.Coins >= dataLevel.Price)
        {
            if (RefSellOptions)
            {
                RefSellOptions.UpdatePrice(dataLevel.Price);
                dataLevel = null;
            }
            GameManager.Instance.AddCoins(-dataLevel.Price);
            EventsToExecuteWhenBuy?.Invoke();
        }
    }

    public void UpdatePrice(DataLevelTurret data)
    {
        if(data == null) return;
        if(dataLevel != null)
        {
            // before update
            if (RefSellOptions) RefSellOptions.UpdatePrice(dataLevel.Price);
        }
        dataLevel = data;
        PriceText.text = data.Price.ToString();
    }
}
