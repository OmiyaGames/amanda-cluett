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
        if(unlockEvent != null)
        {
            // Check if this event is a conversation
            if(unlockEvent.ContainsConversation == true)
            {
                // Queue this event.  We can afford to delay this as necessary
                eventQueue.Enqueue(unlockEvent);
            }
            else
            {
                // Unlock this event immediately.  We cannot afford to delay this.
                unlockEvent.UnlockEverything(this);
            }
        }
    }

    public void OnUpdate()
    {
        // Check if there's any events
        if(eventQueue.Count > 0)
        {
            // Check if we're ready to play an event
            if(IsReadyToPlayEvent == true)
            {
                // Run the next event
                nextEvent = eventQueue.Dequeue();
                nextEvent.UnlockEverything(this);

                // Indicate we started running an event
                ResetTime();
            }
        }
    }

    public void ResetTime()
    {
        lastRanEvent = Time.time;
    }
}
