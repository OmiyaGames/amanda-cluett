using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EventQueue
{
    [SerializeField]
    float gapBetweenEventsSeconds = 5f;

    readonly Queue<UnlockEvent> eventQueue = new Queue<UnlockEvent>();
    DialogPanel dialog;
    GamePanel game;
    float lastRanEvent = -1f;
    UnlockEvent nextEvent;

    #region Properties
    public float GapBetweenEventsSeconds
    {
        get
        {
            return gapBetweenEventsSeconds;
        }
    }

    public GamePanel Game
    {
        get
        {
            return game;
        }
    }

    public DialogPanel Dialog
    {
        get
        {
            return dialog;
        }
    }

    bool IsReadyToPlayEvent
    {
        get
        {
            bool flag = false;
            if(Dialog.IsVisible == false)
            {
                if ((lastRanEvent < 0) || ((Time.time - lastRanEvent) > GapBetweenEventsSeconds))
                {
                    flag = true;
                }
            }
            return flag;
        }
    }
    #endregion

    public void Setup(GamePanel gamePanel, DialogPanel dialogPanel)
    {
        game = gamePanel;
        dialog = dialogPanel;
    }

    public void AddEvent(UnlockEvent unlockEvent)
    {
		if((unlockEvent != null) && (unlockEvent.IsUnlocked == false))
        {
			// Queue this event.  We can afford to delay this as necessary
			eventQueue.Enqueue(unlockEvent);
			Debug.Log("Queueing event: " + unlockEvent.name);
        }
    }

    public void OnUpdate()
    {
		// Check if we're ready to play an event
		if((eventQueue.Count > 0) && (IsReadyToPlayEvent == true))
		{
			// Check if there's any events
			nextEvent = null;
			while((eventQueue.Count > 0) && (nextEvent == null))
			{
                // Grab the next event
                nextEvent = eventQueue.Dequeue();
				if(nextEvent.IsUnlocked == true)
				{
					// Skip unlocked events
					nextEvent = null;
				}
				else
				{
					// Indicate we started running an event
					nextEvent.UnlockEverything(this);
					ResetTime();
				}
            }
		}
    }

    public void ResetTime()
    {
        lastRanEvent = Time.time;
    }
}
