using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ble.test
{
    
    public partial class Form1 : Form
    {
        //static DeviceWatcher watcher = null;

        Bleservice ble;
        Bleservice ble2;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            ble = new Bleservice();
            ble2 = new Bleservice();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            listDevice.Items.Clear();
            ble.StartScan();
            listStatus.Items.Add("Start Scan");
        }

        // close device
        private void button4_Click(object sender, EventArgs e)
        {
            // Remove all subscriptions
            //if (_subscribers.Count > 0) Unsubscribe("all");
            ble.CloseDevice();

        }


        private async void RunTask(TaskName taskName, string arg1, string arg2, Action<ERROR_CODE> callback)
        {
            // 비동기로 Worker Thread에서 도는 task1
            // Task.Run(): .NET Framework 4.5+
            ERROR_CODE result= ERROR_CODE.NONE;

            switch (taskName)
            {
                case TaskName.OPEN_DEVICE:
                    result = await ble.OpenDevice(arg1);
                    listStatus.Items.Add($"ErrorCode: {result}");
                    break;

                case TaskName.SET_SERVICE:
                    //task1 = Task.Run(() => bleservice.SetService(deviceName));
                    //result = await task1;
                    result = await ble.SetService(arg2);
                    listStatus.Items.Add($"ErrorCode: {result}");
                    break;

                case TaskName.READ_CHARACTERISTIC:
                    var resultString = await ble.ReadCharacteristic(arg1, arg2);
                    listStatus.Items.Add($"{resultString}");              
                    break;

            }
            callback(result);
        }

        // Click List Device button
        private void button2_Click(object sender, EventArgs e)
        {
            
            listDevice.Items.Clear();
            var result = ble.GetDeviceList();
            for (int i = 0; i < result.Count(); i++)
            {
                listDevice.Items.Add($"{result[i]}");
            }
        }


        // Click Open Device button
        private void button3_Click(object sender, EventArgs e)
        {
            string parameters = listDevice.SelectedItem.ToString();
            // only allow for one connection to be open at a time
            listStatus.Items.Add(parameters);
            RunTask(TaskName.OPEN_DEVICE, parameters, null, (arg) => { } );  
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listService.Items.Clear();
            var result = ble.GetServiceList();
            for (int i = 0; i < result.Count(); i++)
            {
                listService.Items.Add($"{result[i]}");
            }
        }
        // click Set Service Button
        private void button6_Click(object sender, EventArgs e)
        {
            string devName = listDevice.SelectedItem.ToString();
            string parameters = listService.SelectedItem.ToString();
            // only allow for one connection to be open at a time
            listStatus.Items.Add(parameters);
            RunTask(TaskName.SET_SERVICE, devName, parameters, (result) => { });
            //button7_Click(sender, e);
        }

        // click Show Charateristics button
        private void button7_Click(object sender, EventArgs e)
        {
            listCharacteristic.Items.Clear();
            var result = ble.GetCharacteristicList();
            for (int i = 0; i < result.Count(); i++)
            {
                listCharacteristic.Items.Add($"{result[i]}");
            }
        }

        // click Read Characteristics button
        private void button8_Click(object sender, EventArgs e)
        {
            string devName = listDevice.SelectedItem.ToString();
            string parameters = listCharacteristic.SelectedItem.ToString();
            // only allow for one connection to be open at a time

            listStatus.Items.Add(parameters);
            RunTask(TaskName.READ_CHARACTERISTIC, devName, parameters, (arg) => { });
        }


        /// <summary>
        /// click Scan button, it will scan ble device with ble name in text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button8_Click_1(object sender, EventArgs e)
        {
            string parameters = textBox1.Text.ToString();
            listDevice.Items.Clear();
            listStatus.Items.Add("Start Scan");


            //var result = ble.StartScan(parameters, (d) => listStatus.Items.Add(d) );
            var result = ble.StartScan(parameters, (d) => {});
            listStatus.Items.Add(result.ToString());
            if (result.Equals(ERROR_CODE.BLE_FOUND_DEVICE))
            {
                var error_code = await ble.OpenDevice(parameters);
                listStatus.Items.Add($"Connection Result: {error_code}");
            }
        }

        private async void button9_Click(object sender, EventArgs e)
        {
            string dev_name = textBox1.Text.ToString();
            string characteristic_name = "Battery/BatteryLevel";
            listStatus.Items.Add($"set {characteristic_name}");
            var error_code = await ble.ReadCharacteristic(dev_name, characteristic_name);
            listStatus.Items.Add($"{error_code}");
        }

        /// <summary>
        /// click temperature button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button10_Click(object sender, EventArgs e)
        {
            string dev_name = textBox1.Text.ToString();
            string characteristic_name = "EnvironmentalSensing/Temperature";
            listStatus.Items.Add($"set {characteristic_name}");
            var error_code = await ble.ReadCharacteristic(dev_name, characteristic_name);
            listStatus.Items.Add($"{error_code}");
        }
        /// <summary>
        /// click Humidity button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button11_Click(object sender, EventArgs e)
        {
            string dev_name = textBox1.Text.ToString();
            string characteristic_name = "EnvironmentalSensing/Humidity";
            listStatus.Items.Add($"set {characteristic_name}");
            var error_code = await ble.ReadCharacteristic(dev_name, characteristic_name);
            listStatus.Items.Add($"{error_code}");
        }

        /// <summary>
        /// click TVOC button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button12_Click(object sender, EventArgs e)
        {
            string dev_name = textBox1.Text.ToString();
            string characteristic_name = "EnvironmentalSensing/TVOC";
            listStatus.Items.Add($"set {characteristic_name}");
            var error_code = await ble.ReadCharacteristic(dev_name, characteristic_name);
            listStatus.Items.Add($"{error_code}");
        }
        /// <summary>
        /// click FanSpeed button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button13_Click(object sender, EventArgs e)
        {
            string dev_name = textBox1.Text.ToString();
            string characteristic_name = "VCService/FanSpeed";
            listStatus.Items.Add($"set {characteristic_name}");
            var error_code = await ble.ReadCharacteristic(dev_name, characteristic_name);
            listStatus.Items.Add($"{error_code}");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            button8_Click_1(sender, e);
            button10_Click(sender, e); // temperature button
            button11_Click(sender, e); // humidity button
            button12_Click(sender, e); // TVOC button
            button13_Click(sender, e); //  FanSpeed button
            button9_Click(sender, e);  // battery button
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button16_Click_1(object sender, EventArgs e)
        {
            string parameters = textBox3.Text.ToString();
            listDevice.Items.Clear();
            listStatus.Items.Add("Start Scan");

            // var result = ble2.StartScan(parameters, (d) => listStatus.Items.Add(d));
            var result = ble2.StartScan(parameters, (d) => { });

            listStatus.Items.Add(result.ToString());
            if (result.Equals(ERROR_CODE.BLE_FOUND_DEVICE))
            {
                var error_code = await ble2.OpenDevice(parameters);
                listStatus.Items.Add($"Connection Result: {error_code}");
            }
        }

        private async void button17_Click(object sender, EventArgs e)
        {
            string parameters = textBox1.Text.ToString();
            listDevice.Items.Clear();
            listStatus.Items.Add("Start Scan");
            
            ble.CloseDevice();
            var result = ble.StartScan(parameters, (d) => listStatus.Items.Add(d));

            listStatus.Items.Add(result.ToString());

            var error_code = await ble.Pairing(parameters);
            listStatus.Items.Add($"result: {parameters}: {error_code} ");
            
        }

        private async void button18_Click(object sender, EventArgs e)
        {
            string parameters = textBox1.Text.ToString();
            listDevice.Items.Clear();
            listStatus.Items.Add("Start UnPair");

            var error_code = await ble.UnPairing(parameters);
            listStatus.Items.Add($"result: {parameters}: {error_code} ");
        }

        private async void button19_Click(object sender, EventArgs e)
        {
            var index = 0;
            for (; ;index++ ) {
                listStatus.Items.Add($"try index : {index}");
                button17_Click(sender, e);
                await Task.Delay(10000);
                button18_Click(sender, e);
                await Task.Delay(10000);
                ble.CloseDevice();
                listStatus.TopIndex = listStatus.Items.Count - 1;
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {

        }

        private async void button24_Click(object sender, EventArgs e)
        {
            string dev_name = textBox3.Text.ToString();
            string characteristic_name = "EnvironmentalSensing/Temperature";
            listStatus.Items.Add($"set {characteristic_name}");
            var error_code = await ble2.ReadCharacteristic(dev_name, characteristic_name);
            listStatus.Items.Add($"{error_code}");
        }

        private async void button23_Click(object sender, EventArgs e)
        {
            string dev_name = textBox3.Text.ToString();
            string characteristic_name = "EnvironmentalSensing/Humidity";
            listStatus.Items.Add($"set {characteristic_name}");
            var error_code = await ble2.ReadCharacteristic(dev_name, characteristic_name);
            listStatus.Items.Add($"{error_code}");
        }

        private async void button22_Click(object sender, EventArgs e)
        {
            string dev_name = textBox3.Text.ToString();
            string characteristic_name = "EnvironmentalSensing/TVOC";
            listStatus.Items.Add($"set {characteristic_name}");
            var error_code = await ble2.ReadCharacteristic(dev_name, characteristic_name);
            listStatus.Items.Add($"{error_code}");
        }

        private async void button21_Click(object sender, EventArgs e)
        {
            string dev_name = textBox3.Text.ToString();
            string characteristic_name = "VCService/FanSpeed";
            listStatus.Items.Add($"set {characteristic_name}");
            var error_code = await ble2.ReadCharacteristic(dev_name, characteristic_name);
            listStatus.Items.Add($"{error_code}");
        }

        private async void button25_Click(object sender, EventArgs e)
        {
            string dev_name = textBox3.Text.ToString();
            string characteristic_name = "Battery/BatteryLevel";
            listStatus.Items.Add($"set {characteristic_name}");
            var error_code = await ble2.ReadCharacteristic(dev_name, characteristic_name);
            listStatus.Items.Add($"{error_code}");
        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void button26_Click(object sender, EventArgs e)
        {
        }

        private async void button26_Click_1(object sender, EventArgs e)
        {
            string dev_name = textBox1.Text.ToString();
            string characteristic_name = "EnvironmentalSensing/Co2";
            listStatus.Items.Add($"set {characteristic_name}");
            var error_code = await ble.ReadCharacteristic(dev_name, characteristic_name);
            listStatus.Items.Add($"{error_code}");
        }

        private async void button27_Click(object sender, EventArgs e)
        {
            string dev_name = textBox3.Text.ToString();
            string characteristic_name = "EnvironmentalSensing/Co2";
            listStatus.Items.Add($"set {characteristic_name}");
            var error_code = await ble2.ReadCharacteristic(dev_name, characteristic_name);
            listStatus.Items.Add($"{error_code}");

        }
    }
}

