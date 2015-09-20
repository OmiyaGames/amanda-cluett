using System;
using UnityEngine;

public class SupplyCondition : IEventCondition
{
    [Tooltip("This list is an OR-condition")]
    [SerializeField]
    GamePowerUp.Change[] supplyCheck;

    int currentQuantity = -1, index = 0;

    public override bool Passed(UnlockEvent unlockEvent, GamePanel panel)
    {
        // Check if any event is not unlocked
        bool returnFlag = false;
        for (index = 0; index < supplyCheck.Length; ++index)
        {
            if (CheckSupply(supplyCheck[index], panel) == true)
            {
                returnFlag = true;
                break;
            }
        }

        // Return whether all events are unlocked
        return returnFlag;
    }

    bool CheckSupply(GamePowerUp.Change change, GamePanel panel)
    {
        // Grab the quantity in question (based on units)
        currentQuantity = -1;
        switch (change.units)
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
        return (currentQuantity >= change.quantity);
    }
}
