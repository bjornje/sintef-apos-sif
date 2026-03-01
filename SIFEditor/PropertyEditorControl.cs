using Sintef.Apos.Sif;
using Sintef.Apos.Sif.Model;

namespace SIFEditor
{
    public partial class PropertyEditorControl : UserControl
    {
        public IAttribute Property { get; private set; }
        public Type ControlType { get; }
        public event EventHandler TextChanged;
        public event EventHandler GotFocus;
        public PropertyEditorControl(IAttribute property)
        {
            InitializeComponent();

            labelValue.Visible = true;
            textBoxValue.Visible = false;
            comboBoxValue.Visible = false;
            orderedListBoxValue.Visible = false;

            labelName.Text = property.Name;
            if (!property.IsMandatory)
            {
                if (property.IsPartOfModel)
                {
                    labelName.ForeColor = Color.Gray;
                    labelType.ForeColor = Color.Gray;
                }
                else
                {
                    labelName.ForeColor = Color.CadetBlue;
                    labelType.ForeColor = Color.CadetBlue;
                }
            }

            if (property.Owner == null)
            {
                labelName.Visible = false;
                tableLayoutPanel1.ColumnStyles[0].Width = 10;
                tableLayoutPanel1.ColumnStyles[2].Width = 90;
            }

            toolTipProperty.SetToolTip(labelName, property.Description);

            if (property.IsOrderedList)
            {
                var listItem = property.CreateItem();

                if (Definition.TryGetAttributeTypeName(listItem.RefAttributeType, out var typeName))
                {
                    labelType.Text = $"List of {typeName} values";
                }

                if (Definition.TryGetAttributeUnit(listItem.RefAttributeType, out var unit))
                {
                    labelType.Text = $"[{unit}] {labelType.Text}";
                }

                if (Definition.TryGetAttributeTypeDescription(listItem.RefAttributeType, out var description))
                {
                    toolTipPropertyType.SetToolTip(labelType, description);
                }

                orderedListBoxValue.OpenEditorClick += OrderedListBoxValue_OpenEditorClick;
                orderedListBoxValue.GotFocus += OrderedListBoxValue_GotFocus;
            }
            else
            {
                if (Definition.TryGetAttributeTypeName(property.RefAttributeType, out var typeName))
                {
                    labelType.Text = typeName;
                }

                if (Definition.TryGetAttributeUnit(property.RefAttributeType, out var unit))
                {
                    labelType.Text = $"[{unit}] {labelType.Text}";
                }

                if (Definition.TryGetAttributeTypeDescription(property.RefAttributeType, out var description))
                {
                    toolTipPropertyType.SetToolTip(labelType, description);
                }
            }


            if (property.IsOrderedList)
            {
                tableLayoutPanel1.Controls.Add(orderedListBoxValue, 1, 0);
                orderedListBoxValue.Dock = DockStyle.Fill;
                ControlType = orderedListBoxValue.GetType();
            }
            else if (Definition.TryGetAttributeTypeValues(property.RefAttributeType, out var values) || property.TryGetValueOptions(out values))
            {
                tableLayoutPanel1.Controls.Add(comboBoxValue, 1, 0);
                comboBoxValue.Dock = DockStyle.Fill;
                comboBoxValue.Items.Add("");
                foreach (var item in values)
                {
                    comboBoxValue.Items.Add(item);
                }

                ControlType = comboBoxValue.GetType();
            }
            else
            {
                tableLayoutPanel1.Controls.Add(textBoxValue, 1, 0);
                textBoxValue.Dock = DockStyle.Fill;
                ControlType = textBoxValue.GetType();

                if (property.RefAttributeType == "info")
                {
                    textBoxValue.Enabled = false;
                }
            }

            comboBoxValue.MouseWheel += new MouseEventHandler(comboBoxValue_MouseWheel);
        }

        private void OrderedListBoxValue_GotFocus(object? sender, EventArgs e)
        {
            GotFocus?.Invoke(this, e);
        }

