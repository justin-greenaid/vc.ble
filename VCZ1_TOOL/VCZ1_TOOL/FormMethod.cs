using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace VCZ1_TOOL
{
    public partial class FormZ1 : Form
    {
        private void Read_Configuration()
        {
            int i;

            //--- init gCfg values
            gCfg.temp = new double[3];
            gCfg.humi = new double[3];
            gCfg.tvoc = new double[3];
            gCfg.fans = new double[3];
            gCfg.co2 = new double[3];

            // read configuration
            string strRet;
            string[] srVals =  { "1", "2", "3" };

            INIFile inif = new INIFile(strCheckFolder+"\\Z1CONFIG.ini");
            if (File.Exists(strCheckFolder + "\\Z1CONFIG.ini"))
            {
                strRet = inif.Read("Configuration", "temperature");
                srVals = strRet.Split(',');
                for (i = 0; i < 3; i++)
                    gCfg.temp[i] = double.Parse(srVals[i]);

                strRet = inif.Read("Configuration", "humidity");
                srVals = strRet.Split(',');
                for (i = 0; i < 3; i++)
                    gCfg.humi[i] = double.Parse(srVals[i]);

                strRet = inif.Read("Configuration", "tvoc");
                srVals = strRet.Split(',');
                for (i = 0; i < 3; i++)
                    gCfg.tvoc[i] = double.Parse(srVals[i]);

                strRet = inif.Read("Configuration", "fan_speed");
                srVals = strRet.Split(',');
                for (i = 0; i < 3; i++)
                    gCfg.fans[i] = double.Parse(srVals[i]);

                strRet = inif.Read("Configuration", "co2");
                srVals = strRet.Split(',');
                for (i = 0; i < 3; i++)
                    gCfg.co2[i] = double.Parse(srVals[i]);

                gCfg.duration = int.Parse(inif.Read("Configuration", "duration"));
                gCfg.read_freq = int.Parse(inif.Read("Configuration", "read_freq"));
                gCfg.log_method = int.Parse(inif.Read("Configuration", "log_method"));
                gCfg.numMaxDevice = int.Parse(inif.Read("Configuration", "num_max_devices"));
                gCfg.log_dir = inif.Read("Configuration", "log_dir");
            }
            else
            {
                // 100, 150, 50
                double value = 100;
                for (i = 0; i < 3; i++)
                {
                    if (i == 1)
                        value = 150;
                    else if (i == 2)
                        value = 50;
                    else
                        value = 100;

                    gCfg.temp[i] = value;
                    gCfg.humi[i] = value;
                    gCfg.tvoc[i] = value;
                    gCfg.fans[i] = value;
                    gCfg.co2[i] = value;
                }

                gCfg.duration = 5;
                gCfg.read_freq = 3;
                gCfg.log_method = 0;
                gCfg.numMaxDevice = 50;

                gCfg.log_dir = "c:\\temp";
            }
        }

        private void Write_Configuration()
        {
            // Write configuration
            string inifilename = strCheckFolder + "\\Z1CONFIG.ini";
            INIFile inif = new INIFile(inifilename);

            string strData = string.Format("{0},{1},{2}", gCfg.temp[0], gCfg.temp[1], gCfg.temp[2]);
            inif.Write("Configuration", "temperature", strData);

            strData = string.Format("{0},{1},{2}", gCfg.humi[0], gCfg.humi[1], gCfg.humi[2]);
            inif.Write("Configuration", "humidity", strData);

            strData = string.Format("{0},{1},{2}", gCfg.tvoc[0], gCfg.tvoc[1], gCfg.tvoc[2]);
            inif.Write("Configuration", "tvoc", strData);

            strData = string.Format("{0},{1},{2}", gCfg.fans[0], gCfg.fans[1], gCfg.fans[2]);
            inif.Write("Configuration", "fan_speed", strData);

            strData = string.Format("{0},{1},{2}", gCfg.co2[0], gCfg.co2[1], gCfg.co2[2]);
            inif.Write("Configuration", "co2", strData);

            strData = string.Format("{0}", gCfg.duration);
            inif.Write("Configuration", "duration", strData);

            strData = string.Format("{0}", gCfg.read_freq);
            inif.Write("Configuration", "read_freq", strData);

            strData = string.Format("{0}", gCfg.log_method);
            inif.Write("Configuration", "log_method", strData);

            strData = string.Format("{0}", gCfg.numMaxDevice);
            inif.Write("Configuration", "num_max_devices", strData);

            inif.Write("Configuration", "log_dir", gCfg.log_dir);
        }

        private void Create_Grid_Component()
        {
            //--- init gOp values
            gOp.SN = new List<string>();
            gOp.ScannedSN = new List<string>();
            gOp.ValidDevice = new int[MAX_NUM_SN];
            gOp.numRead = new int[MAX_NUM_SN];
            gOp.id = new int[MAX_NUM_SN];
            gOp.Clear();
            gOp.ResetSN();
            gOp.mode = 1;   // measuring


            //--------------------------------------------------------------------
            // Add Row Headers For MEASURE
            string[] row = { "1" };
            dgvForm.DefaultCellStyle.BackColor = Color.LightGray;

            for (int n = 0; n < MAX_NUM_SN; n++)
            {
                row[0] = Convert.ToString(n + 1);
                dgvForm.Rows.Add(row);
                dgvForm.Rows[n].Height = CELL_HEIGHT;
                dgvForm.Rows[n].Cells[1].Style.BackColor = Color.White;
            }

            dgvForm.Rows[0].Cells[1].Selected = true;

            dgvForm.EnableHeadersVisualStyles = false;
            dgvForm.ColumnHeadersDefaultCellStyle.BackColor = HEADERCOLOR;
            foreach (DataGridViewColumn col in dgvForm.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            for (int k = 0; k < dgvForm.ColumnCount; k++)
                dgvForm.Columns[k].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.dgvForm.DefaultCellStyle.SelectionBackColor = SELECTCOLOR;
            this.dgvForm.DefaultCellStyle.SelectionForeColor = this.dgvForm.DefaultCellStyle.ForeColor;

            //--------------------------------------------------------------------
            // Add Row Headers For STANDARD
            dgvStd.EnableHeadersVisualStyles = false;
            dgvStd.ColumnHeadersDefaultCellStyle.BackColor = HEADERCOLOR;
            foreach (DataGridViewColumn col in dgvStd.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            for (int k = 0; k < dgvStd.ColumnCount; k++)
                dgvStd.Columns[k].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //dgvStd.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //dgvStd.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            string stdtype;
            stdtype = "온도" + "(°C)";
            dgvStd.Rows.Add(stdtype);
            stdtype = "습도" + "(%)";
            dgvStd.Rows.Add(stdtype);
            stdtype = "TVOC" + "(ppm)";
            dgvStd.Rows.Add(stdtype);
            stdtype = "FAN속도" + "(rpm)";
            dgvStd.Rows.Add(stdtype);
            stdtype = "CO2" + "(ppm)";
            dgvStd.Rows.Add(stdtype);

            for (int n = 0; n < 5; n++)
            {
                dgvStd.Rows[n].Height = CELL_HEIGHT_STD;
            }

            for (int k = 1; k <= 3; k++)
                dgvStd.Rows[0].Cells[k].Value = gCfg.temp[k - 1];
            for (int k = 1; k <= 3; k++)
                dgvStd.Rows[1].Cells[k].Value = gCfg.humi[k - 1];
            for (int k = 1; k <= 3; k++)
                dgvStd.Rows[2].Cells[k].Value = gCfg.tvoc[k - 1];
            for (int k = 1; k <= 3; k++)
                dgvStd.Rows[3].Cells[k].Value = gCfg.fans[k - 1];
            for (int k = 1; k <= 3; k++)
                dgvStd.Rows[4].Cells[k].Value = gCfg.co2[k - 1];

            //--------------------------------------------------------------------
            //--- text box
            textBox2.AutoSize = false;
            textBox2.Height = 40;
            textBox2.BackColor = HEADERCOLOR;
            textBox3.AutoSize = false;
            textBox3.Height = 40;
            textBox3.Text = gCfg.duration.ToString();

            //--- log method
            if (gCfg.log_method == 1)
                RB_ALL.Select();
            else
                RB_AVG.Select();

            dgvStd.DefaultCellStyle.BackColor = Color.LightGray;
            textBox3.BackColor = Color.LightGray;
            this.dgvStd.DefaultCellStyle.SelectionBackColor = this.dgvStd.DefaultCellStyle.BackColor;
            this.dgvStd.DefaultCellStyle.SelectionForeColor = this.dgvStd.DefaultCellStyle.ForeColor;

            for (int i = 0; i < 5; i++)
                dgvStd.Rows[i].Cells[0].Style.BackColor = HEADERCOLOR;
            dgvStd.Rows[0].Cells[1].Selected = true;

            // create
            for (int n = 0; n < MAX_NUM_SN; n++)
            {
                ble[n] = new Bleservice();
            }

        }

        private void ClearGridValuesExceptSN()
        {
            //--- Clear Cell Values
            for (int i = 0; i < MAX_NUM_SN; i++)
            {
                for (int k = 2; k <= 9; k++)
                {
                    dgvForm.Rows[i].Cells[k].Value = "";
                    dgvForm.Rows[i].Cells[k].Style.BackColor = DISABLEDCOLOR;
                }
            }

        }

        private async void Start_Measure()
        {
            //--- initialize operation variables
            gOp.Clear();
            gOp.mode = 2;   // measuring
            Button_Enable(gOp.mode);

            ClearGridValuesExceptSN();

            //--- initialize statistics values
            for (int i = 0; i < MAX_NUM_SN; i++)
                for (int k = 0; k < 6; k++)
                    gMeasure[i, k].Reset();

            //--- connect 
            //Task<int> ret = Z1_PAIR_SN(gOp.curIndex, gOp.SN[gOp.curIndex]);
            //int result = await ret;

            //--- file open
            gOp.curStartTime = DateTime.Now;
            gOp.startTime = gOp.curStartTime;
            gOp.strLogDate = gOp.curStartTime.ToString("yyyyMMdd_hhmmss");

            for (int i=0; i < MAX_NUM_SN; i++)
            {
                gOp.id[i] = i;
                if (gOp.SN[i].Length >= 4)
                {
                    string strfile = string.Format("{0}\\{1}_{2}.csv", gCfg.log_dir, gOp.SN[i], gOp.strLogDate);
                    System.IO.File.WriteAllText(strfile, "DateTime, SN, 온도, 습도, TVOC, FAN속도, CO2, 배터리, 결과\r\n", Encoding.Default);
                    gOp.numSN++;
                }
            }

#if _USE_TIMER
            //--- Start Measuring
            gOp.curIndex = Get_Next_SN(gOp.curIndex);
            if (gOp.curIndex < 0)
            {
                Warning_Message("No Devices");
                Stop_Measure();
                return;
            }

            //--- Enable Timers
            timerPolling.Interval = gCfg.read_freq; // * 1000;
            timerPolling.Enabled = true;
            timer500.Enabled = true;
#else
            if (gOp.numSN == 0)
            {
                Warning_Message("No Devices");
                Stop_Measure();
                return;
            }
            int ix = 0;
            for (int i = 0; i < MAX_NUM_SN; i++)
            {
                if (gOp.SN[i].Length >= 4)
                {
                    ix = i;
                    gThread[ix] = new Thread(() => DataReading(gOp.id[ix], this));
                    gThread[ix].Start();
                    Thread.Sleep(20);
                }
            }
#endif
        }

        private void Stop_Measure()
        {
            gOp.mode = 1;   // measuring
            timerPolling.Enabled = false;
            timer500.Enabled = false;

            Button_Enable(gOp.mode);
            for (int i = 0; i < MAX_NUM_SN; i++)
            {
                if (gOp.ValidDevice[i] == 1)
                {
                    gOp.ValidDevice[i] = 0;
                    gThread[i].Join();
                }
            }
        }

        public async void DataReading(int index, FormZ1 myclass)
        {
            FormZ1 pMain = (FormZ1)myclass;
            double[] values = new double[6];
            long iElapsedTime = 0;
            DateTimeOffset dtStartTime = DateTime.Now;
            DateTimeOffset dtCurTime = DateTime.Now;

            while (pMain.gOp.ValidDevice[index] == 1)
            {
                Thread.Sleep(1000);

                ERROR_CODE result = ERROR_CODE.NONE;
                listDebug.Items.Insert(0, "=================== Start " + index.ToString() + "-th Device");

                //--- check if SN is connected
                string device_name = "VC Z1 " + pMain.gOp.SN[index].Substring(pMain.gOp.SN[index].Length - 4);
                if (pMain.Z1_GetDeviceConnectionStatus(index, device_name) == ERROR_CODE.BLE_NO_CONNECTED)
                {
                    result = await pMain.BleConnect(index, device_name);
                    if ((result != ERROR_CODE.NONE))
                    {
                        dtCurTime = DateTime.Now;
                        iElapsedTime = (dtCurTime.Ticks - dtStartTime.Ticks) / 10000000;
                        if (iElapsedTime >= 120)
                            pMain.Set_Connection_Status(index, false);
                        continue;
                    }
                    pMain.Set_Connection_Status(index, true);
                    dtStartTime = DateTime.Now;
                }

                try
                {
                    // 온도
                    string characteristic_name = "EnvironmentalSensing/Temperature";
                    string dev_name = "VC Z1 " + gOp.SN[index].Substring(gOp.SN[index].Length - 4);
                    string[] srVals = { "0", "0", "0", "0", "0" };
                    string srData;

                    srData = await ble[index].ReadCharacteristic(dev_name, characteristic_name);
                    if (!srData.StartsWith("ERROR_CODE.NONE"))
                    {
                        listDebug.Items.Insert(0, "### " + gOp.SN[index] + "(온도):" + "Read Error");
                        continue;
                    }

                    srVals = srData.Split(' ');
                    values[0] = int.Parse(srVals[2]) * 256 + int.Parse(srVals[1]);
                    values[0] = values[0] / 100.0;
                    listDebug.Items.Insert(0, gOp.SN[index] + "(온도):" + srVals[1] + " "+ srVals[2] + "==>" + values[0].ToString());

                    // 습도
                    characteristic_name = "EnvironmentalSensing/Humidity";
                    srData = await ble[index].ReadCharacteristic(dev_name, characteristic_name);
                    if (!srData.StartsWith("ERROR_CODE.NONE"))
                    {
                        listDebug.Items.Insert(0, "### " + gOp.SN[index] + "(습도):" + "Read Error");
                        continue;
                    }

                    srVals = srData.Split(' ');
                    values[1] = int.Parse(srVals[2]) * 256 + int.Parse(srVals[1]);
                    values[1] = values[1] / 100.0;
                    listDebug.Items.Insert(0, gOp.SN[index] + "(습도):" + srVals[1] + " " + srVals[2] + "==>" + values[1].ToString());

                    // TVOC
                    characteristic_name = "EnvironmentalSensing/TVOC";
                    srData = await ble[index].ReadCharacteristic(dev_name, characteristic_name);
                    if (!srData.StartsWith("ERROR_CODE.NONE"))
                    {
                        listDebug.Items.Insert(0, "### " + gOp.SN[index] + "(TVOC):" + "Read Error");
                        continue;
                    }

                    srVals = srData.Split(' ');
                    values[2] = int.Parse(srVals[2]) * 256 + int.Parse(srVals[1]);
                    listDebug.Items.Insert(0, gOp.SN[index] + "(TVOC):" + srVals[1] + " " + srVals[2] + "==>" + values[2].ToString());

                    // FANSPEED
                    characteristic_name = "VCService/FanSpeed";
                    srData = await ble[index].ReadCharacteristic(dev_name, characteristic_name);
                    if (!srData.StartsWith("ERROR_CODE.NONE"))
                    {
                        listDebug.Items.Insert(0, "### " + gOp.SN[index] + "(FAN속도):" + "Read Error");
                        continue;
                    }
                    
                    srVals = srData.Split(' ');
                    values[3] = int.Parse(srVals[2]) * 256 + int.Parse(srVals[1]);
                    listDebug.Items.Insert(0, gOp.SN[index] + "(FANS):" + srVals[1] + " " + srVals[2] + "==>" + values[3].ToString());

                    // CO2
                    characteristic_name = "EnvironmentalSensing/Co2";
                    srData = await ble[index].ReadCharacteristic(dev_name, characteristic_name);
                    if (!srData.StartsWith("ERROR_CODE.NONE"))
                    {
                        listDebug.Items.Insert(0, "### " + gOp.SN[index] + "(CO2):" + "Read Error");
                        continue;
                    }

                    srVals = srData.Split(' ');
                    values[4] = int.Parse(srVals[4]) * 16777215 + int.Parse(srVals[3]) * 655536 + int.Parse(srVals[2]) * 256 + int.Parse(srVals[1]);
                    listDebug.Items.Insert(0, gOp.SN[index] + "(CO2):" + ble[index].getCharacteristic() + "==>" + values[4].ToString());

                    // BATTERY
                    characteristic_name = "Battery/BatteryLevel";
                    srData = await ble[index].ReadCharacteristic(dev_name, characteristic_name);
                    if (!srData.StartsWith("ERROR_CODE.NONE"))
                    {
                        listDebug.Items.Insert(0, "### " + gOp.SN[index] + "(BATTERY):" + "Read Error");
                        continue;
                    }
                    
                    srVals = srData.Split(' ');
                    values[5] = int.Parse(srVals[1]);
                    listDebug.Items.Insert(0, gOp.SN[index] + "(BATT):" + ble[index].getCharacteristic() + "==>" + values[4].ToString());
                }
                catch (Exception error)
                {
                    Warning_Message("Read Exception Occurred!");
                    listDebug.Items.Insert(0, gOp.SN[index] + "ERROR OCCURRED");
                    continue;
                }

                //--- All Data Read
                Post_Processing_Result(index, values);
                Set_Connection_Status(index, true);
            }

            pMain.listDebug.Items.Insert(0, ">>>>>>>>>>>>>>>>>>>>>> Ended " + index.ToString() + "-th Device");
            pMain.Write_AverageMinMax(index);
            pMain.listDebug.Items.Insert(0, ">>>>>>>>>>>>>>>>>>>>>> Writed average " + index.ToString() + "-th Device");
        }

        private void Post_Processing_Result(int index, double [] values)
        {
            DateTimeOffset dateNow = DateTime.Now;
            gOp.elpased = (dateNow.Ticks - gOp.curStartTime.Ticks) / 10000000;

            if (gOp.elpased >= gCfg.duration * 60)
            {
                // All Done
                Stop_Measure();
                return;
            }
            gOp.numRead[index]++;

            //--- Averaging
            for (int k = 0; k < 6; k++)
            {
                gMeasure[index, k].sum += values[k];
                gMeasure[index, k].avg = gMeasure[index, k].sum / gOp.numRead[index];
                if (values[k] < gMeasure[index, k].min)
                    gMeasure[index, k].min = values[k];
                if (values[k] > gMeasure[index, k].max)
                    gMeasure[index, k].max = values[k];
            }

            //--- display value and error with color
            dgvForm.Rows[index].Cells[3].Value = ((int)(gMeasure[index, 0].avg * 10)) / 10.0;
            dgvForm.Rows[index].Cells[4].Value = ((int)(gMeasure[index, 1].avg * 10)) / 10.0;
            dgvForm.Rows[index].Cells[5].Value = ((int)(gMeasure[index, 2].avg * 10)) / 10.0;
            dgvForm.Rows[index].Cells[6].Value = ((int)(gMeasure[index, 3].avg * 10)) / 10.0;
            dgvForm.Rows[index].Cells[7].Value = ((int)(gMeasure[index, 4].avg * 10)) / 10.0;
            dgvForm.Rows[index].Cells[8].Value = ((int)(gMeasure[index, 5].avg * 10)) / 10.0;

            if (gMeasure[index, 0].avg > gCfg.temp[1] || gMeasure[index, 0].avg < gCfg.temp[2])
                dgvForm.Rows[index].Cells[3].Style.BackColor = WARNCOLOR;
            else
                dgvForm.Rows[index].Cells[3].Style.BackColor = NORMALCOLOR;

            if (gMeasure[index, 1].avg > gCfg.humi[1] || gMeasure[index, 1].avg < gCfg.humi[2])
                dgvForm.Rows[index].Cells[4].Style.BackColor = WARNCOLOR;
            else
                dgvForm.Rows[index].Cells[4].Style.BackColor = NORMALCOLOR;

            if (gMeasure[index, 2].avg > gCfg.tvoc[1] || gMeasure[index, 2].avg < gCfg.tvoc[2])
                dgvForm.Rows[index].Cells[5].Style.BackColor = WARNCOLOR;
            else
                dgvForm.Rows[index].Cells[5].Style.BackColor = NORMALCOLOR;

            if (gMeasure[index, 3].avg > gCfg.fans[1] || gMeasure[index, 3].avg < gCfg.fans[2])
                dgvForm.Rows[index].Cells[6].Style.BackColor = WARNCOLOR;
            else
                dgvForm.Rows[index].Cells[6].Style.BackColor = NORMALCOLOR;

            if (gMeasure[index, 4].avg > gCfg.co2[1] || gMeasure[index, 4].avg < gCfg.co2[2])
                dgvForm.Rows[index].Cells[7].Style.BackColor = WARNCOLOR;
            else
                dgvForm.Rows[index].Cells[7].Style.BackColor = NORMALCOLOR;

            dgvForm.Rows[index].Cells[8].Style.BackColor = NORMALCOLOR;

            //--- Logging
            if (gCfg.log_method == 1)   // all
            {
                string strDate = dateNow.ToString("yyyyMMdd_hhmmss");
                string strfile = string.Format("{0}\\{1}_{2}.csv", gCfg.log_dir, gOp.SN[index], gOp.strLogDate);
                string str = string.Format("{0}, {1}, {2:0.0}, {3:0.0}, {4:0.0}, {5:0.0}, {6:0.0}, {7:0.0}\r\n",
                                    strDate, gOp.SN[index], values[0], values[1], values[2], values[3], values[4], values[5]);
                System.IO.File.AppendAllText(strfile, str, Encoding.Default);
            }

            //--- Display Elapsed Time, GetValues(0:temp, 1:humi, 2:tvoc, 3:fans, 4:co2, 5:battery)
            string strValue = string.Format("현재값({0},{1}) #read={2}: <{3},  {4},  {5},  {6},  {7}, {8}>", index, gOp.SN[index], gOp.numRead[index], values[0], values[1], values[2], values[3], values[4], values[5]);
            LMessage2.Text = strValue;
            return;

        }

        private void Write_AverageMinMax(int index)
        {
            string[] strResult = { "PASS", "FAIL" };

            string strfile = string.Format("{0}\\{1}_{2}.csv", gCfg.log_dir, gOp.SN[index], gOp.strLogDate);
            string str = string.Format("AVERAGE, {0}, {1:0.0}, {2:0.0}, {3:0.0}, {4:0.0}, {5:0.0}, {6:0.0}\r\n",
                        gOp.SN[index], gMeasure[index, 0].avg, gMeasure[index, 1].avg,
                        gMeasure[index, 2].avg, gMeasure[index, 3].avg, gMeasure[index, 4].avg, gMeasure[index, 5].avg);
            System.IO.File.AppendAllText(strfile, str, Encoding.Default);

            str = string.Format("MAX, {0}, {1:0.0}, {2:0.0}, {3:0.0}, {4:0.0}, {5:0.0}, {6:0.0}\r\n",
                        gOp.SN[index], gMeasure[index, 0].max, gMeasure[index, 1].max,
                        gMeasure[index, 2].max, gMeasure[index, 3].max, gMeasure[index, 4].max, gMeasure[index, 5].max);
            System.IO.File.AppendAllText(strfile, str, Encoding.Default);

            str = string.Format("MIN, {0}, {1:0.0}, {2:0.0}, {3:0.0}, {4:0.0}, {5:0.0}, {6:0.0}\r\n",
                        gOp.SN[index], gMeasure[index, 0].min, gMeasure[index, 1].min,
                        gMeasure[index, 2].min, gMeasure[index, 3].min, gMeasure[index, 4].min, gMeasure[index, 5].min);
            System.IO.File.AppendAllText(strfile, str, Encoding.Default);

            //--- RESULT
            int[] iFail = new int[6];
            if (gMeasure[index, 0].avg > gCfg.temp[1] || gMeasure[index, 0].avg < gCfg.temp[2])
            {
                dgvForm.Rows[index].Cells[3].Style.BackColor = WARNCOLOR;
                iFail[0] = 1;
            }
            else
            {
                dgvForm.Rows[index].Cells[3].Style.BackColor = NORMALCOLOR;
                iFail[0] = 0;
            }

            if (gMeasure[index, 1].avg > gCfg.humi[1] || gMeasure[index, 1].avg < gCfg.humi[2])
            {
                dgvForm.Rows[index].Cells[4].Style.BackColor = WARNCOLOR;
                iFail[1] = 1;
            }
            else
            {
                dgvForm.Rows[index].Cells[4].Style.BackColor = NORMALCOLOR;
                iFail[1] = 0;
            }

            if (gMeasure[index, 2].avg > gCfg.tvoc[1] || gMeasure[index, 2].avg < gCfg.tvoc[2])
            {
                dgvForm.Rows[index].Cells[5].Style.BackColor = WARNCOLOR;
                iFail[2] = 1;
            }
            else
            {
                dgvForm.Rows[index].Cells[5].Style.BackColor = NORMALCOLOR;
                iFail[2] = 0;
            }

            if (gMeasure[index, 3].avg > gCfg.fans[1] || gMeasure[index, 3].avg < gCfg.fans[2])
            {
                dgvForm.Rows[index].Cells[6].Style.BackColor = WARNCOLOR;
                iFail[3] = 1;
            }
            else
            {
                dgvForm.Rows[index].Cells[6].Style.BackColor = NORMALCOLOR;
                iFail[3] = 0;
            }

            if (gMeasure[index, 4].avg > gCfg.co2[1] || gMeasure[index, 4].avg < gCfg.co2[2])
            {
                dgvForm.Rows[index].Cells[7].Style.BackColor = WARNCOLOR;
                iFail[4] = 1;
            }
            else
            {
                dgvForm.Rows[index].Cells[7].Style.BackColor = NORMALCOLOR;
                iFail[4] = 0;
            }

            dgvForm.Rows[index].Cells[8].Style.BackColor = NORMALCOLOR;

            if (iFail[0] == 1 || iFail[1] == 1 || iFail[2] == 1 || iFail[3] == 1 || iFail[4] == 1)
            {
                dgvForm.Rows[index].Cells[9].Style.BackColor = FAILCOLOR;
                dgvForm.Rows[index].Cells[9].Value = "FAIL";
                iFail[5] = 1;
            }
            else
            {
                dgvForm.Rows[index].Cells[9].Style.BackColor = NORMALCOLOR;
                dgvForm.Rows[index].Cells[9].Value = "PASS";
                iFail[5] = 0;
            }

            str = string.Format("RESULT, {0}, {1}, {2}, {3}, {4}, {5}, , {6}\r\n",
                        gOp.SN[index],
                        strResult[iFail[0]], strResult[iFail[1]],
                        strResult[iFail[2]], strResult[iFail[3]], strResult[iFail[4]], strResult[iFail[5]]);
            System.IO.File.AppendAllText(strfile, str, Encoding.Default);
        }

        private void Write_AverageMinMax()
        {
            string[] strResult = { "PASS", "FAIL" };

            for (int i = 0; i < MAX_NUM_SN; i++)
            {
                if (gOp.SN[i].Length > 0)
                {
                    Write_AverageMinMax(i);
                }
            }

        }

        /* Get Next Valid index */
        private int Get_Next_SN(int start_ix)
        {
            int num = 0;
            int ix = start_ix;

            while (num < MAX_NUM_SN)
            {
                if (gOp.SN[ix] != "" && Z1_Is_SN_Valid(ix))
                    return ix;
                ix = (ix + 1) % MAX_NUM_SN;
                num++;
            }

            return -1;
        }

        private void Warning_Message(string msg)
        {
            LWARNING.Text = msg;
            timer3000.Enabled = true;
        }

        private void Set_Connection_Status(int index, bool bOk)
        {
            if (bOk)
            {
                dgvForm.Rows[index].Cells[2].Value = "OK";
                dgvForm.Rows[index].Cells[2].Style.BackColor = NORMALCOLOR;
            } else
            {
                dgvForm.Rows[index].Cells[2].Value = "NG";
                dgvForm.Rows[index].Cells[2].Style.BackColor = PAIRFAIL;
            }
        }

        private void Display_Status()
        {

        }

        private void Data_Received()
        {

        }

        private void Log_Data()
        {

        }

        private void SN_Read()
        {

        }

        private void SetCurrentInputPostion()
        {
            dgvForm.Focus();
            for (int k = 0; k < MAX_NUM_SN; k++)
            {
                if (dgvForm.Rows[k].Cells[1].Value == null)
                {
                    dgvForm.Rows[k].Cells[1].Selected = true;
                    break;
                }
            }
        }

        private void Button_Enable(int Operation_Mode)
        {
            if (Operation_Mode == 1)    // Ready
            {
                //--- Enable :
                //            검사기준버튼, (dvgStandard, textBox3), LogAvg/ALL, LogDir, dvgForm, Btn_Pair
                //--- Disable : 
                //            Btn_Unpair, Btn_Stop

                Btn_Standard.Enabled = true;
                Btn_Standard.BackColor = UPCOLOR;
                RB_AVG.Enabled = true;
                RB_ALL.Enabled = true;
                Btn_Log.Enabled = true;
                Btn_Log.BackColor = UPCOLOR;
                Btn_Pair.Enabled = true;
                Btn_Pair.BackColor = UPCOLOR;
                dgvForm.Enabled = true;

                Btn_Unpair.Enabled = false;
                Btn_Unpair.BackColor = DISABLEDCOLOR;
                Btn_Stop.Enabled = false;
                Btn_Stop.BackColor = DISABLEDCOLOR;
                panel2.BackColor = UPCOLOR;

                //--- coloring dgvForm
                for (int k = 0; k < MAX_NUM_SN; k++)
                    dgvForm.Rows[k].Cells[1].Style.BackColor = ENABLEDCOLOR;

                this.dgvForm.DefaultCellStyle.SelectionBackColor = SELECTCOLOR;

                Btn_Action.Text = "준비";
                Btn_Action.BackColor = UPCOLOR;
                timer500.Enabled = false;

            }
            else if (Operation_Mode == 2) // Measuring
            {
                // All Disable: 검사기준버튼, (dvgStandard, textBox3), LogAvg/ALL, LogDir, dvgForm with color, Btn_Pair, Btn_Unpair 
                // Enable : Btn_Stop
                Btn_Standard.Enabled = false;
                Btn_Standard.BackColor = DISABLEDCOLOR;
                RB_AVG.Enabled = false;
                RB_ALL.Enabled = false;
                Btn_Log.Enabled = false;
                Btn_Log.BackColor = DISABLEDCOLOR;
                Btn_Pair.Enabled = false;
                Btn_Pair.BackColor = DISABLEDCOLOR;
                dgvForm.Enabled = false;

                Btn_Unpair.Enabled = false;
                Btn_Unpair.BackColor = DISABLEDCOLOR;
                Btn_Stop.Enabled = true;
                Btn_Stop.BackColor = UPCOLOR;

                panel2.BackColor = DISABLEDCOLOR;

                if (gOp.std_mode == 1)
                    Btn_Standard_Click(Btn_Standard, null);

                //--- coloring dgvForm
                //dgvForm.Rows[MAX_NUM_SN - 1].Cells[1].Selected = true;
                for (int k = 0; k < MAX_NUM_SN; k++)
                {
                    dgvForm.Rows[k].Cells[1].Style.BackColor = DISABLEDCOLOR;
                    if (gOp.ValidDevice[k] == 1)
                        dgvForm.Rows[k].Cells[1].Style.BackColor = DISABLEDCOLOR;
                    //else if (dgvForm.Rows[k].Cells[1].Value != null && dgvForm.Rows[k].Cells[1].Value.ToString().Length > 4)
                    //    dgvForm.Rows[k].Cells[1].Style.BackColor = FAILCOLOR;
                }

                this.dgvForm.DefaultCellStyle.SelectionBackColor = DISABLEDCOLOR;
                Btn_Action.Text = "측정중";
                timer500.Enabled = true;
            }
        }

        public async Task<ERROR_CODE> BleConnect(int index, string devName)
        {
            ERROR_CODE result = ERROR_CODE.NONE;
            result = ble[index].StartScan(devName, (d) => { });
            if (result.Equals(ERROR_CODE.BLE_FOUND_DEVICE))
            {
                result = await ble[index].OpenDevice(devName);
            };
            return result;
        }
    }


}
