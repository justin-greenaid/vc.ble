﻿
namespace ble.test
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDialog2 = new System.Windows.Forms.PrintDialog();
            this.listDevice = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.listStatus = new System.Windows.Forms.ListBox();
            this.buttonShow = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.listService = new System.Windows.Forms.ListBox();
            this.listCharacteristic = new System.Windows.Forms.ListBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.button22 = new System.Windows.Forms.Button();
            this.button23 = new System.Windows.Forms.Button();
            this.button24 = new System.Windows.Forms.Button();
            this.button25 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(428, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 73);
            this.button1.TabIndex = 0;
            this.button1.Text = "Scan";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(428, 109);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 71);
            this.button2.TabIndex = 1;
            this.button2.Text = "List Devices";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // printDialog2
            // 
            this.printDialog2.UseEXDialog = true;
            // 
            // listDevice
            // 
            this.listDevice.FormattingEnabled = true;
            this.listDevice.HorizontalScrollbar = true;
            this.listDevice.ItemHeight = 15;
            this.listDevice.Location = new System.Drawing.Point(30, 30);
            this.listDevice.Name = "listDevice";
            this.listDevice.Size = new System.Drawing.Size(373, 334);
            this.listDevice.TabIndex = 2;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(428, 202);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(111, 71);
            this.button3.TabIndex = 3;
            this.button3.Text = "Connect";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(428, 769);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(111, 41);
            this.button4.TabIndex = 4;
            this.button4.Text = "Close";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // listStatus
            // 
            this.listStatus.FormattingEnabled = true;
            this.listStatus.HorizontalScrollbar = true;
            this.listStatus.ItemHeight = 15;
            this.listStatus.Location = new System.Drawing.Point(558, 30);
            this.listStatus.Name = "listStatus";
            this.listStatus.ScrollAlwaysVisible = true;
            this.listStatus.Size = new System.Drawing.Size(391, 784);
            this.listStatus.TabIndex = 5;
            // 
            // buttonShow
            // 
            this.buttonShow.Location = new System.Drawing.Point(428, 405);
            this.buttonShow.Name = "buttonShow";
            this.buttonShow.Size = new System.Drawing.Size(111, 63);
            this.buttonShow.TabIndex = 6;
            this.buttonShow.Text = "Show Service";
            this.buttonShow.UseVisualStyleBackColor = true;
            this.buttonShow.Click += new System.EventHandler(this.button5_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(428, 485);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(111, 65);
            this.button5.TabIndex = 8;
            this.button5.Text = "Set Service";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button6_Click);
            // 
            // listService
            // 
            this.listService.FormattingEnabled = true;
            this.listService.ItemHeight = 15;
            this.listService.Location = new System.Drawing.Point(30, 388);
            this.listService.Name = "listService";
            this.listService.Size = new System.Drawing.Size(373, 244);
            this.listService.TabIndex = 9;
            // 
            // listCharacteristic
            // 
            this.listCharacteristic.FormattingEnabled = true;
            this.listCharacteristic.ItemHeight = 15;
            this.listCharacteristic.Location = new System.Drawing.Point(30, 653);
            this.listCharacteristic.Name = "listCharacteristic";
            this.listCharacteristic.Size = new System.Drawing.Size(373, 154);
            this.listCharacteristic.TabIndex = 10;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(428, 653);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(111, 62);
            this.button6.TabIndex = 11;
            this.button6.Text = "Show Characteristic";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button7_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(428, 721);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(111, 42);
            this.button7.TabIndex = 12;
            this.button7.Text = "Read Value";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button8_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1465, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(991, 32);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(147, 25);
            this.textBox1.TabIndex = 15;
            this.textBox1.Text = "VC Z1 5B7A";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(991, 369);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(147, 40);
            this.button9.TabIndex = 16;
            this.button9.Text = "Battery Level";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(991, 169);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(147, 40);
            this.button10.TabIndex = 17;
            this.button10.Text = "온도";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(991, 215);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(147, 40);
            this.button11.TabIndex = 18;
            this.button11.Text = "습도";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(991, 261);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(147, 40);
            this.button12.TabIndex = 19;
            this.button12.Text = "TVOC";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(991, 307);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(147, 40);
            this.button13.TabIndex = 20;
            this.button13.Text = "FAN Speed(RPM)";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(991, 429);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(147, 40);
            this.button14.TabIndex = 21;
            this.button14.Text = "Dispose";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(991, 538);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(147, 40);
            this.button15.TabIndex = 22;
            this.button15.Text = "Multiple Test";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(991, 507);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(90, 25);
            this.textBox2.TabIndex = 23;
            this.textBox2.Text = "30";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1087, 510);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 15);
            this.label1.TabIndex = 24;
            this.label1.Text = "Sec";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(991, 63);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(147, 40);
            this.button8.TabIndex = 25;
            this.button8.Text = "Scan";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click_1);
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(1144, 63);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(147, 40);
            this.button16.TabIndex = 27;
            this.button16.Text = "Scan";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click_1);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(1144, 32);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(147, 25);
            this.textBox3.TabIndex = 26;
            this.textBox3.Text = "VC Z1 9F82";
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(1317, 63);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(73, 40);
            this.button17.TabIndex = 28;
            this.button17.Text = "Pairing";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(1317, 103);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(73, 40);
            this.button18.TabIndex = 29;
            this.button18.Text = "UnPair";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(1317, 149);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(73, 40);
            this.button19.TabIndex = 30;
            this.button19.Text = "Run Pair/Unpair";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // button20
            // 
            this.button20.Location = new System.Drawing.Point(1144, 429);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(147, 40);
            this.button20.TabIndex = 36;
            this.button20.Text = "Dispose";
            this.button20.UseVisualStyleBackColor = true;
            // 
            // button21
            // 
            this.button21.Location = new System.Drawing.Point(1144, 307);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(147, 40);
            this.button21.TabIndex = 35;
            this.button21.Text = "FAN Speed(RPM)";
            this.button21.UseVisualStyleBackColor = true;
            this.button21.Click += new System.EventHandler(this.button21_Click);
            // 
            // button22
            // 
            this.button22.Location = new System.Drawing.Point(1144, 261);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(147, 40);
            this.button22.TabIndex = 34;
            this.button22.Text = "TVOC";
            this.button22.UseVisualStyleBackColor = true;
            this.button22.Click += new System.EventHandler(this.button22_Click);
            // 
            // button23
            // 
            this.button23.Location = new System.Drawing.Point(1144, 215);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(147, 40);
            this.button23.TabIndex = 33;
            this.button23.Text = "습도";
            this.button23.UseVisualStyleBackColor = true;
            this.button23.Click += new System.EventHandler(this.button23_Click);
            // 
            // button24
            // 
            this.button24.Location = new System.Drawing.Point(1144, 169);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(147, 40);
            this.button24.TabIndex = 32;
            this.button24.Text = "온도";
            this.button24.UseVisualStyleBackColor = true;
            this.button24.Click += new System.EventHandler(this.button24_Click);
            // 
            // button25
            // 
            this.button25.Location = new System.Drawing.Point(1144, 369);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(147, 40);
            this.button25.TabIndex = 31;
            this.button25.Text = "Battery Level";
            this.button25.UseVisualStyleBackColor = true;
            this.button25.Click += new System.EventHandler(this.button25_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1465, 828);
            this.Controls.Add(this.button20);
            this.Controls.Add(this.button21);
            this.Controls.Add(this.button22);
            this.Controls.Add(this.button23);
            this.Controls.Add(this.button24);
            this.Controls.Add(this.button25);
            this.Controls.Add(this.button19);
            this.Controls.Add(this.button18);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.listCharacteristic);
            this.Controls.Add(this.listService);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.buttonShow);
            this.Controls.Add(this.listStatus);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.listDevice);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.PrintDialog printDialog2;
        private System.Windows.Forms.ListBox listDevice;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ListBox listStatus;
        private System.Windows.Forms.Button buttonShow;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ListBox listService;
        private System.Windows.Forms.ListBox listCharacteristic;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.Button button22;
        private System.Windows.Forms.Button button23;
        private System.Windows.Forms.Button button24;
        private System.Windows.Forms.Button button25;
    }
}

