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
using System.Threading;
using System.Threading.Tasks;

namespace VCZ1_TOOL
{

    public partial class FormZ1 : Form
    {
        const int MAX_NUM_SN = 10;
        const int CELL_HEIGHT = 62;
        const int CELL_HEIGHT_STD = 45;

        public struct Z1_Config
        {
            public double[] temp;
            public double[] humi;
            public double[] tvoc;
            public double[] fans;
            public double[] co2;
            public int duration;
            public int read_freq;
            public int log_method;  // 0: avg, 1:all
            public int numMaxDevice;
            public string log_dir;
            public string prefix;   //"    "로 둘러쌓여야 함.
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
            public int[] id;                        // unique id. normally index of List
            public DateTimeOffset startTime;        // Measure start time
            public DateTimeOffset curStartTime;     // Current SN start time
            public long elpased;
            public int numSN;
            public int numFoundSN;
            public List<string> SN;
            public List<string> ScannedSN;
            public string strLogDate;               // Log filename = SN_strLogDate.csv
            public int[] ThreadStatus;               // 0: ended, 1:running

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

        Thread[] gThread = new Thread[MAX_NUM_SN];
        Z1_Config gCfg;
        Z1_Operation gOp;
        Z1_MeasureStat[,] gMeasure = new Z1_MeasureStat[MAX_NUM_SN, 6];  // temp, humi, tvoc, fans, battery, co2     
        string strCheckFolder = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));
        int numDoubleClick = 0;
        double[] z1_values = new double[6]; // 0:온도, 1:습도, 2:TVOC, 3:FANSPEED, 4:CO2, 5:BATTERY
        int stopTimeout = 0;    // forced Stop after 10 seconds (value 100)
        Bleservice[] ble = new Bleservice[MAX_NUM_SN];
        bool display_current = false;    // display current value instead of average.
        FormMessage mFormMessage = new FormMessage();

        Color UPCOLOR = Color.FromArgb(0, 128, 255);
        Color DOWNCOLOR = Color.FromArgb(0, 64, 128);
        Color WARNCOLOR = Color.FromArgb(255, 0, 0);
        Color NORMALCOLOR = Color.FromArgb(128, 192, 255);
        Color FAILCOLOR = Color.FromArgb(255, 0, 0);
        Color PASSCOLOR = Color.FromArgb(0, 192, 255);
        Color PAIRFAIL = Color.FromArgb(255, 0, 0);
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            display_current = checkBox1.Checked;
        }

        private void timer100_Tick(object sender, EventArgs e)
        {
            // Set selected row to numSN
            this.timer100.Enabled = false;
            SetCurrentInputPostion();

            //if (gOp.numSN < dgvForm.RowCount)
            //    dgvForm.Rows[gOp.numSN].Cells[1].Selected = true;
        }

        private void timer400_Tick(object sender, EventArgs e)
        {
            numDoubleClick = 0;
            timer400.Enabled = false;
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

        private void timer3000_Tick(object sender, EventArgs e)
        {
            timer3000.Enabled = false;
            LWARNING.Text = "-";
        }

        private void timerStop_Tick(object sender, EventArgs e)
        {
            int i;
            bool bStatusRunning = false;
            stopTimeout++;

            for (i = 0; i < MAX_NUM_SN; i++)
            {
                if (gOp.ThreadStatus[i] == 1)
                {
                    bStatusRunning = true;
                    break;
                }
            }
            string str = string.Format("{0},{1},{2},{3},{4},   {5},{6},{7},{8},{9}   ({10})",
                gOp.ThreadStatus[0], gOp.ThreadStatus[1], gOp.ThreadStatus[2],
                gOp.ThreadStatus[3], gOp.ThreadStatus[4], gOp.ThreadStatus[5],
                gOp.ThreadStatus[6], gOp.ThreadStatus[7], gOp.ThreadStatus[8],
                gOp.ThreadStatus[9], stopTimeout);

            mFormMessage.Set_Message(str);

            if (bStatusRunning == false)// || stopTimeout > 100)
            {
                mFormMessage.Hide();
                timerStop.Enabled = false;
                Button_Enable(gOp.mode);
                SetCurrentInputPostion();
            }
        }
    }
}
