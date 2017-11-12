using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Microsoft.VisualBasic.FileIO;
using System.Threading;

namespace SimulatedDevice
{
    class DeviceInfo
    {
        private String deviceId;
        private String deviceKey;
        private int direction;
        private int currentStep;
        private float currentLat;
        private float currentLong;
        private ExitPoint fromExit;
        private ExitPoint toExit;
        private float temperature;
        private DateTime eventDateTime;

        public String DeviceId
        {
            get
            {
                return deviceId;
            }

            set
            {
                deviceId = value;
            }
        }

        public int Direction
        {
            get
            {
                return direction;
            }

            set
            {
                direction = value;
            }
        }

        public int CurrentStep
        {
            get
            {
                return currentStep;
            }

            set
            {
                currentStep = value;
            }
        }

        public float CurrentLat
        {
            get
            {
                return currentLat;
            }

            set
            {
                currentLat = value;
            }
        }

        public float CurrentLong
        {
            get
            {
                return currentLong;
            }

            set
            {
                currentLong = value;
            }
        }

        internal ExitPoint FromExit
        {
            get
            {
                return fromExit;
            }

            set
            {
                fromExit = value;
            }
        }

        internal ExitPoint ToExit
        {
            get
            {
                return toExit;
            }

            set
            {
                toExit = value;
            }
        }

        public float Temperature
        {
            get
            {
                return temperature;
            }

            set
            {
                temperature = value;
            }
        }

        public DateTime EventDateTime
        {
            get
            {
                return eventDateTime;
            }

            set
            {
                eventDateTime = value;
            }
        }

        public string DeviceKey
        {
            get
            {
                return deviceKey;
            }

            set
            {
                deviceKey = value;
            }
        }

        public DeviceInfo(String deviceId, string deviceKey, int direction, int currentStep, float currentLat, float currentLong, ExitPoint fromExit, ExitPoint toExit, float temperature, DateTime eventDateTime)
        {
            this.deviceId = deviceId;
            this.deviceKey = deviceKey;
            this.direction = direction;
            this.currentStep = currentStep;
            this.currentLat = currentLat;
            this.currentLong = currentLong;
            this.fromExit = fromExit;
            this.toExit = toExit;
            this.temperature = temperature;
            this.eventDateTime = eventDateTime;
        }

        public void printMe()
        {
            Console.WriteLine("+++++++++++++++++++++");
            Console.WriteLine("deviceId: " + deviceId);
            Console.WriteLine("deviceKey: " + deviceKey);
            Console.WriteLine("direction (1 means from west to east): " + direction);
            Console.WriteLine("currentStep: " + currentStep);
            Console.WriteLine("currentLat: " + currentLat);
            Console.WriteLine("currentLong: " + currentLong);
            Console.WriteLine("temperature: " + temperature);
            Console.WriteLine("eventDateTime: " + eventDateTime);
            if (fromExit != null)
                fromExit.printMe();
            else
                Console.WriteLine("fromExit is null");
            if (toExit != null)
                toExit.printMe();
            else
                Console.WriteLine("toExit is null");
        }
    }

    class ExitPoint
    {
        private int exitNumber;
        private string townName;
        private float latitude;
        private float longitude;

        public int ExitNumber
        {
            get
            {
                return exitNumber;
            }

            set
            {
                exitNumber = value;
            }
        }

        public string TownName
        {
            get
            {
                return townName;
            }

            set
            {
                townName = value;
            }
        }

        public float Latitude
        {
            get
            {
                return latitude;
            }

            set
            {
                latitude = value;
            }
        }

        public float Longitude
        {
            get
            {
                return longitude;
            }

            set
            {
                longitude = value;
            }
        }

        public ExitPoint(int exitNumber, string townName, float latitude, float longitude)
        {
            this.exitNumber = exitNumber;
            this.townName = townName;
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public void printMe()
        {
            Console.WriteLine("exitNumber " + exitNumber + ": townName: " + townName + "., Lat=" + latitude + ", Long=" + longitude);
        }
    }
    class La2Ny
    {
        private List<ExitPoint> listExit = new List<ExitPoint>();

