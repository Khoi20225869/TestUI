using UnityEngine;

[CreateAssetMenu(menuName = "Game/Key Data")]
public class KeySave : ScriptableObject
{
    private static KeySave instance;

    public static KeySave Instance
    {
        get
        {
            if(instance == null)
                instance = Resources.Load("Key") as KeySave;

            return instance;
        }
    }
    [Header("PlayerPrefs Save/Load Keys")]
    public string playerPrefsPlayerMoney = "Money";
    public string playerPrefsPlayerVehicle = "Vehicle";
    public string playerPrefsPlayerVehicleUnlock = "VehicleUnlock";
    public string playerPrefsPlayerAchievementProgress = "ProgressAchievement";
    public string playerPrefsPlayerMissionLevel = "MissionLevel";
    public string playerPrefsPlayerLevelOfModeCurrent = "CurrentLevelOfMode";
    public string playerPrefsPlayerStatusOfDetailLevelInMode = "StatusOfDetailLevelInMode";
}
