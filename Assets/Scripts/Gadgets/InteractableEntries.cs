using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* To enable true decoupling between the interactable script components we must have a way to indirectly reference information from other interactable
 * behaviours without directly referencing or hardcoding them into our script. To solve this issue we use a table of strings each interactable object has
 * attached to it. In this list we can make entries and see if a certain entry is already there. If we now want to see if the object is in a certain state
 * (independent of whether the object can actually be in that state) we check for a inspector defined string. An example:
 * 
 * We want to unhinge a door of a coffin only when this door is already opened. In the coffin door interactable script we access the entry list and store a 
 * string named 'closed'. If we now activate the unhinge-script it checks for a string 'open' in the entry list. If there is one, it continues as usual, if 
 * there is none, it can do something different or nothing at all. */

[System.Serializable]
public class InteractableEntries : MonoBehaviour
{
    [SerializeField]
    private List<string> entries = new List<string>();
    public int Count { get { return entries.Count; } }

    public string this[int i]
    {
        get
        {
            return entries[i];
        }
    }

    public void Write(string content)
    {
        if (!entries.Contains(content))
        {
            entries.Add(content);
        }
    }

    public bool Read(string content)
    {
        return entries.Contains(content);
    }

    public bool Remove(string content)
    {
        return entries.Remove(content);
    }

    public bool ConditionsFulfilled(ref List<string> genericList)
    {
        for (int i = 0; i < genericList.Count; i++)
        {
            if (!Read(genericList[i]))
            {
                return false;
            }
        }
        return true;
    }
}
