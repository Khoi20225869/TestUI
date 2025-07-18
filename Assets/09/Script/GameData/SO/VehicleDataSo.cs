using UnityEngine;

[CreateAssetMenu(menuName = "Game/Vehicle Data")]
public class VehicleDataSo : ScriptableObject
{
    private static VehicleDataSo _instance;
    
    public static VehicleDataSo Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load("Vehicle/Vehicle Data So") as VehicleDataSo;
                if (_instance == null)
                {
                    Debug.LogError("VehicleDataSo instance not found. Please ensure it is created in the Resources folder.");
                }
            }
            return _instance;
        }
    }
    public Vehicle[] vehicles;

    [System.Serializable]
    public class Vehicle
    {
        public string vehicleSaveName;
        public RCC_CarControllerV3 vehicle;
        public Sprite vehicleImage;
        public VehicleCollectType collectType;

        [Space()][Range(100f, 1400f)] public float engineTorque = 350f;
        [Range(0f, 1f)] public float handling = .1f;
        [Range(160f, 380f)] public float speed = 240f;

        [Space()][Range(1f, 2f)] public float upgradedEngineEfficiency = 1.2f;
        [Range(1f, 2f)] public float upgradedHandlingEfficiency = 1.2f;
        [Range(1f, 2f)] public float upgradedSpeedEfficiency = 1.2f;


        public int price;
        public int pieceCount;
    }
}
