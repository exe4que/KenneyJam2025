using System;
using KenneyJam2025;
using UnityEngine;
using UnityEngine.UI;

public class TimerControler : MonoBehaviour
{
    [SerializeField] private Image[] _timerImages;
    [SerializeField] private Animator[] _upgradeWindowsAnimators;
    [SerializeField] private RectTransform[] _upgradeWindowsRectTransforms;
    [SerializeField] private GameObject _pressButtonSign;

    private void OnEnable()
    {
        GlobalEvents.MainMechanicTimerTicked += OnMainMechanicTimerTicked;
        GlobalEvents.UpgradeWindowOpen += OnUpgradeWindowOpen;
        GlobalEvents.UpgradeWindowClosed += OnUpgradeWindowClosed;
    }
    
    private void OnDisable()
    {
        GlobalEvents.MainMechanicTimerTicked -= OnMainMechanicTimerTicked;
        GlobalEvents.UpgradeWindowOpen -= OnUpgradeWindowOpen;
        GlobalEvents.UpgradeWindowClosed -= OnUpgradeWindowClosed;
    }

    private void Start()
    {
        var levelSettings = GameManager.Instance.CurrentLevelSettings;

        Vector2 upgrade1Window = levelSettings.UpgradeWindow1Range;
        Vector2 upgrade1WindowInRectSize = new Vector2(
            Utils.Remap(0f, 1f, 60f, 1144f, upgrade1Window.x),
            Utils.Remap(0f, 1f, 60f, 1144f, upgrade1Window.y)
        );
        _upgradeWindowsRectTransforms[0].anchoredPosition = new Vector2(upgrade1WindowInRectSize.x, 0);
        _upgradeWindowsRectTransforms[0].sizeDelta = new Vector2(upgrade1WindowInRectSize.y - upgrade1WindowInRectSize.x, 27f);
        
        Vector2 upgrade2Window = levelSettings.UpgradeWindow2Range;
        Vector2 upgrade2WindowInRectSize = new Vector2(
            Utils.Remap(0f, 1f, 60f, 1144f, upgrade2Window.x),
            Utils.Remap(0f, 1f, 60f, 1144f, upgrade2Window.y)
        );
        _upgradeWindowsRectTransforms[1].anchoredPosition = new Vector2(upgrade2WindowInRectSize.x, 0);
        _upgradeWindowsRectTransforms[1].sizeDelta = new Vector2(upgrade2WindowInRectSize.y - upgrade2WindowInRectSize.x, 27f);
        
        Vector2 upgrade3Window = levelSettings.UpgradeWindow3Range;
        Vector2 upgrade3WindowInRectSize = new Vector2(
            Utils.Remap(0f, 1f, 60f, 1144f, upgrade3Window.x),
            Utils.Remap(0f, 1f, 60f, 1144f, upgrade3Window.y)
        );
        _upgradeWindowsRectTransforms[2].anchoredPosition = new Vector2(upgrade3WindowInRectSize.x, 0);
        _upgradeWindowsRectTransforms[2].sizeDelta = new Vector2(upgrade3WindowInRectSize.y - upgrade3WindowInRectSize.x, 27f);
        
    }

    private void Update()
    {
        if (GameManager.Instance.GameOver) return;

        for (var i = 0; i < _upgradeWindowsRectTransforms.Length; i++)
        {
            GameObject go = _upgradeWindowsRectTransforms[i].gameObject;
            go.SetActive(i == GameManager.Instance.WeaponIndex);
        }
    }

    private void OnUpgradeWindowOpen(int index)
    {
        _upgradeWindowsAnimators[index].SetBool("Animating", true);
        _pressButtonSign.SetActive(true);
    }
    
    private void OnUpgradeWindowClosed(int index)
    {
        _upgradeWindowsAnimators[index].SetBool("Animating", false);
        _pressButtonSign.SetActive(false);
    }
    
    private void OnMainMechanicTimerTicked(float timeLeft)
    {
        for (int i = 0; i < _timerImages.Length; i++)
        {
            _timerImages[i].fillAmount = timeLeft;
        }
    }
}
