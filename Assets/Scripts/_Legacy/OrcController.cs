using UnityEngine;

public class OrcController : HumanoidController
{
    [SerializeField] private float _timeBetweenRoams = 1;
    [SerializeField, Range(0, 1)] private float _roamTimeVariation = 0.25f;
    [SerializeField] private float _roamRadius = 3;
    [SerializeField, Range(0, 1)] private float _roamDistanceVariation;
    [SerializeField] private float _maxRoamDistanceFromOrigin = 10;

    private Vector3 _origin;

    private bool _isMoving;

    private void Start()
    {
        _origin = transform.position;
        InvokeRoam();
    }

    private void InvokeRoam()
    {
        var roamTime = _timeBetweenRoams + (_timeBetweenRoams * _roamTimeVariation * Random.Range(-1f, 1f));
        Invoke(nameof(StartRoam), roamTime);
    }

    private void StartRoam()
    {
        do
        {
            var distance = _roamRadius + (_roamRadius * _roamDistanceVariation * Random.Range(-1f, 1f));
            Destination = transform.position + Random.rotation * Vector2.right * distance;
        } while (Vector2.Distance(Destination, _origin) > _maxRoamDistanceFromOrigin);

        Destination = BitMath.RoundToInt(Destination);
        
        _isMoving = true;
    }

    private void Update()
    {
        if (!_isMoving) return;
        if (Move(Destination)) return;
        transform.position = Destination;
        _isMoving = false;
        InvokeRoam();
    }
}