using UnityEngine;

public class HouseController : BuildingController
{
    [HideInInspector] public VillagerController VillagerController;

    public float SleepinessRestore = .01f;

    public void SetOwner(VillagerController villagerController)
    {
        if (VillagerController != null)
        {
            return;
        }
        VillagerController = villagerController;
    }

    public bool HasOwner()
    {
        return VillagerController != null;
    }
}