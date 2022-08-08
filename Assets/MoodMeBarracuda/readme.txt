MoodMe Emotions Unity Plugin V1.0 (c) MoodMe 2021

To run this plugin you need to install these packages, one of them (Barracuda) is still in the preview state:
. BURST Compiler version >= 1.3.4
. BARRACUDA version = 1.3.2 
	To install: Open the Package Manager, click on the Add button (+) then select Add Package from git URL and enter this URL https://github.com/Unity-Technologies/barracuda-release.git

. TEXTMESHPRO >= 3.0.4 This is usually installed with Unity 2020 but if you have removed it in your project then you have to install again to run the demo scene. This package is not mandatory to use the plugin in your scenes.


In Project Settings check this scripts order:

. MoodMe.CameraManager
. MoodMe.EmotionsManager
. MoodMe.FaceDetector
. MoodMe.ManageEmotionsNetwork
. MoodMe.GetEmotionValue
. MoodMe.MorphController

This plugin works only on landscape mode (if runs on mobile). Next updates will enable portrait mode.

The plugin is released AS IS with support for plugin errors only, but if you have any question, please write to support@mood-me.com and we will try to help you.






