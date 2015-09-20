using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GamePowerUp : MonoBehaviour
{
    public enum Units
    {
        Cents,
        Books,
        Wear
    }

    [Header("Components")]
    [SerializeField]
    GamePanel parentPanel = null;
    [SerializeField]
    Button productButton = null;
    [SerializeField]
    Text productName = null;
    [SerializeField]
    Text priceText = null;

    [Header("Save Information")]
    [SerializeField]
    string uniqueName = "test";
    [SerializeField]
    int victoryPointGained = 0;
    [SerializeField]
    int costInCents = 0;

    int quantity = 0;

    // Use this for initialization
    void Awake ()
    {
        parentPanel.OnMoneyGained += OnMoneyChange;
	}
	
	// Update is called once per frame
	void OnMoneyChange (GamePanel panel)
    {
        productButton.interactable = (panel.CurrentCurrencyCents >= costInCents);
    }
}
