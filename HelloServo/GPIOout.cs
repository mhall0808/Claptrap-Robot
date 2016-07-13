using System;
using Windows.Devices.Gpio;


namespace HelloServo
{
    public class GPIOout
    {
        // Get the default GPIO controller on the system
        private GpioController gpio;

        private GpioPin pin_23;
        private GpioPin pin_24;
        private GpioPin pin_25;
        private GpioPin pin_8;

        public GPIOout()
        {
            gpio = GpioController.GetDefault();

            if (gpio == null)
                return; // GPIO not available on this system

            pin_23 = gpio.OpenPin(23);
            pin_24 = gpio.OpenPin(24);
            pin_25 = gpio.OpenPin(25);
            pin_8 = gpio.OpenPin(8);

            pin_23.SetDriveMode(GpioPinDriveMode.Output);
            pin_24.SetDriveMode(GpioPinDriveMode.Output);
            pin_25.SetDriveMode(GpioPinDriveMode.Output);
            pin_8.SetDriveMode(GpioPinDriveMode.Output);

            pin_23.Write(GpioPinValue.Low);
            pin_24.Write(GpioPinValue.Low);
            pin_25.Write(GpioPinValue.Low);
            pin_8.Write(GpioPinValue.Low);
        }


        public void turnLeft()
        {
            pin_23.Write(GpioPinValue.Low);
            pin_24.Write(GpioPinValue.Low);
            pin_25.Write(GpioPinValue.High);
            pin_8.Write(GpioPinValue.Low);
        }
        public void turnRight()
        {
            pin_23.Write(GpioPinValue.High);
            pin_24.Write(GpioPinValue.Low);
            pin_25.Write(GpioPinValue.Low);
            pin_8.Write(GpioPinValue.Low);
        }
        public void forward()
        {
            pin_23.Write(GpioPinValue.High);
            pin_24.Write(GpioPinValue.Low);
            pin_25.Write(GpioPinValue.High);
            pin_8.Write(GpioPinValue.Low);
        }
        public void reverse()
        {
            pin_23.Write(GpioPinValue.Low);
            pin_24.Write(GpioPinValue.High);
            pin_25.Write(GpioPinValue.Low);
            pin_8.Write(GpioPinValue.High);
        }
        public void reverseLeft()
        {
            pin_23.Write(GpioPinValue.Low);
            pin_24.Write(GpioPinValue.Low);
            pin_25.Write(GpioPinValue.Low);
            pin_8.Write(GpioPinValue.High);
        }
        public void reverseRight()
        {
            pin_23.Write(GpioPinValue.High);
            pin_24.Write(GpioPinValue.Low);
            pin_25.Write(GpioPinValue.High);
            pin_8.Write(GpioPinValue.Low);
        }
        public void stop()
        {
            pin_23.Write(GpioPinValue.Low);
            pin_24.Write(GpioPinValue.Low);
            pin_25.Write(GpioPinValue.Low);
            pin_8.Write(GpioPinValue.Low);
        }

    }

}
