using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class GamePanel : MonoBehaviour
{
    public const string CurrencyKey = "CurrentCurrency";
    public const string VictoryKey = "VictoryPoints";
    public const string IncomeKey = "ReceivingIncome";

    public event System.Action<GamePanel> OnMoneyGained;

    [Header("Labels")]
    [SerializeField]
    Text currencyLabel;
    [SerializeField]
    Text victoryLabel;

    [Header("Other")]
    [SerializeField]
    GameObject victorySet;

    readonly StringBuilder builder = new StringBuilder();
    float lastStarted = -1f;
    int currencyCents = 0;
    int victoryPoints = 0;
    int tempDisplay = 0;
    bool isReceivingIncome = true;

    #region Properties
    public int CurrentCurrencyCents
    {
        get
        {
            return currencyCents;
        }
        set
        {
            if((value >= 0) && (currencyCents != value))
            {
                currencyCents = value;
                PlayerPrefs.SetInt(CurrencyKey, currencyCents);
                Start();
            }
        }
    }

    public int CurrentVictoryPoints
    {
        get
        {
            return victoryPoints;
        }
        set
        {
            if ((value >= 0) && (victoryPoints != value))
            {
                victoryPoints = value;
                PlayerPrefs.SetInt(VictoryKey, victoryPoints);

                // Update labels
                UpdateLabels();
            }
        }
    }

    public bool IsReceivingIncome
    {
        get
        {
            return isReceivingIncome;
        }
        set
        {
            if (isReceivingIncome != value)
            {
                isReceivingIncome = value;
                PlayerPrefs.SetInt(IncomeKey, (isReceivingIncome ? 1 : 0));
            }
        }
    }
    #endregion

    public void Pause()
    {
        lastStarted = -1f;
    }

    public void Resume(DialogPanel panel)
    {
        lastStarted = Time.time;
        if(lastStarted < 0.1f)
        {
            lastStarted = 0.1f;
        }
    }

    // Use this for initialization
    void Awake ()
    {
        // Retrieve settings
        currencyCents = PlayerPrefs.GetInt(CurrencyKey, 0);
        victoryPoints = PlayerPrefs.GetInt(VictoryKey, 0);
        isReceivingIncome = (PlayerPrefs.GetInt(IncomeKey, 1) != 0);
    }
	
    void Start()
    {
        // Update labels
        UpdateLabels();

        // Check the amount of money gained
        if (OnMoneyGained != null)
        {
            OnMoneyGained(this);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        // Check if the second passed
	    if((lastStarted > 0) && ((Time.time - lastStarted) > 1f))
        {
            // Increment currency
            currencyCents += CurrentVictoryPoints;
            if (IsReceivingIncome == true)
            {
                currencyCents += 100;
            }
            PlayerPrefs.SetInt(CurrencyKey, currencyCents);
            Start();

            // Save settings
            PlayerPrefs.Save();
        }
    }

    void UpdateLabels()
    {
        // Setup the currency text
        builder.Length = 0;
        builder.Append('$');

        // Get the dollar amount
        tempDisplay = (CurrentCurrencyCents / 100);
        builder.Append(tempDisplay.ToString("N0"));

        // Get the cents
        tempDisplay = (CurrentCurrencyCents % 100);
        builder.Append('.');
        builder.Append(tempDisplay.ToString("00"));

        // Display the currency
        currencyLabel.text = builder.ToString();

        // Update Victory Points
        if (victoryPoints > 0)
        {
            victoryLabel.text = CurrentVictoryPoints.ToString();
            victorySet.SetActive(true);
        }
        else
        {
            victorySet.SetActive(false);
        }
    }
}
