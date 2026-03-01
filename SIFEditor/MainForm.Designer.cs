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
            components = new System.ComponentModel.Container();
            treeView1 = new TreeView();
            listBox1 = new ListBox();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItemUMLModel = new ToolStripMenuItem();
            contextMenuStripSIFs = new ContextMenuStrip(components);
            addSifToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStripSIF = new ContextMenuStrip(components);
            addSubsystemToolStripMenuItem = new ToolStripMenuItem();
            inputDeviceToolStripMenuItem = new ToolStripMenuItem();
            logicSolverToolStripMenuItem = new ToolStripMenuItem();
            finalElementToolStripMenuItem = new ToolStripMenuItem();
            removeSIFToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStripSubsystem = new ContextMenuStrip(components);
            addGroupToolStripMenuItem = new ToolStripMenuItem();
            removeSubsystemToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStripGroup = new ContextMenuStrip(components);
            addGroupToolStripMenuItem1 = new ToolStripMenuItem();
            addComponentToolStripMenuItem = new ToolStripMenuItem();
            removeComponentToolStripMenuItem = new ToolStripMenuItem();
            setAsCrossVotingGroupToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStripSISComponent = new ContextMenuStrip(components);
            removeComponentToolStripMenuItem1 = new ToolStripMenuItem();
            contextMenuCrossGroup = new ContextMenuStrip(components);
            removeFromCrossVotingToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStripDocuments = new ContextMenuStrip(components);
            addDocumentToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStripDocument = new ContextMenuStrip(components);
            removeDocumentToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItemAddAttribute = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            menuStrip1.SuspendLayout();
            contextMenuStripSIFs.SuspendLayout();
            contextMenuStripSIF.SuspendLayout();
            contextMenuStripSubsystem.SuspendLayout();
            contextMenuStripGroup.SuspendLayout();
            contextMenuStripSISComponent.SuspendLayout();
            contextMenuCrossGroup.SuspendLayout();
            contextMenuStripDocuments.SuspendLayout();
            contextMenuStripDocument.SuspendLayout();
            SuspendLayout();
            // 
            // treeView1
            // 
            treeView1.Dock = DockStyle.Fill;
            treeView1.Location = new Point(0, 0);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(368, 677);
            treeView1.TabIndex = 0;
            treeView1.AfterSelect += treeView1_AfterSelect;
            treeView1.NodeMouseClick += treeView1_NodeMouseClick;
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.ForeColor = Color.Red;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(0, 0);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(734, 318);
            listBox1.TabIndex = 2;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(0, 27);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(treeView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(1106, 677);
            splitContainer1.SplitterDistance = 368;
            splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(listBox1);
            splitContainer2.Size = new Size(734, 677);
            splitContainer2.SplitterDistance = 355;
            splitContainer2.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, viewToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1106, 24);
            menuStrip1.TabIndex = 4;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(123, 22);
            newToolStripMenuItem.Text = "New";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(123, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(123, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(123, 22);
            saveAsToolStripMenuItem.Text = "Save As...";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemAddAttribute });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "Edit";
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemUMLModel });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(44, 20);
            viewToolStripMenuItem.Text = "View";
            // 
            // toolStripMenuItemUMLModel
            // 
            toolStripMenuItemUMLModel.Name = "toolStripMenuItemUMLModel";
            toolStripMenuItemUMLModel.Size = new Size(136, 22);
            toolStripMenuItemUMLModel.Text = "UML Model";
            toolStripMenuItemUMLModel.Click += toolStripMenuItemUMLModel_Click;
            // 
            // contextMenuStripSIFs
            // 
            contextMenuStripSIFs.Items.AddRange(new ToolStripItem[] { addSifToolStripMenuItem });
            contextMenuStripSIFs.Name = "contextMenuStrip1";
            contextMenuStripSIFs.Size = new Size(115, 26);
            // 
            // addSifToolStripMenuItem
            // 
            addSifToolStripMenuItem.Name = "addSifToolStripMenuItem";
            addSifToolStripMenuItem.Size = new Size(114, 22);
            addSifToolStripMenuItem.Text = "Add SIF";
            addSifToolStripMenuItem.Click += addSifToolStripMenuItem_Click;
            // 
            // contextMenuStripSIF
            // 
            contextMenuStripSIF.Items.AddRange(new ToolStripItem[] { addSubsystemToolStripMenuItem, removeSIFToolStripMenuItem });
            contextMenuStripSIF.Name = "contextMenuStripSIF";
            contextMenuStripSIF.Size = new Size(172, 48);
            // 
            // addSubsystemToolStripMenuItem
            // 
            addSubsystemToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { inputDeviceToolStripMenuItem, logicSolverToolStripMenuItem, finalElementToolStripMenuItem });
            addSubsystemToolStripMenuItem.Name = "addSubsystemToolStripMenuItem";
            addSubsystemToolStripMenuItem.Size = new Size(171, 22);
            addSubsystemToolStripMenuItem.Text = "Add SIFSubsystem";
            // 
            // inputDeviceToolStripMenuItem
            // 
            inputDeviceToolStripMenuItem.Name = "inputDeviceToolStripMenuItem";
            inputDeviceToolStripMenuItem.Size = new Size(142, 22);
            inputDeviceToolStripMenuItem.Text = "InputDevice";
            inputDeviceToolStripMenuItem.Click += inputDeviceToolStripMenuItem_Click;
            // 
            // logicSolverToolStripMenuItem
            // 
            logicSolverToolStripMenuItem.Name = "logicSolverToolStripMenuItem";
            logicSolverToolStripMenuItem.Size = new Size(142, 22);
            logicSolverToolStripMenuItem.Text = "LogicSolver";
            logicSolverToolStripMenuItem.Click += logicSolverToolStripMenuItem_Click;
            // 
            // finalElementToolStripMenuItem
            // 
            finalElementToolStripMenuItem.Name = "finalElementToolStripMenuItem";
            finalElementToolStripMenuItem.Size = new Size(142, 22);
            finalElementToolStripMenuItem.Text = "FinalElement";
            finalElementToolStripMenuItem.Click += finalElementToolStripMenuItem_Click;
            // 
            // removeSIFToolStripMenuItem
            // 
            removeSIFToolStripMenuItem.Name = "removeSIFToolStripMenuItem";
            removeSIFToolStripMenuItem.Size = new Size(171, 22);
            removeSIFToolStripMenuItem.Text = "Remove SIF";
            removeSIFToolStripMenuItem.Click += removeSIFToolStripMenuItem_Click;
            // 
            // contextMenuStripSubsystem
            // 
            contextMenuStripSubsystem.Items.AddRange(new ToolStripItem[] { addGroupToolStripMenuItem, removeSubsystemToolStripMenuItem });
            contextMenuStripSubsystem.Name = "contextMenuStripSIFComponent";
            contextMenuStripSubsystem.Size = new Size(178, 48);
            // 
            // addGroupToolStripMenuItem
            // 
            addGroupToolStripMenuItem.Name = "addGroupToolStripMenuItem";
            addGroupToolStripMenuItem.Size = new Size(177, 22);
            addGroupToolStripMenuItem.Text = "Add Group";
            addGroupToolStripMenuItem.Click += addGroupToolStripMenuItem_Click;
            // 
            // removeSubsystemToolStripMenuItem
            // 
            removeSubsystemToolStripMenuItem.Name = "removeSubsystemToolStripMenuItem";
            removeSubsystemToolStripMenuItem.Size = new Size(177, 22);
            removeSubsystemToolStripMenuItem.Text = "Remove Subsystem";
            removeSubsystemToolStripMenuItem.Click += removeSubsystemToolStripMenuItem_Click;
            // 
            // contextMenuStripGroup
            // 
            contextMenuStripGroup.Items.AddRange(new ToolStripItem[] { addGroupToolStripMenuItem1, addComponentToolStripMenuItem, removeComponentToolStripMenuItem, setAsCrossVotingGroupToolStripMenuItem });
            contextMenuStripGroup.Name = "contextMenuStripGroup";
            contextMenuStripGroup.Size = new Size(207, 92);
            // 
            // addGroupToolStripMenuItem1
            // 
            addGroupToolStripMenuItem1.Name = "addGroupToolStripMenuItem1";
            addGroupToolStripMenuItem1.Size = new Size(206, 22);
            addGroupToolStripMenuItem1.Text = "Add Group";
            addGroupToolStripMenuItem1.Click += addGroupToolStripMenuItem1_Click;
            // 
            // addComponentToolStripMenuItem
            // 
            addComponentToolStripMenuItem.Name = "addComponentToolStripMenuItem";
            addComponentToolStripMenuItem.Size = new Size(206, 22);
            addComponentToolStripMenuItem.Text = "Add Component";
            addComponentToolStripMenuItem.Click += addComponentToolStripMenuItem_Click;
            // 
            // removeComponentToolStripMenuItem
            // 
            removeComponentToolStripMenuItem.Name = "removeComponentToolStripMenuItem";
            removeComponentToolStripMenuItem.Size = new Size(206, 22);
            removeComponentToolStripMenuItem.Text = "Remove Group";
            removeComponentToolStripMenuItem.Click += removeComponentToolStripMenuItem_Click;
            // 
            // setAsCrossVotingGroupToolStripMenuItem
            // 
            setAsCrossVotingGroupToolStripMenuItem.Name = "setAsCrossVotingGroupToolStripMenuItem";
            setAsCrossVotingGroupToolStripMenuItem.Size = new Size(206, 22);
            setAsCrossVotingGroupToolStripMenuItem.Text = "Set as cross voting group";
            setAsCrossVotingGroupToolStripMenuItem.Click += setAsCrossVotingGroupToolStripMenuItem_Click;
            // 
            // contextMenuStripSISComponent
            // 
            contextMenuStripSISComponent.Items.AddRange(new ToolStripItem[] { removeComponentToolStripMenuItem1 });
            contextMenuStripSISComponent.Name = "contextMenuStripSISComponent";
            contextMenuStripSISComponent.Size = new Size(185, 26);
            // 
            // removeComponentToolStripMenuItem1
            // 
            removeComponentToolStripMenuItem1.Name = "removeComponentToolStripMenuItem1";
            removeComponentToolStripMenuItem1.Size = new Size(184, 22);
            removeComponentToolStripMenuItem1.Text = "Remove Component";
            removeComponentToolStripMenuItem1.Click += removeComponentToolStripMenuItem1_Click;
            // 
            // contextMenuCrossGroup
            // 
            contextMenuCrossGroup.Items.AddRange(new ToolStripItem[] { removeFromCrossVotingToolStripMenuItem });
            contextMenuCrossGroup.Name = "contextMenuCrossGroup";
            contextMenuCrossGroup.Size = new Size(214, 26);
            // 
            // removeFromCrossVotingToolStripMenuItem
            // 
            removeFromCrossVotingToolStripMenuItem.Name = "removeFromCrossVotingToolStripMenuItem";
            removeFromCrossVotingToolStripMenuItem.Size = new Size(213, 22);
            removeFromCrossVotingToolStripMenuItem.Text = "Remove from cross voting";
            removeFromCrossVotingToolStripMenuItem.Click += removeFromCrossVotingToolStripMenuItem_Click;
            // 
            // contextMenuStripDocuments
            // 
            contextMenuStripDocuments.Items.AddRange(new ToolStripItem[] { addDocumentToolStripMenuItem });
            contextMenuStripDocuments.Name = "contextMenuStripDocuments";
            contextMenuStripDocuments.Size = new Size(156, 26);
            // 
            // addDocumentToolStripMenuItem
            // 
            addDocumentToolStripMenuItem.Name = "addDocumentToolStripMenuItem";
            addDocumentToolStripMenuItem.Size = new Size(155, 22);
            addDocumentToolStripMenuItem.Text = "Add Document";
            addDocumentToolStripMenuItem.Click += addDocumentToolStripMenuItem_Click;
            // 
            // contextMenuStripDocument
            // 
            contextMenuStripDocument.Items.AddRange(new ToolStripItem[] { removeDocumentToolStripMenuItem });
            contextMenuStripDocument.Name = "contextMenuStripDocument";
            contextMenuStripDocument.Size = new Size(177, 26);
            // 
            // removeDocumentToolStripMenuItem
            // 
            removeDocumentToolStripMenuItem.Name = "removeDocumentToolStripMenuItem";
            removeDocumentToolStripMenuItem.Size = new Size(176, 22);
            removeDocumentToolStripMenuItem.Text = "Remove Document";
            removeDocumentToolStripMenuItem.Click += removeDocumentToolStripMenuItem_Click;
            // 
            // toolStripMenuItemAddAttribute
            // 
            toolStripMenuItemAddAttribute.Name = "toolStripMenuItemAddAttribute";
            toolStripMenuItemAddAttribute.Size = new Size(180, 22);
            toolStripMenuItemAddAttribute.Text = "Add Attribute...";
            toolStripMenuItemAddAttribute.Click += toolStripMenuItemAddAttribute_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1106, 704);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "APOS SIF Editor";
            FormClosing += Form1_FormClosing;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            contextMenuStripSIFs.ResumeLayout(false);
            contextMenuStripSIF.ResumeLayout(false);
            contextMenuStripSubsystem.ResumeLayout(false);
            contextMenuStripGroup.ResumeLayout(false);
            contextMenuStripSISComponent.ResumeLayout(false);
            contextMenuCrossGroup.ResumeLayout(false);
            contextMenuStripDocuments.ResumeLayout(false);
            contextMenuStripDocument.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

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
        private ToolStripMenuItem setAsCrossVotingGroupToolStripMenuItem;
        private ContextMenuStrip contextMenuCrossGroup;
        private ToolStripMenuItem removeFromCrossVotingToolStripMenuItem;
        private ContextMenuStrip contextMenuStripDocuments;
        private ContextMenuStrip contextMenuStripDocument;
        private ToolStripMenuItem removeDocumentToolStripMenuItem;
        private ToolStripMenuItem addDocumentToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItemUMLModel;
        private ToolStripMenuItem toolStripMenuItemAddAttribute;
    }
}