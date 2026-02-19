using Sintef.Apos.Sif;
using Sintef.Apos.Sif.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
