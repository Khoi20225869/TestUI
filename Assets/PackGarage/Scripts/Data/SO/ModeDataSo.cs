using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Mode Data")]
public class ModePageSoData : ScriptableObject
{
    public List<Mode> Modes;


    [System.Serializable]
    public class Mode
    {
        public GameMode mode;
        public int totalLevel;
        public Sprite icon;
        public string[] levelSceneAddresses;
    }
}
