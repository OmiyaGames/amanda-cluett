using UnityEngine;
using System.Collections;

public class Dialog : MonoBehaviour
{
    [SerializeField]
    DialogPanel.Character character = DialogPanel.Character.AmandaCluett;
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

    public virtual string Text
    {
        get
        {
            return text;
        }
    }
}
