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

    }
}
