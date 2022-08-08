using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoodMe;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.Collections;
using Unity.Jobs;

namespace MoodMe
{

    public class EmotionsManager : MonoBehaviour
    {
        //public MeshRenderer PreviewMR;
        //[Header("ENTER LICENSE HERE")]
        //public string Email = "";
        //public string AndroidLicense = "";
        //public string IosLicense = "";
        //public string OsxLicense = "";
        //public string WindowsLicense = "";

        [Header("Input")]
        public ManageEmotionsNetwork EmotionNetworkManager;
        public FaceDetector FaceDetectorManager;

        [Header("Performance")]
        [Range(1, 60)]
        public int ProcessEveryNFrames = 15;
        [Header("Processing")]
        public bool FilterAllZeros = true;
        [Range(0.1f, 60f)]
        public float Frequency = 30f;
        [Range(0.1f, 60f)]
        public float MinCutOff = 1.1f;

        [Header("Emotions")]
        public bool TestMode = false;
        [Range(0, 1f)]
        public float Angry;
        [Range(0, 1f)]
        public float Disgust;
        [Range(0, 1f)]
        public float Happy;
        [Range(0, 1f)]
        public float Neutral;
        [Range(0, 1f)]
        public float Sad;
        [Range(0, 1f)]
        public float Scared;
        [Range(0, 1f)]
        public float Surprised;
        [Range(0, 1f)]

        public static float EmotionIndex;

        public static MoodMeEmotions.MDMEmotions Emotions;
        public static bool BufferProcessed = false;
        public static bool ValidData = false;										   
        private static MoodMeEmotions.MDMEmotions CurrentEmotions;


        //Main buffer texture
        public static WebCamTexture CameraTexture;

        private EmotionsInterface _emotionNN;


        //Main buffer

      
        private byte[] _buffer;


        private int NFramePassed;

        private static DateTime timestamp;

        private OneEuroFilter angryFilter, disgustFilter, happyFilter, neutralFilter, sadFilter, scaredFilter, surprisedFilter;



        // Start is called before the first frame update
        void Start()
        {
            _emotionNN = new EmotionsInterface(EmotionNetworkManager, FaceDetectorManager);

            angryFilter = new OneEuroFilter(Frequency, MinCutOff);
            disgustFilter = new OneEuroFilter(Frequency, MinCutOff);
            happyFilter = new OneEuroFilter(Frequency, MinCutOff);
            neutralFilter = new OneEuroFilter(Frequency, MinCutOff);
            sadFilter = new OneEuroFilter(Frequency, MinCutOff);
            scaredFilter = new OneEuroFilter(Frequency, MinCutOff);
            surprisedFilter = new OneEuroFilter(Frequency, MinCutOff);


            //int remainingDays = _emotionNN.SetLicense(Email == "" ? null : Email, EnvKey == "" ? null : EnvKey);

            //if (remainingDays == -1)
            //{
            //    Debug.Log("INVALID OR EMPTY LICENSE. The SDK will run in demo mode.");
            //    remainingDays = _emotionNN.SetLicense(null, EnvKey);
            //}

            //if (remainingDays < 0x7ff)
            //{
            //    Debug.Log("Remaining " + remainingDays + " days");
            //    if (remainingDays == 0)
            //    {
            //        Debug.Log("LICENSE EXPIRED. Please contact sales@mood-me.com to extend the license.");
            //    }
            //}
            //else
            //{
            //    Debug.Log("Lifetime license!");
            //}

        }

        void OnDestroy()
        {
            _emotionNN = null;
        }


        // Update is called once per frame
        void LateUpdate()
        {
            //If a Render Texture is provided in the VideoTexture (or just a still image), Webcam image will be ignored
            BufferProcessed = false;
            ValidData = false;
            if (!TestMode)
            {
                if (CameraManager.WebcamReady)
                {

                    NFramePassed = (NFramePassed + 1) % ProcessEveryNFrames;
                    if (NFramePassed == 0)
                    {

                        try
                        {
                            _emotionNN.ProcessFrame();
                            BufferProcessed = true;

                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.Message);
                            BufferProcessed = false;
                        }

                        if (BufferProcessed)
                        {
                            ValidData = !_emotionNN.DetectedEmotions.AllZero;                        
                            if (!(_emotionNN.DetectedEmotions.AllZero && FilterAllZeros))
                            {
                                CurrentEmotions = _emotionNN.DetectedEmotions;
                                Emotions = Filter(Emotions, CurrentEmotions, Frequency, MinCutOff);
                                //Debug.Log("angry " + Emotions.angry);
                                //Debug.Log("disgust " + Emotions.disgust);
                                //Debug.Log("happy " + Emotions.happy);
                                //Debug.Log("neutral " + Emotions.neutral);
                                //Debug.Log("sad " + Emotions.sad);
                                //Debug.Log("scared " + Emotions.scared);
                                //Debug.Log("surprised " + Emotions.surprised);
                                Angry = Emotions.angry;
                                Disgust = Emotions.disgust;
                                Happy = Emotions.happy;
                                Neutral = Emotions.neutral;
                                Sad = Emotions.sad;
                                Scared = Emotions.scared;
                                Surprised = Emotions.surprised;
                            }
                            else
                            {
                                ValidData = false;
                                BufferProcessed = false;
                            }										 

                        }
                        else
                        {
                            Emotions.Error = true;
                        }                    
                    }
                }

            }
            else
            {
                Emotions.angry = Angry;
                Emotions.disgust = Disgust;
                Emotions.happy = Happy;
                Emotions.neutral = Neutral;
                Emotions.sad = Sad;
                Emotions.scared = Scared;
                Emotions.surprised = Surprised;
            }
            EmotionIndex = (((3f * Happy + Surprised - (Sad + Scared + Disgust + Angry)) / 3f) + 1f) / 2f;

            angryFilter.UpdateParams(Frequency, MinCutOff);
            disgustFilter.UpdateParams(Frequency, MinCutOff);
            happyFilter.UpdateParams(Frequency, MinCutOff);
            neutralFilter.UpdateParams(Frequency, MinCutOff);
            sadFilter.UpdateParams(Frequency, MinCutOff);
            scaredFilter.UpdateParams(Frequency, MinCutOff);
            surprisedFilter.UpdateParams(Frequency, MinCutOff);

        }

        // Smoothing function
        MoodMeEmotions.MDMEmotions Filter(MoodMeEmotions.MDMEmotions target, MoodMeEmotions.MDMEmotions source, float frequency, float mincutoff)
        {
            target.angry = angryFilter.Filter(source.angry);
            target.disgust = disgustFilter.Filter(source.disgust);
            target.happy = happyFilter.Filter(source.happy);
            target.neutral = neutralFilter.Filter(source.neutral);
            target.sad = sadFilter.Filter(source.sad);
            target.scared = scaredFilter.Filter(source.scared);
            target.surprised = surprisedFilter.Filter(source.surprised);

            return target;
        }
    }

}