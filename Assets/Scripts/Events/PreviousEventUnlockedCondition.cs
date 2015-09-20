using UnityEngine;

public class PreviousEventUnlockedCondition : IEventCondition
{
    [SerializeField]
    UnlockEvent[] allPreviousEvents;

    int index = 0;

    public override bool Passed(UnlockEvent unlockEvent, GamePanel panel)
    {
        // Check if any event is not unlocked
        bool returnFlag = true;
        for(index = 0; index < allPreviousEvents.Length; ++index)
        {
            if(allPreviousEvents[index].IsUnlocked == false)
            {
                returnFlag = false;
                break;
            }
        }

        // Return whether all events are unlocked
        return returnFlag;
    }
}
