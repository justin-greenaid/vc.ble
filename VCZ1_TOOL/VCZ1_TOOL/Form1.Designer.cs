namespace VCZ1_TOOL
{
    partial class FormZ1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
       }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvForm = new System.Windows.Forms.DataGridView();
            this.NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PAIR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TEMP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HUMI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TVOC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FANS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CO2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BATT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RESU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Btn_Action = new System.Windows.Forms.Button();
            this.Btn_Standard = new System.Windows.Forms.Button();
            this.dgvStd = new System.Windows.Forms.DataGridView();
            this.TYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.RB_ALL = new System.Windows.Forms.RadioButton();
            this.RB_AVG = new System.Windows.Forms.RadioButton();
            this.Btn_Stop = new System.Windows.Forms.Button();
            this.Btn_Log = new System.Windows.Forms.Button();
            this.Btn_Pair = new System.Windows.Forms.Button();
            this.Btn_Unpair = new System.Windows.Forms.Button();
            this.LMessage = new System.Windows.Forms.Label();
            this.LMessage2 = new System.Windows.Forms.Label();
            this.listDebug = new System.Windows.Forms.ListBox();
            this.timer100 = new System.Windows.Forms.Timer(this.components);
            this.timerPolling = new System.Windows.Forms.Timer(this.components);
            this.LWARNING = new System.Windows.Forms.Label();
            this.timer3000 = new System.Windows.Forms.Timer(this.components);
            this.timer400 = new System.Windows.Forms.Timer(this.components);
            this.timer500 = new System.Windows.Forms.Timer(this.components);
            this.Btn_ScanAll = new System.Windows.Forms.Button();
            this.Btn_ListDevice = new System.Windows.Forms.Button();
            this.Btn_SNLoad = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvForm)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStd)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvForm
            // 
            this.dgvForm.AllowUserToAddRows = false;
            this.dgvForm.AllowUserToDeleteRows = false;
            this.dgvForm.AllowUserToResizeColumns = false;
            this.dgvForm.AllowUserToResizeRows = false;
            this.dgvForm.BackgroundColor = System.Drawing.Color.DarkGray;
            this.dgvForm.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvForm.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvForm.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvForm.ColumnHeadersHeight = 70;
            this.dgvForm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvForm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NO,
            this.SN,
            this.PAIR,
            this.TEMP,
            this.HUMI,
            this.TVOC,
            this.FANS,
            this.CO2,
            this.BATT,
            this.RESU});
            this.dgvForm.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dgvForm.Location = new System.Drawing.Point(0, 2);
            this.dgvForm.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.dgvForm.MultiSelect = false;
            this.dgvForm.Name = "dgvForm";
            this.dgvForm.RowHeadersVisible = false;
            this.dgvForm.RowHeadersWidth = 15;
            this.dgvForm.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvForm.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvForm.RowTemplate.Height = 40;
            this.dgvForm.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvForm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvForm.Size = new System.Drawing.Size(1042, 672);
            this.dgvForm.TabIndex = 0;
            this.dgvForm.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvForm_CellBeginEdit);
            this.dgvForm.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvForm_CellEndEdit);
            this.dgvForm.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvForm_ColumnHeaderMouseDoubleClick);
            this.dgvForm.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgvForm_PreviewKeyDown);
            // 
            // NO
            // 
            this.NO.HeaderText = "NO";
            this.NO.Name = "NO";
            this.NO.ReadOnly = true;
            this.NO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.NO.Width = 50;
            // 
            // SN
            // 
            this.SN.HeaderText = "S/N";
            this.SN.Name = "SN";
            this.SN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SN.Width = 180;
            // 
            // PAIR
            // 
            this.PAIR.HeaderText = "페어링";
            this.PAIR.Name = "PAIR";
            this.PAIR.ReadOnly = true;
            this.PAIR.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TEMP
            // 
            this.TEMP.HeaderText = "온도(°C)";
            this.TEMP.Name = "TEMP";
            this.TEMP.ReadOnly = true;
            this.TEMP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // HUMI
            // 
            this.HUMI.HeaderText = "습도(%)";
            this.HUMI.Name = "HUMI";
            this.HUMI.ReadOnly = true;
            this.HUMI.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TVOC
            // 
            this.TVOC.HeaderText = "TVOC";
            this.TVOC.Name = "TVOC";
            // 
            // FANS
            // 
            this.FANS.HeaderText = "FAN속도 (RPM)";
            this.FANS.Name = "FANS";
            this.FANS.ReadOnly = true;
            this.FANS.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.FANS.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FANS.Width = 120;
            // 
            // CO2
            // 
            this.CO2.HeaderText = "CO2";
            this.CO2.Name = "CO2";
            this.CO2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // BATT
            // 
            this.BATT.HeaderText = "Battery잔량(%)";
            this.BATT.Name = "BATT";
            this.BATT.ReadOnly = true;
            this.BATT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // RESU
            // 
            this.RESU.HeaderText = "결과";
            this.RESU.Name = "RESU";
            this.RESU.ReadOnly = true;
            this.RESU.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RESU.Width = 90;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel1.Controls.Add(this.Btn_Action);
            this.panel1.Controls.Add(this.Btn_Standard);
            this.panel1.Controls.Add(this.dgvStd);
            this.panel1.Controls.Add(this.textBox3);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panel1.Location = new System.Drawing.Point(1050, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(445, 593);
            this.panel1.TabIndex = 3;
            // 
            // Btn_Action
            // 
            this.Btn_Action.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.Btn_Action.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Maroon;
            this.Btn_Action.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.Btn_Action.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Action.ForeColor = System.Drawing.Color.Black;
            this.Btn_Action.Location = new System.Drawing.Point(2, 0);
            this.Btn_Action.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.Btn_Action.Name = "Btn_Action";
            this.Btn_Action.Size = new System.Drawing.Size(434, 50);
            this.Btn_Action.TabIndex = 9;
            this.Btn_Action.Text = "준비";
            this.Btn_Action.UseVisualStyleBackColor = false;
            this.Btn_Action.Click += new System.EventHandler(this.Btn_Action_Click);
            this.Btn_Action.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Btn_Action_MouseClick);
            // 
            // Btn_Standard
            // 
            this.Btn_Standard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Btn_Standard.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Standard.Location = new System.Drawing.Point(2, 68);
            this.Btn_Standard.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.Btn_Standard.Name = "Btn_Standard";
            this.Btn_Standard.Size = new System.Drawing.Size(434, 50);
            this.Btn_Standard.TabIndex = 8;
            this.Btn_Standard.Text = "검사기준 편집";
            this.Btn_Standard.UseVisualStyleBackColor = false;
            this.Btn_Standard.Click += new System.EventHandler(this.Btn_Standard_Click);
            this.Btn_Standard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Btn_Action_MouseDown);
            this.Btn_Standard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Btn_Action_MouseUp);
            // 
            // dgvStd
            // 
            this.dgvStd.AllowUserToAddRows = false;
            this.dgvStd.AllowUserToDeleteRows = false;
            this.dgvStd.AllowUserToResizeColumns = false;
            this.dgvStd.AllowUserToResizeRows = false;
            this.dgvStd.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvStd.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvStd.ColumnHeadersHeight = 40;
            this.dgvStd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvStd.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TYPE,
            this.Column1,
            this.Column2,
            this.Column3});
            this.dgvStd.Enabled = false;
            this.dgvStd.Location = new System.Drawing.Point(4, 116);
            this.dgvStd.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.dgvStd.MultiSelect = false;
            this.dgvStd.Name = "dgvStd";
            this.dgvStd.RowHeadersVisible = false;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStd.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvStd.RowTemplate.Height = 40;
            this.dgvStd.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvStd.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvStd.Size = new System.Drawing.Size(431, 264);
            this.dgvStd.TabIndex = 7;
            this.dgvStd.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStd_CellContentClick);
            // 
            // TYPE
            // 
            this.TYPE.Frozen = true;
            this.TYPE.HeaderText = "구분";
            this.TYPE.Name = "TYPE";
            this.TYPE.ReadOnly = true;
            this.TYPE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TYPE.ToolTipText = "TYPE";
            this.TYPE.Width = 180;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "스펙";
            this.Column1.Name = "Column1";
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 90;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "상한치";
            this.Column2.Name = "Column2";
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 80;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "하한치";
            this.Column3.Name = "Column3";
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column3.Width = 80;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.Control;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox3.Enabled = false;
            this.textBox3.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(275, 380);
            this.textBox3.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(160, 46);
            this.textBox3.TabIndex = 6;
            this.textBox3.Text = "5";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(4, 380);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(272, 46);
            this.textBox2.TabIndex = 5;
            this.textBox2.Text = "측정시간(분)";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.Btn_Stop);
            this.panel3.Controls.Add(this.Btn_Log);
            this.panel3.Controls.Add(this.Btn_Pair);
            this.panel3.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel3.Location = new System.Drawing.Point(4, 431);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(431, 151);
            this.panel3.TabIndex = 11;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel2.Controls.Add(this.RB_ALL);
            this.panel2.Controls.Add(this.RB_AVG);
            this.panel2.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(4, 6);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(194, 65);
            this.panel2.TabIndex = 10;
            // 
            // RB_ALL
            // 
            this.RB_ALL.AutoSize = true;
            this.RB_ALL.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RB_ALL.Location = new System.Drawing.Point(5, 34);
            this.RB_ALL.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.RB_ALL.Name = "RB_ALL";
            this.RB_ALL.Size = new System.Drawing.Size(156, 29);
            this.RB_ALL.TabIndex = 1;
            this.RB_ALL.TabStop = true;
            this.RB_ALL.Text = "전체 LOG 기록";
            this.RB_ALL.UseVisualStyleBackColor = true;
            this.RB_ALL.Click += new System.EventHandler(this.RB_ALL_Click);
            // 
            // RB_AVG
            // 
            this.RB_AVG.AutoSize = true;
            this.RB_AVG.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RB_AVG.Location = new System.Drawing.Point(5, 5);
            this.RB_AVG.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.RB_AVG.Name = "RB_AVG";
            this.RB_AVG.Size = new System.Drawing.Size(172, 29);
            this.RB_AVG.TabIndex = 0;
            this.RB_AVG.TabStop = true;
            this.RB_AVG.Text = "평균값 LOG 기록";
            this.RB_AVG.UseVisualStyleBackColor = true;
            this.RB_AVG.CheckedChanged += new System.EventHandler(this.RB_AVG_CheckedChanged);
            this.RB_AVG.Click += new System.EventHandler(this.RB_AVG_Click);
            // 
            // Btn_Stop
            // 
            this.Btn_Stop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Btn_Stop.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Stop.Location = new System.Drawing.Point(231, 76);
            this.Btn_Stop.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.Btn_Stop.Name = "Btn_Stop";
            this.Btn_Stop.Size = new System.Drawing.Size(189, 65);
            this.Btn_Stop.TabIndex = 6;
            this.Btn_Stop.Text = "측정강제종료";
            this.Btn_Stop.UseVisualStyleBackColor = false;
            this.Btn_Stop.Click += new System.EventHandler(this.Btn_Stop_Click);
            this.Btn_Stop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Btn_Action_MouseDown);
            this.Btn_Stop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Btn_Action_MouseUp);
            // 
            // Btn_Log
            // 
            this.Btn_Log.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Btn_Log.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Log.Location = new System.Drawing.Point(231, 6);
            this.Btn_Log.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.Btn_Log.Name = "Btn_Log";
            this.Btn_Log.Size = new System.Drawing.Size(189, 65);
            this.Btn_Log.TabIndex = 7;
            this.Btn_Log.Text = "LOG DATA";
            this.Btn_Log.UseVisualStyleBackColor = false;
            this.Btn_Log.Click += new System.EventHandler(this.Btn_Log_Click);
            this.Btn_Log.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Btn_Action_MouseDown);
            this.Btn_Log.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Btn_Action_MouseUp);
            // 
            // Btn_Pair
            // 
            this.Btn_Pair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Btn_Pair.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Pair.Location = new System.Drawing.Point(4, 76);
            this.Btn_Pair.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.Btn_Pair.Name = "Btn_Pair";
            this.Btn_Pair.Size = new System.Drawing.Size(194, 65);
            this.Btn_Pair.TabIndex = 4;
            this.Btn_Pair.Text = "측정 시작";
            this.Btn_Pair.UseVisualStyleBackColor = false;
            this.Btn_Pair.Click += new System.EventHandler(this.Btn_Pair_Click);
            this.Btn_Pair.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Btn_Action_MouseDown);
            this.Btn_Pair.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Btn_Action_MouseUp);
            // 
            // Btn_Unpair
            // 
            this.Btn_Unpair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Btn_Unpair.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Unpair.Location = new System.Drawing.Point(722, 730);
            this.Btn_Unpair.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.Btn_Unpair.Name = "Btn_Unpair";
            this.Btn_Unpair.Size = new System.Drawing.Size(167, 92);
            this.Btn_Unpair.TabIndex = 5;
            this.Btn_Unpair.Text = "페어링 삭제";
            this.Btn_Unpair.UseVisualStyleBackColor = false;
            this.Btn_Unpair.Click += new System.EventHandler(this.Btn_Unpair_Click);
            this.Btn_Unpair.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Btn_Action_MouseDown);
            this.Btn_Unpair.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Btn_Action_MouseUp);
            // 
            // LMessage
            // 
            this.LMessage.AutoSize = true;
            this.LMessage.Location = new System.Drawing.Point(661, 565);
            this.LMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LMessage.Name = "LMessage";
            this.LMessage.Size = new System.Drawing.Size(0, 30);
            this.LMessage.TabIndex = 12;
            // 
            // LMessage2
            // 
            this.LMessage2.AutoSize = true;
            this.LMessage2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LMessage2.Location = new System.Drawing.Point(1050, 608);
            this.LMessage2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LMessage2.Name = "LMessage2";
            this.LMessage2.Size = new System.Drawing.Size(17, 21);
            this.LMessage2.TabIndex = 13;
            this.LMessage2.Text = "-";
            // 
            // listDebug
            // 
            this.listDebug.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.listDebug.FormattingEnabled = true;
            this.listDebug.ItemHeight = 17;
            this.listDebug.Location = new System.Drawing.Point(0, 730);
            this.listDebug.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.listDebug.Name = "listDebug";
            this.listDebug.Size = new System.Drawing.Size(695, 157);
            this.listDebug.TabIndex = 14;
            // 
            // timer100
            // 
            this.timer100.Tick += new System.EventHandler(this.timer100_Tick);
            // 
            // timerPolling
            // 
            this.timerPolling.Interval = 2000;
            this.timerPolling.Tick += new System.EventHandler(this.timerPolling_Tick);
            // 
            // LWARNING
            // 
            this.LWARNING.AutoSize = true;
            this.LWARNING.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LWARNING.ForeColor = System.Drawing.Color.Red;
            this.LWARNING.Location = new System.Drawing.Point(1050, 643);
            this.LWARNING.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LWARNING.Name = "LWARNING";
            this.LWARNING.Size = new System.Drawing.Size(20, 25);
            this.LWARNING.TabIndex = 15;
            this.LWARNING.Text = "-";
            // 
            // timer3000
            // 
            this.timer3000.Interval = 3000;
            this.timer3000.Tick += new System.EventHandler(this.timer3000_Tick);
            // 
            // timer400
            // 
            this.timer400.Interval = 400;
            this.timer400.Tick += new System.EventHandler(this.timer400_Tick);
            // 
            // timer500
            // 
            this.timer500.Interval = 500;
            this.timer500.Tick += new System.EventHandler(this.timer500_Tick);
            // 
            // Btn_ScanAll
            // 
            this.Btn_ScanAll.Location = new System.Drawing.Point(897, 730);
            this.Btn_ScanAll.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.Btn_ScanAll.Name = "Btn_ScanAll";
            this.Btn_ScanAll.Size = new System.Drawing.Size(147, 47);
            this.Btn_ScanAll.TabIndex = 16;
            this.Btn_ScanAll.Text = "Scan All";
            this.Btn_ScanAll.UseVisualStyleBackColor = true;
            this.Btn_ScanAll.Click += new System.EventHandler(this.Btn_ScanAll_Click);
            // 
            // Btn_ListDevice
            // 
            this.Btn_ListDevice.Location = new System.Drawing.Point(897, 784);
            this.Btn_ListDevice.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.Btn_ListDevice.Name = "Btn_ListDevice";
            this.Btn_ListDevice.Size = new System.Drawing.Size(147, 38);
            this.Btn_ListDevice.TabIndex = 17;
            this.Btn_ListDevice.Text = "List Devices";
            this.Btn_ListDevice.UseVisualStyleBackColor = true;
            this.Btn_ListDevice.Click += new System.EventHandler(this.Btn_ListDevice_Click);
            // 
            // Btn_SNLoad
            // 
            this.Btn_SNLoad.Location = new System.Drawing.Point(722, 833);
            this.Btn_SNLoad.Name = "Btn_SNLoad";
            this.Btn_SNLoad.Size = new System.Drawing.Size(322, 40);
            this.Btn_SNLoad.TabIndex = 18;
            this.Btn_SNLoad.Text = "Load snlist.txt";
            this.Btn_SNLoad.UseVisualStyleBackColor = true;
            this.Btn_SNLoad.Click += new System.EventHandler(this.Btn_SNLoad_Click);
            // 
            // FormZ1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1500, 686);
            this.Controls.Add(this.Btn_SNLoad);
            this.Controls.Add(this.Btn_ListDevice);
            this.Controls.Add(this.Btn_ScanAll);
            this.Controls.Add(this.LWARNING);
            this.Controls.Add(this.Btn_Unpair);
            this.Controls.Add(this.listDebug);
            this.Controls.Add(this.LMessage2);
            this.Controls.Add(this.LMessage);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvForm);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormZ1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Z1 검사 툴 (버전 1.0.0.1)";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormZ1_FormClosed);
            this.Load += new System.EventHandler(this.FormZ1_Load);
            this.Shown += new System.EventHandler(this.FormZ1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvForm)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStd)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvForm;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button Btn_Pair;
        private System.Windows.Forms.Button Btn_Unpair;
        private System.Windows.Forms.Button Btn_Stop;
        private System.Windows.Forms.Button Btn_Log;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton RB_ALL;
        private System.Windows.Forms.RadioButton RB_AVG;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dgvStd;
        private System.Windows.Forms.Label LMessage;
        private System.Windows.Forms.Button Btn_Standard;
        private System.Windows.Forms.Label LMessage2;
        private System.Windows.Forms.ListBox listDebug;
        private System.Windows.Forms.Timer timer100;
        private System.Windows.Forms.Timer timerPolling;
        private System.Windows.Forms.Label LWARNING;
        private System.Windows.Forms.Timer timer3000;
        private System.Windows.Forms.Timer timer400;
        private System.Windows.Forms.Timer timer500;
        private System.Windows.Forms.Button Btn_ScanAll;
        private System.Windows.Forms.Button Btn_ListDevice;
        private System.Windows.Forms.Button Btn_Action;
        private System.Windows.Forms.DataGridViewTextBoxColumn TYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn SN;
        private System.Windows.Forms.DataGridViewTextBoxColumn PAIR;
        private System.Windows.Forms.DataGridViewTextBoxColumn TEMP;
        private System.Windows.Forms.DataGridViewTextBoxColumn HUMI;
        private System.Windows.Forms.DataGridViewTextBoxColumn TVOC;
        private System.Windows.Forms.DataGridViewTextBoxColumn FANS;
        private System.Windows.Forms.DataGridViewTextBoxColumn CO2;
        private System.Windows.Forms.DataGridViewTextBoxColumn BATT;
        private System.Windows.Forms.DataGridViewTextBoxColumn RESU;
        private System.Windows.Forms.Button Btn_SNLoad;
    }
}

