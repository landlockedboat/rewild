using UnityEngine;

public class BitModel<T> : BitSingletonModel<T> where T : MonoBehaviour
{
}

public class BitSingletonModel<T> : BitModel where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance => SingletonHelper.GetInstance(ref _instance);
}

public class BitModel : BitMonoBehaviour
{
}