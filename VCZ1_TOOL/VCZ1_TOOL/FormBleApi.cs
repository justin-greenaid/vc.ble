using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.ObjectModel;


namespace VCZ1_TOOL
{

    public partial class FormZ1 : Form
    {

        /// <summary>
        /// click Scan button, it will scan ble device with ble name in text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async Task<int> Z1_PAIR_SN(int index, string serial_number)
        {
            //--- parameters : VC_Z1_XXXX
            // string parameters = textBox1.Text.ToString();
            if (index < 0 || index >= MAX_NUM_SN)
                return -1;

            string parameters = "VC Z1 " + serial_number.Substring(serial_number.Length - 4);
            var result =ble[index].StartScan(parameters, (d) => listDebug.Items.Insert(0, d));

            listDebug.Items.Insert(0, "[After Scan:" +gOp.SN[gOp.curIndex] +"]" + result.ToString());
            if (result.Equals(ERROR_CODE.BLE_FOUND_DEVICE))
            {
                var error_code = await ble[index].OpenDevice(parameters);
                listDebug.Items.Insert(0, $"    Connection Result: {error_code}");
                return 1;
            }
            else
            {
                return -1;
            }
        }

        private void Z1_UNPAIR_SN()
        {

        }

        private bool Z1_IsConnected(string strSN)
        {
            return true;
        }

        private void Z1_SCAN_ALL()
        {
            ble[0].StartScan();
        }

        private void Z1_LIST_DEVICES()
        {
            string s1, s2;
            int i, j, k;

            var result = ble[0].GetDeviceList();
            gOp.numFoundSN = result.Count();
            for (i = 0; i < result.Count(); i++)
            {
                gOp.ScannedSN[i] = result[i].ToString();
                listDebug.Items.Add($"{result[i]}");
            }

            //--- Check if SN is in ScannedSN;
            for (i = 0; i < MAX_NUM_SN; i++)
            {
                gOp.ValidDevice[i] = 0;
                if (gOp.SN[i].Length < 4)
                    continue;

                s1 = gOp.SN[i].Substring(gOp.SN[i].Length - 4);
                for (k = 0; k < gOp.numFoundSN; k++)
                {
                    if (gOp.ScannedSN[k].Length >= 4)
                    {
                        s2 = gOp.ScannedSN[k].Substring(gOp.ScannedSN[k].Length - 4);
                        if (s1.Equals(s2))
                        {
                            gOp.ValidDevice[i] = 1;
                            break;
                        }
                    }
                }
            }
        }
        
        private bool Z1_Is_SN_Valid(int ix)
        {
            if (gOp.ValidDevice[ix] == 1)
                return true;

            return false;
        }

        private void Z1_SET_CONFIG()
        {

        }

