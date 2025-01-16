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
            this.components = new System.ComponentModel.Container();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelType = new System.Windows.Forms.Label();
            this.toolTipProperty = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipPropertyType = new System.Windows.Forms.ToolTip(this.components);
            this.comboBoxValue = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelName.Location = new System.Drawing.Point(3, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(160, 23);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "property name";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxValue
            // 
            this.textBoxValue.Location = new System.Drawing.Point(157, 82);
            this.textBoxValue.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.Size = new System.Drawing.Size(32, 23);
            this.textBoxValue.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 166F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 366F));
            this.tableLayoutPanel1.Controls.Add(this.labelName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelType, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(665, 23);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelType.Location = new System.Drawing.Point(302, 0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(360, 23);
            this.labelType.TabIndex = 2;
            this.labelType.Text = "property type";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolTipProperty
            // 
            this.toolTipProperty.AutoPopDelay = 30000;
            this.toolTipProperty.InitialDelay = 500;
            this.toolTipProperty.ReshowDelay = 100;
            // 
            // comboBoxValue
            // 
            this.comboBoxValue.FormattingEnabled = true;
            this.comboBoxValue.Location = new System.Drawing.Point(243, 117);
            this.comboBoxValue.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxValue.Name = "comboBoxValue";
            this.comboBoxValue.Size = new System.Drawing.Size(68, 23);
            this.comboBoxValue.TabIndex = 3;
            // 
            // PropertyEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxValue);
            this.Controls.Add(this.textBoxValue);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PropertyEditorControl";
            this.Size = new System.Drawing.Size(665, 23);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
