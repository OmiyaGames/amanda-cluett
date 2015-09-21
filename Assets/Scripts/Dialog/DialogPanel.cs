using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using OmiyaGames;

public class DialogPanel : MonoBehaviour
{
    public enum Character
    {
        GeorgeCluett,
        AmandaCluett,
        Narrator,
        Messenger,
        MaryWarren
    }

    [System.Serializable]
    public struct CharacterColorPair
    {
        public Character character;
        public Color textColor;
    }

    [Header("Components")]
    [SerializeField]
    Animator animator;
    [SerializeField]
    Text presentText;
    [SerializeField]
    Text nextText;
    [SerializeField]
    GamePanel game;

    [Header("Animation Fields")]
    [SerializeField]
    string visibleBoolField = "visible";
    [SerializeField]
    string nextTrigger = "next";
    [SerializeField]
    string visibleStateName = "Dialog Visible Still";

    [Header("Dialogs")]
    [SerializeField]
    CharacterColorPair[] characterMap;
    [SerializeField]
    DialogCollection initialDialogs;

    public readonly Dictionary<Character, Color> characterTextColorMap = new Dictionary<Character, Color>();
    public System.Action<DialogPanel> OnClose = null;

    DialogCollection currentDialogs = null;
    int dialogIndex = -1;
    MenuManager manager = null;

    public bool IsVisible
    {
        get
        {
            return animator.GetBool(visibleBoolField);
        }
    }

    public DialogCollection CurrentDialogCollection
    {
        get
        {
            return currentDialogs;
        }
    }

    public Dialog CurrentDialog
    {
        get
        {
            Dialog returnDialog = null;
            if((CurrentDialogCollection != null) && (dialogIndex >= 0) && (dialogIndex < CurrentDialogCollection.Count))
            {
                returnDialog = CurrentDialogCollection[dialogIndex];
            }
            return returnDialog;
        }
    }

    void Start()
    {
        // Setup character map
        characterTextColorMap.Clear();
        for(int i = 0; i < characterMap.Length; ++i)
        {
            characterTextColorMap.Add(characterMap[i].character, characterMap[i].textColor);
        }

        // Check if this is the first time loading the game
        if(Singleton.Get<GameSettings>().Status == GameSettings.AppStatus.FirstTimeOpened)
        {
            // Start the initial dialog
            ShowDialog(initialDialogs);
        }
        else
        {
            // Hide this dialog
            HideDialog();
        }
    }

    public void ShowDialog(DialogCollection collection, System.Action<DialogPanel> closeAction = null)
    {
        // Setup member variables
        currentDialogs = collection;
        dialogIndex = 0;
        OnClose = closeAction;

        // Update the present text
        presentText.text = CurrentDialog.Text;
        presentText.color = characterTextColorMap[CurrentDialog.Character];

        // Make time stop
        game.Pause();

        // Play an animation
        animator.SetBool(visibleBoolField, true);
    }

    public void OnNextClicked()
    {
        // Check to make sure the last dialog is shown
        if(animator.GetCurrentAnimatorStateInfo(0).IsName(visibleStateName) == true)
        {
            // Check if there's still more dialog
            ++dialogIndex;
            if(dialogIndex < CurrentDialogCollection.Count)
            {
                // Update the next text
                nextText.text = CurrentDialog.Text;
                nextText.color = characterTextColorMap[CurrentDialog.Character];

                // Play an animation
                animator.SetTrigger(nextTrigger);
            }
            else
            {
                // Hide the dialog
                HideDialog();
            }

            // Play sound
            PlaySound();
        }
    }

    void HideDialog()
    {
        // Run the close event, if there's any
        if (OnClose != null)
        {
            OnClose(this);
            OnClose = null;
        }

        // Make time resume
        game.Resume();

        // Play an animation
        animator.SetBool(visibleBoolField, false);
    }

    public void SwapText()
    {
        presentText.text = nextText.text;
        presentText.color = nextText.color;
    }

    void PlaySound()
    {
        if (manager == null)
        {
            manager = Singleton.Get<MenuManager>();
        }
        manager.ButtonClick.Play();
    }
}
