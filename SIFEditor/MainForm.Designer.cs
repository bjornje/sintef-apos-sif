namespace SIFEditor
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripSIFs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addSifToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripSIF = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addSubsystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logicSolverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.finalElementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSIFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripSubsystem = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSubsystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addGroupToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripSISComponent = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeComponentToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStripSIFs.SuspendLayout();
            this.contextMenuStripSIF.SuspendLayout();
            this.contextMenuStripSubsystem.SuspendLayout();
            this.contextMenuStripGroup.SuspendLayout();
            this.contextMenuStripSISComponent.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(368, 677);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.ForeColor = System.Drawing.Color.Red;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(734, 318);
            this.listBox1.TabIndex = 2;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 27);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1106, 677);
            this.splitContainer1.SplitterDistance = 368;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listBox1);
            this.splitContainer2.Size = new System.Drawing.Size(734, 677);
            this.splitContainer2.SplitterDistance = 355;
            this.splitContainer2.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1106, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // contextMenuStripSIFs
            // 
            this.contextMenuStripSIFs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSifToolStripMenuItem});
            this.contextMenuStripSIFs.Name = "contextMenuStrip1";
            this.contextMenuStripSIFs.Size = new System.Drawing.Size(115, 26);
            // 
            // addSifToolStripMenuItem
            // 
            this.addSifToolStripMenuItem.Name = "addSifToolStripMenuItem";
            this.addSifToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.addSifToolStripMenuItem.Text = "Add SIF";
            this.addSifToolStripMenuItem.Click += new System.EventHandler(this.addSifToolStripMenuItem_Click);
            // 
            // contextMenuStripSIF
            // 
            this.contextMenuStripSIF.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSubsystemToolStripMenuItem,
            this.removeSIFToolStripMenuItem});
            this.contextMenuStripSIF.Name = "contextMenuStripSIF";
            this.contextMenuStripSIF.Size = new System.Drawing.Size(172, 48);
            // 
            // addSubsystemToolStripMenuItem
            // 
            this.addSubsystemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inputDeviceToolStripMenuItem,
            this.logicSolverToolStripMenuItem,
            this.finalElementToolStripMenuItem});
            this.addSubsystemToolStripMenuItem.Name = "addSubsystemToolStripMenuItem";
            this.addSubsystemToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.addSubsystemToolStripMenuItem.Text = "Add SIFSubsystem";
            // 
            // inputDeviceToolStripMenuItem
            // 
            this.inputDeviceToolStripMenuItem.Name = "inputDeviceToolStripMenuItem";
            this.inputDeviceToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.inputDeviceToolStripMenuItem.Text = "InputDevice";
            this.inputDeviceToolStripMenuItem.Click += new System.EventHandler(this.inputDeviceToolStripMenuItem_Click);
            // 
            // logicSolverToolStripMenuItem
            // 
            this.logicSolverToolStripMenuItem.Name = "logicSolverToolStripMenuItem";
            this.logicSolverToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.logicSolverToolStripMenuItem.Text = "LogicSolver";
            this.logicSolverToolStripMenuItem.Click += new System.EventHandler(this.logicSolverToolStripMenuItem_Click);
            // 
            // finalElementToolStripMenuItem
            // 
            this.finalElementToolStripMenuItem.Name = "finalElementToolStripMenuItem";
            this.finalElementToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.finalElementToolStripMenuItem.Text = "FinalElement";
            this.finalElementToolStripMenuItem.Click += new System.EventHandler(this.finalElementToolStripMenuItem_Click);
            // 
            // removeSIFToolStripMenuItem
            // 
            this.removeSIFToolStripMenuItem.Name = "removeSIFToolStripMenuItem";
            this.removeSIFToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.removeSIFToolStripMenuItem.Text = "Remove SIF";
            this.removeSIFToolStripMenuItem.Click += new System.EventHandler(this.removeSIFToolStripMenuItem_Click);
            // 
            // contextMenuStripSubsystem
            // 
            this.contextMenuStripSubsystem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addGroupToolStripMenuItem,
            this.removeSubsystemToolStripMenuItem});
            this.contextMenuStripSubsystem.Name = "contextMenuStripSIFComponent";
            this.contextMenuStripSubsystem.Size = new System.Drawing.Size(178, 48);
            // 
            // addGroupToolStripMenuItem
            // 
            this.addGroupToolStripMenuItem.Name = "addGroupToolStripMenuItem";
            this.addGroupToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.addGroupToolStripMenuItem.Text = "Add Group";
            this.addGroupToolStripMenuItem.Click += new System.EventHandler(this.addGroupToolStripMenuItem_Click);
            // 
            // removeSubsystemToolStripMenuItem
            // 
            this.removeSubsystemToolStripMenuItem.Name = "removeSubsystemToolStripMenuItem";
            this.removeSubsystemToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.removeSubsystemToolStripMenuItem.Text = "Remove Subsystem";
            this.removeSubsystemToolStripMenuItem.Click += new System.EventHandler(this.removeSubsystemToolStripMenuItem_Click);
            // 
            // contextMenuStripGroup
            // 
            this.contextMenuStripGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addGroupToolStripMenuItem1,
            this.addComponentToolStripMenuItem,
            this.removeComponentToolStripMenuItem});
            this.contextMenuStripGroup.Name = "contextMenuStripGroup";
            this.contextMenuStripGroup.Size = new System.Drawing.Size(181, 92);
            // 
            // addGroupToolStripMenuItem1
            // 
            this.addGroupToolStripMenuItem1.Name = "addGroupToolStripMenuItem1";
            this.addGroupToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.addGroupToolStripMenuItem1.Text = "Add Group";
            this.addGroupToolStripMenuItem1.Click += new System.EventHandler(this.addGroupToolStripMenuItem1_Click);
            // 
            // addComponentToolStripMenuItem
            // 
            this.addComponentToolStripMenuItem.Name = "addComponentToolStripMenuItem";
            this.addComponentToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addComponentToolStripMenuItem.Text = "Add Component";
            this.addComponentToolStripMenuItem.Click += new System.EventHandler(this.addComponentToolStripMenuItem_Click);
            // 
            // removeComponentToolStripMenuItem
            // 
            this.removeComponentToolStripMenuItem.Name = "removeComponentToolStripMenuItem";
            this.removeComponentToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removeComponentToolStripMenuItem.Text = "Remove Group";
            this.removeComponentToolStripMenuItem.Click += new System.EventHandler(this.removeComponentToolStripMenuItem_Click);
            // 
            // contextMenuStripSISComponent
            // 
            this.contextMenuStripSISComponent.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeComponentToolStripMenuItem1});
            this.contextMenuStripSISComponent.Name = "contextMenuStripSISComponent";
            this.contextMenuStripSISComponent.Size = new System.Drawing.Size(185, 26);
            // 
            // removeComponentToolStripMenuItem1
            // 
            this.removeComponentToolStripMenuItem1.Name = "removeComponentToolStripMenuItem1";
            this.removeComponentToolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            this.removeComponentToolStripMenuItem1.Text = "Remove Component";
            this.removeComponentToolStripMenuItem1.Click += new System.EventHandler(this.removeComponentToolStripMenuItem1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 704);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "APOS SIF Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStripSIFs.ResumeLayout(false);
            this.contextMenuStripSIF.ResumeLayout(false);
            this.contextMenuStripSubsystem.ResumeLayout(false);
            this.contextMenuStripGroup.ResumeLayout(false);
            this.contextMenuStripSISComponent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TreeView treeView1;
        private ListBox listBox1;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ContextMenuStrip contextMenuStripSIFs;
        private ToolStripMenuItem addSifToolStripMenuItem;
        private ContextMenuStrip contextMenuStripSIF;
        private ToolStripMenuItem addSubsystemToolStripMenuItem;
        private ToolStripMenuItem removeSIFToolStripMenuItem;
        private ContextMenuStrip contextMenuStripSubsystem;
        private ToolStripMenuItem addGroupToolStripMenuItem;
        private ToolStripMenuItem removeSubsystemToolStripMenuItem;
        private ContextMenuStrip contextMenuStripGroup;
        private ToolStripMenuItem addComponentToolStripMenuItem;
        private ToolStripMenuItem removeComponentToolStripMenuItem;
        private ContextMenuStrip contextMenuStripSISComponent;
        private ToolStripMenuItem removeComponentToolStripMenuItem1;
        private ToolStripMenuItem addGroupToolStripMenuItem1;
        private ToolStripMenuItem inputDeviceToolStripMenuItem;
        private ToolStripMenuItem logicSolverToolStripMenuItem;
        private ToolStripMenuItem finalElementToolStripMenuItem;
    }
}