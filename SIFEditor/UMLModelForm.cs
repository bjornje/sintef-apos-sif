using Sintef.Apos.Sif;

namespace SIFEditor
{
    public partial class UMLModelForm : Form
    {
        private double zoomFactor = 1.0;
        private const double ZoomStep = 1.1; // 10% zoom per step
        private Point startPanOffset;
        private bool isPanning = false;

        public UMLModelForm()
        {
            InitializeComponent();
            Text = $"UML Model {Definition.Version}";

            // Bind events programmatically if not done in designer
            pictureBox1.MouseWheel += PictureBox1_MouseWheel1;
            pictureBox1.MouseDown += PictureBox1_MouseDown2;
            pictureBox1.MouseMove += PictureBox1_MouseMove1;
            pictureBox1.MouseUp += PictureBox1_MouseUp1;

            // Ensure the picturebox starts at top-left
            pictureBox1.Location = new Point(0, 0);
        }

        private void PictureBox1_MouseUp1(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPanning = false;
                pictureBox1.Cursor = Cursors.Default;
            }
        }

        private void PictureBox1_MouseMove1(object? sender, MouseEventArgs e)
        {
            if (isPanning)
            {
                // Compute how far the mouse has moved
                int deltaX = startPanOffset.X - e.X;
                int deltaY = startPanOffset.Y - e.Y;

                // Shift the panel scroll bars by that delta
                panel1.AutoScrollPosition = new Point(
                    deltaX + panel1.HorizontalScroll.Value,
                    deltaY + panel1.VerticalScroll.Value
                );
            }
        }

        private void PictureBox1_MouseDown2(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPanning = true;
                pictureBox1.Cursor = Cursors.Hand;
                // Record where the drag started relative to the cursor position
                startPanOffset = e.Location;
            }
        }

        private void PictureBox1_MouseWheel1(object? sender, MouseEventArgs e)
        {
            if (pictureBox1.Image == null) return;

            // Store original mouse position relative to the PictureBox before scaling
            Point mousePosBeforeZoom = e.Location;

            // Calculate new zoom factor
            if (e.Delta > 0)
                zoomFactor *= ZoomStep;
            else
                zoomFactor /= ZoomStep;

            // Cap zoom levels to avoid UI freezing or division errors
            zoomFactor = Math.Max(0.1, Math.Min(zoomFactor, 10.0));

            // Calculate new size based on the original image aspect ratio
            int newWidth = (int)(pictureBox1.Image.Width * zoomFactor);
            int newHeight = (int)(pictureBox1.Image.Height * zoomFactor);

            // Update PictureBox size
            pictureBox1.Width = newWidth;
            pictureBox1.Height = newHeight;

            // Adjust scroll positions to keep the mouse pointer centered over the same image spot
            int scrollX = (int)(mousePosBeforeZoom.X * (e.Delta > 0 ? ZoomStep : (1.0 / ZoomStep))) - e.X;
            int scrollY = (int)(mousePosBeforeZoom.Y * (e.Delta > 0 ? ZoomStep : (1.0 / ZoomStep))) - e.Y;

            panel1.AutoScrollPosition = new Point(
                scrollX + panel1.HorizontalScroll.Value,
                scrollY + panel1.VerticalScroll.Value
            );
        }


    }
}
