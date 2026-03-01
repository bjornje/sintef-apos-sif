using Sintef.Apos.Sif.Model;

namespace SIFEditor
{
    public partial class OrderedListEditor : Form
    {
        private readonly IAttribute _orderedList;
        private IAttribute _currentItem;
        public OrderedListEditor(IAttribute orderdList)
        {
            InitializeComponent();

            _orderedList = orderdList;

            Text = $"List Editor - {orderdList.Name}";

            foreach (var item in orderdList.Items)
            {
                var newItem = _orderedList.CreateItem();
                newItem.StringValue = item.StringValue;

                AddItem(newItem);
            }
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

        private void Control_TextChanged(object? sender, EventArgs e)
        {
            if (sender is TextBox textBox && textBox.Tag is IAttribute attribute)
            {
                if (textBox.Text == null)
                {
                    attribute.StringValue = null;
                }
                else
                {
                    attribute.StringValue = textBox.Text;
                }
            }
            else if (sender is ComboBox comboBox && comboBox.Tag is IAttribute attribute2)
            {
                if (comboBox.Text == null)
                {
                    attribute2.StringValue = null;
                }
                else
                {
                    attribute2.StringValue = comboBox.Text;
                }
            }
        }

        public void FocusLast()
        {
            var last = flowLayoutPanel1.Controls[flowLayoutPanel1.Controls.Count - 1];

            if (last is not PropertyEditorControl propertyEditor)
            {
                return;
            }

            _currentItem = propertyEditor.Property;

            propertyEditor.Focus();

            RenderFocus();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var item = _orderedList.CreateItem();
            AddItem(item);

            FocusLast();
        }

        private void AddItem(IAttribute item)
        {
            var control = new PropertyEditorControl(item);

            control.Width = 380;
            control.TextChanged += Control_TextChanged;
            control.GotFocus += Control_GotFocus;
            flowLayoutPanel1.Controls.Add(control);
            control.Show(item);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                var control = flowLayoutPanel1.Controls[i];

                if (control.Tag == _currentItem)
                {
                    flowLayoutPanel1.Controls.Remove(control);

                    var index = i > flowLayoutPanel1.Controls.Count - 1 ? flowLayoutPanel1.Controls.Count - 1 : i;

                    if (index > -1 && flowLayoutPanel1.Controls[index].Tag is IAttribute property)
                    {
                        _currentItem = property;
                        RenderFocus();
                    }

                    break;
                }
            }
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            var n = flowLayoutPanel1.Controls.Count;

            for (var i = 1; i < n; i++)
            {
                var control = flowLayoutPanel1.Controls[i];

                if (control.Tag == _currentItem)
                {
                    flowLayoutPanel1.Controls.SetChildIndex(control, i - 1);
                    break;
                }
            }
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            var n = flowLayoutPanel1.Controls.Count - 1;

            for (var i = 0; i < n; i++)
            {
                var control = flowLayoutPanel1.Controls[i];

                if (control.Tag == _currentItem)
                {
                    flowLayoutPanel1.Controls.SetChildIndex(control, i + 1);
                    break;
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _orderedList.Items.Clear();

            foreach (var control in flowLayoutPanel1.Controls)
            {
                if (control is PropertyEditorControl propertyEditor && propertyEditor.Tag is IAttribute item && !string.IsNullOrEmpty(item.StringValue))
                {
                    _orderedList.Items.Add(item);
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            buttonAdd_Click(sender, e);
            timer1.Enabled = false;
        }
    }
}
