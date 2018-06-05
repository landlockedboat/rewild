using UnityEngine;

public class HumanoidController : BitController
{
    private Rigidbody2D _rigidbody2D;
    protected HumanoidModel HumanoidModel;
    private const float CloseEnough = .1f;
    protected Vector3 Destination;


    protected virtual void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        HumanoidModel = GetComponent<HumanoidModel>();
    }

    protected bool Move(Vector2 destination)
    {
        Debug.Log(name);
        var newPos =
            Vector2.MoveTowards(
                _rigidbody2D.position,
                destination,
                HumanoidModel.Speed * Time.deltaTime);

        _rigidbody2D.MovePosition(newPos);

        return Vector2.Distance(destination, _rigidbody2D.position) >= CloseEnough;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Not Walkable!");
        Destination = BitMath.RoundToInt(transform.position);
    }
}