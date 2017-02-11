using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.IO.Ports;
namespace HomeTrid
{
    public class Program
    {
       

       
        public static void Main()
        {
            HCSR04 mUS = new HCSR04(Pins.GPIO_PIN_D0, Pins.GPIO_PIN_D1);
            SensorReaderScheduler sensor = new SensorReaderScheduler(mUS);

            //buzzer sensors
            OutputPort rgbLedBlue = new OutputPort(Pins.GPIO_PIN_D2, false);
            OutputPort rgbLedRed = new OutputPort(Pins.GPIO_PIN_D3, false);
            OutputPort rgbLedGreen = new OutputPort(Pins.GPIO_PIN_D4, false);

            while (true)
            {
                bool result = sensor.ReadAll();
                
                if(!result)
                {
                    rgbLedRed.Write(true);

                    Debug.Print("Someone went into my room");
                    HTTPHandler.sendRequest();

                    rgbLedRed.Write(false);
                    rgbLedGreen.Write(true);
                    Thread.Sleep(800);
                    rgbLedGreen.Write(false);
              
                }

                Thread.Sleep(10);
            }

        }

    }
}