        private async Task<int> Z1_GET_DATA(double[] values)
        {
            //--- check if SN is connected
            if (Z1_IsConnected(gOp.SN[gOp.curIndex]) == false)
            {
                
            }

            string[] srVals = { "0", "0", "0" };
            string srData;

            if (gOp.curIndex < 0 || gOp.curIndex >= MAX_NUM_SN)
                return -1;

            // 온도
            string characteristic_name = "EnvironmentalSensing/Temperature";
            string dev_name = "VC Z1 " + gOp.SN[gOp.curIndex].Substring(gOp.SN[gOp.curIndex].Length - 4);

            //listDebug.Items.Insert(0, $"set {characteristic_name}");
            var error_code = await ble[gOp.curIndex].ReadCharacteristic(dev_name, characteristic_name);
            if (error_code == ERROR_CODE.NONE)
            {
                //listDebug.Items.Insert(0, $"{characteristic_name}: {ble.getCharacteristic()}");
                srData = ble[gOp.curIndex].getCharacteristic();
                srVals = srData.Split(' ');
                values[0] = int.Parse(srVals[1]) * 256 + int.Parse(srVals[0]);
                values[0] = values[0] / 100.0;
                listDebug.Items.Insert(0, gOp.SN[gOp.curIndex] + "(온도):" + ble[gOp.curIndex].getCharacteristic() + "==>" + values[0].ToString());

                //values[0] = double.Parse(ble.getCharacteristic());
            } else
            {
                Task<int> ret = Z1_PAIR_SN(gOp.curIndex, gOp.SN[gOp.curIndex]);
                int result = await ret;
                listDebug.Items.Insert(0, "ERROR: TEMP");
                read_complete = -2;
                return read_complete;
            }

            // 습도
            characteristic_name = "EnvironmentalSensing/Humidity";
            //listDebug.Items.Insert(0, $"set {characteristic_name}");
            error_code = await ble[gOp.curIndex].ReadCharacteristic(dev_name, characteristic_name);
            if (error_code == ERROR_CODE.NONE)
            {
                //listDebug.Items.Insert(0, $"{characteristic_name}: {ble.getCharacteristic()}");
                srData = ble[gOp.curIndex].getCharacteristic();
                srVals = srData.Split(' ');
                values[1] = int.Parse(srVals[1]) * 256 + int.Parse(srVals[0]);
                values[1] = values[1] / 100.0;
                listDebug.Items.Insert(0, gOp.SN[gOp.curIndex] + "(습도):" + ble[gOp.curIndex].getCharacteristic() + "==>" + values[1].ToString());

                Set_Connection_Status(true);
            }
            else
            {
                Task<int> ret = Z1_PAIR_SN(gOp.curIndex, gOp.SN[gOp.curIndex]);
                int result = await ret;
                listDebug.Items.Insert(0, "ERROR: HUMI");
                read_complete = -2;
                Set_Connection_Status(false);
                return read_complete;
            }

            // TVOC
            characteristic_name = "EnvironmentalSensing/TVOC";
            //listDebug.Items.Insert(0, $"set {characteristic_name}");
            error_code = await ble[gOp.curIndex].ReadCharacteristic(dev_name, characteristic_name);
            if (error_code == ERROR_CODE.NONE)
            {
                //listDebug.Items.Insert(0, $"{characteristic_name}: {ble.getCharacteristic()}");
                srData = ble[gOp.curIndex].getCharacteristic();
                srVals = srData.Split(' ');
                values[2] = int.Parse(srVals[1]) * 256 + int.Parse(srVals[0]);
                values[2] = values[2] / 100.0;
                listDebug.Items.Insert(0, gOp.SN[gOp.curIndex] + "(TVOC):" + ble[gOp.curIndex].getCharacteristic() + "==>" + values[2].ToString());
                Set_Connection_Status(true);
            }
            else
            {
                Task<int> ret = Z1_PAIR_SN(gOp.curIndex, gOp.SN[gOp.curIndex]);
                int result = await ret;
                listDebug.Items.Insert(0, "ERROR: TVOC");
                read_complete = -2;
                Set_Connection_Status(false);
                return read_complete;
            }

            // FANSPEED
            characteristic_name = "VCService/FanSpeed";
            //listDebug.Items.Insert(0, $"set {characteristic_name}");
            error_code = await ble[gOp.curIndex].ReadCharacteristic(dev_name, characteristic_name);
            if (error_code == ERROR_CODE.NONE)
            {
                //listDebug.Items.Insert(0, $"{characteristic_name}: {ble.getCharacteristic()}");
                srData = ble[gOp.curIndex].getCharacteristic();
                srVals = srData.Split(' ');
                values[3] = int.Parse(srVals[0]);
                listDebug.Items.Insert(0, gOp.SN[gOp.curIndex] + "(FANS):" + ble[gOp.curIndex].getCharacteristic() + "==>" + values[3].ToString());
                Set_Connection_Status(true);
            }
            else
            {
                Task<int> ret = Z1_PAIR_SN(gOp.curIndex, gOp.SN[gOp.curIndex]);
                int result = await ret;
                listDebug.Items.Insert(0, "ERROR: FANS");
                read_complete = -2;
                Set_Connection_Status(false);
                return read_complete;
            }

            // BATTERY
            characteristic_name = "Battery/BatteryLevel";
            //listDebug.Items.Insert(0, $"set {characteristic_name}");
            error_code = await ble[gOp.curIndex].ReadCharacteristic(dev_name, characteristic_name);
            if (error_code == ERROR_CODE.NONE)
            {
                //listDebug.Items.Insert(0, $"{characteristic_name}: {ble.getCharacteristic()}");
                srData = ble[gOp.curIndex].getCharacteristic();
                srVals = srData.Split(' ');
                values[4] = int.Parse(srVals[0]);
                listDebug.Items.Insert(0, gOp.SN[gOp.curIndex] + "(BATT):" + ble[gOp.curIndex].getCharacteristic() + "==>" + values[4].ToString());
                Set_Connection_Status(true);
            }
            else
            {
                Task<int> ret = Z1_PAIR_SN(gOp.curIndex, gOp.SN[gOp.curIndex]);
                int result = await ret;
                listDebug.Items.Insert(0, "ERROR: BATT");
                read_complete = -2;
                Set_Connection_Status(false);
                return read_complete;
            }

            Random _random = new Random();
            /*
            values[0] = _random.Next(1000)/10.0; // 온도
            values[1] = _random.Next(1000)/10.0; // 습도
            values[2] = _random.Next(100); // TVOC
            values[3] = _random.Next(5000)/10.0; // fan speed
            values[4] = _random.Next(1000)/10.0; // battery
            */
            read_complete = 10;
            return read_complete;
        }
    }
}