        private void OrderedListBoxValue_OpenEditorClick(object? sender, EventArgs e)
        {

            if (orderedListBoxValue.Tag is not IAttribute ordetedList)
            {
                return;
            }

            GotFocus?.Invoke(this, e);

            var editor = new OrderedListEditor(ordetedList);

            if (editor.ShowDialog() == DialogResult.OK)
            {
                orderedListBoxValue.Text = string.Join(", ", ordetedList.Items.Select(x => x.StringValue));
                TextChanged?.Invoke(sender, e);
            }
        }

        public void Show(IAttribute property)
        {
            Property = property;
            Tag = property;

            if (ControlType == typeof(ComboBox))
            {
                comboBoxValue.Tag = property;
                comboBoxValue.Text = property.StringValue;
                labelValue.Text = property.StringValue;
            }
            else if (ControlType == typeof(OrderedListBox))
            {
                orderedListBoxValue.Tag = property;
                orderedListBoxValue.Text = string.Join(", ", property.Items.Select(x => x.StringValue));
                labelValue.Text = orderedListBoxValue.Text;
            }
            else
            {
                textBoxValue.Tag = property;
                textBoxValue.Text = property.StringValue;
                labelValue.Text = property.StringValue;
            }
        }

        public new void Focus()
        {
            ShowEditorControl();

            if (ControlType == typeof(ComboBox))
            {
                if (!comboBoxValue.DroppedDown)
                {
                    comboBoxValue.Focus();
                }
            }
            else if (ControlType == typeof(OrderedListBox))
            {
                orderedListBoxValue.FocusOnButton();
            }
            else
            {
                textBoxValue.Focus();
            }
        }

        void comboBoxValue_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void textBoxValue_TextChanged(object sender, EventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }

        private void comboBoxValue_TextChanged(object sender, EventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }

        private void textBoxValue_MouseClick(object sender, MouseEventArgs e)
        {
            GotFocus?.Invoke(this, e);
        }

        private void comboBoxValue_MouseClick(object sender, MouseEventArgs e)
        {
            GotFocus?.Invoke(this, e);
        }

        private void orderedListBoxValue_MouseClick(object sender, MouseEventArgs e)
        {
            GotFocus?.Invoke(this, e);
        }

        private void labelName_Click(object sender, EventArgs e)
        {
            HideEditorControl();
            GotFocus?.Invoke(this, e);
        }

        private void labelType_Click(object sender, EventArgs e)
        {
            HideEditorControl();
            GotFocus?.Invoke(this, e);
        }

        private void labelValue_Click(object sender, EventArgs e)
        {
            HideEditorControl();
            ShowEditorControl();
            GotFocus?.Invoke(this, e);
        }

        private void ShowEditorControl()
        {
            labelValue.Visible = false;
            textBoxValue.Visible = ControlType == typeof(TextBox);
            comboBoxValue.Visible = ControlType == typeof(ComboBox);
            orderedListBoxValue.Visible = ControlType == typeof(OrderedListBox);
        }

        private void HideEditorControl()
        {
            labelValue.Visible = true;

            if (textBoxValue.Visible)
            {
                textBoxValue.Visible = false;
                labelValue.Text = Property.StringValue;
                return;
            }

            if (comboBoxValue.Visible)
            {
                comboBoxValue.Visible = false;
                labelValue.Text = Property.StringValue;
                return;
            }

            if (orderedListBoxValue.Visible)
            {
                orderedListBoxValue.Visible = false;
                labelValue.Text = string.Join(", ", Property.Items.Select(x => x.StringValue));
            }
        }

        private void textBoxValue_Leave(object sender, EventArgs e)
        {
            HideEditorControl();
        }

        private void comboBoxValue_Leave(object sender, EventArgs e)
        {
            HideEditorControl();
        }

        private void orderedListBoxValue_Leave(object sender, EventArgs e)
        {
            HideEditorControl();
        }

        private void PropertyEditorControl_BackColorChanged(object sender, EventArgs e)
        {
            if (BackColor == Color.Transparent)
            {
                HideEditorControl();
            }
        }

        private void labelValue_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
