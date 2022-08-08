using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoodMe;
using System;
using UnityEngine.Events;

namespace MoodMe
{
    
    public class GetEmotionTrigger : MonoBehaviour
    {
        public enum EmotionEnum
        {
            Angry, Disgust, Happy, Neutral, Sad, Scared, Surprised, EmotionIndex
        }

        public EmotionEnum Emotion;
        [Serializable]
        public class TriggerClass : UnityEvent { }
        public TriggerClass EventToTrigger;

        [Range (0,1)]
        public float Threshold;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float value = 0;

            switch (Emotion)
            {
                case EmotionEnum.Angry:
                    value = EmotionsManager.Emotions.angry;
                    break;
                case EmotionEnum.Disgust:
                    value = EmotionsManager.Emotions.disgust;
                    break;
                case EmotionEnum.Happy:
                    value = EmotionsManager.Emotions.happy;
                    break;
                case EmotionEnum.Neutral:
                    value = EmotionsManager.Emotions.neutral;
                    break;
                case EmotionEnum.Sad:
                    value = EmotionsManager.Emotions.sad;
                    break;
                case EmotionEnum.Scared:
                    value = EmotionsManager.Emotions.scared;
                    break;
                case EmotionEnum.Surprised:
                    value = EmotionsManager.Emotions.surprised;
                    break;
                case EmotionEnum.EmotionIndex:
                    value = EmotionsManager.EmotionIndex;
                    break;
            }

            if (value>Threshold)
            {
                EventToTrigger.Invoke();
            }
        }


    }
}