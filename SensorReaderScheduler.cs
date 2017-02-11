using System;
using Microsoft.SPOT;
using System.Threading;

namespace HomeTrid
{
    class SensorReaderScheduler
    {
        private const int MIN_TRIGGER = 700;
        private const int AVERAGE_SIZE = 6;
        private const int SUSPEND_TIME = 10000;

        private HCSR04 hcSensor;
        private bool isSuspended = false;

        public int last_hc_read { get; private set; }

        public SensorReaderScheduler(HCSR04 hcSensor)
        {
            this.hcSensor = hcSensor;
        }

        /**
         * This function reads all the input from all the sensors.
         * It will return true if there's nothing to update and false otherwise.
         */
        public bool ReadAll()
        {
            bool flag = true;

            if (!isSuspended)
            {
                ReadHCSensor();
                flag &= CheckHCSensor();
            }

            return flag;
        }



        private void ReadHCSensor()
        {
            int sum = 0;
            int count = 0;
            
            for(int i = 0; i < AVERAGE_SIZE; i++)
            {
                int read = (int)hcSensor.Ping();
                if(read != -1)
                {
                    sum += read;
                    count++;
                }
            }

            if (count == 0) count = 1;
            last_hc_read = sum / count;
        }

        
        /**
         * This function return true if there is no object close to the sensor and false otherwise.
         */
        public bool CheckHCSensor()
        {
            if(last_hc_read > MIN_TRIGGER || isSuspended)
            {
                return true;
            }
            else
            {
                suspend();
                return false;
            }
        }

        /**
         * This function suspends the HCSensor for a minute in case someone went into or left the room. 
         */
        private void suspend()
        {
            Thread aNewThread = new Thread(
                () =>
                    {
                        isSuspended = true;
                        Thread.Sleep(SUSPEND_TIME);
                        isSuspended = false;
                    }
               );
            aNewThread.Start();
        }

        
    }
}
