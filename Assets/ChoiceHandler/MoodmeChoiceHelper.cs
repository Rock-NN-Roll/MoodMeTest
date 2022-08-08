using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoodMe;
using Naninovel;
using Naninovel.UI;
using UnityEngine;
using UnityEngine.UI;

public class MoodmeChoiceHelper : MonoBehaviour
{
    private ChoiceHandlerPanel _choiceHandlerPanel;
    private EmotionsManager _emotionsManager;
    private double _smoothHappyValue;
    public Slider theHappinessSlider;
    
    public Text CountDownText;
    
    private double happyChoiceshreshold = 0.25;
    public int countDownInSeconds;
    public float curRemainingInSeconds;
    public bool isCountingDown = false;
    // Start is called before the first frame update    
    [SerializeField]
    private List<Button> ChoiceButtons
    {
        get;
        set;
    }
    void Start()
    {
        _choiceHandlerPanel = this.GetComponent<ChoiceHandlerPanel>();
        var GO2 = FindObjectOfType<EmotionsManager>();
        _emotionsManager = GO2.GetComponent<EmotionsManager>();
        _smoothHappyValue = 0.0f;
        _choiceHandlerPanel.OnChoice += OnChoiceChosen;
        _choiceHandlerPanel.OnVisibilityChanged += OnVisibilityChanged;
    }
    private void OnVisibilityChanged(bool visibility)
    {
        if (visibility == true)
        {
            CountDownText.gameObject.SetActive(true);
            theHappinessSlider.gameObject.SetActive(true);
            StartCoroutine(GetButtons());
            StartCountDown();
            // StartCoroutine(ChoiceAfterCountDown(countDownInSeconds));
        }
        else
        {
            CountDownText.gameObject.SetActive(false);
            theHappinessSlider.gameObject.SetActive(false);
        }
    }
    IEnumerator GetButtons()
    {
        yield return new WaitForEndOfFrame();
        ChoiceButtons = GetComponentsInChildren<Button>().ToList();
    }
    private void MakeChoice()
    {
        if (_smoothHappyValue > happyChoiceshreshold)
        {
            ChoiceButtons[0].onClick?.Invoke();
        }
        else
        {
            ChoiceButtons[1].onClick?.Invoke();
        }
    }
    private void OnChoiceChosen(ChoiceState choiceState)
    {
        Debug.Log($"{choiceState.Summary} Choice chosen!");
    }

    private void Update()
    {
        _smoothHappyValue = _smoothHappyValue * 0.95 + _emotionsManager.Happy * 0.05;
        theHappinessSlider.value = 0.5f - (float)_smoothHappyValue;
        if (isCountingDown)
        {
            curRemainingInSeconds -= Time.deltaTime;
            if (curRemainingInSeconds >= 0)
            {
                CountDownText.text = ((int)curRemainingInSeconds).ToString();
            }
            else
            {
                isCountingDown = false;
                MakeChoice();
            }
        }
    }
    private void StartCountDown()
    {
        curRemainingInSeconds = countDownInSeconds;
        isCountingDown = true;
    }
    private void OnEnable()
    {
        Debug.Log($"MoodMeChoiceHelper OnEnable");
    }
    private void OnDisable()
    {
        Debug.Log($"MoodMeChoiceHelper OnDisable");
    }
    private void OnDestroy()
    {
        _choiceHandlerPanel.OnChoice -= OnChoiceChosen;
    }
}