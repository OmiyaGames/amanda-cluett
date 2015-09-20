using UnityEngine;
using UnityEngine.UI;

public class GamePowerUpGroup : MonoBehaviour
{
    [SerializeField]
    GamePanel parentPanel = null;

    [Header("Units to Represent")]
    [SerializeField]
    GamePanel.Units units;
    [SerializeField]
    Text unitsLabel = null;

    [Header("Stuff to update on Unlock")]
    [SerializeField]
    GamePowerUp[] allPowerUps = null;

    string originalProductString = null;
    System.Action<GamePanel> unitsChanged = null;
    int currentQuantity = -1;

    public void Unlock(GamePanel panel)
    {
        gameObject.SetActive(true);
        OnUnitsChange(panel);
        for(currentQuantity = 0; currentQuantity < allPowerUps.Length; ++currentQuantity)
        {
            allPowerUps[currentQuantity].OnUnitsChange(panel);
        }
    }

    // Use this for initialization
    void Awake()
    {
        originalProductString = null;
        if (unitsLabel != null)
        {
            // Setup private variables
            originalProductString = unitsLabel.text;

            // Setup events
            OnDestroy();
            switch (units)
            {
                case GamePanel.Units.Books:
                case GamePanel.Units.Dresses:
                case GamePanel.Units.Furnitures:
                case GamePanel.Units.SewingMachines:
                    unitsChanged = new System.Action<GamePanel>(OnUnitsChange);
                    parentPanel.OnSupplyChanged += unitsChanged;
                    break;
            }
        }
    }

    void OnDestroy()
    {
        if (unitsChanged != null)
        {
            switch (units)
            {
                case GamePanel.Units.Books:
                case GamePanel.Units.Dresses:
                case GamePanel.Units.Furnitures:
                case GamePanel.Units.SewingMachines:
                    parentPanel.OnSupplyChanged -= unitsChanged;
                    unitsChanged = null;
                    break;
            }
        }
    }

    // Update is called once per frame
    void OnUnitsChange(GamePanel panel)
    {
        if(string.IsNullOrEmpty(originalProductString) == false)
        {
            // Check for the quantity
            currentQuantity = -1;
            switch (units)
            {
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

            // Print the results
            if(currentQuantity >= 0)
            {
                unitsLabel.text = parentPanel.UnitsToString(originalProductString, currentQuantity);
            }
        }
    }
}
