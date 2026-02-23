namespace SIFEditor
{
    partial class PropertyEditorControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            labelName = new Label();
            textBoxValue = new TextBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            labelType = new Label();
            toolTipProperty = new ToolTip(components);
            toolTipPropertyType = new ToolTip(components);
            comboBoxValue = new ComboBox();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Dock = DockStyle.Fill;
            labelName.Location = new Point(3, 0);
            labelName.Name = "labelName";
            labelName.Size = new Size(268, 23);
            labelName.TabIndex = 0;
            labelName.Text = "property name";
            labelName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBoxValue
            // 
            textBoxValue.Location = new Point(157, 82);
            textBoxValue.Margin = new Padding(0);
            textBoxValue.Name = "textBoxValue";
            textBoxValue.Size = new Size(32, 23);
            textBoxValue.TabIndex = 1;
            textBoxValue.TextChanged += textBoxValue_TextChanged;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 274F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 206F));
            tableLayoutPanel1.Controls.Add(labelName, 0, 0);
            tableLayoutPanel1.Controls.Add(labelType, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(665, 23);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // labelType
            // 
            labelType.AutoSize = true;
            labelType.Dock = DockStyle.Fill;
            labelType.Location = new Point(462, 0);
            labelType.Name = "labelType";
            labelType.Size = new Size(200, 23);
            labelType.TabIndex = 2;
            labelType.Text = "property type";
            labelType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // toolTipProperty
            // 
            toolTipProperty.AutoPopDelay = 30000;
            toolTipProperty.InitialDelay = 500;
            toolTipProperty.ReshowDelay = 100;
            // 
            // comboBoxValue
            // 
            comboBoxValue.FormattingEnabled = true;
            comboBoxValue.Location = new Point(243, 117);
            comboBoxValue.Margin = new Padding(0);
            comboBoxValue.Name = "comboBoxValue";
            comboBoxValue.Size = new Size(68, 23);
            comboBoxValue.TabIndex = 3;
            comboBoxValue.TextChanged += comboBoxValue_TextChanged;
            // 
            // PropertyEditorControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(comboBoxValue);
            Controls.Add(textBoxValue);
            Controls.Add(tableLayoutPanel1);
            Name = "PropertyEditorControl";
            Size = new Size(665, 23);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private Label labelName;
        private TextBox textBoxValue;
        private TableLayoutPanel tableLayoutPanel1;
        private Label labelType;
        private ToolTip toolTipProperty;
        private ToolTip toolTipPropertyType;
        private ComboBox comboBoxValue;
    }
}
