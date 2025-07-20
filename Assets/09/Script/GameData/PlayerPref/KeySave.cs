using UnityEngine;

[CreateAssetMenu(menuName = "Game/Key Data")]
public class KeySave : ScriptableObject
{
    private static KeySave _instance;

    public static KeySave Instance
    {
        get
        {
            if(_instance == null)
                _instance = Resources.Load("Key") as KeySave;

            return _instance;
        }
    }

    [Header("PlayerPrefs Save/Load Keys")] 
    public string playerPrefsPlayerSelectedCar = "SelectedCar";
    public string playerPrefsPlayerCustomize = "Customize";
    public string playerPrefsPlayerMoney = "Money";
    public string playerPrefsPlayerVehicle = "Vehicle";
    public string playerPrefsPlayerVehicleUnlock = "VehicleUnlock";
    public string playerPrefsPlayerAchievementProgress = "ProgressAchievement";
    public string playerPrefsPlayerMissionLevel = "MissionLevel";
    public string playerPrefsPlayerLevelOfModeCurrent = "CurrentLevelOfMode";
    public string playerPrefsPlayerStatusOfDetailLevelInMode = "StatusOfDetailLevelInMode";
}
