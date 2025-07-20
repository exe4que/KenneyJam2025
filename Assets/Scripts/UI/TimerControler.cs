using UnityEngine;
using UnityEngine.UI;

public class TimerControler : MonoBehaviour
{
    [SerializeField] private Image _timerImage;
    [SerializeField] private float _maxTime = 4f;
    private float _remainingTime;

    void Start()
    {
        _timerImage = GetComponent<Image>();

        _remainingTime = _maxTime;
    }

    void Update()
    {
        if (_remainingTime > 0)
        {
            _remainingTime -= Time.deltaTime;
            _timerImage.fillAmount = _remainingTime / _maxTime;
        }
    }


}
