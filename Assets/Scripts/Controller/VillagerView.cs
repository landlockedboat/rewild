using UnityEngine;

public class VillagerView : BitView
{
    private VillagerModel _villagerModel;
    private VillagerStateController _villagerStateController;
    private Rigidbody2D _rigidbody2D;

    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _villagerModel = GetComponent<VillagerModel>();
        _villagerStateController = GetComponent<VillagerStateController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _villagerStateController.RegisterCallback(NotificationType.OnStateChanged, OnStateChanged);
    }

    public bool Move(Vector2Int destination)
    {
        var newPos =
            Vector2.MoveTowards(
                _rigidbody2D.position,
                destination,
                _villagerModel.Speed * Time.deltaTime);

        _rigidbody2D.MovePosition(newPos);

        if (!BitMath.CloseEnough(_rigidbody2D.position, destination))
        {
            return false;
        }

        _rigidbody2D.MovePosition(destination);
        return true;
    }

    private void OnStateChanged()
    {
        var state = _villagerModel.CurrentVillagerState;
        _animator.SetBool("isWalking", state != VillagerState.ProcessOrder && state != VillagerState.Idle);
    }
}