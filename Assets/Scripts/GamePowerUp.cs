using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GamePowerUp : MonoBehaviour
{
    [System.Serializable]
    public struct Change
    {
        public GamePanel.Units units;
        public int quantity;
    }

    [SerializeField]
    GamePanel parentPanel = null;
    [SerializeField]
    Button powerUpButton = null;
    [SerializeField]
    Change cost;
    [SerializeField]
    Change gain;

    Text powerUpLabel = null;
    string originalProductString = null;
    int currentQuantity = -1;

    System.Action<GamePanel> unitsChanged = null;

    // Use this for initialization
    void Awake ()
    {
        // Setup private variables
        powerUpLabel = powerUpButton.GetComponentInChildren<Text>();
        originalProductString = powerUpLabel.text;

        // Setup text
        string costText = cost.quantity.ToString();
        if (cost.units == GamePanel.Units.Cents)
        {
            costText = parentPanel.CentsToString(cost.quantity);
        }
        powerUpLabel.text = string.Format(originalProductString, costText);

        // Setup events
        OnDestroy();
        switch (cost.units)
        {
            case GamePanel.Units.Cents:
                unitsChanged = new System.Action<GamePanel>(OnUnitsChange);
                parentPanel.OnCurrencyChanged += unitsChanged;
                break;
            case GamePanel.Units.Books:
            case GamePanel.Units.Dresses:
            case GamePanel.Units.Furnitures:
            case GamePanel.Units.SewingMachines:
                unitsChanged = new System.Action<GamePanel>(OnUnitsChange);
                parentPanel.OnSupplyChanged += unitsChanged;
                break;
        }
    }

    void OnDestroy()
    {
        switch (cost.units)
        {
            case GamePanel.Units.Cents:
                if (unitsChanged != null)
                {
                    parentPanel.OnCurrencyChanged -= unitsChanged;
                    unitsChanged = null;
                }
                break;
            case GamePanel.Units.Books:
            case GamePanel.Units.Dresses:
            case GamePanel.Units.Furnitures:
            case GamePanel.Units.SewingMachines:
                if (unitsChanged != null)
                {
                    parentPanel.OnSupplyChanged -= unitsChanged;
                    unitsChanged = null;
                }
                break;
        }
    }

    // Update is called once per frame
    void OnUnitsChange (GamePanel panel)
    {
        // Check for the quantity
        currentQuantity = -1;
        switch (cost.units)
        {
            case GamePanel.Units.Cents:
                currentQuantity = panel.CurrentCurrencyCents;
                break;
            case GamePanel.Units.Books:
                currentQuantity = panel.CurrentBooks;
                break;
            case GamePanel.Units.Dresses:
                currentQuantity = panel.CurrentDresses;
                break;
            case GamePanel.Units.Furnitures:
                currentQuantity = panel.CurrentFurnitures;
                break;
            case GamePanel.Units.SewingMachines:
                currentQuantity = panel.CurrentSewingMachines;
                break;
        }

        // See if we found the correct quantity
        if(currentQuantity > 0)
        {
            powerUpButton.interactable = (currentQuantity >= cost.quantity);
        }
    }
}
