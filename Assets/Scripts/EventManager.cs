using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public enum EVENT
    {
        OnPause,
        OnHit,
        OnShoot,
        OnDodge,
    }

    private static Dictionary<EVENT, Action> eventTable = new Dictionary<EVENT, Action>(); //Stores the delegate that get called when an event id fired

    /// <summary>
    /// Add a delegate to get called from the specific event
    /// </summary>
    /// <param name="evnt">Event where to add the delegate </param>
    /// <param name="action">Action to add</param>
    public static void AddHandler(EVENT evnt, Action action)
    {
        if (!eventTable.ContainsKey(evnt))
        {
            eventTable[evnt] = action;
        }
        else
        {
            eventTable[evnt] += action;
        }
    }

    /// <summary>
    /// Remove a delegate to get called from the specifc event
    /// </summary>
    /// <param name="evnt">Event where is the action to remove</param>
    /// <param name="action">Action to remove</param>
    public static void RemoveHandler(EVENT evnt, Action action)
    {
        if (eventTable[evnt] != null)
            eventTable[evnt] -= action;
        if (eventTable[evnt] == null)
            eventTable.Remove(evnt);
    }

    /// <summary>
    /// Fires the selected event
    /// </summary>
    /// <param name="evnt">event to fire</param>
    public static void Broadcast(EVENT evnt)
    {
        if (eventTable[evnt] != null)
        {
            eventTable[evnt]();
        }
    }
}
