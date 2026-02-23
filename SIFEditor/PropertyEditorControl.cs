using Sintef.Apos.Sif;
using Sintef.Apos.Sif.Model.Attributes;

namespace SIFEditor
{
    public partial class PropertyEditorControl : UserControl
    {
        public Type Type { get; }
        public Type ControlType { get; }
        public event EventHandler TextChanged;
        public PropertyEditorControl(AttributeType property)
        {
            InitializeComponent();

            Type = property.GetType();

            labelName.Text = property.Name;
            if (!property.IsMandatory)
            {
                labelName.ForeColor = Color.Gray;
                labelType.ForeColor = Color.Gray;
            }

            toolTipProperty.SetToolTip(labelName, property.Description);

            labelType.Text = Type.Name;

            if (Definition.TryGetAttributeTypeDescription(property.RefAttributeType, out var description))
            {
                toolTipPropertyType.SetToolTip(labelType, description);
            }

            if (Definition.TryGetAttributeTypeValues(property.RefAttributeType, out var values))
            {
                tableLayoutPanel1.Controls.Add(comboBoxValue, 1, 0);
                comboBoxValue.Dock = DockStyle.Fill;
                comboBoxValue.Items.Add("");
                foreach(var item in values) comboBoxValue.Items.Add(item);
                ControlType = comboBoxValue.GetType();
            }
            else
            {
                tableLayoutPanel1.Controls.Add(textBoxValue,1, 0);
                textBoxValue.Dock = DockStyle.Fill;
                ControlType = textBoxValue.GetType();
            }
        }

        public void Show(AttributeType property)
        {
            if (ControlType == typeof(ComboBox))
            {
                comboBoxValue.Tag = property;
                comboBoxValue.Text = property.StringValue;
            }
            else
            {
                textBoxValue.Tag = property;
                textBoxValue.Text = property.StringValue;
            }
        }

        public new void Focus()
        {
            if (ControlType == typeof(ComboBox))
            {
                comboBoxValue.Focus();
            }
            else
            {
                textBoxValue.Focus();
            }
        }

        private void textBoxValue_TextChanged(object sender, EventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }

        private void comboBoxValue_TextChanged(object sender, EventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }
    }
}
