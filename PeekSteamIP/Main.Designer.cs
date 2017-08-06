namespace PeekSteamIP
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gridIpList = new System.Windows.Forms.DataGridView();
            this.colIp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colElapsed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHttpStatusCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnBeginTest = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gridDnsList = new System.Windows.Forms.DataGridView();
            this.colDns = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDnsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDnsResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDnsResultStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gridIpList)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDnsList)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridIpList
            // 
            this.gridIpList.AllowUserToAddRows = false;
            this.gridIpList.AllowUserToDeleteRows = false;
            this.gridIpList.AllowUserToResizeRows = false;
            this.gridIpList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridIpList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridIpList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIp,
            this.colLocation,
            this.colElapsed,
            this.colStatus,
            this.colHttpStatusCode});
            this.gridIpList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridIpList.Location = new System.Drawing.Point(3, 3);
            this.gridIpList.Name = "gridIpList";
            this.gridIpList.ReadOnly = true;
            this.gridIpList.RowTemplate.Height = 23;
            this.gridIpList.ShowEditingIcon = false;
            this.gridIpList.Size = new System.Drawing.Size(759, 419);
            this.gridIpList.TabIndex = 0;
            this.gridIpList.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.gridIpList_SortCompare);
            // 
            // colIp
            // 
            this.colIp.HeaderText = "IP";
            this.colIp.Name = "colIp";
            this.colIp.ReadOnly = true;
            this.colIp.Width = 160;
            // 
            // colLocation
            // 
            this.colLocation.HeaderText = "位置";
            this.colLocation.Name = "colLocation";
            this.colLocation.ReadOnly = true;
            this.colLocation.Width = 130;
            // 
            // colElapsed
            // 
            this.colElapsed.HeaderText = "耗时（ms）";
            this.colElapsed.Name = "colElapsed";
            this.colElapsed.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "状态";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 180;
            // 
            // colHttpStatusCode
            // 
            this.colHttpStatusCode.HeaderText = "HTTP状态码";
            this.colHttpStatusCode.Name = "colHttpStatusCode";
            this.colHttpStatusCode.ReadOnly = true;
            // 
            // btnBeginTest
            // 
            this.btnBeginTest.Location = new System.Drawing.Point(7, 7);
            this.btnBeginTest.Name = "btnBeginTest";
            this.btnBeginTest.Size = new System.Drawing.Size(183, 23);
            this.btnBeginTest.TabIndex = 0;
            this.btnBeginTest.Text = "开始测试";
            this.btnBeginTest.UseVisualStyleBackColor = true;
            this.btnBeginTest.Click += new System.EventHandler(this.btnBeginTest_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 37);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(773, 451);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gridIpList);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(765, 425);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "IP";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gridDnsList);
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(766, 425);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "DNS";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gridDnsList
            // 
            this.gridDnsList.AllowUserToAddRows = false;
            this.gridDnsList.AllowUserToDeleteRows = false;
            this.gridDnsList.AllowUserToResizeRows = false;
            this.gridDnsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDnsList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDns,
            this.colDnsName,
            this.colDnsResult,
            this.colDnsResultStatus});
            this.gridDnsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDnsList.Location = new System.Drawing.Point(3, 43);
            this.gridDnsList.Name = "gridDnsList";
            this.gridDnsList.ReadOnly = true;
            this.gridDnsList.RowTemplate.Height = 23;
            this.gridDnsList.ShowEditingIcon = false;
            this.gridDnsList.Size = new System.Drawing.Size(760, 379);
            this.gridDnsList.TabIndex = 1;
            // 
            // colDns
            // 
            this.colDns.HeaderText = "DNS";
            this.colDns.Name = "colDns";
            this.colDns.ReadOnly = true;
            this.colDns.Width = 160;
            // 
            // colDnsName
            // 
            this.colDnsName.HeaderText = "名称";
            this.colDnsName.Name = "colDnsName";
            this.colDnsName.ReadOnly = true;
            this.colDnsName.Width = 200;
            // 
            // colDnsResult
            // 
            this.colDnsResult.HeaderText = "解析结果";
            this.colDnsResult.Name = "colDnsResult";
            this.colDnsResult.ReadOnly = true;
            this.colDnsResult.Width = 160;
            // 
            // colDnsResultStatus
            // 
            this.colDnsResultStatus.HeaderText = "状态";
            this.colDnsResultStatus.Name = "colDnsResultStatus";
            this.colDnsResultStatus.ReadOnly = true;
            this.colDnsResultStatus.Width = 180;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(760, 40);
            this.panel2.TabIndex = 0;
            this.panel2.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnBeginTest);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(773, 37);
            this.panel1.TabIndex = 1;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 488);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Name = "Main";
            this.Text = "Steam Websites\' IP Status";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridIpList)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDnsList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridIpList;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colElapsed;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHttpStatusCode;
        private System.Windows.Forms.Button btnBeginTest;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView gridDnsList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDns;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDnsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDnsResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDnsResultStatus;
    }
}

