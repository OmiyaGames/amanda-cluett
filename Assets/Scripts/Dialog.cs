using UnityEngine;
using System.Collections;

public class Dialog : MonoBehaviour
{
    [SerializeField]
    DialogPanel.Character character = DialogPanel.Character.Wife;
    [SerializeField]
    [Multiline]
    string text = "This is a test";

    public DialogPanel.Character Character
    {
        get
        {
            return character;
        }
    }

    public string Text
    {
        get
        {
            return text;
        }
    }
}
