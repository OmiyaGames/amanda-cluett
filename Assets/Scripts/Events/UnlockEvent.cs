using UnityEngine;
using OmiyaGames;

public class UnlockEvent : MonoBehaviour
{
    [SerializeField]
    GamePanel parentPanel = null;

    [Header("Stored Settings")]
    [SerializeField]
    string storedKey = "test";

    [Header("Stuff to unlock")]
    [SerializeField]
    GamePowerUpGroup group;
    [SerializeField]
    DialogCollection conversation;
	[SerializeField]
	[UnityEngine.Serialization.FormerlySerializedAs("newsEntries")]
	NewsEntry[] newsToAdd;
	[SerializeField]
	NewsEntry[] newsToRemove;

    [Header("Change numbers")]
    [SerializeField]
    bool changeHusbandIncome = false;
    [SerializeField]
    int husbandIncomeCents = 10000;
    [SerializeField]
    bool increaseAccount = false;
    [SerializeField]
    int increaseCents = 3500000;

    [Header("Reset everything")]
    [SerializeField]
    bool resetData = false;

    IEventCondition[] allConditionsToMeet = null;
    bool isUnlocked = false, allConditionsPassed = false;
    int index = 0;
    string savedKey = null;

    System.Action<GamePanel> unitsChanged = null;

    #region Properties
    public bool IsUnlocked
    {
        get
        {
            return isUnlocked;
        }
    }

    public bool ContainsConversation
    {
        get
        {
            return (conversation != null);
        }
    }

    public string PlayerPrefsKey
    {
        get
        {
            if(string.IsNullOrEmpty(savedKey) == true)
            {
                savedKey = "Event." + storedKey;
            }
            return savedKey;
        }
    }

    public int IncreaseCents
    {
        get
        {
            return increaseCents;
        }
    }
    #endregion

    public void UnlockEverything(EventQueue queue)
    {
        if (isUnlocked == false)
        {
            // Mark as unlocked
            isUnlocked = true;
            PlayerPrefs.SetInt(PlayerPrefsKey, 1);

            // Disassociate with all events
            OnDestroy();

            if(resetData == true)
            {
                Singleton.Get<GameSettings>().ClearSettings();
            }

            // Check to see if there's a conversation to unlock first
            if (ContainsConversation == true)
            {
                queue.Dialog.ShowDialog(conversation, AfterConversation);
            }
            else
            {
                // Just unlock the group
                AfterConversation(null);
            }
        }
    }

    // Use this for initialization
    void Awake ()
    {
        // Reset events
        OnDestroy();

        // Grab all conditions
        allConditionsToMeet = GetComponentsInChildren<IEventCondition>();

        // Check to see if this event is unlocked or not
        isUnlocked = (PlayerPrefs.GetInt(PlayerPrefsKey, 0) != 0);
        if(isUnlocked == true)
        {
            // Unlock any groups or news associated with this event
            UnlockGroupAndNews(true);
        }
        else
        {
            // Bind to events in the game panel
            unitsChanged = new System.Action<GamePanel>(CheckIfUnlocked);
            parentPanel.OnCurrencyChanged += unitsChanged;
            parentPanel.OnSupplyChanged += unitsChanged;
        }
    }

    void OnDestroy()
    {
        if(unitsChanged != null)
        {
            parentPanel.OnCurrencyChanged -= unitsChanged;
            parentPanel.OnSupplyChanged -= unitsChanged;
            unitsChanged = null;
        }
    }

    void CheckIfUnlocked(GamePanel panel)
    {
        // Check if all conditions passed
        allConditionsPassed = true;
        for(index = 0; index < allConditionsToMeet.Length; ++index)
        {
            if (allConditionsToMeet[index].Passed(this, panel) == false)
            {
                allConditionsPassed = false;
                break;
            }
        }

        if(allConditionsPassed == true)
        {
            // If so, unlock everything!
            parentPanel.Queue.AddEvent(this);
        }
    }

    void UnlockGroupAndNews(bool isStart)
    {
        // Unlock the group
        if (group != null)
        {
            group.Unlock(parentPanel);
        }

		// Remove news
		for(index = 0; index < newsToRemove.Length; ++index)
		{
			parentPanel.News.AllEntries.Remove(newsToRemove[index]);
		}
	
		// Add news here
		for(index = 0; index < newsToAdd.Length; ++index)
		{
			parentPanel.News.AllEntries.Add(newsToAdd[index]);
		}

		// Display the top-most news
		if((newsToAdd.Length > 0) && (isStart == false))
		{
			parentPanel.News.NextNewsEntry(newsToAdd[0]);
		}
	}

    void AfterConversation(DialogPanel panel)
    {
        // Unlock the group
        UnlockGroupAndNews(false);

        // Unlock other things
        if (changeHusbandIncome == true)
        {
            parentPanel.HusbandsIncome = husbandIncomeCents;
        }
        if (increaseAccount == true)
        {
            parentPanel.CurrentCurrencyCents += increaseCents;
        }

        parentPanel.Queue.ResetTime();

        if (resetData == true)
        {
            // Reload this scene
            Singleton.Get<TimeManager>().PauseFor(3f);
            Singleton.Get<GameSettings>().ClearSettings();
            Singleton.Get<SceneManager>().ReloadCurrentScene();
        }
    }
}
