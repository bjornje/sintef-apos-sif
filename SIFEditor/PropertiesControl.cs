using Sintef.Apos.Sif.Model;
using System.Data;

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

            if (node == null)
            {
                return;
            }

            Type = node.GetType();

            if (node is CrossSubsystemGroups groups)
            {
                var control = new PropertyEditorControl(groups.NumberOfGroups_N);
                control.TextChanged += Control_TextChanged;
                flowLayoutPanel1.Controls.Add(control);
                _editorControls.Add(groups.NumberOfGroups_N.Name, control);

                var control2 = new PropertyEditorControl(groups.VoteBetweenGroups_M_in_MooN);
                control2.TextChanged += Control_TextChanged;
                flowLayoutPanel1.Controls.Add(control2);
                _editorControls.Add(groups.VoteBetweenGroups_M_in_MooN.Name, control2);
            }

            foreach (var property in node.Attributes.OrderByDescending(x => x.IsMandatory).ThenBy(x => x.Name))
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
            if (node is CrossSubsystemGroups groups)
            {
                _editorControls[groups.NumberOfGroups_N.Name].Show(groups.NumberOfGroups_N);
                _editorControls[groups.VoteBetweenGroups_M_in_MooN.Name].Show(groups.VoteBetweenGroups_M_in_MooN);
            }

            foreach (var property in node.Attributes)
            {
                _editorControls[property.Name].Show(property);
            }
        }

        public void Focus(string name)
        {
            _editorControls[name].Focus();
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
