using UnityEngine;

public static class PlayerData 
{
    public static int GetMoney()
    {
        return PlayerPrefs.GetInt(KeySave.Instance.playerPrefsPlayerMoney, 0);
    }

    public static int GetCurrentCarIndex()
    {
        return PlayerPrefs.GetInt(KeySave.Instance.playerPrefsPlayerVehicle, 0);
    }

    public static void SetMoney(int amount)
    {
        int currentMoney = GetMoney();
        PlayerPrefs.SetInt(KeySave.Instance.playerPrefsPlayerMoney, currentMoney + amount);
    }

    public static void SetCurrentCarIndex(int index)
    {
        PlayerPrefs.SetInt(KeySave.Instance.playerPrefsPlayerVehicle, index);
    }
    
    public static int GetCarUnlockStatus(int index)
    {
        return PlayerPrefs.GetInt(KeySave.Instance.playerPrefsPlayerVehicleUnlock + index, 0);
    }

    public static void SetCarUnlockStatus(int index)
    {
        PlayerPrefs.SetInt(KeySave.Instance.playerPrefsPlayerVehicleUnlock + index, 1);
    }
    
    // Achievement
    public static float GetAchievementProgress(string name)
    {
        return PlayerPrefs.GetFloat(KeySave.Instance.playerPrefsPlayerAchievementProgress + name, 0);
    }

    public static void SetAchievementProgress(string name, float progress)
    {
        float currentProgress = GetAchievementProgress(name);
        PlayerPrefs.SetFloat(KeySave.Instance.playerPrefsPlayerAchievementProgress + name, currentProgress + progress);
    }
    
    public static int GetCurrentAchievementTypeLevel(string name)
    {
        return PlayerPrefs.GetInt(KeySave.Instance.playerPrefsPlayerMissionLevel + name, 0);
    }

    public static void SetCurrentAchievementTypeLevel(string name)
    {
        int currentMissionLevel = GetCurrentAchievementTypeLevel(name);
        PlayerPrefs.SetInt(KeySave.Instance.playerPrefsPlayerMissionLevel + name, currentMissionLevel + 1);
    }
    //Level

    public static int GetCurrentLevelMode(string mode)
    {
        return PlayerPrefs.GetInt(KeySave.Instance.playerPrefsPlayerLevelOfModeCurrent + mode, 0);
    }

    public static void SetCurrentLevelMode(string mode, int index)
    {
        PlayerPrefs.SetInt(KeySave.Instance.playerPrefsPlayerLevelOfModeCurrent + mode, index);
    }

    public static int GetStatusOfDetailLevelInMode(string mode, int index)
    {
        return PlayerPrefs.GetInt(KeySave.Instance.playerPrefsPlayerStatusOfDetailLevelInMode + mode + index, 0);
    }
    
    public static void SetStatusOfDetailLevelInMode(string mode, int index, int status)
    {
        PlayerPrefs.SetInt(KeySave.Instance.playerPrefsPlayerStatusOfDetailLevelInMode + mode + index, status);
    }
}
