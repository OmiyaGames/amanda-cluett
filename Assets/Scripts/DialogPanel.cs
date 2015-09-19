using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogPanel : MonoBehaviour
{
    public enum Character
    {
        Husband,
        Wife
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

    [Header("Animation Fields")]
    [SerializeField]
    string visibleBoolField = "visible";
    [SerializeField]
    string nextTrigger = "next";

    [Header("Dialogs")]
    [SerializeField]
    CharacterColorPair[] characterMap;
    [SerializeField]
    DialogCollection initialDialogs;

    public readonly Dictionary<Character, Color> characterTextColorMap = new Dictionary<Character, Color>();

    DialogCollection currentDialogs = null;
    int dialogIndex = -1;

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
    }

    public void OnNextClicked()
    {
        // FIXME: go to next dialog

        // For not, just switch texts
        animator.SetTrigger(nextTrigger);
    }

    public void SwapText()
    {
        presentText.text = nextText.text;
        presentText.color = nextText.color;
    }
}
