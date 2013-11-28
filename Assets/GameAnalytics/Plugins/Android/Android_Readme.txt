How to enabled android ID for Android:

1) Change the com.yourcompany.yourgame bundle identifier to reflect your own company/game in both the AndroidManifest.xml file, and the GA_Android.java file. These files can be found in the GameAnalytics > Plugins > Android folder.

2) Move the AndroidManifest.xml file from the GameAnalytics > Plugins > Android folder to a new folder called Plugins > Android in your Assets folder. This will cause Unity to include the file in your compiled project when you build for Android. NOTE: If you already have your own AndroidManifest.xml you can simply combine the two, making sure that you have the correct permissions from the GA AndroidManifest.xml, and that you include the GA_Android activity (as in the GA AndroidManifest.xml).

3) Compiled the GA_Android.java file into a .jar file called GA_Android.jar, and place the GA_Android.jar file in the Plugins > Android folder in your Assets folder.

Hint: You can compile a .jar file using f.x. Eclipse or the Terminal/commandprompt. You will need the android SDK installed. These commands entered in the Terminal should create a working GA_Android.jar file for you:

	a) javac GA_Android.java -classpath C:\Program Files (x86)\Unity\Editor\Data\PlaybackEngines\androidplayer\bin\classes.jar -bootclasspath C:\android-sdk-windows\platforms\android-8\android.jar -d .

	b) javap -s com.yourcompany.yourgamename.GA_Android

	c) jar cvfM ../GA_Android.jar com/

In a) the -classpath corresponds to your classes.jar Unity file, and the -bootclasspath corresponds to the android.jar file in your android sdk. In b) you have to input you company name and game name in com.yourcompany.yourgamename.GA_Android (as in your bundle id).

4) In the GA_Settings class file (found at GameAnalytics > Plugins > Framework > Scripts > GA_Settings) uncomment the following code at the top:

		//#define ANDROID_ID

.. and at the top of the GA_SETTINGS class file change the value of ANDROID_CLASS_NAME to your own bundle id instead of "com.yourcompany.yourgame".