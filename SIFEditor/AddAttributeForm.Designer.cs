namespace SIFEditor
{
    partial class AddAttributeForm
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
            tableLayoutPanel1 = new TableLayoutPanel();
            label1 = new Label();
            label2 = new Label();
            textBoxName = new TextBox();
            comboBoxType = new ComboBox();
            label4 = new Label();
            comboBoxTarget = new ComboBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            radioButtonSingleValue = new RadioButton();
            radioButtonOrderedList = new RadioButton();
            buttonOk = new Button();
            buttonCancel = new Button();
            tableLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17.73472F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 82.26528F));
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label2, 0, 1);
            tableLayoutPanel1.Controls.Add(textBoxName, 1, 0);
            tableLayoutPanel1.Controls.Add(comboBoxType, 1, 1);
            tableLayoutPanel1.Controls.Add(label4, 0, 3);
            tableLayoutPanel1.Controls.Add(comboBoxTarget, 1, 3);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 1, 2);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(671, 130);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(112, 29);
            label1.TabIndex = 0;
            label1.Text = "Name";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(3, 29);
            label2.Name = "label2";
            label2.Size = new Size(112, 29);
            label2.TabIndex = 1;
            label2.Text = "Type";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(121, 3);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(545, 23);
            textBoxName.TabIndex = 2;
            textBoxName.TextChanged += textBoxName_TextChanged;
            // 
            // comboBoxType
            // 
            comboBoxType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxType.FormattingEnabled = true;
            comboBoxType.Location = new Point(121, 32);
            comboBoxType.Name = "comboBoxType";
            comboBoxType.Size = new Size(545, 23);
            comboBoxType.TabIndex = 3;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(3, 100);
            label4.Name = "label4";
            label4.Size = new Size(112, 30);
            label4.TabIndex = 4;
            label4.Text = "Target";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // comboBoxTarget
            // 
            comboBoxTarget.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTarget.FormattingEnabled = true;
            comboBoxTarget.Items.AddRange(new object[] { "SafetyInstrumentedFunction", "InputDeviceSubsystem", "LogicSolverSubsystem", "FinalElementSubsystem", "InputDeviceRequirements", "LogicSolverDeviceRequirements", "FinalElementDeviceRequirements" });
            comboBoxTarget.Location = new Point(121, 103);
            comboBoxTarget.Name = "comboBoxTarget";
            comboBoxTarget.Size = new Size(280, 23);
            comboBoxTarget.TabIndex = 5;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(radioButtonSingleValue);
            flowLayoutPanel1.Controls.Add(radioButtonOrderedList);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(121, 61);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(547, 36);
            flowLayoutPanel1.TabIndex = 6;
            // 
            // radioButtonSingleValue
            // 
            radioButtonSingleValue.AutoSize = true;
            radioButtonSingleValue.Checked = true;
            radioButtonSingleValue.Location = new Point(3, 3);
            radioButtonSingleValue.Name = "radioButtonSingleValue";
            radioButtonSingleValue.Size = new Size(88, 19);
            radioButtonSingleValue.TabIndex = 6;
            radioButtonSingleValue.TabStop = true;
            radioButtonSingleValue.Text = "Single Value";
            radioButtonSingleValue.UseVisualStyleBackColor = true;
            // 
            // radioButtonOrderedList
            // 
            radioButtonOrderedList.AutoSize = true;
            radioButtonOrderedList.Location = new Point(97, 3);
            radioButtonOrderedList.Name = "radioButtonOrderedList";
            radioButtonOrderedList.Size = new Size(89, 19);
            radioButtonOrderedList.TabIndex = 6;
            radioButtonOrderedList.Text = "Ordered List";
            radioButtonOrderedList.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonOk.Enabled = false;
            buttonOk.Location = new Point(570, 181);
            buttonOk.Margin = new Padding(10);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(106, 26);
            buttonOk.TabIndex = 5;
            buttonOk.Text = "Ok";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Location = new Point(451, 181);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(106, 26);
            buttonCancel.TabIndex = 6;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // AddAttributeForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(695, 226);
            Controls.Add(buttonOk);
            Controls.Add(buttonCancel);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "AddAttributeForm";
            Text = "Add Attribute";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private Label label2;
        private TextBox textBoxName;
        private ComboBox comboBoxType;
        private Button buttonOk;
        private Button buttonCancel;
        private ComboBox comboBoxTarget;
        private Label label4;
        private FlowLayoutPanel flowLayoutPanel1;
        private RadioButton radioButtonSingleValue;
        private RadioButton radioButtonOrderedList;
    }
}