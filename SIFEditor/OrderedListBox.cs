namespace SIFEditor
{
    public partial class OrderedListBox : UserControl
    {
        public event EventHandler OpenEditorClick;
        public event EventHandler GotFocus;

        public OrderedListBox()
        {
            InitializeComponent();
        }

        public new string Text
        {
            get => label1.Text;
            set => label1.Text = value;
        }

        private void OrderedListBox_SizeChanged(object sender, EventArgs e)
        {
            label1.Width = button1.Left + 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenEditorClick?.Invoke(this, e);
        }

        public void FocusOnButton()
        {
            button1.Focus();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            GotFocus?.Invoke(this, e);
        }
    }
}
