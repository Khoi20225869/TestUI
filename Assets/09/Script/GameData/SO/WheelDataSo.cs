using UnityEngine;


[CreateAssetMenu(menuName = "Game/Wheel Data")]
public class WheelDataSo : ScriptableObject
{
    public Sprite iconImage;
    public Wheel[] wheels;

    [System.Serializable]
    public class Wheel
    {
        public GameObject wheel;
        public Sprite wheelImage;
        public VehicleCollectType type;
        public int price;
        public int piece;
    }
}
