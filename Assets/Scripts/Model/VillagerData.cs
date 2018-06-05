using System;

[Serializable]
public class VillagerData : CellObjectData
{
    public override void Spawn()
    {
        SpawnController.Instance.SpawnVillager(Position);
    }
}