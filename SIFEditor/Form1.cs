using Sintef.Apos.Sif;
using Sintef.Apos.Sif.Model;
using System.Reflection;

namespace SIFEditor
{
    public partial class Form1 : Form
    {
        private string? _fileName = null;
        private Builder? _builder;
        private readonly string _text;
        public Form1()
        {
            InitializeComponent();
            listBox1.ValueMember = "Message";

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            _text = $"APOS SIF Editor {version.Major}.{version.Minor}.{version.Build} UML Model {Definition.Version}";
            Text = _text;
        }

        private void RefreshTreeView()
        {
            var updateNeeded = false;
            var treeNodes = new List<TreeNode>();

            FindAllTreeNodes(treeView1.TopNode, treeNodes);

            foreach(var treeNode in treeNodes) 
            {
                if (treeNode.Tag is not Node node) continue;

                var text = node.DisplayName();
                if (treeNode.Text != text)
                {
                    treeNode.Text = text;
                    updateNeeded = true;
                }

            }

            if (updateNeeded) treeView1.Refresh();
        }

        private void AddRoot(Root root)
        {
            treeView1.Nodes.Clear();

            var node = new TreeNode(root.DisplayName());
            node.ContextMenuStrip = contextMenuStripSIFs;
            node.Tag = root;
            treeView1.Nodes.Add(node);
            AddSIFs(root.SIFs, node);

            treeView1.ExpandAll();
        }

        private void AddSIFs(SIFs sifs, TreeNode node)
        {
            foreach(var sif in sifs)
            {
                var childNode = new TreeNode(sif.DisplayName());
                childNode.ContextMenuStrip = contextMenuStripSIF;
                childNode.Tag = sif;
                node.Nodes.Add(childNode);
                AddSIFSubsystems(sif.Subsystems, childNode);
            }
        }

        private void AddSIFSubsystems(SIFSubsystems subsystems, TreeNode node)
        {
            foreach(var subsystem in subsystems)
            {
                var childNode = new TreeNode(subsystem.DisplayName());
                childNode.ContextMenuStrip = contextMenuStripSubsystem;
                childNode.Tag = subsystem;
                node.Nodes.Add(childNode);

                if (subsystem.InputDevice != null)
                {
                    var grandChildNode = new TreeNode(subsystem.InputDevice.DisplayName());
                    grandChildNode.ContextMenuStrip = contextMenuStripSIFComponent;
                    grandChildNode.Tag = subsystem.InputDevice;
                    childNode.Nodes.Add(grandChildNode);
                    AddGroups(subsystem.InputDevice.Groups, grandChildNode);
                }

                if (subsystem.LogicSolver != null)
                {
                    var grandChildNode = new TreeNode(subsystem.LogicSolver.DisplayName());
                    grandChildNode.ContextMenuStrip = contextMenuStripSIFComponent;
                    grandChildNode.Tag = subsystem.LogicSolver;
                    childNode.Nodes.Add(grandChildNode);
                    AddGroups(subsystem.LogicSolver.Groups, grandChildNode);
                }

                if (subsystem.FinalElement != null)
                {
                    var grandChildNode = new TreeNode(subsystem.FinalElement.DisplayName());
                    grandChildNode.ContextMenuStrip = contextMenuStripSIFComponent;
                    grandChildNode.Tag = subsystem.FinalElement;
                    childNode.Nodes.Add(grandChildNode);
                    AddGroups(subsystem.FinalElement.Groups, grandChildNode);
                }
            }
        }

        private void AddGroups(Groups groups, TreeNode node)
        {
            foreach (var group in groups)
            {
                var childNode = new TreeNode(group.DisplayName());
                childNode.ContextMenuStrip = contextMenuStripGroup;
                childNode.Tag = group;
                node.Nodes.Add(childNode);
                AddComponents(group.Components, childNode);
                AddGroups(group.Groups, childNode);
            }
        }

        private void AddComponents(SISComponents components, TreeNode node)
        {
            foreach (var component in components)
            {
                var childNode = new TreeNode(component.DisplayName());
                childNode.ContextMenuStrip = contextMenuStripSISComponent;
                childNode.Tag = component;
                node.Nodes.Add(childNode);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is not ModelError error) return;

            var list = new List<TreeNode>();
            FindAllTreeNodes(treeView1.TopNode, list);

            var result = list.SingleOrDefault(x => x.Tag == error.Node);

            if (result == null) return;

            treeView1.SelectedNode = result;
            result.EnsureVisible();
            treeView1.Focus();
        }