        public void Initialize()
        {
            using (TextFieldParser parser = new TextFieldParser("C:\\LAtoNY.csv"))
            {
                parser.HasFieldsEnclosedInQuotes = true;
                parser.TrimWhiteSpace = true;
                parser.Delimiters = new string[] { "," };
                int i = -1;
                while (!parser.EndOfData)
                {
                    try
                    {
                        i++;
                        string[] fieldRow = parser.ReadFields();
                        ExitPoint e = new ExitPoint(i,
                            fieldRow[0],
                            float.Parse(fieldRow[1]),
                            float.Parse(fieldRow[2])
                            );
                        listExit.Add(e);
                    }
                    catch (System.FormatException)
                    {
                        Console.WriteLine("{0} is not a good data point");
                    }
                }

                foreach (ExitPoint e in listExit)
                {
                    e.printMe();
                }

                Console.WriteLine("Total exit points = " + listExit.Count);
            }
        }

        public int getExitPointCount()
        {
            return listExit.Count;
        }

        public ExitPoint getExitPoint(int i)
        {
            if (i >= 0 && i < getExitPointCount())
                return listExit[i];
            else
                return null;
        }
    }

    class DeviceKeys
    {
        private List<KeyValuePair<string, string>> listDevice = new List<KeyValuePair<string, string>>();

        public void Initialize()
        {
            using (TextFieldParser parser = new TextFieldParser("C:\\Users\\xinxue\\Desktop\\Customer\\CMI\\sql16\\TelemetryDeviceIdKey.txt"))
            {
                parser.HasFieldsEnclosedInQuotes = true;
                parser.TrimWhiteSpace = true;
                parser.Delimiters = new string[] { "," };
                int i = -1;
                while (!parser.EndOfData)
                {
                    try
                    {
                        i++;
                        string[] fieldRow = parser.ReadFields();
                        listDevice.Add(new KeyValuePair<string, string>(fieldRow[0], fieldRow[1]));
                    }
                    catch (System.FormatException)
                    {
                        Console.WriteLine("{0} is not a good data point");
                    }
                }

                foreach (KeyValuePair<string, string> e in listDevice)
                {
                    printMe(e);
                }

                Console.WriteLine("Total number of devices = " + listDevice.Count);
            }
        }

        public void printMe(KeyValuePair<string, string> e)
        {
            Console.WriteLine(e.Key + ", " + e.Value);
        }

        public string getDeviceKey(string deviceId)
        {
            foreach (KeyValuePair<string, string> e in listDevice)
            {
                if (e.Key == deviceId)
                    return e.Value;
            }
            return null;
        }
    }

    class Program
    {
        static string iotHubUri = "iot.azure-devices.net";
        static La2Ny theRoute = new La2Ny();
        static DeviceKeys deviceKeys = new DeviceKeys();

