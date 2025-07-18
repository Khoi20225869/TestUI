using UnityEngine;

[CreateAssetMenu(menuName = "Game/Manage Data")]
public class ManageCustomizeSo : ScriptableObject
{
    private static ManageCustomizeSo _instance;

    public static ManageCustomizeSo Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load("Customize/Manage Customize So") as ManageCustomizeSo;    
                if (_instance == null)
                {
                    Debug.LogError("ManageCustomizeSo not found in Resources folder.");
                }
            }
            return _instance;
        }
    }
    public int totalOption;

    public ColorDataSo colorDataSo;
    public WheelDataSo wheelDataSo;
}
