using TMPro;
using UnityEngine;

public class DroppedItemAmmount : MonoBehaviour
{
    private DroppedItemController _droppedItemController;

    [SerializeField] private TextMeshPro _text;

    private void Awake()
    {
        _droppedItemController = transform.parent.GetComponent<DroppedItemController>();
    }

    private void Start()
    {
        _text.text = _droppedItemController.Ammount.ToString();
    }
}