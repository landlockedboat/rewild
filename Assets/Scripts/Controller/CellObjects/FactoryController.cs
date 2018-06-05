using System;
using System.Collections;
using UnityEngine;

public class FactoryController : BuildingController
{
    public int MaterialAmmountNeeded = 1;
    [SerializeField] private float _productionTime;
    [SerializeField] private DropItemType _producedItem;
    [SerializeField] private int _producedItemAmmount;

    [SerializeField] private float _resourceAskingTime = 3f;
    [SerializeField] private uint _resourceAskingAmmount = 10;

    [HideInInspector] public int RawMaterialStored;

    private float _currentProductionTime;

    [HideInInspector] public bool IsProducing;
    public bool IsFull => RawMaterialStored >= MaterialAmmountNeeded;

    public FactoryWidthType WidthType = FactoryWidthType.Small;

    private OrderType _orderTypeToAsk;
    private DropItemType _dropItemTypeToAskFor;

    // This is all because of you, non-nesting prefabs...
    private void Start()
    {
        Instantiate(SpawnModel.Instance.FactoryProgressPrefab, transform);
        InitVariables();

        StartCoroutine(AskForResources());
    }

    private void InitVariables()
    {
        switch (_producedItem)
        {
            case DropItemType.Bread:
                _orderTypeToAsk = OrderType.BakeBreadFindWheat;
                _dropItemTypeToAskFor = DropItemType.Wheat;
                break;
            case DropItemType.Tofu:
                _orderTypeToAsk = OrderType.FermentTofuFindSoybean;
                _dropItemTypeToAskFor = DropItemType.Soybean;
                break;
            case DropItemType.Piggy:
                _orderTypeToAsk = OrderType.FeedPigFindWheat;
                _dropItemTypeToAskFor = DropItemType.Wheat;
                break;
            case DropItemType.Meat:
                _orderTypeToAsk = OrderType.MakeMeatFindPig;
                _dropItemTypeToAskFor = DropItemType.Piggy;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator AskForResources()
    {
        while (true)
        {
            if (!IsProducing &&
                !TownController.Instance.ExistsOrderOfType(_orderTypeToAsk) &&
                TownController.Instance.GetStored(_dropItemTypeToAskFor) >= _resourceAskingAmmount)
            {
                TownController.Instance.PushNewOrder(_orderTypeToAsk, null, _resourceAskingAmmount);
            }

            yield return new WaitForSeconds(_resourceAskingTime);
        }
    }

    public void AddRawItems(int ammount)
    {
        RawMaterialStored += ammount;
        TriggerCallback(NotificationType.OnFactoryProgressChanged);

        if (RawMaterialStored < MaterialAmmountNeeded)
        {
            return;
        }

        IsProducing = true;
    }

    private void Update()
    {
        if (!IsProducing)
        {
            return;
        }

        _currentProductionTime += Time.deltaTime;

        if (_currentProductionTime < _productionTime) return;

        SpawnController.Instance.SpawnDroppedItem(_producedItem, Position + BitMath.RoundToInt(Vector2.down),
            _producedItemAmmount);

        _currentProductionTime = 0;
        RawMaterialStored -= MaterialAmmountNeeded;
        TriggerCallback(NotificationType.OnFactoryProgressChanged);

        if (RawMaterialStored < MaterialAmmountNeeded)
        {
            IsProducing = false;
        }
    }
}

public enum FactoryWidthType
{
    Small,
    Large
}