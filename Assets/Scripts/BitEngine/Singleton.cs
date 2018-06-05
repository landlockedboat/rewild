using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if (_instance != null)
			{
				return _instance;
			}

			var instances = (T[])FindObjectsOfType(typeof(T));

			if (instances.Length > 1)
			{
				throw new System.Exception("More than one Singleton of type" +
				                           typeof(T) + " in scene.");
			}

			if (instances.Length == 0)
			{
				throw new System.Exception("Singleton " + typeof(T) + " not in scene.");
			}

			_instance = instances[0];
			
			return _instance;
		}
	}
}
