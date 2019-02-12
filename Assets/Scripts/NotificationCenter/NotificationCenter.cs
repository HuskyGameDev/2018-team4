using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This delegate is similar to an EventHandler:
///     The first parameter is the sender, 
///     The second parameter is the arguments / info to pass
/// </summary>
using Handler = System.Action<System.Object, System.Object>;
// fuction w/ 2 parameters

/// <summary>
/// The SenderTable maps from an object (sender of a notification), 
/// to a List of Handler methods
///     * Note - When no sender is specified for the SenderTable, 
///         the NotificationCenter itself is used as the sender key
/// </summary>
using SenderTable = System.Collections.Generic.Dictionary<System.Object, System.Collections.Generic.List<System.Action<System.Object, System.Object>>>;
//Dictionary<Object_That_Is_Listening, List<Handler>>

public class NotificationCenter {
	#region Properties
	/// <summary>
	/// The dictionary "key" (string) represents a notificationName property to be observed
	/// The dictionary "value" (SenderTable) maps between sender and observer sub tables
	/// </summary>
	private Dictionary<string, SenderTable> _table = new Dictionary<string, SenderTable>();
	private HashSet<List<Handler>> _invoking = new HashSet<List<Handler>>();    // Probably is concurrency protection????????????????
	#endregion

	#region Singleton Pattern
	public readonly static NotificationCenter instance = new NotificationCenter();
	private NotificationCenter() { }
	#endregion

	#region Public
	// AddObserver (method_name_no_brackets, notificationName, particular_notification_sender_to_look_for)
	/// <summary>
	/// Add an observer looking for a particular notificationName, and potentialy a particular notification sender.
	/// When receives a notification and sender that match it requirements, it calls the given method
	/// </summary>
	/// <param name="handler"></param>
	/// <param name="notificationName"></param>
	/// <param name="sender"></param>
	public void AddObserver(Handler handler, string notificationName, System.Object sender = null) {
		if (handler == null)    // needs to have a method to call
		{
			Debug.LogError("Can't add a null event handler for notification, " + notificationName);
			return;
		}

		if (string.IsNullOrEmpty(notificationName)) // needs an associated notification name
		{
			Debug.LogError("Can't observe an unnamed notification");
			return;
		}

		if (!_table.ContainsKey(notificationName))  // create a subtable(SenderTable) in _table that is associated w/ the given notificationName, if it does not exist
			_table.Add(notificationName, new SenderTable());

		SenderTable subTable = _table[notificationName];    // get reference to subtable(SenderTable) in _table that is associated w/ the given notificationName 

		System.Object key = sender ?? (this);   // key is either given sender or NotificationCenter.instance

		// if subtable(SenderTable, associated w/ given notificationName) does not contain a list of handlers 
		// associated w/ the given key(sender/NotificationCenter.instance), add one
		if (!subTable.ContainsKey(key))
			subTable.Add(key, new List<Handler>());

		// get reference to list of handlers, that is in the subtable(SenderTable) that is associated w/ given notificationName
		List<Handler> list = subTable[key];
		if (!list.Contains(handler))    // adds the handler(method), if it does not exist
		{
			if (_invoking.Contains(list))   // does something????????????
				subTable[key] = list = new List<Handler>(list);

			list.Add(handler);
		}
	}

	/// <summary>
	/// removes observer with given attributes
	/// </summary>
	/// <param name="handler"></param>
	/// <param name="notificationName"></param>
	/// <param name="sender"></param>
	public void RemoveObserver(Handler handler, string notificationName, System.Object sender = null) {
		if (handler == null) {
			Debug.LogError("Can't remove a null event handler for notification, " + notificationName);
			return;
		}

		if (string.IsNullOrEmpty(notificationName)) {
			Debug.LogError("A notification name is required to stop observation");
			return;
		}

		// No need to take action if we dont monitor this notification
		if (_table.ContainsKey(notificationName) == false)
			return;

		SenderTable subTable = _table[notificationName];
		System.Object key = sender ?? (this);

		if (!subTable.ContainsKey(key))
			return;

		List<Handler> list = subTable[key];
		int index = list.IndexOf(handler);
		if (index != -1) {
			if (_invoking.Contains(list))
				subTable[key] = list = new List<Handler>(list);
			list.RemoveAt(index);
		}
	}

	/// <summary>
	/// Post notification w/ given notificationName, sender, and method arguments
	/// </summary>
	/// <param name="notificationName"></param>
	/// <param name="sender"></param>
	/// <param name="arg"></param>
	public void PostNotification(string notificationName, System.Object sender = null, System.Object arg = null) {
		if (string.IsNullOrEmpty(notificationName)) {
			Debug.LogError("A notification name is required");
			return;
		}

		// No need to take action if we dont monitor this notification
		if (!_table.ContainsKey(notificationName))
			return;

		// Post to subscribers who specified a sender to observe
		SenderTable subTable = _table[notificationName];
		if (sender != null && subTable.ContainsKey(sender)) {
			List<Handler> handlers = subTable[sender];
			_invoking.Add(handlers);
			for (int i = 0; i < handlers.Count; ++i)
				handlers[i](sender, arg);
			_invoking.Remove(handlers);
		}

		// Post to subscribers who did not specify a sender to observe
		if (subTable.ContainsKey(this)) {
			List<Handler> handlers = subTable[this];
			_invoking.Add(handlers);
			for (int i = 0; i < handlers.Count; ++i)
				handlers[i](sender, arg);
			_invoking.Remove(handlers);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public void Clean() {
		string[] notKeys = new string[_table.Keys.Count];
		_table.Keys.CopyTo(notKeys, 0);

		for (int i = notKeys.Length - 1; i >= 0; --i) {
			string notificationName = notKeys[i];
			SenderTable senderTable = _table[notificationName];

			object[] senKeys = new object[senderTable.Keys.Count];
			senderTable.Keys.CopyTo(senKeys, 0);

			for (int j = senKeys.Length - 1; j >= 0; --j) {
				object sender = senKeys[j];
				List<Handler> handlers = senderTable[sender];
				if (handlers.Count == 0)
					senderTable.Remove(sender);
			}

			if (senderTable.Count == 0)
				_table.Remove(notificationName);
		}
	}

	/// <summary>
	/// Yields a coroutine until a message is recieved by the Notification center
	/// </summary>
	/// <param name="message"></param>
	/// <returns></returns>
	public IEnumerator<object> WaitForMessage(string message) {
		bool messageFlag = false;
		Handler callback = (object sender, object args) => { messageFlag = true; };
		AddObserver(callback, message);
		while (messageFlag == false)
			yield return null;

		RemoveObserver(callback, message);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public List<string> GetNotificationKeys() {
		Clean();
		List<string> keys = new List<string>();
		foreach (string itm in _table.Keys) {
			//if (_table[itm].Count > 0)
			keys.Add(itm);
		}
		return keys;
	}
	#endregion
}