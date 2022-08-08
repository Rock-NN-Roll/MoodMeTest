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
    private TestSceneCommands _testSceneCommands;
    private ChoiceHandlerPanel _choiceHandlerPanel;
    private EmotionsManager _emotionsManager;
    private double _smoothHappyValue;
    private double happyChoiceshreshold = 0.25;

    // Start is called before the first frame update    
    [SerializeField]
    private List<Button> ChoiceButtons
    {
        get;
        set;
    }

    void Start()
    {
        var GO = FindObjectOfType<TestSceneCommands>();
        _testSceneCommands = GO.GetComponent<TestSceneCommands>();
        _choiceHandlerPanel = this.GetComponent<ChoiceHandlerPanel>();
        var GO2 = FindObjectOfType<EmotionsManager>();
        _emotionsManager = GO2.GetComponent<EmotionsManager>();
        _smoothHappyValue = 0.0f;
        _testSceneCommands.OnGUIButtonClicked += PrintEmotion;
        _testSceneCommands.OnGUIButtonClicked += PrintButtonsCount;
        _choiceHandlerPanel.OnChoice += OnChoiceChosen;
        _choiceHandlerPanel.OnVisibilityChanged += OnVisibilityChanged;
    }

    private void OnVisibilityChanged(bool visibility)
    {
        Debug.Log($"Visibility Changed to {visibility}!");
        if (visibility == true)
        {
            StartCoroutine(GetButtons());
            StartCoroutine(ChoiceAfterCountDown());
        }
    }

    IEnumerator GetButtons()
    {
        yield return new WaitForEndOfFrame();
        ChoiceButtons = GetComponentsInChildren<Button>().ToList();
        Debug.Log($"ChoiceButtons Count {ChoiceButtons.Count}");
    }

    IEnumerator ChoiceAfterCountDown()
    {
        yield return new WaitForSeconds(3);
        if (_smoothHappyValue > happyChoiceshreshold)
        {
            Debug.Log($"Happiness is {_smoothHappyValue} with shreshold {happyChoiceshreshold}");
            ChoiceButtons[0].onClick?.Invoke();
            Debug.Log($"Choice {ChoiceButtons[0].GetComponentInChildren<Text>().text} chosen");
        }
        else
        {
            Debug.Log($"Happiness is {_smoothHappyValue} with shreshold {happyChoiceshreshold}");
            ChoiceButtons[1].onClick?.Invoke();
            Debug.Log($"Choice {ChoiceButtons[1].GetComponentInChildren<Text>().text} chosen");
        }
    }

    private void ChooseButton(Button button)
    {
        button.onClick.Invoke();
    }
    
    private void OnChoiceChosen(ChoiceState choiceState)
    {
        Debug.Log($"Choice chosen!");
    }

    private void Update()
    {
        _smoothHappyValue = _smoothHappyValue * 0.95 + _emotionsManager.Happy * 0.05;
        // Debug.Log($"Happy Value: {_smoothHappyValue}");
    }

    private void OnEnable()
    {
        Debug.Log($"MoodMeChoiceHelper OnEnable");
    }

    private void OnDisable()
    {
        Debug.Log($"MoodMeChoiceHelper OnDisable");
    }
    // Update is called once per frame
    void PrintEmotion()
    {
        if (_testSceneCommands.chosenEmotion != Emotions.None)
        {
            Debug.Log($"Current Happy Value: {_testSceneCommands.chosenEmotion.ToString()}, Current Avg Value: {_smoothHappyValue}");
        }
    }

    void PrintButtonsCount()
    {
        Debug.Log(_choiceHandlerPanel.ChoiceHandlerButtons.Count);
    }

    private void OnDestroy()
    {
        _testSceneCommands.OnGUIButtonClicked -= PrintEmotion;
        _testSceneCommands.OnGUIButtonClicked -= PrintButtonsCount;
        _choiceHandlerPanel.OnChoice -= OnChoiceChosen;
    }
}