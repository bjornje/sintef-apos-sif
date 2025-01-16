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
    public partial class PropertiesControl : UserControl
    {

        public PropertiesControl()
        {
            InitializeComponent();


        }

        public void Show(Node node)
        {
            flowLayoutPanel1.Controls.Clear();

            if (node is SIFComponent sifComponent)
            {
                var control = new PropertyEditorControl(sifComponent.GroupVoter.M);
                flowLayoutPanel1.Controls.Add(control);
            }
            else if (node is Group group)
            {
                var control = new PropertyEditorControl(group.ComponentVoter.K);
                flowLayoutPanel1.Controls.Add(control);
            }
            else if (node is SISComponent sisComponent)
            {
                var control = new PropertyEditorControl(sisComponent.Name);
                flowLayoutPanel1.Controls.Add(control);
            }

            foreach (var property in node.Attributes)
            {
                var control = new PropertyEditorControl(property);
                flowLayoutPanel1.Controls.Add(control);
            }
        }

        public void Clear()
        {
            flowLayoutPanel1.Controls.Clear();
        }

        private void flowLayoutPanel1_Click(object sender, EventArgs e)
        {
            Parent.Focus();
        }
    }
}
