using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class GamePanel : MonoBehaviour
{
    public const string CurrencyKey = "CurrentCurrency";
    public const string VictoryKey = "VictoryPoints";
    public const string IncomeKey = "ReceivingIncome";
    public const string DressesKey = "Dresses";
    public const string BooksKey = "Books";
    public const string SewingMachineKey = "SewingMachines";
    public const string FurnituresKey = "Furnitures";

    public enum Units
    {
        None = -1,
        Cents = 0,
        Books,
        Dresses,
        SewingMachines,
        Furnitures,
        HusbandsIncome,
        VictoryPoints
    }

    public event System.Action<GamePanel> OnCurrencyChanged;
    public event System.Action<GamePanel> OnSupplyChanged;

    [Header("Labels")]
    [SerializeField]
    Text currencyLabel;
    [SerializeField]
    Text victoryLabel;

    [Header("Other")]
    [SerializeField]
    GameObject victorySet;

    // Important stuff
    int currencyCents = 0;
    int victoryPoints = 0;

    // Resources
    int husbandsIncome = 100;
    int dresses = 0;
    int books = 0;
    int sewingMachines = 0;
    int furnitures = 0;

    // Temporary stuff
    readonly StringBuilder builder = new StringBuilder();
    float lastStarted = -1f;
    int tempDisplay = 0;

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
                OnMoneyChanged();
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

    public int HusbandsIncome
    {
        get
        {
            return husbandsIncome;
        }
        set
        {
            if (husbandsIncome != value)
            {
                husbandsIncome = value;
                PlayerPrefs.SetInt(IncomeKey, husbandsIncome);
            }
        }
    }

    public int CurrentDresses
    {
        get
        {
            return dresses;
        }
        set
        {
            if ((value >= 0) && (dresses != value))
            {
                dresses = value;
                PlayerPrefs.SetInt(DressesKey, dresses);
                if(OnSupplyChanged != null)
                {
                    OnSupplyChanged(this);
                }
            }
        }
    }

    public int CurrentBooks
    {
        get
        {
            return books;
        }
        set
        {
            if ((value >= 0) && (books != value))
            {
                books = value;
                PlayerPrefs.SetInt(BooksKey, books);
                if (OnSupplyChanged != null)
                {
                    OnSupplyChanged(this);
                }
            }
        }
    }

    public int CurrentSewingMachines
    {
        get
        {
            return sewingMachines;
        }
        set
        {
            if ((value >= 0) && (sewingMachines != value))
            {
                sewingMachines = value;
                PlayerPrefs.SetInt(SewingMachineKey, sewingMachines);
                if (OnSupplyChanged != null)
                {
                    OnSupplyChanged(this);
                }
            }
        }
    }

    public int CurrentFurnitures
    {
        get
        {
            return furnitures;
        }
        set
        {
            if ((value >= 0) && (furnitures != value))
            {
                furnitures = value;
                PlayerPrefs.SetInt(FurnituresKey, furnitures);
                if (OnSupplyChanged != null)
                {
                    OnSupplyChanged(this);
                }
            }
        }
    }
    #endregion

    public void Pause()
    {
        lastStarted = -1f;
    }

    public void Resume()
    {
        lastStarted = Time.time;
        if(lastStarted < 0.1f)
        {
            lastStarted = 0.1f;
        }
    }

    public string CentsToString(int cents)
    {
        // Setup the currency text
        builder.Length = 0;
        builder.Append('$');

        // Get the dollar amount
        tempDisplay = (cents / 100);
        builder.Append(tempDisplay.ToString("N0"));

        // Get the cents
        tempDisplay = (cents % 100);
        builder.Append('.');
        builder.Append(tempDisplay.ToString("00"));
        return builder.ToString();
    }

    public string UnitsToString(string format, int units)
    {
        builder.Length = 0;
        builder.AppendFormat(format, units);
        return builder.ToString();
    }

    #region Events
    // Use this for initialization
    void Awake ()
    {
        // Retrieve settings
        currencyCents = PlayerPrefs.GetInt(CurrencyKey, 0);
        victoryPoints = PlayerPrefs.GetInt(VictoryKey, 0);

        husbandsIncome = PlayerPrefs.GetInt(IncomeKey, 100);
        dresses = PlayerPrefs.GetInt(DressesKey, 0);
        books = PlayerPrefs.GetInt(BooksKey, 0);
        sewingMachines = PlayerPrefs.GetInt(SewingMachineKey, 0);
        furnitures = PlayerPrefs.GetInt(FurnituresKey, 0);
    }

    void Start()
    {
        OnMoneyChanged();
        if(OnSupplyChanged != null)
        {
            OnSupplyChanged(this);
        }
    }
	
    // Update is called once per frame
    void Update ()
    {
        // Check if the second passed
	    if((lastStarted > 0) && ((Time.time - lastStarted) > 1f))
        {
            // Increment currency
            CurrentCurrencyCents += CurrentVictoryPoints + husbandsIncome;

            // Save settings
            PlayerPrefs.Save();

            // Reset time
            lastStarted = Time.time;
        }
    }
    #endregion

    void UpdateLabels()
    {
        // Display the currency
        currencyLabel.text = CentsToString(CurrentCurrencyCents);

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

    void OnMoneyChanged()
    {
        // Update labels
        UpdateLabels();

        // Check the amount of money gained
        if (OnCurrencyChanged != null)
        {
            OnCurrencyChanged(this);
        }
    }
}
