using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Game/Color Data")]
public class ColorDataSo : ScriptableObject
{
    public Sprite iconImage;
    public ColorDetail[] colors;

    [System.Serializable]
    public class ColorDetail
    {
        public VehicleCollectType type;
        public Color color;
        public int price;
        public int piece;
    }
}
