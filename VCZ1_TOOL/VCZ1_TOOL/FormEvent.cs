using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VCZ1_TOOL
{

    public partial class FormZ1 : Form
    {

        private void Btn_Action_MouseDown(object sender, MouseEventArgs e)
        {
            ((Button)sender).BackColor = DOWNCOLOR;
        }

        private void Btn_Action_MouseUp(object sender, MouseEventArgs e)
        {
            if (((Button)sender).Enabled)
               ((Button)sender).BackColor = UPCOLOR;
        }

        private void Btn_Action_Click(object sender, EventArgs e)
        {
            // nothing to do
            SetCurrentInputPostion();
        }
        private void Btn_Action_MouseClick(object sender, MouseEventArgs e)
        {
            if (gOp.mode == 2)
                return;

            numDoubleClick++;

            if (numDoubleClick >= 2)
            {
                for (int k = 0; k < MAX_NUM_SN; k++)
                {
                    for (int j = 1; j < 9; j++)
                    {
                        dgvForm.Rows[k].Cells[j].Value = "";
                        if (j == 1)
                            dgvForm.Rows[k].Cells[j].Style.BackColor = Color.White;
                        else
                            dgvForm.Rows[k].Cells[j].Style.BackColor = DISABLEDCOLOR;

                    }
                }
                gOp.Clear();
            }

            timer400.Enabled = true;
            dgvForm.Focus();
            dgvForm.Rows[0].Cells[1].Selected = true;
        }

        /* dgvForm.Rows[0].Cells[1].Value */

        private void Btn_Pair_Click(object sender, EventArgs e)
        {
            //--- GetScanned Devices
            Z1_SCAN_ALL();
            Z1_LIST_DEVICES();

            //--- Get SN 
            gOp.numSN = 0;

            for (int i = 0; i < MAX_NUM_SN; i++)
            {
                if (dgvForm.Rows[i].Cells[1].Value != null)
                {
                    gOp.SN[i] = dgvForm.Rows[i].Cells[1].Value.ToString();
                }
                else
                    gOp.SN[i] = "";

                if (gOp.SN[i].Length >= 4)
                    gOp.numSN++;
            }
            //--- Start Measure
            Start_Measure();
            SetCurrentInputPostion();
        }

        private void Btn_Unpair_Click(object sender, EventArgs e)
        {
            // nothing to do
            SetCurrentInputPostion();
        }

        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            Stop_Measure();
            SetCurrentInputPostion();
        }

        private void RB_AVG_Click(object sender, EventArgs e)
        {
            gCfg.log_method = 0;
            SetCurrentInputPostion();
        }

        private void RB_ALL_Click(object sender, EventArgs e)
        {
            gCfg.log_method = 1;
            SetCurrentInputPostion();
        }

        private void Btn_Log_Click(object sender, EventArgs e)
        {
            /*
             * SaveFileDialog ofd = new SaveFileDialog();

            ofd.Title = "로그 파일 디렉토리";
            ofd.InitialDirectory = gCfg.log_dir;
            ofd.FileName = "LOG";
            DialogResult dr = ofd.ShowDialog();
            */

            FolderBrowserDialog ofd = new FolderBrowserDialog();
            ofd.SelectedPath = gCfg.log_dir;
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string fileFullName = ofd.SelectedPath;
                gCfg.log_dir = fileFullName;
            }


            SetCurrentInputPostion();
        }

        private void Btn_Standard_Click(object sender, EventArgs e)
        {
            // Enable/Disable editing
            if (gOp.std_mode == 1)   // to disable
            {
                gOp.std_mode = 0;
                dgvStd.Enabled = false;
                dgvStd.DefaultCellStyle.BackColor = Color.LightGray;
                this.dgvStd.DefaultCellStyle.SelectionBackColor = this.dgvStd.DefaultCellStyle.BackColor;
                this.dgvStd.DefaultCellStyle.SelectionForeColor = this.dgvStd.DefaultCellStyle.ForeColor;

                textBox3.BackColor = Color.LightGray;
                textBox3.Enabled = false;

                //--- store standard
                for (int i = 1; i <= 3; i++)
                {
                    gCfg.temp[i-1] = double.Parse(dgvStd.Rows[0].Cells[i].Value.ToString());
                    gCfg.humi[i-1] = double.Parse(dgvStd.Rows[1].Cells[i].Value.ToString());
                    gCfg.tvoc[i-1] = double.Parse(dgvStd.Rows[2].Cells[i].Value.ToString());
                    gCfg.fans[i-1] = double.Parse(dgvStd.Rows[3].Cells[i].Value.ToString());
                }
                gCfg.duration = int.Parse(textBox3.Text.ToString());
                //--- set focus to BARCODE
                SetCurrentInputPostion();
            }
            else
            {
                gOp.std_mode = 1;
                dgvStd.Enabled = true;
                dgvStd.DefaultCellStyle.BackColor = Color.White;
                for (int i = 0; i < 4; i++)
                    dgvStd.Rows[i].Cells[0].Style.BackColor = HEADERCOLOR;

                dgvStd.Rows[0].Cells[1].Selected = true;
                this.dgvStd.DefaultCellStyle.SelectionBackColor = SELECTCOLOR;
                this.dgvStd.DefaultCellStyle.SelectionForeColor = this.dgvStd.DefaultCellStyle.ForeColor;
                textBox3.BackColor = Color.White;
                textBox3.Enabled = true;
            }
        }

        private void Btn_ScanAll_Click(object sender, EventArgs e)
        {
            Z1_SCAN_ALL();
        }

        private void Btn_ListDevice_Click(object sender, EventArgs e)
        {
            Z1_LIST_DEVICES();
        }

        private void dgvForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }

        private void dgvForm_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex != 1)
                return;

            //--- check if it changes existing SN
            if (dgvForm.Rows[e.RowIndex].Cells[1].Value != null && dgvForm.Rows[e.RowIndex].Cells[1].Value.ToString().Length >= 2) // change existing
                dgvForm.Rows[e.RowIndex].Cells[1].Value = "";
        }

        private void dgvForm_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 1)
                return;

            //--- Check if duplicated ==> Focus to the previous cell, clear the cell
            if (e.RowIndex >= dgvForm.RowCount || e.ColumnIndex != 1)
                return;

            listDebug.Items.Clear();
            listDebug.Items.Insert(0, "Cur #: " + gOp.numSN.ToString() + "(" + dgvForm.Rows[e.RowIndex].Cells[1].Value + ")");
            for (int i = 0; i < gOp.numSN; i++)
            {
                listDebug.Items.Insert(0, (i + 1).ToString() + ":" + dgvForm.Rows[i].Cells[1].Value);
                if (e.RowIndex != i && dgvForm.Rows[e.RowIndex].Cells[1].Value != null && dgvForm.Rows[i].Cells[1].Value != null) // compare
                {
                    if (dgvForm.Rows[e.RowIndex].Cells[1].Value.ToString() == dgvForm.Rows[i].Cells[1].Value.ToString())  // duplicated
                    {
                        LMessage2.Text = "Duplicated" + e.RowIndex.ToString();
                        dgvForm.Rows[e.RowIndex].Cells[1].Value = "";
                        this.timer100.Enabled = true;
                        return;
                    }
                }
            }

            gOp.SN[e.RowIndex] = dgvForm.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (e.RowIndex >= gOp.numSN)
                gOp.numSN++;

            this.timer100.Enabled = true;
        }

    }
}
