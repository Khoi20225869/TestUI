using TMPro;
using UnityEngine.UI;
using UnityEngine;


public class TaskItem_09 : MonoBehaviour
{
    [SerializeField] private Button _completeBtn;
    [SerializeField] private GameObject _inCompleteObj;
    [SerializeField] private GameObject _doneObj;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private TextMeshProUGUI _achievementName;
    [SerializeField] private TextMeshProUGUI _achievementProgressTxt;
    [SerializeField] private TextMeshProUGUI _rewardtxt;

    public void SetUp(AchievementDataSo.Mission mission)
    {
        
    }
}
