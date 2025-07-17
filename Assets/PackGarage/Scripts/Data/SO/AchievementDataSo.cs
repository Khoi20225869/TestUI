using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Mission Data")]
public class AchievementDataSo : ScriptableObject
{
    public Mission[] missions;

    [System.Serializable]
    public class Mission
    {
        public string description;
        public AchievementType achievementType;
        public VehicleCollectType collectType;
        public DetailMission[] detailMission;
    }

    [System.Serializable]
    public class DetailMission
    {
        public int coin;
        public int maxProgress;
    }
}