        private static void FindAllTreeNodes(TreeNode node, List<TreeNode> list)
        {
            if (node == null) return;

            list.Add(node);

            foreach(var childNode in node.Nodes)
            {
                if (childNode is TreeNode treeNode) FindAllTreeNodes(treeNode, list);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            if (treeView1.SelectedNode.Tag is not Node node) return;

            propertiesControl1.Show(node);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Focus();

            if (_builder != null && _builder.HasChanges)
            {
                var result = MessageBox.Show("Save changes?", "APOS SIF Editor", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Cancel) return;

                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                    return;
                }

            }

            _fileName = null;
            _builder = new Builder();

            listBox1.Items.Clear();
            propertiesControl1.Clear();

            AddRoot(_builder.SIFs.Parent);
            addSifToolStripMenuItem_Click(sender, e);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Focus();

            using var openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "AML-files|*.aml";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _fileName = openFileDialog.FileName;

                _builder = new Builder();

                _builder.LoadFromFile(openFileDialog.FileName);

                listBox1.Items.Clear();
                propertiesControl1.Clear();

                foreach (var error in _builder.Errors) listBox1.Items.Add(error);

                var root = _builder.Roots.FirstOrDefault();
                if (root != null) AddRoot(root);
            }
        }

        private bool _hasChanges;
        private void timer1_Tick(object sender, EventArgs e)
        {
            saveToolStripMenuItem.Enabled = _builder != null;
            saveAsToolStripMenuItem.Enabled = _builder != null;
            if (_builder == null) return;
 
            RefreshTreeView();

            var hasChanges = _builder.HasChanges;
            if (hasChanges || (_hasChanges != hasChanges))
            {
                _builder.Validate();
                listBox1.Items.Clear();
                foreach (var error in _builder.Errors) listBox1.Items.Add(error);
            }
            _hasChanges = hasChanges;

            var status = hasChanges ? "*" : "";
            var fileName = _fileName == null ? "New SIF Builder Document" : _fileName;

            var title = $"{_text} - {fileName}{status}";

            if (title != Text) Text = title;
        }

