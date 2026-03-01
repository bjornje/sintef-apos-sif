using Sintef.Apos.Sif.Model;
using System.Data;

namespace SIFEditor
{
    public partial class PropertiesControl : UserControl
    {
        public Type Type { get; }
        private readonly Dictionary<string, PropertyEditorControl> _editorControls = new();
        public event EventHandler TextChanged;
        private IAttribute _currentItem;

        public PropertiesControl(Node node = null)
        {
            InitializeComponent();

            if (node == null)
            {
                return;
            }

            Type = node.GetType();

            foreach (var property in node.Attributes.OrderByDescending(x => x.IsMandatory).ThenBy(x => x.Name))
            {
                AddControl(property);
            }
        }

        private PropertyEditorControl AddControl(IAttribute property)
        {
            var control = new PropertyEditorControl(property);

            control.TextChanged += Control_TextChanged;
            control.GotFocus += Control_GotFocus;
            flowLayoutPanel1.Controls.Add(control);
            _editorControls.Add(property.Name, control);

            return control;
        }
        private void Control_GotFocus(object? sender, EventArgs e)
        {
            if (sender is PropertyEditorControl propertyEditor)
            {
                propertyEditor.Focus();
                _currentItem = propertyEditor.Property;
                RenderFocus();
            }
        }

        private void Control_TextChanged(object? sender, EventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }

        public void Show(Node node)
        {
            var sortNeeded = false;

            foreach (var property in node.Attributes)
            {
                if (!_editorControls.TryGetValue(property.Name, out var control))
                {
                    control = AddControl(property);
                    sortNeeded = true;
                }

                control.Show(property);
            }

            if (sortNeeded)
            {
                SortControls();
            }
        }

        private void SortControls()
        {
            var desiredOrder = _editorControls.Values.OrderByDescending(x => x.Property.IsMandatory).ThenBy(x => x.Property.Name).ToList();

            for (var i = 0; i < desiredOrder.Count; i++)
            {
                var control = flowLayoutPanel1.Controls[i];

                if (control == desiredOrder[i])
                {
                    continue;
                }

                flowLayoutPanel1.Controls.Add(desiredOrder[i]);
            }
        }

        public void Focus(string name)
        {
            var control = _editorControls[name];
            _currentItem = control.Property;
            control.Focus();
            RenderFocus();
        }

        private void RenderFocus()
        {
            for (var i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                var control = flowLayoutPanel1.Controls[i];

                if (control.Tag == _currentItem)
                {
                    control.BackColor = Color.PaleTurquoise;
                }
                else
                {
                    control.BackColor = Color.Transparent;
                }
            }
        }

        private void flowLayoutPanel1_Click(object sender, EventArgs e)
        {
            Parent.Focus();
        }
    }
}
