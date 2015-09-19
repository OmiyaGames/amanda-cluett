using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class DialogCollection : MonoBehaviour, IList<Dialog>
{
    readonly List<Dialog> allDialogs = new List<Dialog>();

    #region Overrides
    public Dialog this[int index]
    {
        get
        {
            return allDialogs[index];
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    public int Count
    {
        get
        {
            return allDialogs.Count;
        }
    }

    public bool IsReadOnly
    {
        get
        {
            return true;
        }
    }

    public void Add(Dialog item)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(Dialog item)
    {
        return allDialogs.Contains(item);
    }

    public void CopyTo(Dialog[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<Dialog> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public int IndexOf(Dialog item)
    {
        return allDialogs.IndexOf(item);
    }

    public void Insert(int index, Dialog item)
    {
        throw new NotImplementedException();
    }

    public bool Remove(Dialog item)
    {
        throw new NotImplementedException();
    }

    public void RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return allDialogs.GetEnumerator();
    }
    #endregion

    // Use this for initialization
    void Start ()
    {
        allDialogs.Clear();
        Dialog childDialog = null;
	    for(int i = 0; i < transform.childCount; ++i)
        {
            childDialog = transform.GetChild(i).GetComponent<Dialog>();
            if(childDialog != null)
            {
                allDialogs.Add(childDialog);
            }
        }
	}
}
