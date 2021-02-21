using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace VCZ1_TOOL
{

    public partial class FormZ1 : Form
    {
        const int MAX_NUM_SN = 30;

        public struct Z1_Config
        {
            public double[] temp;
            public double[] humi;
            public double[] tvoc;
            public double[] fans;
            public int duration;
            public int read_freq;
            public int log_method;  // 0: avg, 1:all
            public string log_dir;
        }

        public struct Z1_Operation
        {
            public int mode;        // 1: 준비, 2: 측정중
            public int std_mode;    // 0: disable, 1:enable
            public int count;
            public int numFailed;
            public int curIndex;
            public int[] numRead;
            public int[] ValidDevice;               // 0: not exist, 1:exist in scanned device
            public DateTimeOffset startTime;        // Measure start time
            public DateTimeOffset curStartTime;     // Current SN start time
            public long elpased;
            public int numSN;
            public int numFoundSN;
            public List<string> SN;
            public List<string> ScannedSN;
            public string strLogDate;               // Log filename = SN_strLogDate.csv
 
            public void Clear()
            {
                std_mode = 0;
                numSN = 0;
                numFoundSN = 0;
                count = 0;
                numFailed = 0;
                curIndex = 0;
                for (int i = 0; i < MAX_NUM_SN; i++)
                {
                    numRead[i] = 0;
                    //ValidDevice[i] = 0;
                }
            }

            public void ResetSN()
            {
                SN.Clear();
                ScannedSN.Clear();
                for (int i = 0; i < MAX_NUM_SN; i++)
                {
                    SN.Add("");
                    ScannedSN.Add("----");
                }
            }
        }

        public struct Z1_MeasureStat
        {
            public double value;
            public double sum;
            public double avg;
            public double max;
            public double min;
            public void Reset()
            {
                sum = 0;
                avg = 0;
                max = 0;
                min = 10000;
            }
        }

        Z1_Config gCfg;
        Z1_Operation gOp;
        Z1_MeasureStat[,] gMeasure = new Z1_MeasureStat[MAX_NUM_SN, 5];  // temp, humi, tvoc, fans, battery      
        string strCheckFolder = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));
        int numDoubleClick = 0;
        double[] z1_values = new double[5];
        int read_complete = 0;
        Bleservice[] ble = new Bleservice[MAX_NUM_SN];

        Color UPCOLOR = Color.FromArgb(0, 128, 255);
        Color DOWNCOLOR = Color.FromArgb(0, 64, 128);
        Color WARNCOLOR = Color.FromArgb(255, 255, 0);
        Color NORMALCOLOR = Color.FromArgb(128, 192, 255);
        Color FAILCOLOR = Color.FromArgb(255, 0, 0);
        Color PASSCOLOR = Color.FromArgb(0, 192, 255);
        Color PAIRFAIL = Color.FromArgb(50, 210, 90);
        Color HEADERCOLOR = Color.FromArgb(50, 210, 90);
        Color SELECTCOLOR = Color.FromArgb(100, 192, 255);
        Color DISABLEDCOLOR = Color.LightGray; // Color.FromArgb(20, 148, 255);
        Color ENABLEDCOLOR = Color.White;
        Color ONCOLOR = Color.LightGray;
        Color OFFCOLOR = Color.Gray;

        public FormZ1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            System.Reflection.PropertyInfo aProp = typeof(System.Windows.Forms.Control)
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
            aProp.SetValue(dgvForm, true, null);

        }

        private void FormZ1_Load(object sender, EventArgs e)
        {
            Read_Configuration();
            Create_Grid_Component();
            Button_Enable(gOp.mode);
        }

        private void FormZ1_Shown(object sender, EventArgs e)
        {
            dgvForm.Focus();
            Button_Enable(gOp.mode);
        }

        private void FormZ1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Write_Configuration();
        }

        private void timer100_Tick(object sender, EventArgs e)
        {
            // Set selected row to numSN
            this.timer100.Enabled = false;
            SetCurrentInputPostion();

            //if (gOp.numSN < dgvForm.RowCount)
            //    dgvForm.Rows[gOp.numSN].Cells[1].Selected = true;
        }

        private void timer3000_Tick(object sender, EventArgs e)
        {
            timer3000.Enabled = false;
            LWARNING.Text = "-";
        }

        private async void timerPolling_Tick(object sender, EventArgs e)
        {
            timerPolling.Enabled = false;
            int iresult = 0;
            DateTimeOffset dateNow = DateTime.Now;
            gOp.elpased = (dateNow.Ticks - gOp.curStartTime.Ticks) / 10000000;

            //--- Getting Data
            read_complete = 0;
            Task<int> ret = Z1_GET_DATA(z1_values);
            try
            {
                iresult = await ret;
            }
            catch (Exception ex)
            {
                listDebug.Items.Insert(0, "################## GET DATA ERROR");
            }
            if (iresult > 0)
            {
                gOp.numRead[gOp.curIndex]++;
            } 
            else
            {
                //--- Check Duration and Save Result
                if (gOp.elpased >= gCfg.duration * 60)
                {
                    Write_AverageMinMax();
                    // All Done
                    Stop_Measure();
                    return;
                }

                // skip
                //gOp.curIndex = Get_Next_SN(gOp.curIndex + 1);
                if (gOp.mode == 2)
                    timerPolling.Enabled = true;
                return;
            }

            //--- Averaging
            for (int k = 0; k < 5; k++)
            {
                gMeasure[gOp.curIndex,k].sum += z1_values[k];
                gMeasure[gOp.curIndex, k].avg = gMeasure[gOp.curIndex, k].sum / gOp.numRead[gOp.curIndex];
                if (z1_values[k] < gMeasure[gOp.curIndex, k].min) 
                    gMeasure[gOp.curIndex, k].min = z1_values[k];
                if (z1_values[k] > gMeasure[gOp.curIndex, k].max) 
                    gMeasure[gOp.curIndex, k].max = z1_values[k];
            }

            //--- display value and error with color
            dgvForm.Rows[gOp.curIndex].Cells[3].Value = ((int)(gMeasure[gOp.curIndex, 0].avg * 10)) / 10.0;
            dgvForm.Rows[gOp.curIndex].Cells[4].Value = ((int)(gMeasure[gOp.curIndex, 1].avg * 10)) / 10.0;
            dgvForm.Rows[gOp.curIndex].Cells[5].Value = ((int)(gMeasure[gOp.curIndex, 2].avg * 10)) / 10.0;
            dgvForm.Rows[gOp.curIndex].Cells[6].Value = ((int)(gMeasure[gOp.curIndex, 3].avg * 10)) / 10.0;
            dgvForm.Rows[gOp.curIndex].Cells[7].Value = ((int)(gMeasure[gOp.curIndex, 4].avg * 10)) / 10.0;

            if (gMeasure[gOp.curIndex, 0].avg > gCfg.temp[1] || gMeasure[gOp.curIndex, 0].avg < gCfg.temp[2])
                dgvForm.Rows[gOp.curIndex].Cells[3].Style.BackColor = WARNCOLOR;
            else
                dgvForm.Rows[gOp.curIndex].Cells[3].Style.BackColor = NORMALCOLOR;

            if (gMeasure[gOp.curIndex, 1].avg > gCfg.humi[1] || gMeasure[gOp.curIndex, 1].avg < gCfg.humi[2])
                dgvForm.Rows[gOp.curIndex].Cells[4].Style.BackColor = WARNCOLOR;
            else
                dgvForm.Rows[gOp.curIndex].Cells[4].Style.BackColor = NORMALCOLOR;

            if (gMeasure[gOp.curIndex, 2].avg > gCfg.tvoc[1] || gMeasure[gOp.curIndex, 2].avg < gCfg.tvoc[2])
                dgvForm.Rows[gOp.curIndex].Cells[5].Style.BackColor = WARNCOLOR;
            else
                dgvForm.Rows[gOp.curIndex].Cells[5].Style.BackColor = NORMALCOLOR;

            if (gMeasure[gOp.curIndex, 3].avg > gCfg.fans[1] || gMeasure[gOp.curIndex, 3].avg < gCfg.fans[2])
                dgvForm.Rows[gOp.curIndex].Cells[6].Style.BackColor = WARNCOLOR;
            else
                dgvForm.Rows[gOp.curIndex].Cells[6].Style.BackColor = NORMALCOLOR;

            dgvForm.Rows[gOp.curIndex].Cells[7].Style.BackColor = NORMALCOLOR;

            //--- Logging
            if (gCfg.log_method == 1)   // all
            {
                string strDate = dateNow.ToString("yyyyMMdd_hhmmss");
                string strfile = string.Format("{0}\\{1}_{2}.csv", gCfg.log_dir, gOp.SN[gOp.curIndex], gOp.strLogDate);
                string str = string.Format("{0}, {1}, {2:0.0}, {3:0.0}, {4:0.0}, {5:0.0}, {6:0.0}\r\n", 
                                    strDate, gOp.SN[gOp.curIndex], z1_values[0], z1_values[1], z1_values[2], z1_values[3], z1_values[4]);
                System.IO.File.AppendAllText(strfile, str, Encoding.Default);
            }

            //--- Check Duration and Save Result
            if (gOp.elpased >= gCfg.duration*60)
            {
                Write_AverageMinMax();
                // All Done
                Stop_Measure();
                return;
            }

            //--- Display Elapsed Time, GetValues(0:temp, 1:humi, 2:tvoc, 3:fans, 4:battery)
            string strValue = string.Format("현재값({0},{1}): {2},  {3},  {4},  {5},  {6}", gOp.curIndex, gOp.SN[gOp.curIndex],z1_values[0], z1_values[1], z1_values[2], z1_values[3], z1_values[4]);
            LMessage2.Text = strValue;
            if (gOp.mode == 2)
                timerPolling.Enabled = true;

            gOp.curIndex = Get_Next_SN(gOp.curIndex + 1);

        }

        private void timer400_Tick(object sender, EventArgs e)
        {
            numDoubleClick = 0;
            timer400.Enabled = false;
        }

        private void RB_AVG_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void timer500_Tick(object sender, EventArgs e)
        {
            // blinking during measurement
            DateTimeOffset dateNow = DateTime.Now;
            gOp.elpased = (dateNow.Ticks - gOp.curStartTime.Ticks) / 10000000;
            if (gOp.elpased > 1000000)
                return;

            string str = string.Format("측정중 ({0:00}:{1:00}:{2:00})", gOp.elpased / 3600, (gOp.elpased / 60) % 60, gOp.elpased % 60);
            Btn_Action.Text = str;
            gOp.count++;
            if ((gOp.count % 2) == 0)
            {
                Btn_Action.BackColor = ONCOLOR;
            }
            else
            {
                Btn_Action.BackColor = OFFCOLOR;
            }
        }

    }
}
