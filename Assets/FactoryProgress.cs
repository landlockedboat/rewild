using TMPro;
using UnityEngine;

public class FactoryProgress : MonoBehaviour
{
    private FactoryController _factoryController;

    [SerializeField] private TextMeshPro _text;

    private void Awake()
    {
        _factoryController = transform.parent.GetComponent<FactoryController>();
        _factoryController.RegisterCallback(
            NotificationType.OnFactoryProgressChanged, OnFactoryProgressChanged);

        if (_factoryController.WidthType == FactoryWidthType.Large)
        {
            _text.GetComponent<RectTransform>().sizeDelta = new Vector3(2, 1);
        }

        OnFactoryProgressChanged();
    }

    private void OnFactoryProgressChanged()
    {
        _text.text = $" {_factoryController.RawMaterialStored}/{_factoryController.MaterialAmmountNeeded} ";
    }
}