        private void addSifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.TopNode == null) return;

            if (treeView1.TopNode.Tag is Root root)
            {
                var sif = root.SIFs.Append("New SIF");
                var sifTreeNode = AppendTreeNode(sif, treeView1.TopNode, contextMenuStripSIF);

                //SIFSubsystem
                var subsystem = sif.Subsystems.Append();
                var subsystemTreeNode = AppendTreeNode(subsystem, sifTreeNode, contextMenuStripSubsystem);
                
                //InputDevice
                var inputDevice = subsystem.CreateInputDevice();
                var inputDeviceTreeNode = AppendTreeNode(inputDevice, subsystemTreeNode, contextMenuStripSIFComponent);

                var inputDeviceGroup = inputDevice.Groups.Append();
                var inputDeviceGroupTreeNode = AppendTreeNode(inputDeviceGroup, inputDeviceTreeNode, contextMenuStripGroup);

                var initiatorComponent = inputDeviceGroup.Components.Append("New Initiator Component");
                AppendTreeNode(initiatorComponent, inputDeviceGroupTreeNode, contextMenuStripSISComponent);

                //LogicSolver
                var logicSolver = subsystem.CreateLogicSolver();
                var logicSolverTreeNode = AppendTreeNode(logicSolver, subsystemTreeNode, contextMenuStripSIFComponent);

                var logicSolverGroup = logicSolver.Groups.Append();
                var logicSolverGroupTreeNode = AppendTreeNode(logicSolverGroup, logicSolverTreeNode, contextMenuStripGroup);

                var solverComponent = logicSolverGroup.Components.Append("New Solver Component");
                AppendTreeNode(solverComponent, logicSolverGroupTreeNode, contextMenuStripSISComponent);

                //FinalElement
                var finalElement = subsystem.CreateFinalElement();
                var finalElementTreeNode = AppendTreeNode(finalElement, subsystemTreeNode, contextMenuStripSIFComponent);

                var finalElementGroup = finalElement.Groups.Append();
                var finalElementGroupTreeNode = AppendTreeNode(finalElementGroup, finalElementTreeNode, contextMenuStripGroup);

                var fianlComponent = finalElementGroup.Components.Append("New Final Component");
                AppendTreeNode(fianlComponent, finalElementGroupTreeNode, contextMenuStripSISComponent);
            }
        }

        private static TreeNode AppendTreeNode(Node tag, TreeNode parent, ContextMenuStrip contextMenuStrip)
        {
            var treeNode = new TreeNode(tag.DisplayName());
            treeNode.ContextMenuStrip = contextMenuStrip;
            treeNode.Tag = tag;
            parent.Nodes.Add(treeNode);
            parent.Expand();
            return treeNode;
        }

        private void addSubsystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            if (treeView1.SelectedNode.Tag is SIF sif)
            {
                var subsystem = sif.Subsystems.Append();
                AppendTreeNode(subsystem, treeView1.SelectedNode, contextMenuStripSubsystem);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeView1.SelectedNode = e.Node;
            }
        }

        private void addInitiatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            if (treeView1.SelectedNode.Tag is SIFSubsystem subsystem)
            {
                if (subsystem.InputDevice != null)
                {
                    MessageBox.Show("InputDevice already exists.");
                    return;
                }
                var inputDevice = subsystem.CreateInputDevice();
                AppendTreeNode(inputDevice, treeView1.SelectedNode, contextMenuStripSIFComponent);
            }
        }

        private void addSolverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            if (treeView1.SelectedNode.Tag is SIFSubsystem subsystem)
            {
                if (subsystem.LogicSolver != null)
                {
                    MessageBox.Show("LogicSolver already exists.");
                    return;
                }
                var logicSolver = subsystem.CreateLogicSolver();
                AppendTreeNode(logicSolver, treeView1.SelectedNode, contextMenuStripSIFComponent);
            }
        }

        private void addFinalElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            if (treeView1.SelectedNode.Tag is SIFSubsystem subsystem)
            {
                if (subsystem.FinalElement != null)
                {
                    MessageBox.Show("FinalElement already exists.");
                    return;
                }
                var finalElement = subsystem.CreateFinalElement();
                AppendTreeNode(finalElement, treeView1.SelectedNode, contextMenuStripSIFComponent);
            }
        }

        private void addGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            if (treeView1.SelectedNode.Tag is SIFComponent component)
            {
                var group = component.Groups.Append();
                AppendTreeNode(group, treeView1.SelectedNode, contextMenuStripGroup);
            }
        }

        private void addComponentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            if (treeView1.SelectedNode.Tag is Group group)
            {
                var name = "New Component";
                if (group.Parent is InputDevice) name = "New Initiator Component";
                else if (group.Parent is LogicSolver) name = "New Solver Component";
                else if (group.Parent is FinalElement) name = "New Final Component";

                var component = group.Components.Append(name);
                AppendTreeNode(component, treeView1.SelectedNode, contextMenuStripSISComponent);
            }
        }

        private void removeComponentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            if (treeView1.SelectedNode.Tag is SISComponent component)
            {
                if (component.Parent is Group group) 
                {
                    group.Remove(component);
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                }
            }
        }

        private void removeComponentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            if (treeView1.SelectedNode.Tag is Group group)
            {
                if (group.Parent is Group parentGroup)
                {
                    parentGroup.Remove(group);
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                }
            }
        }

        private void removeSIFComponentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            if (treeView1.SelectedNode.Tag is SIFComponent component)
            {
                if (component.Parent is SIFSubsystem subsystem)
                {
                    subsystem.Remove(component);
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                }
            }
        }

        private void removeSIFSubsystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            if (treeView1.SelectedNode.Tag is SIFSubsystem subsystem)
            {
                if (subsystem.Parent is SIF sif)
                {
                    sif.Remove(subsystem);
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                }
            }
        }

        private void removeSIFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            if (treeView1.SelectedNode.Tag is SIF sif)
            {
                if (sif.Parent is Root root)
                {
                    root.SIFs.Remove(sif);
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Focus();

            if (_fileName == null)
            {
                using var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "AML-files|*.aml";

                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

                _fileName = saveFileDialog.FileName;
            }

            _builder?.SaveToFile(_fileName);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Focus();

            using var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "AML-files|*.aml";

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            _fileName = saveFileDialog.FileName;
            _builder?.SaveToFile(_fileName);
        }

        private void addGroupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            if (treeView1.SelectedNode.Tag is Group group)
            {
                var subGroup = group.Groups.Append();
                AppendTreeNode(subGroup, treeView1.SelectedNode, contextMenuStripGroup);
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            treeView1.Focus();

            if (_builder == null) return;

            if (_builder.HasChanges)
            {
                var answer = MessageBox.Show("Save changes before closing?", "SIF Editor", MessageBoxButtons.YesNoCancel);

                if (answer == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }

                if (answer == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
            }
        }
    }
}