        static void Main(string[] args)
        {
            deviceKeys.Initialize();
            theRoute.Initialize();
            //Console.ReadLine();

            Console.WriteLine("Before start Worker");
            ThreadStart testThread0Start = new ThreadStart(new Program().testThread0);
            ThreadStart testThread1Start = new ThreadStart(new Program().testThread1);
            ThreadStart testThread2Start = new ThreadStart(new Program().testThread2);
            ThreadStart testThread3Start = new ThreadStart(new Program().testThread3);
            ThreadStart testThread4Start = new ThreadStart(new Program().testThread4);
            ThreadStart testThread5Start = new ThreadStart(new Program().testThread5);
            ThreadStart testThread6Start = new ThreadStart(new Program().testThread6);
            ThreadStart testThread7Start = new ThreadStart(new Program().testThread7);
            ThreadStart testThread8Start = new ThreadStart(new Program().testThread8);
            ThreadStart testThread9Start = new ThreadStart(new Program().testThread9);
            ThreadStart testThread10Start = new ThreadStart(new Program().testThread10);
            ThreadStart testThread11Start = new ThreadStart(new Program().testThread11);
            ThreadStart testThread12Start = new ThreadStart(new Program().testThread12);
            ThreadStart testThread13Start = new ThreadStart(new Program().testThread13);
            ThreadStart testThread14Start = new ThreadStart(new Program().testThread14);
            ThreadStart testThread15Start = new ThreadStart(new Program().testThread15);
            ThreadStart testThread16Start = new ThreadStart(new Program().testThread16);
            ThreadStart testThread17Start = new ThreadStart(new Program().testThread17);
            ThreadStart testThread18Start = new ThreadStart(new Program().testThread18);
            ThreadStart testThread19Start = new ThreadStart(new Program().testThread19);
            ThreadStart testThread20Start = new ThreadStart(new Program().testThread20);
            ThreadStart testThread21Start = new ThreadStart(new Program().testThread21);
            ThreadStart testThread22Start = new ThreadStart(new Program().testThread22);
            ThreadStart testThread23Start = new ThreadStart(new Program().testThread23);
            ThreadStart testThread24Start = new ThreadStart(new Program().testThread24);
            ThreadStart testThread25Start = new ThreadStart(new Program().testThread25);
            ThreadStart testThread26Start = new ThreadStart(new Program().testThread26);
            ThreadStart testThread27Start = new ThreadStart(new Program().testThread27);
            ThreadStart testThread28Start = new ThreadStart(new Program().testThread28);
            ThreadStart testThread29Start = new ThreadStart(new Program().testThread29);
            ThreadStart testThread30Start = new ThreadStart(new Program().testThread30);
            ThreadStart testThread31Start = new ThreadStart(new Program().testThread31);
            ThreadStart testThread32Start = new ThreadStart(new Program().testThread32);
            ThreadStart testThread33Start = new ThreadStart(new Program().testThread33);
            ThreadStart testThread34Start = new ThreadStart(new Program().testThread34);
            ThreadStart testThread35Start = new ThreadStart(new Program().testThread35);
            ThreadStart testThread36Start = new ThreadStart(new Program().testThread36);
            ThreadStart testThread37Start = new ThreadStart(new Program().testThread37);
            ThreadStart testThread38Start = new ThreadStart(new Program().testThread38);
            ThreadStart testThread39Start = new ThreadStart(new Program().testThread39);
            ThreadStart testThread40Start = new ThreadStart(new Program().testThread40);
            ThreadStart testThread41Start = new ThreadStart(new Program().testThread41);
            ThreadStart testThread42Start = new ThreadStart(new Program().testThread42);
            ThreadStart testThread43Start = new ThreadStart(new Program().testThread43);
            ThreadStart testThread44Start = new ThreadStart(new Program().testThread44);
            ThreadStart testThread45Start = new ThreadStart(new Program().testThread45);
            ThreadStart testThread46Start = new ThreadStart(new Program().testThread46);
            ThreadStart testThread47Start = new ThreadStart(new Program().testThread47);
            ThreadStart testThread48Start = new ThreadStart(new Program().testThread48);
            ThreadStart testThread49Start = new ThreadStart(new Program().testThread49);
            ThreadStart testThread50Start = new ThreadStart(new Program().testThread50);
            ThreadStart testThread51Start = new ThreadStart(new Program().testThread51);
            ThreadStart testThread52Start = new ThreadStart(new Program().testThread52);
            ThreadStart testThread53Start = new ThreadStart(new Program().testThread53);
            ThreadStart testThread54Start = new ThreadStart(new Program().testThread54);
            ThreadStart testThread55Start = new ThreadStart(new Program().testThread55);
            ThreadStart testThread56Start = new ThreadStart(new Program().testThread56);
            ThreadStart testThread57Start = new ThreadStart(new Program().testThread57);
            ThreadStart testThread58Start = new ThreadStart(new Program().testThread58);
            ThreadStart testThread59Start = new ThreadStart(new Program().testThread59);
            ThreadStart testThread60Start = new ThreadStart(new Program().testThread60);
            ThreadStart testThread61Start = new ThreadStart(new Program().testThread61);
            ThreadStart testThread62Start = new ThreadStart(new Program().testThread62);
            ThreadStart testThread63Start = new ThreadStart(new Program().testThread63);

            Thread[] testThread = new Thread[64];
            testThread[0] = new Thread(testThread0Start);
            testThread[1] = new Thread(testThread1Start);
            testThread[2] = new Thread(testThread2Start);
            testThread[3] = new Thread(testThread3Start);
            testThread[4] = new Thread(testThread4Start);
            testThread[5] = new Thread(testThread5Start);
            testThread[6] = new Thread(testThread6Start);
            testThread[7] = new Thread(testThread7Start);
            testThread[8] = new Thread(testThread8Start);
            testThread[9] = new Thread(testThread9Start);
            testThread[10] = new Thread(testThread10Start);
            testThread[11] = new Thread(testThread11Start);
            testThread[12] = new Thread(testThread12Start);
            testThread[13] = new Thread(testThread13Start);
            testThread[14] = new Thread(testThread14Start);
            testThread[15] = new Thread(testThread15Start);
            testThread[16] = new Thread(testThread16Start);
            testThread[17] = new Thread(testThread17Start);
            testThread[18] = new Thread(testThread18Start);
            testThread[19] = new Thread(testThread19Start);
            testThread[20] = new Thread(testThread20Start);
            testThread[21] = new Thread(testThread21Start);
            testThread[22] = new Thread(testThread22Start);
            testThread[23] = new Thread(testThread23Start);
            testThread[24] = new Thread(testThread24Start);
            testThread[25] = new Thread(testThread25Start);
            testThread[26] = new Thread(testThread26Start);
            testThread[27] = new Thread(testThread27Start);
            testThread[28] = new Thread(testThread28Start);
            testThread[29] = new Thread(testThread29Start);
            testThread[30] = new Thread(testThread30Start);
            testThread[31] = new Thread(testThread31Start);
            testThread[32] = new Thread(testThread32Start);
            testThread[33] = new Thread(testThread33Start);
            testThread[34] = new Thread(testThread34Start);
            testThread[35] = new Thread(testThread35Start);
            testThread[36] = new Thread(testThread36Start);
            testThread[37] = new Thread(testThread37Start);
            testThread[38] = new Thread(testThread38Start);
            testThread[39] = new Thread(testThread39Start);
            testThread[40] = new Thread(testThread40Start);
            testThread[41] = new Thread(testThread41Start);
            testThread[42] = new Thread(testThread42Start);
            testThread[43] = new Thread(testThread43Start);
            testThread[44] = new Thread(testThread44Start);
            testThread[45] = new Thread(testThread45Start);
            testThread[46] = new Thread(testThread46Start);
            testThread[47] = new Thread(testThread47Start);
            testThread[48] = new Thread(testThread48Start);
            testThread[49] = new Thread(testThread49Start);
            testThread[50] = new Thread(testThread50Start);
            testThread[51] = new Thread(testThread51Start);
            testThread[52] = new Thread(testThread52Start);
            testThread[53] = new Thread(testThread53Start);
            testThread[54] = new Thread(testThread54Start);
            testThread[55] = new Thread(testThread55Start);
            testThread[56] = new Thread(testThread56Start);
            testThread[57] = new Thread(testThread57Start);
            testThread[58] = new Thread(testThread58Start);
            testThread[59] = new Thread(testThread59Start);
            testThread[60] = new Thread(testThread60Start);
            testThread[61] = new Thread(testThread61Start);
            testThread[62] = new Thread(testThread62Start);
            testThread[63] = new Thread(testThread63Start);

            foreach (Thread myThread in testThread)
            {
                myThread.Start();
                //myThread.Abort();
            }
            Console.WriteLine("End of Main");
            Console.ReadLine();
        }

