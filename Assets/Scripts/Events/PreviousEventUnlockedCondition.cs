using UnityEngine;

public class PreviousEventUnlockedCondition : IEventCondition
{
    [Tooltip("This list is an OR-condition")]
    [SerializeField]
    UnlockEvent[] allPreviousEvents;

    int index = 0;

    public override bool Passed(UnlockEvent unlockEvent, GamePanel panel)
    {
        // Check if any event is not unlocked
        bool returnFlag = false;
        for(index = 0; index < allPreviousEvents.Length; ++index)
        {
            if(allPreviousEvents[index].IsUnlocked == true)
            {
                returnFlag = true;
                break;
            }
        }

        // Return whether all events are unlocked
        return returnFlag;
    }
}
