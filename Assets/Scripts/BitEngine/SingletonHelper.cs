using UnityEngine;

public class SingletonHelper : MonoBehaviour
{
    public static T GetInstance<T>(ref T instance) where T : MonoBehaviour
    {
        if (instance != null)
        {
            return instance;
        }

        var instances = (T[]) FindObjectsOfType(typeof(T));

        if (instances.Length > 1)
        {
            throw new System.Exception("More than one Singleton of type" +
                                       typeof(T) + " in scene.");
        }

        if (instances.Length == 0)
        {
            throw new System.Exception("Singleton " + typeof(T) + " not in scene.");
        }

        instance = instances[0];

        return instance;
    }
}