        public void testThread0()
        {
            string deviceId = "Device0";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);

            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread1()
        {
            string deviceId = "Device1";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread2()
        {
            string deviceId = "Device2";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread3()
        {
            string deviceId = "Device3";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread4()
        {
            string deviceId = "Device4";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread5()
        {
            string deviceId = "Device5";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread6()
        {
            string deviceId = "Device6";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread7()
        {
            string deviceId = "Device7";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread8()
        {
            string deviceId = "Device8";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// ///////////////////////////////////////
        /// </summary>
        /// 
        public void testThread9()
        {
            string deviceId = "Device9";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread10()
        {
            string deviceId = "Device10";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread11()
        {
            string deviceId = "Device11";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread12()
        {
            string deviceId = "Device12";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread13()
        {
            string deviceId = "Device13";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread14()
        {
            string deviceId = "Device14";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread15()
        {
            string deviceId = "Device15";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread16()
        {
            string deviceId = "Device16";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }




        /// <summary>
        /// /
        /// </summary>
        /// 
        public void testThread17()
        {
            string deviceId = "Device17";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread18()
        {
            string deviceId = "Device18";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread19()
        {
            string deviceId = "Device19";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread20()
        {
            string deviceId = "Device20";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread21()
        {
            string deviceId = "Device21";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread22()
        {
            string deviceId = "Device22";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread23()
        {
            string deviceId = "Device23";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread24()
        {
            string deviceId = "Device24";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread25()
        {
            string deviceId = "Device25";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread26()
        {
            string deviceId = "Device26";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread27()
        {
            string deviceId = "Device27";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread28()
        {
            string deviceId = "Device28";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread29()
        {
            string deviceId = "Device29";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread30()
        {
            string deviceId = "Device30";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread31()
        {
            string deviceId = "Device31";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread32()
        {
            string deviceId = "Device32";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread33()
        {
            string deviceId = "Device33";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread34()
        {
            string deviceId = "Device34";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread35()
        {
            string deviceId = "Device35";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread36()
        {
            string deviceId = "Device36";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread37()
        {
            string deviceId = "Device37";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread38()
        {
            string deviceId = "Device38";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread39()
        {
            string deviceId = "Device39";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread40()
        {
            string deviceId = "Device40";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread41()
        {
            string deviceId = "Device41";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread42()
        {
            string deviceId = "Device42";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread43()
        {
            string deviceId = "Device43";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);

            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread44()
        {
            string deviceId = "Device44";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread45()
        {
            string deviceId = "Device45";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread46()
        {
            string deviceId = "Device46";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread47()
        {
            string deviceId = "Device47";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread48()
        {
            string deviceId = "Device48";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread49()
        {
            string deviceId = "Device49";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread50()
        {
            string deviceId = "Device50";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread51()
        {
            string deviceId = "Device51";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread52()
        {
            string deviceId = "Device52";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread53()
        {
            string deviceId = "Device53";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread54()
        {
            string deviceId = "Device54";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread55()
        {
            string deviceId = "Device55";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread56()
        {
            string deviceId = "Device56";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread57()
        {
            string deviceId = "Device57";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread58()
        {
            string deviceId = "Device58";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread59()
        {
            string deviceId = "Device59";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread60()
        {
            string deviceId = "Device60";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread61()
        {
            string deviceId = "Device61";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread62()
        {
            string deviceId = "Device62";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        public void testThread63()
        {
            string deviceId = "Device63";
            DeviceClient deviceClient;
            string deviceKey = deviceKeys.getDeviceKey(deviceId);
            DeviceInfo aDeviceInfo = initializeDeviceInfo(theRoute, deviceId, deviceKey);
            try
            {
                Console.WriteLine("Simulated device: " + deviceId);
                deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));

                SendDeviceToCloudMessagesAsync(deviceClient, theRoute, 1, aDeviceInfo);

                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }
            Console.WriteLine();
        }

        //Function to get random number
        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public static int GetRandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return getrandom.Next(min, max);
            }
        }

        private DeviceInfo initializeDeviceInfo(La2Ny theRoute, String deviceId, String deviceKey)
        {
            DeviceInfo aDevice = null;

            Random r = new Random();
            int direction = 0;
            if ((r.NextDouble() < 0.5) == true)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            int startExit = GetRandomNumber(0, theRoute.getExitPointCount() - 1);
            if (startExit == 0) //start from LA
            {
                ExitPoint fromExit = theRoute.getExitPoint(startExit);
                ExitPoint toExit = theRoute.getExitPoint(startExit + 1);
                aDevice = new DeviceInfo(deviceId, deviceKey, 1, 0, fromExit.Latitude, fromExit.Longitude, fromExit, toExit, GetRandomNumber(50, 200), DateTime.Now);
            }
            else if (startExit == theRoute.getExitPointCount() - 1) // start from NY
            {
                ExitPoint fromExit = theRoute.getExitPoint(startExit);
                ExitPoint toExit = theRoute.getExitPoint(startExit + 1);
                aDevice = new DeviceInfo(deviceId, deviceKey, -1, 0, fromExit.Latitude, fromExit.Longitude, fromExit, toExit, GetRandomNumber(50, 200), DateTime.Now);
            }
            else
            {
                ExitPoint fromExit = theRoute.getExitPoint(startExit);
                ExitPoint toExit = theRoute.getExitPoint(startExit - 1);
                aDevice = new DeviceInfo(deviceId, deviceKey, direction, 0, fromExit.Latitude, fromExit.Longitude, fromExit, toExit, GetRandomNumber(50, 200), DateTime.Now);
            }
            return aDevice;
        }

        private async void SendDeviceToCloudMessagesAsync(DeviceClient deviceClient, La2Ny theRoute, int stepNeeded, DeviceInfo aDevice)
        {
            try
            {
                while (true)
                {
                    DateTime now = DateTime.Now;

                    var telemetryDataPoint = new
                    {
                        DeviceId = aDevice.DeviceId,
                        EventDateTime = aDevice.EventDateTime,
                        Temperature = aDevice.Temperature,
                        Latitude = aDevice.CurrentLat,
                        Longitude = aDevice.CurrentLong,
                        Direction = aDevice.Direction
                    };
                    var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                    var message = new Message(Encoding.ASCII.GetBytes(messageString));

                    await deviceClient.SendEventAsync(message);
                    Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);
                    aDevice.printMe();

                    // next location
                    aDevice.EventDateTime = now;
                    aDevice.Temperature = GetRandomNumber(50, 200);
                    aDevice.CurrentStep = aDevice.CurrentStep + 1;
                    if (aDevice.CurrentStep == stepNeeded)
                    {
                        aDevice.CurrentStep = 0;
                        aDevice.CurrentLat = aDevice.ToExit.Latitude;
                        aDevice.CurrentLong = aDevice.ToExit.Longitude;
                        if (aDevice.ToExit.ExitNumber == 0)
                        {
                            aDevice.Direction = 1;
                            aDevice.FromExit = aDevice.ToExit;
                            aDevice.ToExit = theRoute.getExitPoint(1);
                        }
                        else if (aDevice.ToExit.ExitNumber == theRoute.getExitPointCount() - 1)
                        {
                            aDevice.Direction = -1;
                            aDevice.FromExit = aDevice.ToExit;
                            aDevice.ToExit = theRoute.getExitPoint(aDevice.ToExit.ExitNumber - 1);
                        }
                        else
                        {
                            aDevice.FromExit = aDevice.ToExit;
                            aDevice.ToExit = theRoute.getExitPoint(aDevice.ToExit.ExitNumber + aDevice.Direction);
                        }
                    }
                    else
                    {
                        float alpha = (float)aDevice.CurrentStep / (float)stepNeeded;
                        aDevice.CurrentLat = aDevice.FromExit.Latitude + alpha * (aDevice.ToExit.Latitude - aDevice.FromExit.Latitude);
                        aDevice.CurrentLong = aDevice.ToExit.Longitude + alpha * (aDevice.ToExit.Longitude - aDevice.FromExit.Longitude);
                    }

                    Task.Delay(100000).Wait();
                }
            }
            catch (ArgumentException e1)
            {
                Console.WriteLine("{0}: {1}", e1.GetType().Name, e1.Message);
            }
        }
    }
}