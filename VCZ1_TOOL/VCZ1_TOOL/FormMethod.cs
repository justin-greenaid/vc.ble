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

                gCfg.duration = int.Parse(inif.Read("Configuration", "duration"));
                gCfg.read_freq = int.Parse(inif.Read("Configuration", "read_freq"));
                gCfg.log_method = int.Parse(inif.Read("Configuration", "log_method"));
                gCfg.log_dir = inif.Read("Configuration", "log_dir");
            }
            else
            {
               for (i = 0; i < 3; i++)
                {
                    gCfg.temp[i] = 100;
                    gCfg.humi[i] = 100;
                    gCfg.tvoc[i] = 100;
                    gCfg.fans[i] = 100;
                }

                gCfg.duration = 5;
                gCfg.read_freq = 3;
                gCfg.log_method = 0;
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

            strData = string.Format("{0}", gCfg.duration);
            inif.Write("Configuration", "duration", strData);

            strData = string.Format("{0}", gCfg.read_freq);
            inif.Write("Configuration", "read_freq", strData);

            strData = string.Format("{0}", gCfg.log_method);
            inif.Write("Configuration", "log_method", strData);

            inif.Write("Configuration", "log_dir", gCfg.log_dir);
        }

        private void Create_Grid_Component()
        {
            //--- init gOp values
            gOp.SN = new List<string>();
            gOp.ScannedSN = new List<string>();
            gOp.ValidDevice = new int[MAX_NUM_SN];
            gOp.numRead = new int[MAX_NUM_SN];
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
                dgvForm.Rows[n].Height = 30;
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
            stdtype = "온도" + "\r\n" + "(°C)";
            dgvStd.Rows.Add(stdtype);
            stdtype = "습도" + "\r\n" + "(%)";
            dgvStd.Rows.Add(stdtype);
            stdtype = "TVOC" + "\r\n" + "(ppm)";
            dgvStd.Rows.Add(stdtype);
            stdtype = "FAN Speed" + "\r\n" + "(RPM)";
            dgvStd.Rows.Add(stdtype);

            for (int n = 0; n < 4; n++)
            {
                dgvStd.Rows[n].Height = 36;
            }

            for (int k = 1; k <= 3; k++)
                dgvStd.Rows[0].Cells[k].Value = gCfg.temp[k - 1];
            for (int k = 1; k <= 3; k++)
                dgvStd.Rows[1].Cells[k].Value = gCfg.humi[k - 1];
            for (int k = 1; k <= 3; k++)
                dgvStd.Rows[2].Cells[k].Value = gCfg.tvoc[k - 1];
            for (int k = 1; k <= 3; k++)
                dgvStd.Rows[3].Cells[k].Value = gCfg.fans[k - 1];

            //--------------------------------------------------------------------
            //--- text box
            textBox2.AutoSize = false;
            textBox2.Height = 28;
            textBox2.BackColor = HEADERCOLOR;
            textBox3.AutoSize = false;
            textBox3.Height = 28;
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

            for (int i = 0; i < 4; i++)
                dgvStd.Rows[i].Cells[0].Style.BackColor = HEADERCOLOR;
            dgvStd.Rows[0].Cells[1].Selected = true;

            // create
            for (int n = 0; n < MAX_NUM_SN; n++)
            {
                ble[n] = new Bleservice();
            }

        }

        private int Find_Index(string serial_number)
        {
            int ix = -1;

            return ix;
        }

        private async void Start_Measure()
        {
            //--- initialize operation variables
            gOp.Clear();
            gOp.mode = 2;   // measuring
            Button_Enable(gOp.mode);

            //--- initialize statistics values
            for (int i = 0; i < MAX_NUM_SN; i++)
                for (int k = 0; k < 5; k++)
                    gMeasure[i, k].Reset();

            //--- Start Measuring
            gOp.curIndex = Get_Next_SN(gOp.curIndex);
            if (gOp.curIndex < 0)
            {
                Warning_Message("No Devices");
                Stop_Measure();
                return;
            }

            //--- connect 
            //Task<int> ret = Z1_PAIR_SN(gOp.curIndex, gOp.SN[gOp.curIndex]);
            //int result = await ret;

            //--- file open
            gOp.curStartTime = DateTime.Now;
            gOp.startTime = gOp.curStartTime;
            gOp.strLogDate = gOp.curStartTime.ToString("yyyyMMdd_hhmmss");

            for (int i=0; i < MAX_NUM_SN; i++)
            {
                if (gOp.SN[i].Length > 0)
                {
                    string strfile = string.Format("{0}\\{1}_{2}.csv", gCfg.log_dir, gOp.SN[i], gOp.strLogDate);
                    System.IO.File.WriteAllText(strfile, "DateTime, SN, 온도, 습도, TVOC, FAN속도, 배터리, 결과\r\n", Encoding.Default);
                }
            }
            //--- Enable Timers
            timerPolling.Interval = gCfg.read_freq; // * 1000;
            timerPolling.Enabled = true;
            timer500.Enabled = true;

        }

        private void Stop_Measure()
        {
            gOp.mode = 1;   // measuring
            timerPolling.Enabled = false;
            timer500.Enabled = false;

            Button_Enable(gOp.mode);
        }

        private void Write_Measure_Result()
        {
        }

        private void Write_AverageMinMax()
        {
            string[] strResult = { "PASS", "FAIL" };

            for (int i = 0; i < MAX_NUM_SN; i++)
            {
                if (gOp.SN[i].Length > 0)
                {
                    string strfile = string.Format("{0}\\{1}_{2}.csv", gCfg.log_dir, gOp.SN[i], gOp.strLogDate);
                    string str = string.Format("AVERAGE, {0}, {1:0.0}, {2:0.0}, {3:0.0}, {4:0.0}, {5:0.0}\r\n",
                                gOp.SN[i], gMeasure[i, 0].avg, gMeasure[i, 1].avg,
                                gMeasure[i, 2].avg, gMeasure[i, 3].avg, gMeasure[i, 4].avg);
                    System.IO.File.AppendAllText(strfile, str, Encoding.Default);

                    str = string.Format("MAX, {0}, {1:0.0}, {2:0.0}, {3:0.0}, {4:0.0}, {5:0.0}\r\n",
                                gOp.SN[i], gMeasure[i, 0].max, gMeasure[i, 1].max,
                                gMeasure[i, 2].max, gMeasure[i, 3].max, gMeasure[i, 4].max);
                    System.IO.File.AppendAllText(strfile, str, Encoding.Default);

                    str = string.Format("MIN, {0}, {1:0.0}, {2:0.0}, {3:0.0}, {4:0.0}, {5:0.0}\r\n",
                                gOp.SN[i], gMeasure[i, 0].min, gMeasure[i, 1].min,
                                gMeasure[i, 2].min, gMeasure[i, 3].min, gMeasure[i, 4].min);
                    System.IO.File.AppendAllText(strfile, str, Encoding.Default);

                    //--- RESULT
                    int[] iFail = new int [5];
                    if (gMeasure[i, 0].avg > gCfg.temp[1] || gMeasure[i, 0].avg < gCfg.temp[2])
                    {
                        dgvForm.Rows[i].Cells[3].Style.BackColor = WARNCOLOR;
                        iFail[0] = 1;
                    }
                    else
                    {
                        dgvForm.Rows[i].Cells[3].Style.BackColor = NORMALCOLOR;
                        iFail[0] = 0;
                    }

                    if (gMeasure[i, 1].avg > gCfg.humi[1] || gMeasure[i, 1].avg < gCfg.humi[2])
                    {
                        dgvForm.Rows[i].Cells[4].Style.BackColor = WARNCOLOR;
                        iFail[1] = 1;
                    }
                    else
                    {
                        dgvForm.Rows[i].Cells[4].Style.BackColor = NORMALCOLOR;
                        iFail[1] = 0;
                    }

                    if (gMeasure[i, 2].avg > gCfg.tvoc[1] || gMeasure[i, 2].avg < gCfg.tvoc[2])
                    {
                        dgvForm.Rows[i].Cells[5].Style.BackColor = WARNCOLOR;
                        iFail[2] = 1;
                    }
                    else
                    {
                        dgvForm.Rows[i].Cells[5].Style.BackColor = NORMALCOLOR;
                        iFail[2] = 0;
                    }

                    if (gMeasure[i, 3].avg > gCfg.fans[1] || gMeasure[i, 3].avg < gCfg.fans[2])
                    {
                        dgvForm.Rows[i].Cells[6].Style.BackColor = WARNCOLOR;
                        iFail[3] = 1;
                    }
                    else
                    {
                        dgvForm.Rows[i].Cells[6].Style.BackColor = NORMALCOLOR;
                        iFail[3] = 0;
                    }

                    dgvForm.Rows[i].Cells[7].Style.BackColor = NORMALCOLOR;

                    if (iFail[0] == 1 || iFail[1] == 1 || iFail[2] == 1 || iFail[3] == 1)
                    {
                        dgvForm.Rows[i].Cells[8].Style.BackColor = FAILCOLOR;
                        dgvForm.Rows[i].Cells[8].Value = "FAIL";
                        iFail[4] = 1;
                    } else
                    {
                        dgvForm.Rows[i].Cells[8].Style.BackColor = NORMALCOLOR;
                        dgvForm.Rows[i].Cells[8].Value = "PASS";
                        iFail[4] = 0;
                    }

                    str = string.Format("RESULT, {0}, {1}, {2}, {3}, {4}, , {5}\r\n",
                                gOp.SN[i],
                                strResult[iFail[0]], strResult[iFail[1]],
                                strResult[iFail[2]], strResult[iFail[3]], strResult[iFail[4]]);
                    System.IO.File.AppendAllText(strfile, str, Encoding.Default);
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

        private void Set_Connection_Status(bool bOk)
        {
            if (bOk)
            {
                dgvForm.Rows[gOp.curIndex].Cells[2].Value = "OK";
                dgvForm.Rows[gOp.curIndex].Cells[2].Style.BackColor = NORMALCOLOR;
            } else
            {
                dgvForm.Rows[gOp.curIndex].Cells[2].Value = "NG";
                dgvForm.Rows[gOp.curIndex].Cells[2].Style.BackColor = PAIRFAIL;
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
                    else if (dgvForm.Rows[k].Cells[1].Value != null && dgvForm.Rows[k].Cells[1].Value.ToString().Length > 4)
                        dgvForm.Rows[k].Cells[1].Style.BackColor = FAILCOLOR;
                }

                this.dgvForm.DefaultCellStyle.SelectionBackColor = DISABLEDCOLOR;
                Btn_Action.Text = "측정중";
                timer500.Enabled = true;
            }
        }
    }
}
