namespace ServiceManagament
{
    partial class ServiceControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonController = new System.Windows.Forms.Button();
            this.labelServiceName = new System.Windows.Forms.Label();
            this.textBoxDesc = new System.Windows.Forms.TextBox();
            this.timerCountDown = new System.Windows.Forms.Timer(this.components);
            this.labelCountDown = new System.Windows.Forms.Label();
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.checkBoxShowWindow = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonController
            // 
            this.buttonController.ForeColor = System.Drawing.Color.Green;
            this.buttonController.Location = new System.Drawing.Point(868, 22);
            this.buttonController.Name = "buttonController";
            this.buttonController.Size = new System.Drawing.Size(144, 64);
            this.buttonController.TabIndex = 1;
            this.buttonController.Text = "Start";
            this.buttonController.UseVisualStyleBackColor = false;
            this.buttonController.Click += new System.EventHandler(this.ButtonController_Click);
            // 
            // labelServiceName
            // 
            this.labelServiceName.AutoEllipsis = true;
            this.labelServiceName.AutoSize = true;
            this.labelServiceName.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelServiceName.ForeColor = System.Drawing.Color.Red;
            this.labelServiceName.Location = new System.Drawing.Point(32, 22);
            this.labelServiceName.MaximumSize = new System.Drawing.Size(248, 48);
            this.labelServiceName.Name = "labelServiceName";
            this.labelServiceName.Size = new System.Drawing.Size(236, 48);
            this.labelServiceName.TabIndex = 3;
            this.labelServiceName.Text = "ServiceName";
            // 
            // textBoxDesc
            // 
            this.textBoxDesc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDesc.ForeColor = System.Drawing.Color.Gray;
            this.textBoxDesc.Location = new System.Drawing.Point(37, 85);
            this.textBoxDesc.Name = "textBoxDesc";
            this.textBoxDesc.ReadOnly = true;
            this.textBoxDesc.Size = new System.Drawing.Size(975, 28);
            this.textBoxDesc.TabIndex = 4;
            // 
            // timerCountDown
            // 
            this.timerCountDown.Enabled = true;
            this.timerCountDown.Interval = 1000;
            this.timerCountDown.Tick += new System.EventHandler(this.TimerCountDown_Tick);
            // 
            // labelCountDown
            // 
            this.labelCountDown.AutoSize = true;
            this.labelCountDown.Location = new System.Drawing.Point(323, 45);
            this.labelCountDown.Name = "labelCountDown";
            this.labelCountDown.Size = new System.Drawing.Size(70, 24);
            this.labelCountDown.TabIndex = 5;
            this.labelCountDown.Text = "     ";
            // 
            // linkLabel
            // 
            this.linkLabel.AutoSize = true;
            this.linkLabel.Location = new System.Drawing.Point(323, 44);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(58, 24);
            this.linkLabel.TabIndex = 6;
            this.linkLabel.TabStop = true;
            this.linkLabel.Text = "link";
            this.linkLabel.Visible = false;
            this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked);
            // 
            // checkBoxShowWindow
            // 
            this.checkBoxShowWindow.AutoSize = true;
            this.checkBoxShowWindow.Location = new System.Drawing.Point(700, 41);
            this.checkBoxShowWindow.Name = "checkBoxShowWindow";
            this.checkBoxShowWindow.Size = new System.Drawing.Size(162, 28);
            this.checkBoxShowWindow.TabIndex = 7;
            this.checkBoxShowWindow.Text = "ShowWindow";
            this.checkBoxShowWindow.UseVisualStyleBackColor = true;
            this.checkBoxShowWindow.CheckedChanged += new System.EventHandler(this.CheckBoxShowWindow_CheckedChanged);
            // 
            // ServiceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.checkBoxShowWindow);
            this.Controls.Add(this.linkLabel);
            this.Controls.Add(this.labelCountDown);
            this.Controls.Add(this.textBoxDesc);
            this.Controls.Add(this.labelServiceName);
            this.Controls.Add(this.buttonController);
            this.MaximumSize = new System.Drawing.Size(1040, 123);
            this.Name = "ServiceControl";
            this.Size = new System.Drawing.Size(1038, 121);
            this.Load += new System.EventHandler(this.ServiceControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonController;
        private System.Windows.Forms.Label labelServiceName;
        private System.Windows.Forms.TextBox textBoxDesc;
        private System.Windows.Forms.Timer timerCountDown;
        private System.Windows.Forms.Label labelCountDown;
        private System.Windows.Forms.LinkLabel linkLabel;
        private System.Windows.Forms.CheckBox checkBoxShowWindow;
    }
}
