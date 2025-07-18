
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    private int _oldIndex;
    private static VehicleManager _instance;

    public static VehicleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<VehicleManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("VehicleManager");
                    _instance = obj.AddComponent<VehicleManager>();
                }
            }
            return _instance;
        }
    }
    
    
    [SerializeField] private Transform spawnPoint;
    public void SpawnVehicle(int index, bool isControllable)
    {
        RCC_CarControllerV3 oldVehicle = RCC_SceneManager.Instance.activePlayerVehicle;
        if (oldVehicle != null)
        {
            if (_oldIndex == index) return;
            RCC.DeRegisterPlayerVehicle();
            Destroy(oldVehicle.gameObject);
        }

        RCC_CarControllerV3 getVehicle = VehicleDataSo.Instance.vehicles[index].vehicle;
        RCC_CarControllerV3 vehicle = RCC.SpawnRCC(getVehicle, spawnPoint.position, spawnPoint.rotation, true, isControllable, true);
        RCC_CustomizationApplier customizer = vehicle.gameObject.GetComponentInChildren<RCC_CustomizationApplier>();

        _oldIndex = index;
        
        if (customizer)
        {
            customizer.LoadLoadout();
        }
        else
        {
            Debug.Log("Can't Load Customize, It's null");
        }
    }

    public void DestroyCar()
    {
        RCC_CarControllerV3 vehicle = RCC_SceneManager.Instance.activePlayerVehicle;
        RCC.DeRegisterPlayerVehicle();
        Destroy(vehicle.gameObject);
    }
}
