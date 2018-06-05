using System;
using System.Collections.Generic;
using UnityEngine;

public class VillagerModel : BitModel
{
    public OrderType CurrentOrderType;

    // Maybe later it'll be wise to implement this
    //public Cell CurrentCell;
    public Cell DestinationCell;

    public float Speed;

    public float MaxSecondsIdle = 1;
    public float MinSecondsIdle = .5f;

    public int MaxRoamRadius = 5;
    public int MinRoamRadius = 2;

    public Need Sleepiness;
    public Need Hunger;
    public Need Fear;

    public float FearRestoreAmmount;

    public Dictionary<DropItemType, int> Inventory;


    [HideInInspector] public TownOrder CurrentTownOrder;
    [HideInInspector] public float SecondsIdle;
    [HideInInspector] public HouseController House;
    [HideInInspector] public Rigidbody2D Rigidbody2D;
    [HideInInspector] public List<Node> CurrentPath;
    [HideInInspector] public VillagerState CurrentVillagerState = VillagerState.Idle;
    [HideInInspector] public Queue<OrderType> Orders;

    private void Start()
    {
        Sleepiness = new Need(LevelConfiguration.Instance.SecondsToSleepinessFilled);
        Hunger = new Need(LevelConfiguration.Instance.SecondsToHungerFilled);
        Fear = new Need(LevelConfiguration.Instance.SecondsToFearFilled);
        Speed = LevelConfiguration.Instance.VillagerSpeed;
        FearRestoreAmmount = LevelConfiguration.Instance.FearRestoreAmmount;

        ResetInventory();
    }

    public void ResetInventory()
    {
        Inventory = new Dictionary<DropItemType, int>();
        foreach (DropItemType type in Enum.GetValues(typeof(DropItemType)))
        {
            Inventory.Add(type, 0);
        }
    }
}