using TMPro;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class TaskItem_09 : MonoBehaviour
{
    [SerializeField] private Button completeBtn;
    [SerializeField] private GameObject inCompleteObj;
    [SerializeField] private GameObject doneObj;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI achievementName;
    [SerializeField] private TextMeshProUGUI achievementProgressTxt;
    [SerializeField] private TextMeshProUGUI rewardTxt;

    private int _currentLevelMission;
    private float _progress;
    private int _reward;
    private int _maxProgress;
    private string _missionName;
    private int _totalMission;
    private AchievementDataSo.Mission _mission;
    
    private Tween _sliderTween, _progressTextTween, _rewardTextTween;
    private Sequence _showSequence;
    
    public void SetUp(AchievementDataSo.Mission mission)
    {
        _mission = mission;
        GetIndex();
        GetStatusBtn();
        SetUI();
    }

    private void SetUI()
    {
        achievementName.SetText(_mission.description);
        
        float progressValue = Mathf.Clamp(_progress, 0, _maxProgress);
        float progressPercent = _maxProgress > 0 ? progressValue / _maxProgress : 1f;

        progressSlider.value = 0;
        achievementProgressTxt.SetText($"0/{_maxProgress}");
        rewardTxt.SetText("0");
        completeBtn.gameObject.SetActive(false);
        inCompleteObj.SetActive(true);
        doneObj.SetActive(false);
        
        _showSequence = DOTween.Sequence();
        _sliderTween = progressSlider.DOValue(progressPercent, 0.5f).SetEase(Ease.OutQuad);
        _showSequence.Append(_sliderTween);
        
        _showSequence.AppendCallback(() => {
            _progressTextTween = DOTween.To(
                () => 0f,
                val => achievementProgressTxt.SetText($"{Mathf.RoundToInt(val)}/{_maxProgress}"),
                progressValue,
                0.3f
            ).SetEase(Ease.OutQuad);
        });
        _showSequence.AppendInterval(0.3f);
        
        _showSequence.AppendCallback(() => {
            _rewardTextTween = DOTween.To(
                () => 0,
                val => rewardTxt.SetText(val.ToString()),
                _reward,
                0.3f
            ).SetEase(Ease.OutQuad);
        });
        _showSequence.AppendInterval(0.3f);
        
        _showSequence.OnComplete(() => {
            GetStatusBtn();
            completeBtn.onClick.RemoveAllListeners();
            completeBtn.onClick.AddListener(OnClaimReward);
        });
    }
    private void GetStatusBtn()
    {
        bool isComplete = _progress >= _maxProgress;
        bool isLastLevel = _currentLevelMission == _totalMission;
        completeBtn.gameObject.SetActive(isComplete);
        inCompleteObj.SetActive(!isComplete && !isLastLevel);
        doneObj.SetActive(isComplete && isLastLevel);
    }

    private void GetIndex()
    {
        _totalMission = _mission.detailMission.Length;
        _missionName = _mission.achievementType.ToString();
        _progress = PlayerData.GetAchievementProgress(_missionName);
        _reward = _mission.detailMission[PlayerData.GetCurrentAchievementTypeLevel(_missionName)].reward;
        _maxProgress = _mission.detailMission[PlayerData.GetCurrentAchievementTypeLevel(_missionName)].maxProgress;
        _currentLevelMission = PlayerData.GetCurrentAchievementTypeLevel(_missionName);
    }

    private void OnClaimReward()
    {
        // Claim the reward logic here
    }

    private void OnDestroy()
    {
        _sliderTween?.Kill();
        _progressTextTween?.Kill();
        _rewardTextTween?.Kill();
        _showSequence?.Kill();
        completeBtn.onClick.RemoveAllListeners();
    }
}
