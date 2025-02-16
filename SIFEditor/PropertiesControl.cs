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
using System.Xml.Linq;

namespace SIFEditor
{
    public partial class PropertiesControl : UserControl
    {
        public Type Type { get; }
        private readonly Dictionary<string, PropertyEditorControl> _editorControls = new();
        public event EventHandler TextChanged;

        public PropertiesControl(Node node = null)
        {
            InitializeComponent();

            if (node == null) return;

            Type = node.GetType();

            if (node is SISComponent sisComponent)
            {
                var control = new PropertyEditorControl(sisComponent.Name);
                control.TextChanged += Control_TextChanged;
                flowLayoutPanel1.Controls.Add(control);
                _editorControls.Add(sisComponent.Name.Name, control);
            }

            foreach (var property in node.Attributes.OrderBy(x => x.Name))
            {
                var control = new PropertyEditorControl(property);
                control.TextChanged += Control_TextChanged;
                flowLayoutPanel1.Controls.Add(control);
                _editorControls.Add(property.Name, control);
            }
        }

        private void Control_TextChanged(object? sender, EventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }

        public void Show(Node node)
        {
            if (node is SISComponent sisComponent)
            {
                _editorControls[sisComponent.Name.Name].Show(sisComponent.Name);
            }

            foreach (var property in node.Attributes)
            {
                _editorControls[property.Name].Show(property);
            }
        }

        public void Clear()
        {
            //flowLayoutPanel1.Controls.Clear();
        }

        private void flowLayoutPanel1_Click(object sender, EventArgs e)
        {
            Parent.Focus();
        }
    }
}
