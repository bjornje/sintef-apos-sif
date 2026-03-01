using Sintef.Apos.Sif;
using Sintef.Apos.Sif.Model;

namespace SIFEditor
{
    public partial class AddAttributeForm : Form
    {
        public string AttributeName => textBoxName.Text;
        public string RefAttributeType => comboBoxType.Text;
        public string Target => comboBoxTarget.Text;
        public bool IsOrderedList => radioButtonOrderedList.Checked;
        public AddAttributeForm()
        {
            InitializeComponent();

            var targets = new List<string>
            {
                typeof(SafetyInstrumentedFunction).Name,
                typeof(InputDeviceSubsystem).Name,
                typeof(LogicSolverSubsystem).Name,
                typeof(FinalElementSubsystem).Name,
                typeof(InputDeviceRequirements).Name,
                typeof(LogicSolverRequirements).Name,
                typeof(FinalElementRequirements).Name
            };

            comboBoxTarget.DataSource = targets;
            comboBoxTarget.SelectedIndex = 0;

            comboBoxType.DataSource = Definition.GetTypes();
            comboBoxType.SelectedIndex = 0;
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            buttonOk.Enabled = !string.IsNullOrEmpty(textBoxName.Text);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
