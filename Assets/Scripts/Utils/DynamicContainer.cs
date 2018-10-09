using System.Collections;
using System.Collections.Generic;

public class DynamicContainer {
	
	/// <summary>
	/// The internal storage for this DynamicContainer
	/// </summary>
	private Dictionary<System.Type, Dictionary<string, System.Object>> internalData = new Dictionary<System.Type, Dictionary<string, object>>();
	
	/// <summary>
	/// Sets a value into internal storage via a type and key
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	public void SetData<T>(string key, T value) {
		if (internalData.ContainsKey(typeof(T)) == false) {
			internalData.Add(typeof(T), new Dictionary<string,object>());
		}
		internalData[typeof(T)][key] = (object)value;
	}

	/// <summary>
	/// Access a value from storage via a type and key
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public bool GetData<T>(string key, out T output) {
		output = default(T);
		if (typeof(T) == null)
			return false;

		if (internalData.ContainsKey(typeof(T)) == false) {
			return false;
		}
		else {
			Dictionary<string, object> foundDict = internalData[typeof(T)];
			if (foundDict.ContainsKey(key) == false) {
				return false;
			}
			else {
				output = (T)foundDict[key];
				return true;
			}
		}
	}
}