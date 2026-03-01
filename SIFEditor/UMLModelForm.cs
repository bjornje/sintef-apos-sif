using Sintef.Apos.Sif;

namespace SIFEditor
{
    public partial class UMLModelForm : Form
    {
        public UMLModelForm()
        {
            InitializeComponent();
            Text = $"UML Model {Definition.Version}"; 
        }
    }
}
