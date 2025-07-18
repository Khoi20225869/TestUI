using UnityEngine;

public class SpawnVehicle : MonoBehaviour
{
    private void Start()
    {
        VehicleManager.Instance.SpawnVehicle(PlayerData.GetCurrentCarIndex(), false);
    }
}
