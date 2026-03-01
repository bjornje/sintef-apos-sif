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
            labelValue = new Label();
            labelType = new Label();
            toolTipProperty = new ToolTip(components);
            toolTipPropertyType = new ToolTip(components);
            comboBoxValue = new ComboBox();
            orderedListBoxValue = new OrderedListBox();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Dock = DockStyle.Fill;
            labelName.Location = new Point(3, 0);
            labelName.Name = "labelName";
            labelName.Size = new Size(268, 24);
            labelName.TabIndex = 0;
            labelName.Text = "property name";
            labelName.TextAlign = ContentAlignment.MiddleLeft;
            labelName.Click += labelName_Click;
            // 
            // textBoxValue
            // 
            textBoxValue.Location = new Point(157, 82);
            textBoxValue.Margin = new Padding(0);
            textBoxValue.Name = "textBoxValue";
            textBoxValue.Size = new Size(32, 23);
            textBoxValue.TabIndex = 1;
            textBoxValue.MouseClick += textBoxValue_MouseClick;
            textBoxValue.TextChanged += textBoxValue_TextChanged;
            textBoxValue.Leave += textBoxValue_Leave;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 274F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 341F));
            tableLayoutPanel1.Controls.Add(labelValue, 1, 0);
            tableLayoutPanel1.Controls.Add(labelName, 0, 0);
            tableLayoutPanel1.Controls.Add(labelType, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(816, 24);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // labelValue
            // 
            labelValue.AutoSize = true;
            labelValue.BackColor = Color.WhiteSmoke;
            labelValue.Dock = DockStyle.Fill;
            labelValue.Location = new Point(274, 0);
            labelValue.Margin = new Padding(0);
            labelValue.Name = "labelValue";
            labelValue.Size = new Size(201, 24);
            labelValue.TabIndex = 5;
            labelValue.Text = "labelValue";
            labelValue.TextAlign = ContentAlignment.MiddleLeft;
            labelValue.Click += labelValue_Click;
            // 
            // labelType
            // 
            labelType.AutoSize = true;
            labelType.Dock = DockStyle.Fill;
            labelType.Location = new Point(478, 0);
            labelType.Name = "labelType";
            labelType.Size = new Size(335, 24);
            labelType.TabIndex = 2;
            labelType.Text = "property type";
            labelType.TextAlign = ContentAlignment.MiddleLeft;
            labelType.Click += labelType_Click;
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
            comboBoxValue.Leave += comboBoxValue_Leave;
            comboBoxValue.MouseClick += comboBoxValue_MouseClick;
            // 
            // orderedListBoxValue
            // 
            orderedListBoxValue.Location = new Point(362, 175);
            orderedListBoxValue.Margin = new Padding(0);
            orderedListBoxValue.Name = "orderedListBoxValue";
            orderedListBoxValue.Size = new Size(237, 22);
            orderedListBoxValue.TabIndex = 4;
            orderedListBoxValue.Leave += orderedListBoxValue_Leave;
            orderedListBoxValue.MouseClick += orderedListBoxValue_MouseClick;
            // 
            // PropertyEditorControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(orderedListBoxValue);
            Controls.Add(comboBoxValue);
            Controls.Add(textBoxValue);
            Controls.Add(tableLayoutPanel1);
            Name = "PropertyEditorControl";
            Size = new Size(816, 24);
            BackColorChanged += PropertyEditorControl_BackColorChanged;
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
        private OrderedListBox orderedListBoxValue;
        private Label labelValue;
    }
}
