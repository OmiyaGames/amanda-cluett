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
    // FIXME: consider unlocking news

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
            UnlockGroupAndNews();
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

        //Debug.Log("Unlock: " + gameObject.name + " is " + allConditionsPassed);
        if(allConditionsPassed == true)
        {
            // If so, unlock everything!
            parentPanel.Queue.AddEvent(this);
        }
    }

    void UnlockGroupAndNews()
    {
        // Unlock the group
        if (group != null)
        {
            group.Unlock(parentPanel);
        }

        // FIXME: consider unlocking news here
    }

    void AfterConversation(DialogPanel panel)
    {
        // Unlock the group
        UnlockGroupAndNews();

        // Unlock other things
        if (changeHusbandIncome == true)
        {
            parentPanel.HusbandsIncome = husbandIncomeCents;
        }
        if (increaseAccount == true)
        {
            parentPanel.CurrentCurrencyCents += increaseCents;
        }

        if(resetData == true)
        {
            // Reload this scene
            Singleton.Get<SceneManager>().ReloadCurrentScene();
        }
    }
}
