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

namespace HelloServo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private const int SERVO_PIN_A = 17;
        private const int SERVO_PIN_B = 27;
        private const int SERVO_PIN_C = 22;
        private const int SERVO_PIN_D = 10;
        private const int SERVO_PIN_E = 9;
        private const int SERVO_PIN_F = 11;
        private GpioPin servoPinA;
        private GpioPin servoPinB;
        private GpioPin servoPinC;
        private GpioPin servoPinD;
        private GpioPin servoPinE;
        private GpioPin servoPinF;
        private DispatcherTimer timer;
        private double BEAT_PACE = 1000;
        private double CounterClockwiseDanceMove = 1;
        private double ClockwiseDanceMove = 2;
        private double currentDirection;
        private double PulseFrequency = 20;
        Stopwatch stopwatch;
        MediaCaptureInitializationSettings captureInitSettings;
        List<Windows.Devices.Enumeration.DeviceInformation> deviceList;
        Windows.Media.MediaProperties.MediaEncodingProfile profile;
        Windows.Media.Capture.MediaCapture mediaCapture;



        GPIOout gpioOut;
        public MainPage()
        {
            InitializeComponent();
            gpioOut = new GPIOout();
            EnumerateCameras();
            //StartMediaCaptureSession();
            //this.InitDancing();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            gpioOut.forward();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            gpioOut.turnRight();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            gpioOut.turnLeft();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            gpioOut.stop();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            gpioOut.reverse();
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {

        }



        private void InitDancing()
        {
            // Preparing our GPIO controller
            var gpio = GpioController.GetDefault();

            if (gpio == null)
            {
                servoPinA = null;
                if (GpioStatus != null)
                {
                    GpioStatus.Text = "No GPIO controller found";
                }

                return;
            }

            // Servo set up
            servoPinA = gpio.OpenPin(SERVO_PIN_A);
            servoPinA.SetDriveMode(GpioPinDriveMode.Output);

            servoPinB = gpio.OpenPin(SERVO_PIN_B);
            servoPinB.SetDriveMode(GpioPinDriveMode.Output);

            servoPinC = gpio.OpenPin(SERVO_PIN_C);
            servoPinC.SetDriveMode(GpioPinDriveMode.Output);

            servoPinD = gpio.OpenPin(SERVO_PIN_D);
            servoPinD.SetDriveMode(GpioPinDriveMode.Output);

            servoPinE = gpio.OpenPin(SERVO_PIN_E);
            servoPinE.SetDriveMode(GpioPinDriveMode.Output);

            servoPinF = gpio.OpenPin(SERVO_PIN_F);
            servoPinF.SetDriveMode(GpioPinDriveMode.Output);

            stopwatch = Stopwatch.StartNew();

            currentDirection = 0; // Initially we aren't dancing at all.

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(BEAT_PACE);
            timer.Tick += Beat;

            if (servoPinA != null && servoPinB != null)
            {
                timer.Start();
                Windows.System.Threading.ThreadPool.RunAsync(this.MotorThread, Windows.System.Threading.WorkItemPriority.High);
            }

            if (GpioStatus != null)
            {
                GpioStatus.Text = "GPIO pin ready";
            }
        }

        private void Beat(object sender, object e)
        {
            if (currentDirection != ClockwiseDanceMove)
            {
                currentDirection = ClockwiseDanceMove;
                GpioStatus.Text = "Yay!";
            }
            else
            {
                currentDirection = CounterClockwiseDanceMove;
                GpioStatus.Text = "I'm Dancing!!";
            }
        }

        private void MotorThread(IAsyncAction action)
        {
            while (true)
            {
                if (currentDirection != 0)
                {
                    servoPinA.Write(GpioPinValue.High);
                    servoPinB.Write(GpioPinValue.High);
                    servoPinC.Write(GpioPinValue.High);
                    servoPinD.Write(GpioPinValue.High);
                    servoPinE.Write(GpioPinValue.High);
                    servoPinF.Write(GpioPinValue.High);
                }

                Wait(currentDirection);

                servoPinA.Write(GpioPinValue.Low);
                servoPinB.Write(GpioPinValue.Low);
                servoPinC.Write(GpioPinValue.Low);
                servoPinD.Write(GpioPinValue.Low);
                servoPinE.Write(GpioPinValue.Low);
                servoPinF.Write(GpioPinValue.Low);
                Wait(PulseFrequency - currentDirection);
            }
        }

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









        // Start the Video Capture
        private async void StartMediaCaptureSession()
        {
            //var storageFile = await Windows.Storage.KnownFolders.VideosLibrary.CreateFileAsync("cameraCapture.wmv", Windows.Storage.CreationCollisionOption.GenerateUniqueName);
            //fileName = storageFile.Name;

            //await mediaCapture.StartRecordToStorageFileAsync(profile, storageFile);
            //recording = true;

            // start the preview      
            capturePreview.Source = mediaCapture;
            await mediaCapture.StartPreviewAsync();
        }

        // Stop the video capture
        private async void StopMediaCaptureSession()
        {
            await mediaCapture.StopRecordAsync();
            //recording = false;
            //(App.Current as App).IsRecording = false;

            //stop the preview
            await mediaCapture.StopPreviewAsync();


        }

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

        private void InitCaptureSettings()
        {
            // Set the Capture Setting
            captureInitSettings = null;
            captureInitSettings = new Windows.Media.Capture.MediaCaptureInitializationSettings();
            //captureInitSettings.AudioDeviceId = "";
            captureInitSettings.VideoDeviceId = "";
            captureInitSettings.StreamingCaptureMode = Windows.Media.Capture.StreamingCaptureMode.Video;
            //captureInitSettings.PhotoCaptureSource = Windows.Media.Capture.PhotoCaptureSource.VideoPreview;

            if (deviceList.Count > 0)
            {
                captureInitSettings.VideoDeviceId = deviceList[0].Id;
            }

        }

        private async void InitMediaCapture()
        {
            mediaCapture = null;
            mediaCapture = new Windows.Media.Capture.MediaCapture();

            // for dispose purpose
            //(App.Current as App).MediaCapture = mediaCapture;
            //(App.Current as App).PreviewElement = capturePreview;

            await mediaCapture.InitializeAsync(captureInitSettings);

            // Add video stabilization effect during Live Capture
            // await _mediaCapture.AddEffectAsync(MediaStreamType.VideoRecord, Windows.Media.VideoEffects.VideoStabilization, null); //this will be deprecated soon
            Windows.Media.Effects.VideoEffectDefinition def = new Windows.Media.Effects.VideoEffectDefinition(Windows.Media.VideoEffects.VideoStabilization);

            await mediaCapture.AddVideoEffectAsync(def, MediaStreamType.VideoPreview);

            CreateProfile();

            // start preview

            //capturePreview.Source = mediaCapture;

            //DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;

            //// set the video Rotation
            //  _mediaCapture.SetPreviewRotation(VideoRotation.Clockwise90Degrees);
            //    _mediaCapture.SetRecordRotation(VideoRotation.Clockwise90Degrees);
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

        private void stopCapture(object sender, RoutedEventArgs e)
        {
            StopMediaCaptureSession();
        }

        private void beginSession_Click(object sender, RoutedEventArgs e)
        {
            StartMediaCaptureSession();
        }

        // open the file in stream here














    }
}
