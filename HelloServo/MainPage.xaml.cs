using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Gpio;
using System.Diagnostics;
using Windows.Media.Capture;
using System.Windows.Input;
using Microsoft.IoT.Lightning.Providers;
using Windows.Storage;
using Windows.Media.Playback;

namespace HelloServo
{

    public sealed partial class MainPage : Page
    {
        /**
        * Each servo pin has a constant int declaring the 
        * location of the pin, the actual GPIO pin of the servo
        * pin, and a value containing the previous value of the pin.
        * This value is being used in conjunction with the slider bar
        * in order to successfully determine the direction of the servo
        * RIGHT NOW, which there is no other indication of.
        */

        private const int SERVO_PIN_A = 17;
        private const int SERVO_PIN_B = 27;
        private const int SERVO_PIN_C = 5;
        private const int SERVO_PIN_D = 6;

        private GpioPin servoPinA;
        private GpioPin servoPinB;
        private GpioPin servoPinC;
        private GpioPin servoPinD;

        private int servoPrev1 = 0;
        private int servoPrev2 = 0;
        private int servoPrev3 = 0;
        private int servoPrev4 = 0;

        // Stopwatch is used so I can time my PWM pauses
        Stopwatch stopwatch;


        // These are for the webcam.  
        MediaCaptureInitializationSettings captureInitSettings;
        List<Windows.Devices.Enumeration.DeviceInformation> deviceList;
        Windows.Media.MediaProperties.MediaEncodingProfile profile;
        Windows.Media.Capture.MediaCapture mediaCapture;
        GPIOout gpioOut;


        public MainPage()
        {
            GpioController gpioPin = GpioController.GetDefault();

            if (gpioPin == null)
            {
            }

            // Servo set up
            servoPinA = gpioPin.OpenPin(SERVO_PIN_A);
            servoPinA.SetDriveMode(GpioPinDriveMode.Output);

            servoPinB = gpioPin.OpenPin(SERVO_PIN_B);
            servoPinB.SetDriveMode(GpioPinDriveMode.Output);

            servoPinC = gpioPin.OpenPin(SERVO_PIN_C);
            servoPinC.SetDriveMode(GpioPinDriveMode.Output);

            servoPinD = gpioPin.OpenPin(SERVO_PIN_D);
            servoPinD.SetDriveMode(GpioPinDriveMode.Output);

            stopwatch = Stopwatch.StartNew();


            this.InitializeComponent();

            // Set up the DC motors
            gpioOut = new GPIOout();

            // Set up the camera
            EnumerateCameras();
        }


        /**
         * A snippet of code that I found on Stack Overflow.  Does not currently work, but it does compile.
         * 
         */
        private async void playSound()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/BUMMER.wav"));
            MediaPlayer player = BackgroundMediaPlayer.Current;
            player.Volume = 1;
            player.AutoPlay = false;
            player.SetFileSource(file);
            player.Play();
        }


        // Start the Video Capture
        private async void StartMediaCaptureSession()
        {
            // start the preview      
            capturePreview.Source = mediaCapture;
            await mediaCapture.StartPreviewAsync();
        }

        // Stop the video capture
        private async void StopMediaCaptureSession()
        {
            await mediaCapture.StopRecordAsync();
            //stop the preview
            await mediaCapture.StopPreviewAsync();
        }


        // Sets up the webcam for use.
        private async void EnumerateCameras()
        {
            var devices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(Windows.Devices.Enumeration.DeviceClass.VideoCapture);
            deviceList = new List<Windows.Devices.Enumeration.DeviceInformation>();

            if (devices.Count > 0)
            {
                for (var i = 0; i < devices.Count; i++)
                {
                    deviceList.Add(devices[i]);
                }

                InitCaptureSettings();
                InitMediaCapture();

            }
        }

        // Sets up the capture settings.  We want just a streaming video, we don't need to record anything.
        private void InitCaptureSettings()
        {
            // Set the Capture Setting
            captureInitSettings = null;
            captureInitSettings = new Windows.Media.Capture.MediaCaptureInitializationSettings();
            captureInitSettings.VideoDeviceId = "";
            captureInitSettings.StreamingCaptureMode = Windows.Media.Capture.StreamingCaptureMode.Video;

            if (deviceList.Count > 0)
            {
                captureInitSettings.VideoDeviceId = deviceList[0].Id;
            }

        }

        // Initializes the media capture function.
        private async void InitMediaCapture()
        {
            mediaCapture = null;
            mediaCapture = new Windows.Media.Capture.MediaCapture();


            await mediaCapture.InitializeAsync(captureInitSettings);

            // Add video stabilization effect during Live Capture
            Windows.Media.Effects.VideoEffectDefinition def = new Windows.Media.Effects.VideoEffectDefinition(Windows.Media.VideoEffects.VideoStabilization);

            await mediaCapture.AddVideoEffectAsync(def, MediaStreamType.VideoPreview);

            CreateProfile();
        }

        //Create a profile
        private void CreateProfile()
        {
            profile = Windows.Media.MediaProperties.MediaEncodingProfile.CreateMp4(Windows.Media.MediaProperties.VideoEncodingQuality.Ntsc);

            // Use MediaEncodingProfile to encode the profile
            System.Guid MFVideoRotationGuild = new System.Guid("C380465D-2271-428C-9B83-ECEA3B4A85C1");
            int MFVideoRotation = ConvertVideoRotationToMFRotation(VideoRotation.None);
            profile.Video.Properties.Add(MFVideoRotationGuild, PropertyValue.CreateInt32(MFVideoRotation));

            // add the mediaTranscoder 
            //var transcoder = new Windows.Media.Transcoding.MediaTranscoder();
            //transcoder.AddVideoEffect(Windows.Media.VideoEffects.VideoStabilization);
        }

