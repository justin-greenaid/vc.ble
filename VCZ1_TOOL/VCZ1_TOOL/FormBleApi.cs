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

            listDebug.Items.Insert(0, "[After Scan:" +gOp.SN[index] +"]" + result.ToString());
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
#if _SCAN_FIRST
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
#else
            int i;

            for (i = 0; i < MAX_NUM_SN; i++)
            {
                gOp.ValidDevice[i] = 0;
                if (gOp.SN[i].Length < 4)
                    continue;
                gOp.ValidDevice[i] = 1;
            }
#endif
        }
   
        private bool Z1_Is_SN_Valid(int ix)
        {
            if (gOp.ValidDevice[ix] == 1)
                return true;

            return false;
        }

        public ERROR_CODE Z1_GetDeviceConnectionStatus(int index, string device_name)
        {
            var result = ERROR_CODE.NONE;
            result = ble[index].ConnnectionStatus(device_name);
            return result;
        }

        private async Task<int> Z1_GET_DATA(int index, double[] values)
        {
#if _BLOCKED_BY_JUSTIN_
            //--- check if SN is connected
            if (Z1_GetDeviceConnectionStatus(index, "VC Z1 " + gOp.SN[index].Substring(gOp.SN[index].Length - 4)) == ERROR_CODE.BLE_NO_CONNECTED)
            {
                Task<int> ret = Z1_PAIR_SN(index, gOp.SN[index]);
                int result = await ret;
                if (result > 0)
                {
                    listDebug.Items.Insert(0, index.ToString() + " @@@ Reconnected");
                    Set_Connection_Status(index, true);
                }
                else
                {
                    listDebug.Items.Insert(0, index.ToString() + " ### Connection Error");
                    read_complete = -2;
                    Set_Connection_Status(index, false);
                }
                Set_Connection_Status(index, false);
                return read_complete;
            }

            string[] srVals = { "0", "0", "0" };
            string srData;

            if (index < 0 || index >= MAX_NUM_SN)
                return -1;

            read_complete = 1;
            // 온도
            string characteristic_name = "EnvironmentalSensing/Temperature";
            string dev_name = "VC Z1 " + gOp.SN[index].Substring(gOp.SN[index].Length - 4);

            //listDebug.Items.Insert(0, $"set {characteristic_name}");
            ERROR_CODE error_code = await ble[index].ReadCharacteristic(dev_name, characteristic_name);
            if (error_code == ERROR_CODE.NONE)
            {
                //listDebug.Items.Insert(0, $"{characteristic_name}: {ble.getCharacteristic()}");
                srData = ble[index].getCharacteristic();
                srVals = srData.Split(' ');
                values[0] = int.Parse(srVals[1]) * 256 + int.Parse(srVals[0]);
                values[0] = values[0] / 100.0;
                listDebug.Items.Insert(0, gOp.SN[index] + "(온도):" + ble[index].getCharacteristic() + "==>" + values[0].ToString());

                //values[0] = double.Parse(ble.getCharacteristic());
            } else
            {
                Task<int> ret = Z1_PAIR_SN(index, gOp.SN[index]);
                int result = await ret;
                listDebug.Items.Insert(0, index.ToString() + " ### ERROR: TEMP");
                read_complete = -2;
                return read_complete;
            }

            // 습도
            characteristic_name = "EnvironmentalSensing/Humidity";
            //listDebug.Items.Insert(0, $"set {characteristic_name}");
            error_code = await ble[index].ReadCharacteristic(dev_name, characteristic_name);
            if (error_code == ERROR_CODE.NONE)
            {
                //listDebug.Items.Insert(0, $"{characteristic_name}: {ble.getCharacteristic()}");
                srData = ble[index].getCharacteristic();
                srVals = srData.Split(' ');
                values[1] = int.Parse(srVals[1]) * 256 + int.Parse(srVals[0]);
                values[1] = values[1] / 100.0;
                listDebug.Items.Insert(0, gOp.SN[index] + "(습도):" + ble[index].getCharacteristic() + "==>" + values[1].ToString());

                Set_Connection_Status(index, true);
            }
            else
            {
                Task<int> ret = Z1_PAIR_SN(index, gOp.SN[index]);
                int result = await ret;
                listDebug.Items.Insert(0, index.ToString() + " ### ERROR: HUMI");
                read_complete = -2;
                Set_Connection_Status(index, false);
                return read_complete;
            }

            // TVOC
            characteristic_name = "EnvironmentalSensing/TVOC";
            //listDebug.Items.Insert(0, $"set {characteristic_name}");
            error_code = await ble[index].ReadCharacteristic(dev_name, characteristic_name);
            if (error_code == ERROR_CODE.NONE)
            {
                //listDebug.Items.Insert(0, $"{characteristic_name}: {ble.getCharacteristic()}");
                srData = ble[index].getCharacteristic();
                srVals = srData.Split(' ');
                values[2] = int.Parse(srVals[1]) * 256 + int.Parse(srVals[0]);
                values[2] = values[2] / 100.0;
                listDebug.Items.Insert(0, gOp.SN[index] + "(TVOC):" + ble[index].getCharacteristic() + "==>" + values[2].ToString());
                Set_Connection_Status(index, true);
            }
            else
            {
                Task<int> ret = Z1_PAIR_SN(index, gOp.SN[index]);
                int result = await ret;
                listDebug.Items.Insert(0, index.ToString() + " ### ERROR: TVOC");
                read_complete = -2;
                Set_Connection_Status(index, false);
                return read_complete;
            }

            // FANSPEED
            characteristic_name = "VCService/FanSpeed";
            //listDebug.Items.Insert(0, $"set {characteristic_name}");
            error_code = await ble[index].ReadCharacteristic(dev_name, characteristic_name);
            if (error_code == ERROR_CODE.NONE)
            {
                //listDebug.Items.Insert(0, $"{characteristic_name}: {ble.getCharacteristic()}");
                srData = ble[index].getCharacteristic();
                srVals = srData.Split(' ');
                values[3] = int.Parse(srVals[0]);
                listDebug.Items.Insert(0, gOp.SN[index] + "(FANS):" + ble[index].getCharacteristic() + "==>" + values[3].ToString());
                Set_Connection_Status(index, true);
            }
            else
            {
                Task<int> ret = Z1_PAIR_SN(index, gOp.SN[index]);
                int result = await ret;
                listDebug.Items.Insert(0, index.ToString() + " ### ERROR: FANS");
                read_complete = -2;
                Set_Connection_Status(index, false);
                return read_complete;
            }

            // BATTERY
            characteristic_name = "Battery/BatteryLevel";
            //listDebug.Items.Insert(0, $"set {characteristic_name}");
            error_code = await ble[index].ReadCharacteristic(dev_name, characteristic_name);
            if (error_code == ERROR_CODE.NONE)
            {
                //listDebug.Items.Insert(0, $"{characteristic_name}: {ble.getCharacteristic()}");
                srData = ble[index].getCharacteristic();
                srVals = srData.Split(' ');
                values[4] = int.Parse(srVals[0]);
                listDebug.Items.Insert(0, gOp.SN[index] + "(BATT):" + ble[index].getCharacteristic() + "==>" + values[4].ToString());
                Set_Connection_Status(index, true);
            }
            else
            {
                Task<int> ret = Z1_PAIR_SN(index, gOp.SN[index]);
                int result = await ret;
                listDebug.Items.Insert(0, index.ToString() + " ### ERROR: BATT");
                read_complete = -2;
                Set_Connection_Status(index, false);
                return read_complete;
            }

            listDebug.Items.Insert(0, index.ToString() + " : Z1_GET_DATA_All data read");
#endif // _BLOCKED_BY_JUSTIN_

            return read_complete;
        }
    }
}
