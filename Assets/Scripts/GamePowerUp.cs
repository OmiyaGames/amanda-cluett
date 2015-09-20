using UnityEngine;
using UnityEngine.UI;
using OmiyaGames;

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

    int currentQuantity = -1;
    System.Action<GamePanel> unitsChanged = null;
    static SoundEffect sound = null;

    static SoundEffect Sound
    {
        get
        {
            if(sound == null)
            {
                sound = Singleton.Get<MenuManager>().ButtonClick;
            }
            return sound;
        }
    }

    public void OnClick()
    {
        // Reduce supply based on cost
        switch (gain.units)
        {
            case GamePanel.Units.VictoryPoints:
                parentPanel.CurrentVictoryPoints += gain.quantity;
                break;
            case GamePanel.Units.Books:
                parentPanel.CurrentBooks += gain.quantity;
                break;
            case GamePanel.Units.Dresses:
                parentPanel.CurrentDresses += gain.quantity;
                break;
            case GamePanel.Units.Furnitures:
                parentPanel.CurrentFurnitures += gain.quantity;
                break;
            case GamePanel.Units.SewingMachines:
                parentPanel.CurrentSewingMachines += gain.quantity;
                break;
        }

        // Reduce supply based on cost
        switch (cost.units)
        {
            case GamePanel.Units.Cents:
                parentPanel.CurrentCurrencyCents -= cost.quantity;
                break;
            case GamePanel.Units.Books:
                parentPanel.CurrentBooks -= cost.quantity;
                break;
            case GamePanel.Units.Dresses:
                parentPanel.CurrentDresses -= cost.quantity;
                break;
            case GamePanel.Units.Furnitures:
                parentPanel.CurrentFurnitures -= cost.quantity;
                break;
            case GamePanel.Units.SewingMachines:
                parentPanel.CurrentSewingMachines -= cost.quantity;
                break;
        }

        // Play sound effect
        Sound.Play();
    }

    // Use this for initialization
    void Awake ()
    {
        // Setup private variables
        Text powerUpLabel = powerUpButton.GetComponentInChildren<Text>();
        string originalProductString = powerUpLabel.text;

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
        if(currentQuantity >= 0)
        {
            powerUpButton.interactable = (currentQuantity >= cost.quantity);
        }
    }
}
