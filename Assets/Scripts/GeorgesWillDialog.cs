using UnityEngine;
using System.Collections;

public class GeorgesWillDialog : Dialog
{
    [SerializeField]
    GamePanel panel;
    [SerializeField]
    UnlockEvent georgesWill;

    public override string Text
    {
        get
        {
            return string.Format(base.Text, panel.CentsToString(georgesWill.IncreaseCents));
        }
    }
}
