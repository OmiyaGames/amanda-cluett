using UnityEngine;

public class SupplyCondition : IEventCondition
{
    [SerializeField]
    GamePanel.Units units;
    [SerializeField]
    int quantity;

    int currentQuantity;

    public override bool Passed(UnlockEvent unlockEvent, GamePanel panel)
    {
        // Grab the quantity in question (based on units)
        currentQuantity = -1;
        switch(units)
        {
            case GamePanel.Units.Cents:
                currentQuantity = panel.CurrentCurrencyCents;
                break;
            case GamePanel.Units.VictoryPoints:
                currentQuantity = panel.CurrentVictoryPoints;
                break;
            case GamePanel.Units.HusbandsIncome:
                currentQuantity = panel.HusbandsIncome;
                break;
            case GamePanel.Units.Books:
                currentQuantity = panel.CurrentBooks;
                break;
            case GamePanel.Units.Dresses:
                currentQuantity = panel.CurrentDresses;
                break;
            case GamePanel.Units.SewingMachines:
                currentQuantity = panel.CurrentSewingMachines;
                break;
            case GamePanel.Units.Furnitures:
                currentQuantity = panel.CurrentFurnitures;
                break;
        }

        // Return whether it's greater or now
        return (currentQuantity >= quantity);
    }
}
