using UnityEngine;


[CreateAssetMenu(menuName = "Game/Wheel Data")]
public class WheelDataSo : ScriptableObject
{
    public Wheel[] wheels;

    [System.Serializable]
    public class Wheel
    {
        public GameObject wheel;
        public Sprite colorImage;
        public VehicleCollectType type;
        public int price;
        public int piece;
    }
}
