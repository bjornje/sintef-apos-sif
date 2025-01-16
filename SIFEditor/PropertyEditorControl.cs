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
        public PropertyEditorControl(AttributeType property)
        {
            InitializeComponent();

            labelName.Text = property.Name;
            toolTipProperty.SetToolTip(labelName, property.Description);




            labelType.Text = property.RefAttributeType.Substring(6);
            if (Definition.TryGetAttributeTypeDescription(labelType.Text, out var description)) toolTipPropertyType.SetToolTip(labelType, description);

            if (Definition.TryGetAttributeTypeValues(labelType.Text, out var values))
            {
                tableLayoutPanel1.Controls.Add(comboBoxValue, 1, 0);
                comboBoxValue.Dock = DockStyle.Fill;
                comboBoxValue.Items.Add("");
                foreach(var item in values) comboBoxValue.Items.Add(item);
                comboBoxValue.DataBindings.Add("Text", property, "Value");
            }
            else
            {
                tableLayoutPanel1.Controls.Add(textBoxValue,1, 0);
                textBoxValue.Dock = DockStyle.Fill;
                textBoxValue.DataBindings.Add("Text", property, "Value");
            }
        }
    }
}