        // Rotation of the camera
        int ConvertVideoRotationToMFRotation(VideoRotation rotation)
        {
            int MFVideoRotation = 0;
            switch (rotation)
            {
                case VideoRotation.Clockwise90Degrees:
                    MFVideoRotation = 90;
                    break;
                case VideoRotation.Clockwise180Degrees:
                    MFVideoRotation = 180;
                    break;
                case VideoRotation.Clockwise270Degrees:
                    MFVideoRotation = 270;
                    break;
            }
            return MFVideoRotation;
        }

        // Stops the capture session
        private void stopCapture(object sender, RoutedEventArgs e)
        {
            StopMediaCaptureSession();
        }

        // begins the capture session
        private void beginSession_Click(object sender, RoutedEventArgs e)
        {
            StartMediaCaptureSession();
        }

        /**
         * GPIO Button Click Events
         * 
         *  These click events have been simplified to simply turn on or off.
         *  Because DC motors simply require a pin value, this fits perfectly
         *  with our design.
         */

        // Forward
        private void button_Click(object sender, RoutedEventArgs e)
        {
            gpioOut.forward();
        }

        // Turn Right
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            gpioOut.turnRight();

        }

        // Turn Left
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            gpioOut.turnLeft();

        }

        // Reverse
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            gpioOut.reverse();

        }

        // Stop all pins
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            gpioOut.stop();
        }


        /**
         * SERVO PIN CHANGE VALUE
         * 
         * Each servo pin is attached to a slider bar.  This slider bar is set to rotate X amount, where X is where the
         * user tells it to go.  This is a pulse width modulator:
         * 
         * The user turns the pin on high.  1 ms wait time makes the unit turn clockwise.  2 ms wait time makes the 
         * unit turn counter-clockwise.  A 20 ms low wait time is required after every "high" cycle, allowing the servo
         * to move.  to continue moving, you simply cycle back and keep rotating the servo until satisfied.
         * 
         * In our case, the math is done by moving the slider bar and comparing the older values with the new one.  It then
         * determines which direction to go and makes its move.
         * 
         */
        private void leftShoulder_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {

            int toSend = (int)leftShoulder.Value - servoPrev1;
            servoPrev1 = (int)leftShoulder.Value;


            for (int i = 0; i < Math.Abs(toSend) / 2; i++)
            {

                if (toSend > 0)
                {
                    servoPinA.Write(GpioPinValue.High);
                    Wait(1);
                    servoPinA.Write(GpioPinValue.Low);
                    Wait(19);
                }
                else if (toSend < 0)
                {
                    servoPinA.Write(GpioPinValue.High);
                    Wait(2);
                    servoPinA.Write(GpioPinValue.Low);
                    Wait(18);
                }


            }



        }

        private void leftForearm_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {



            int toSend = (int)leftForearm.Value - servoPrev2;
            servoPrev2 = (int)leftForearm.Value;


            for (int i = 0; i < Math.Abs(toSend) / 2; i++)
            {

                if (toSend > 0)
                {
                    servoPinB.Write(GpioPinValue.High);
                    Wait(1);
                    servoPinB.Write(GpioPinValue.Low);
                    Wait(19);
                }
                else if (toSend < 0)
                {
                    servoPinB.Write(GpioPinValue.High);
                    Wait(2);
                    servoPinB.Write(GpioPinValue.Low);
                    Wait(18);
                }


            }


        }



        private void rightShoulder_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {





            int toSend = (int)rightShoulder.Value - servoPrev3;
            servoPrev3 = (int)rightShoulder.Value;


            for (int i = 0; i < Math.Abs(toSend) / 2; i++)
            {

                if (toSend > 0)
                {
                    servoPinC.Write(GpioPinValue.High);
                    Wait(1);
                    servoPinC.Write(GpioPinValue.Low);
                    Wait(19);
                }
                else if (toSend < 0)
                {
                    servoPinC.Write(GpioPinValue.High);
                    Wait(2);
                    servoPinC.Write(GpioPinValue.Low);
                    Wait(18);
                }


            }


        }

        private void rightForearm_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {


            int toSend = (int)rightForearm.Value - servoPrev4;
            servoPrev4 = (int)rightForearm.Value;


            for (int i = 0; i < Math.Abs(toSend) / 2; i++)
            {

                if (toSend > 0)
                {
                    servoPinD.Write(GpioPinValue.High);
                    Wait(1);
                    servoPinD.Write(GpioPinValue.Low);
                    Wait(19);
                }
                else if (toSend < 0)
                {
                    servoPinD.Write(GpioPinValue.High);
                    Wait(2);
                    servoPinD.Write(GpioPinValue.Low);
                    Wait(18);
                }


            }


        }



        /**
         * WAIT FUNCTION
         * 
         * Apparently you can't just set it to Thread.Sleep(), so this is necessary.  It is a simple
         * ticker event that counts down and shoves  you in a for loop.  I am planning on optimizing and
         * getting rid of this later.
         * 
         */
        private void Wait(double milliseconds)
        {
            long initialTick = stopwatch.ElapsedTicks;
            long initialElapsed = stopwatch.ElapsedMilliseconds;
            double desiredTicks = milliseconds / 1000.0 * Stopwatch.Frequency;
            double finalTick = initialTick + desiredTicks;
            while (stopwatch.ElapsedTicks < finalTick)
            {

            }
        }


    }
}
