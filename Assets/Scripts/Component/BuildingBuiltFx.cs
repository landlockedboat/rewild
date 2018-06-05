using UnityEngine;

public class BuildingBuiltFx : MonoBehaviour
{
    [SerializeField] private float _timeToLive = 2f;
    
    private void Awake()
    {
        Invoke(nameof(Destroy), _timeToLive);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
