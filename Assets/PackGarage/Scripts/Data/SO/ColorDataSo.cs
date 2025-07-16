using UnityEngine;

[CreateAssetMenu(menuName = "Game/Color Data")]
public class ColorDataSo : ScriptableObject
{
    public ColorDetail[] colors;

    [System.Serializable]
    public class ColorDetail
    {
        public Sprite colorImage;
        public ColorCustomize color;
        public VehicleCollectType type;
        public int price;
        public int piece;
    }
}
