using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
	private List<IEventListener> listeners =
		new List<IEventListener>();

	public void CallEvent()
	{
		for (int i = listeners.Count - 1; i >= 0; i--)
			listeners[i].OnEventCalled();
	}

	public void RegisterListener(IEventListener listener)
	{ listeners.Add(listener); }

	public void UnregisterListener(IEventListener listener)
	{ listeners.Remove(listener); }
}