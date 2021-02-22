using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ble.multi
{
    public enum DeviceList : int
    {
        BLE1,
        BLE2,
        BLE3,
        BLE4,
    }
    public partial class Form1 : Form
    {
        string char_temp = "EnvironmentalSensing/Temperature";
        string char_humidity = "EnvironmentalSensing/Humidity";
        string char_TVOC = "EnvironmentalSensing/TVOC";
        string char_FanSpeed = "VCService/FanSpeed";
        string char_BatteryLevel = "Battery/BatteryLevel";
        string char_Co2 = "EnvironmentalSensing/Co2";

        Thread thread1 = null;
        Thread thread2 = null;
        Thread thread3 = null;
        Thread thread4 = null;

        Bleservice ble1 = null;
        Bleservice ble2 = null;
        Bleservice ble3 = null;
        Bleservice ble4 = null;

        bool thread1_run = false;
        bool thread2_run = false;
        bool thread3_run = false;
        bool thread4_run = false;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            ble1 = new Bleservice();
            ble2 = new Bleservice();
            ble3 = new Bleservice();
            ble4 = new Bleservice();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }


        private delegate void SetTextCallback(string text);
        private void WriteTextSafe1(string text)
        {
            if (this.listBle1.InvokeRequired)
            {
                var d = new SetTextCallback(WriteTextSafe1);
                Invoke(d, new object[] { text });
            }
            else
            {
                listBle1.Items.Add(text);
                listBle1.TopIndex = listBle1.Items.Count - 1;
            }
        }
        private void WriteTextSafe2(string text)
        {
            if (this.listBle2.InvokeRequired)
            {
                var d = new SetTextCallback(WriteTextSafe2);
                Invoke(d, new object[] { text });
            }
            else
            {
                listBle2.Items.Add(text);
                listBle2.TopIndex = listBle2.Items.Count - 1;
            }
        }
        private void WriteTextSafe3(string text)
        {
            if (this.listBle3.InvokeRequired)
            {
                var d = new SetTextCallback(WriteTextSafe3);
                Invoke(d, new object[] { text });
            }
            else
            {
                listBle3.Items.Add(text);
                listBle3.TopIndex = listBle3.Items.Count - 1;
            }
        }
        private void WriteTextSafe4(string text)
        {
            if (this.listBle1.InvokeRequired)
            {
                var d = new SetTextCallback(WriteTextSafe4);
                Invoke(d, new object[] { text });
            }
            else
            {
                listBle4.Items.Add(text);
                listBle4.TopIndex = listBle4.Items.Count - 1;
            }
        }

        private void showmessage(DeviceList idx, string msg)
        {
            switch (idx) {
                case DeviceList.BLE1: WriteTextSafe1($"{DateTime.Now}: {msg}"); break;
                case DeviceList.BLE2: WriteTextSafe2($"{DateTime.Now}: {msg}"); break;
                case DeviceList.BLE3: WriteTextSafe3($"{DateTime.Now}: {msg}"); break;
                case DeviceList.BLE4: WriteTextSafe4($"{DateTime.Now}: {msg}"); break;
            }
        }

        private void showmessage1(string msg)
        {
            showmessage(DeviceList.BLE1, msg);
        }
        private void showmessage2(string msg)
        {
            showmessage(DeviceList.BLE2, msg);
        }
        private void showmessage3(string msg)
        {
            showmessage(DeviceList.BLE3, msg);
        }
        private void showmessage4(string msg)
        {
            showmessage(DeviceList.BLE4, msg);
        }
        private async Task<ERROR_CODE> BleConnect(DeviceList idx, string devName)
        {
            ERROR_CODE result = ERROR_CODE.NONE;

            switch (idx)
            {
                case DeviceList.BLE1:
                    result = ble1.StartScan(devName, (d) => { });
                    showmessage1(result.ToString());
                    if (result.Equals(ERROR_CODE.BLE_FOUND_DEVICE))
                    {
                        result = await ble1.OpenDevice(devName);
                        showmessage1($"Connection Result: {result}");
                    };
                    break;
                case DeviceList.BLE2:
                    result = ble2.StartScan(devName, (d)=> { });
                    showmessage2(result.ToString());
                    if (result.Equals(ERROR_CODE.BLE_FOUND_DEVICE))
                    {
                        result = await ble2.OpenDevice(devName);
                        showmessage2($"Connection Result: {result}");
                    };
                    break;
                case DeviceList.BLE3:
                    result = ble3.StartScan(devName, (d) => { });
                    showmessage3(result.ToString());
                    if (result.Equals(ERROR_CODE.BLE_FOUND_DEVICE))
                    {
                        result = await ble3.OpenDevice(devName);
                        showmessage3($"Connection Result: {result}");
                    };
                    break;
                case DeviceList.BLE4:
                    result = ble4.StartScan(devName, (d) => { });
                    showmessage4(result.ToString());
                    if (result.Equals(ERROR_CODE.BLE_FOUND_DEVICE))
                    {
                        result = await ble4.OpenDevice(devName);
                        showmessage4($"Connection Result: {result}");
                    };
                    break;
            }
            return result;
        }

        string ConvertUnit(string item, string dataString)
        {
            string[] srVals = { "0", "0", "0", "0" };
            double resultSum = 0.0;
            switch (item) {
                case "Temperature":
                    srVals = dataString.Split(' ');
                    resultSum = int.Parse(srVals[1]) * 256 + int.Parse(srVals[0]);
                    resultSum = resultSum / 100.0;
                    break;
                case "Humidity":
                    srVals = dataString.Split(' ');
                    resultSum = int.Parse(srVals[1]) * 256 + int.Parse(srVals[0]);
                    resultSum = resultSum / 100.0;
                    break;
                case "TVOC":
                    srVals = dataString.Split(' ');
                    resultSum = int.Parse(srVals[1]) * 256 + int.Parse(srVals[0]);
                    break;
                case "FanSpeed":
                    srVals = dataString.Split(' ');
                    resultSum = int.Parse(srVals[1]) * 256 + int.Parse(srVals[0]);
                    break;
                    
                case "BatteryLevel":
                    resultSum = int.Parse(dataString);
                    break;

                case "Co2":
                    srVals = dataString.Split(' ');
                    resultSum = int.Parse(srVals[3]) * 16777215 + int.Parse(srVals[2]) * 655536 + int.Parse(srVals[1]) * 256 + int.Parse(srVals[0]);
                    
                    break;
            }
            return resultSum.ToString();
        }
        private async Task<ERROR_CODE> BleGetCharacteristic(DeviceList idx, string devName, string characterName)
        {
            ERROR_CODE result = ERROR_CODE.NONE;
            
            try
            {
                var parts = characterName.Split('/');
                switch (idx)
                {
                    case DeviceList.BLE1:
                        result = await ble1.ReadCharacteristic(devName, characterName);
                        if (result == ERROR_CODE.NONE)
                        {
                            var readChar = ble1.getCharacteristic();
                            var resultString = ConvertUnit(parts[1], readChar);
                            showmessage(idx, $"{parts[1]}: {readChar} => {resultString}");
                        }
                        else
                        {
                            showmessage(idx, $"{parts[1]}:ErrorCode: {result}");
                        }
                        break;
                    case DeviceList.BLE2:
                        result = await ble2.ReadCharacteristic(devName, characterName);
                        if (result == ERROR_CODE.NONE)
                        {
                            var readChar = ble2.getCharacteristic();
                            var resultString = ConvertUnit(parts[1], readChar);
                            showmessage(idx, $"{parts[1]}: {readChar} => {resultString}");
                        }
                        else
                        {
                            showmessage(idx, $"{parts[1]}:ErrorCode: {result}");
                        }
                        break;
                    case DeviceList.BLE3:
                        result = await ble3.ReadCharacteristic(devName, characterName);
                        if (result == ERROR_CODE.NONE)
                        {
                            var readChar = ble3.getCharacteristic();
                            var resultString = ConvertUnit(parts[1], readChar);
                            showmessage(idx, $"{parts[1]}: {readChar} => {resultString}");
                        }
                        else
                        {
                            showmessage(idx, $"{parts[1]}:ErrorCode: {result}");
                        }
                        break;
                    case DeviceList.BLE4:
                        result = await ble4.ReadCharacteristic(devName, characterName);
                        if (result == ERROR_CODE.NONE)
                        {
                            var readChar = ble4.getCharacteristic();
                            var resultString = ConvertUnit(parts[1], readChar);
                            showmessage(idx, $"{parts[1]}: {readChar} => {resultString}");
                        }
                        else
                        {
                            showmessage(idx, $"{parts[1]}:ErrorCode: {result}");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                result = ERROR_CODE.READ_EXCEPTION_1;
                Console.WriteLine($"Read Exception");
            }
            return result;
        }

        public ERROR_CODE GetDeviceConnectionStatus(DeviceList idx)
        {
            var result = ERROR_CODE.NONE;
            var device_name = "";
            switch (idx)
            {
                case DeviceList.BLE1:
                    device_name = GetDeviceNameFromList(idx);
                    result = ble1.ConnnectionStatus(device_name); 
                    break;
                case DeviceList.BLE2:
                    device_name = GetDeviceNameFromList(idx);
                    result = ble2.ConnnectionStatus(device_name); 
                    break;
                case DeviceList.BLE3:
                    device_name = GetDeviceNameFromList(idx);
                    result = ble3.ConnnectionStatus(device_name); 
                    break;
                case DeviceList.BLE4:
                    device_name = GetDeviceNameFromList(idx);
                    result = ble4.ConnnectionStatus(device_name); 
                    break;
                default:
                    device_name = "UnknownDevice";
                    result = ble1.ConnnectionStatus(device_name);
                    break;
            }
            return result;
        }
        public string GetDeviceNameFromList(DeviceList idx)
        {
            var device_name = "";
            switch (idx)
            {
                case DeviceList.BLE1: device_name = textBle1.Text; break;
                case DeviceList.BLE2: device_name = textBle2.Text; break;
                case DeviceList.BLE3: device_name = textBle3.Text; break;
                case DeviceList.BLE4: device_name = textBle4.Text; break;

            }
            return device_name;
        }

        public void SetThreadStatus(DeviceList idx, bool status)
        {
            switch (idx)
            {
                case DeviceList.BLE1: thread1_run = status; break;
                case DeviceList.BLE2: thread2_run = status; break;
                case DeviceList.BLE3: thread3_run = status; break;
                case DeviceList.BLE4: thread4_run = status; break;

            }
            return;
        }
        public bool GetThreadStatus(DeviceList idx)
        {
            bool result = false;
            switch (idx)
            {
                case DeviceList.BLE1: result = thread1_run; break;
                case DeviceList.BLE2: result = thread2_run; break;
                case DeviceList.BLE3: result = thread3_run; break;
                case DeviceList.BLE4: result = thread4_run; break;

            }
            return result;
        }
        public async  void ThreadRunxx(DeviceList idx)
        {

            var device_name = "";


            ERROR_CODE result = ERROR_CODE.NONE;
            
            while (GetThreadStatus(idx)) {
                
                if (GetDeviceConnectionStatus(idx) == ERROR_CODE.BLE_NO_CONNECTED)
                {
                    device_name = GetDeviceNameFromList(idx);
                    result = await BleConnect(idx, device_name);
                    if ((result != ERROR_CODE.NONE))
                    {
                        Task.Delay(9000);
                        continue;
                    }
                }
                result = await BleGetCharacteristic(idx, device_name, char_temp);
                if ((result == ERROR_CODE.BLE_NO_CONNECTED) || (result != ERROR_CODE.NONE)) 
                {
                    continue;
                }
                result = await BleGetCharacteristic(idx, device_name, char_humidity);
                if ((result == ERROR_CODE.BLE_NO_CONNECTED) || (result != ERROR_CODE.NONE))
                {
                    continue;
                }
                result = await BleGetCharacteristic(idx, device_name, char_TVOC);
                if ((result == ERROR_CODE.BLE_NO_CONNECTED) || (result != ERROR_CODE.NONE))
                {
                    continue;
                }
                result = await BleGetCharacteristic(idx, device_name, char_FanSpeed);
                if ((result == ERROR_CODE.BLE_NO_CONNECTED) || (result != ERROR_CODE.NONE))
                {
                    continue;
                }
                result = await BleGetCharacteristic(idx, device_name, char_BatteryLevel);
                if ((result == ERROR_CODE.BLE_NO_CONNECTED) || (result != ERROR_CODE.NONE))
                {
                    continue;
                }
                result = await BleGetCharacteristic(idx, device_name, char_Co2);
                if ((result == ERROR_CODE.BLE_NO_CONNECTED) || (result != ERROR_CODE.NONE))
                {
                    continue;
                }
                //await Task.Delay(1000);
            }
        }
        private void buttonRun_Click(object sender, EventArgs e)
        {

            if (checkBle1.Checked)
            {
                thread1 = new Thread(() => ThreadRunxx(DeviceList.BLE1));
                SetThreadStatus(DeviceList.BLE1, true);
                thread1.Start();
            }
            if (checkBle2.Checked)
            {
                thread2 = new Thread(() => ThreadRunxx(DeviceList.BLE2));
                SetThreadStatus(DeviceList.BLE2, true);
                thread2.Start();
            }
            if (checkBle3.Checked)
            {
                thread3 = new Thread(() => ThreadRunxx(DeviceList.BLE3));
                SetThreadStatus(DeviceList.BLE3, true);
                thread3.Start();
            }
            if (checkBle4.Checked)
            {
                thread4 = new Thread(() => ThreadRunxx(DeviceList.BLE4));
                SetThreadStatus(DeviceList.BLE4, true);
                thread4.Start();
            }
        }
        private async void buttonScan_Click(object sender, EventArgs e)
        {
            Bleservice ble0 = new Bleservice();
            listDevice.Items.Clear();
            listDevice.Items.Add("Waitting...");
            ble0.StartScan();
            await Task.Delay(Int32.Parse(textScanTime.Text)*1000);
            var result = ble0.GetDeviceList();
            listDevice.Items.Clear();
            for (int i = 0; i < result.Count(); i++)
            {
                listDevice.Items.Add($"{result[i]}");
            }
        }

        private void buttonBle1FromList_Click(object sender, EventArgs e)
        {
            string parameters = listDevice.SelectedItem.ToString();
            if (String.IsNullOrEmpty(parameters)) {
                return;
            }
            textBle1.Text = parameters;
            checkBle1.Checked = true;
        }

        private void buttonBle2FromList_Click(object sender, EventArgs e)
        {
            string parameters = listDevice.SelectedItem.ToString();
            if (String.IsNullOrEmpty(parameters))
            {
                return;
            }
            textBle2.Text = parameters;
            checkBle2.Checked = true;
        }

        private void buttonBle3FromList_Click(object sender, EventArgs e)
        {
            string parameters = listDevice.SelectedItem.ToString();
            if (String.IsNullOrEmpty(parameters))
            {
                return;
            }
            textBle3.Text = parameters;
            checkBle3.Checked = true;
        }

        private void buttonBle4FromList_Click(object sender, EventArgs e)
        {
            string parameters = listDevice.SelectedItem.ToString();
            if (String.IsNullOrEmpty(parameters))
            {
                return;
            }
            textBle4.Text = parameters;
            checkBle4.Checked = true;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            SetThreadStatus(DeviceList.BLE1, false);
            SetThreadStatus(DeviceList.BLE2, false);
            SetThreadStatus(DeviceList.BLE3, false);
            SetThreadStatus(DeviceList.BLE4, false);
            if (thread1 != null) 
            {
                thread1.Abort();
                thread1.Join();
                thread1 = null;
            } 
            if (thread2 != null)
            {
                thread2.Abort();
                thread2.Join();
                thread2 = null;
            }
            if (thread3 != null)
            {
                thread3.Abort();
                thread3.Join();
                thread2 = null;
            }
            if (thread3 != null)
            {
                thread3.Abort();
                thread3.Join();
                thread3 = null;
            }
            Task.Delay(1000);

        }
    }
}
