using Aml.Engine.Services;
using Sintef.Apos.Sif;
using Sintef.Apos.Sif.Model;
using System;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Boolean = Sintef.Apos.Sif.Model.Boolean;

namespace SIFEditor
{
    public partial class MainForm : Form
    {
        private string? _fileName = null;
        private Builder? _builder;
        private readonly string _text;
        private readonly List<PropertiesControl> _nodeDetails = new();
        private readonly PropertiesControl _propertieBlankPage = new();
        public MainForm()
        {
            InitializeComponent();
            listBox1.ValueMember = "Message";

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            _text = $"APOS SIF Editor {version.Major}.{version.Minor}.{version.Build} UML Model {Definition.Version}";
            Text = _text;

            _propertieBlankPage.Width = splitContainer2.Panel1.Width;
            _propertieBlankPage.Height = splitContainer2.Panel1.Height;
            _propertieBlankPage.Dock = DockStyle.Fill;

            splitContainer2.Panel1.Controls.Add(_propertieBlankPage);
        }

        private void RefreshTreeView()
        {
            var updateNeeded = false;
            var treeNodes = new List<TreeNode>();

            FindAllTreeNodes(treeView1.TopNode, treeNodes);

            foreach (var treeNode in treeNodes)
            {
                if (treeNode.Tag is not Node node) continue;

                var text = node.DisplayName(treeNode.Parent);
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

            var node = new TreeNode(root.DisplayName(new TreeNode()));
            node.ContextMenuStrip = contextMenuStripSIFs;
            node.Tag = root;
            treeView1.Nodes.Add(node);
            AddSIFs(root.SIFs, node);

            treeView1.ExpandAll();
        }

        private void AddSIFs(SIFs sifs, TreeNode node)
        {
            foreach (var sif in sifs)
            {
                var childNode = new TreeNode(sif.DisplayName(node));
                childNode.ContextMenuStrip = contextMenuStripSIF;
                childNode.Tag = sif;
                node.Nodes.Add(childNode);

                var crossSubsystemTreeNode = AppendTreeNode(sif.CrossSubsystemGroups, childNode, null);

                AddCrossSubsystemGroups(sif.CrossSubsystemGroups, crossSubsystemTreeNode);
                AddSIFSubsystems(sif.Subsystems, childNode);
            }
        }

        private void AddSIFSubsystems(SIFSubsystems subsystems, TreeNode node)
        {
            foreach (var subsystem in subsystems)
            {
                var childNode = new TreeNode(subsystem.DisplayName(node));
                childNode.ContextMenuStrip = contextMenuStripSubsystem;
                childNode.Tag = subsystem;
                node.Nodes.Add(childNode);

                AddGroups(subsystem.Groups, childNode);
            }
        }

        private void AddCrossSubsystemGroups(CrossSubsystemGroups groups, TreeNode node)
        {
            foreach (var group in groups)
            {
                var childNode = new TreeNode(group.DisplayName(node));
                childNode.ContextMenuStrip = contextMenuCrossGroup;
                childNode.Tag = group;
                node.Nodes.Add(childNode);
            }
        }

        private void AddGroups(Groups groups, TreeNode node)
        {
            foreach (var group in groups)
            {
                var childNode = new TreeNode(group.DisplayName(node));
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
                var childNode = new TreeNode(component.DisplayName(node));
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

            var result = list.FirstOrDefault(x => x.Tag == error.Node && x.Parent.Tag is not CrossSubsystemGroups);


            if (result == null) return;

            treeView1.SelectedNode = result;
            result.EnsureVisible();
            treeView1.Focus();
        }

        private static void FindAllTreeNodes(TreeNode node, List<TreeNode> list)
        {
            if (node == null) return;

            list.Add(node);

            foreach (var childNode in node.Nodes)
            {
                if (childNode is TreeNode treeNode) FindAllTreeNodes(treeNode, list);
            }
        }

        private bool _ignoreTextChanged = false;
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            if (treeView1.SelectedNode.Tag is not Node node) return;

            _ignoreTextChanged = true;

            var control = _nodeDetails.FirstOrDefault(x => x.Type == node.GetType());
            if (control == null)
            {
                control = new PropertiesControl(node);
                control.TextChanged += Control_TextChanged;
                control.Width = splitContainer2.Panel1.Width;
                control.Height = splitContainer2.Panel1.Height;
                control.Dock = DockStyle.Fill;
                _nodeDetails.Add(control);
            }

            splitContainer2.Panel1.Controls.Clear();
            splitContainer2.Panel1.Controls.Add(control);
            control.Show(node);

            _ignoreTextChanged = false;
        }

        private void Control_TextChanged(object? sender, EventArgs e)
        {
            if (_ignoreTextChanged) return;

            if (sender is TextBox textBox && textBox.Tag is AttributeType attributeType)
            {
                if (string.IsNullOrEmpty(textBox.Text)) attributeType.StringValue = null;
                else attributeType.StringValue = textBox.Text;
            }
            else if (sender is ComboBox comboBox && comboBox.Tag is AttributeType attributeType2)
            {
                var valueBefore = attributeType2.ObjectValue;

                if (string.IsNullOrEmpty(comboBox.Text)) attributeType2.StringValue = null;
                else attributeType2.StringValue = comboBox.Text;

                if (attributeType2.Owner is Group group && attributeType2 == group.AllowAnyComponents)
                {
                    var change = CrossVotingGroupChange(valueBefore as bool?, group.AllowAnyComponents.Value);
                    if (change == 1)
                    {
                        var treeNodes = new List<TreeNode>();
                        FindAllTreeNodes(treeView1.TopNode, treeNodes);
                        var crossGroupsTreeNode = treeNodes.FirstOrDefault(x => x.Tag == group.GetSIF().CrossSubsystemGroups);

                        if (crossGroupsTreeNode != null && crossGroupsTreeNode.Tag is CrossSubsystemGroups groups)
                        {
                            groups.Add(group);
                            var childNode = new TreeNode(group.DisplayName(crossGroupsTreeNode));
                            childNode.ContextMenuStrip = contextMenuCrossGroup;
                            childNode.Tag = group;
                            crossGroupsTreeNode.Nodes.Add(childNode);
                        }
                    }
                    else if (change == -1)
                    {
                        var treeNodes = new List<TreeNode>();
                        FindAllTreeNodes(treeView1.TopNode, treeNodes);
                        var crossGroupsTreeNode = treeNodes.FirstOrDefault(x => x.Tag == group.GetSIF().CrossSubsystemGroups);
                        if (crossGroupsTreeNode != null && crossGroupsTreeNode.Tag is CrossSubsystemGroups groups)
                        {
                            var groupTreeNode = FindSubNode(crossGroupsTreeNode, group);
                            if (groupTreeNode.Tag == group)
                            {
                                groups.Remove(group);
                                crossGroupsTreeNode.Nodes.Remove(groupTreeNode);
                            }
                        }
                    }
                }
            }

            TreeChanged();
        }

        private static TreeNode FindSubNode(TreeNode treeNode, Node node)
        {
            foreach (TreeNode subTreeNode in treeNode.Nodes)
            {
                if (subTreeNode.Tag == node) return subTreeNode;
            }

            return new TreeNode();
        }

        private static int CrossVotingGroupChange(bool? valueBefore, bool? valueAfter)
        {
            if (!valueBefore.HasValue) valueBefore = false;
            if (!valueAfter.HasValue) valueAfter = false;

            if (valueBefore.Value && !valueAfter.Value) return -1;
            if (!valueBefore.Value && valueAfter.Value) return 1;
            return 0;
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

            AddRoot(_builder.SIFs.Parent);
            addSifToolStripMenuItem_Click(sender, e);

            splitContainer2.Panel1.Controls.Clear();
            splitContainer2.Panel1.Controls.Add(_propertieBlankPage);

            TreeChanged();
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

                foreach (var error in _builder.Errors) listBox1.Items.Add(error);

                var root = _builder.Roots.FirstOrDefault();
                if (root != null) AddRoot(root);

                splitContainer2.Panel1.Controls.Clear();
                splitContainer2.Panel1.Controls.Add(_propertieBlankPage);

                TreeChanged();
            }
        }

        private bool _hasChanges;
        private void TreeChanged()
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

                var crossSubsystemTreeNode = AppendTreeNode(sif.CrossSubsystemGroups, sifTreeNode, null);

                //InputDeviceSubsystem
                var inputDeviceSubsystem = sif.Subsystems.AppendInputDevice();
                inputDeviceSubsystem.MInVotingMooN.ObjectValue = 1;
                inputDeviceSubsystem.NumberOfGroups.ObjectValue = 1;

                var inputDeviceSubsystemTreeNode = AppendTreeNode(inputDeviceSubsystem, sifTreeNode, contextMenuStripSubsystem);

                var inputDeviceGroup = inputDeviceSubsystem.Groups.Append();
                inputDeviceGroup.MInVotingMooN.ObjectValue = 1;
                inputDeviceGroup.NumberOfDevicesWithinGroup.ObjectValue = 1;

                var inputDeviceGroupTreeNode = AppendTreeNode(inputDeviceGroup, inputDeviceSubsystemTreeNode, contextMenuStripGroup);

                var initiatorComponent = inputDeviceGroup.Components.Append();
                AppendTreeNode(initiatorComponent, inputDeviceGroupTreeNode, contextMenuStripSISComponent);

                //LogicSolverSubsystem
                var logicSolverSubsystem = sif.Subsystems.AppendLogicSolver();
                logicSolverSubsystem.MInVotingMooN.ObjectValue = 1;
                logicSolverSubsystem.NumberOfGroups.ObjectValue = 1;

                var logicSolverSubsystemTreeNode = AppendTreeNode(logicSolverSubsystem, sifTreeNode, contextMenuStripSubsystem);

                var logicSolverGroup = logicSolverSubsystem.Groups.Append();
                logicSolverGroup.MInVotingMooN.ObjectValue = 1;
                logicSolverGroup.NumberOfDevicesWithinGroup.ObjectValue = 1;

                var logicSolverGroupTreeNode = AppendTreeNode(logicSolverGroup, logicSolverSubsystemTreeNode, contextMenuStripGroup);

                var solverComponent = logicSolverGroup.Components.Append();
                AppendTreeNode(solverComponent, logicSolverGroupTreeNode, contextMenuStripSISComponent);

                //FinalElementSubsystem
                var finalElementSubsystem = sif.Subsystems.AppendFinalElement();
                finalElementSubsystem.MInVotingMooN.ObjectValue = 1;
                finalElementSubsystem.NumberOfGroups.ObjectValue = 1;

                var finalElementSubsystemTreeNode = AppendTreeNode(finalElementSubsystem, sifTreeNode, contextMenuStripSubsystem);

                var finalElementGroup = finalElementSubsystem.Groups.Append();
                finalElementGroup.MInVotingMooN.ObjectValue = 1;
                finalElementGroup.NumberOfDevicesWithinGroup.ObjectValue = 1;

                var finalElementGroupTreeNode = AppendTreeNode(finalElementGroup, finalElementSubsystemTreeNode, contextMenuStripGroup);

                var finalComponent = finalElementGroup.Components.Append();
                AppendTreeNode(finalComponent, finalElementGroupTreeNode, contextMenuStripSISComponent);

                TreeChanged();
            }
        }

        private static TreeNode AppendTreeNode(Node tag, TreeNode parent, ContextMenuStrip contextMenuStrip)
        {
            var treeNode = new TreeNode(tag.DisplayName(parent));
            treeNode.ContextMenuStrip = contextMenuStrip;
            treeNode.Tag = tag;
            parent.Nodes.Add(treeNode);
            parent.Expand();
            return treeNode;
        }

        private static void AdjustMenuItemEnabled(ContextMenuStrip menuStrip, string key, bool isEnabled)
        {
            var item = menuStrip.Items.Find(key, true).FirstOrDefault();
            if (item == null) return;

            item.Enabled = isEnabled;
        }

        private static void AdjustMenuItemChecked(ContextMenuStrip menuStrip, string key, bool? isChecked)
        {
            var item = menuStrip.Items.Find(key, true).FirstOrDefault();
            if (item is not ToolStripMenuItem menuItem) return;

            menuItem.Checked = isChecked ?? false;
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.Node.Tag is SIF sif)
                {
                    AdjustMenuItemEnabled(e.Node.ContextMenuStrip, "inputDeviceToolStripMenuItem", sif.InputDevice == null);
                    AdjustMenuItemEnabled(e.Node.ContextMenuStrip, "logicSolverToolStripMenuItem", sif.LogicSolver == null);
                    AdjustMenuItemEnabled(e.Node.ContextMenuStrip, "finalElementToolStripMenuItem", sif.FinalElement == null);
                }
                else if (e.Node.Tag is Group group)
                {
                    AdjustMenuItemEnabled(e.Node.ContextMenuStrip, "setAsCrossVotingGroupToolStripMenuItem", !group.AllowAnyComponents.Value.HasValue || group.AllowAnyComponents.Value == false);
                }

                treeView1.SelectedNode = e.Node;
            }
        }

        private void addGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is SIFSubsystem subsystem)
            {
                var group = subsystem.Groups.Append();
                group.MInVotingMooN.ObjectValue = 1;
                group.NumberOfDevicesWithinGroup.ObjectValue = 1;

                AppendTreeNode(group, treeView1.SelectedNode, contextMenuStripGroup);
                TreeChanged();
            }
        }

        private void addComponentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Group group)
            {
                var component = group.Components.Append();
                AppendTreeNode(component, treeView1.SelectedNode, contextMenuStripSISComponent);
                TreeChanged();
            }
        }

        private void removeComponentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is SISComponent component && component.Parent is Group group)
            {
                group.Remove(component);
                treeView1.Nodes.Remove(treeView1.SelectedNode);
                TreeChanged();
            }
        }

        private void removeComponentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Group group && group.Parent is Group parentGroup)
            {
                parentGroup.Remove(group);
                treeView1.Nodes.Remove(treeView1.SelectedNode);
                TreeChanged();
            }
            else if (treeView1.SelectedNode.Tag is Group subsystemGroup && subsystemGroup.Parent is SIFSubsystem subsystem)
            {
                subsystem.Groups.Remove(subsystemGroup);
                treeView1.Nodes.Remove(treeView1.SelectedNode);
                TreeChanged();
            }
        }


        private void removeSubsystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is SIFSubsystem subsystem && subsystem.Parent is SIF sif)
            {
                sif.Remove(subsystem);
                treeView1.Nodes.Remove(treeView1.SelectedNode);
                TreeChanged();
            }
        }

        private void removeSIFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is SIF sif && sif.Parent is Root root)
            {
                root.SIFs.Remove(sif);
                treeView1.Nodes.Remove(treeView1.SelectedNode);
                TreeChanged();
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
            TreeChanged();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Focus();

            using var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "AML-files|*.aml|Text-files|*.txt";

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            if (saveFileDialog.FileName.EndsWith(".txt"))
            {
                SaveAsTextFile(saveFileDialog.FileName);
                return;
            }

            _fileName = saveFileDialog.FileName;

            _builder?.SaveToFile(_fileName);
            TreeChanged();
        }

        private void SaveAsTextFile(string fileName)
        {
            if (_builder == null)
            {
                return;
            }

            var sb = new StringBuilder();
            AppendLine(sb, "Path", "SIFID", "Component name", "Attribute name", "Attribute value");

            foreach(var sif in _builder.SIFs)
            {
                foreach(var attribute in sif.Attributes)
                {
                    AppendLine(sb, sif.GetPath(sif.Parent), sif.SIFID.Value, "", attribute.Name, attribute.StringValue);

                }

                foreach (var subsystem in sif.Subsystems)
                {
                    foreach(var attribute in subsystem.Attributes)
                    {
                        AppendLine(sb, subsystem.GetPath(sif.Parent), sif.SIFID.Value, "", attribute.Name, attribute.StringValue);
                    }
                }

                foreach (var group in sif.GetAllGroups())
                {
                    AppendGroup(sb, sif, group);
                }
            }

            File.WriteAllText(fileName, sb.ToString());
        }

        private static void AppendGroup(StringBuilder sb, SIF sif, Group group)
        {
            foreach (var attribute in group.Attributes)
            {
                AppendLine(sb, group.GetPath(sif.Parent), sif.SIFID.Value, "", attribute.Name, attribute.StringValue);
            }

            foreach (var component in group.Components)
            {
                foreach(var attribute in component.Attributes)
                {
                    AppendLine(sb, component.GetPath(sif.Parent), sif.SIFID.Value, component.Name.Value, attribute.Name, attribute.StringValue);
                }
            }
        }

        private static void AppendLine(StringBuilder sb, string col1, string col2, string col3, string col4, string col5)
        {
            sb.AppendLine($"{col1}\t{col2}\t{col3}\t{col4}\t{col5}");
        }

        private void addGroupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is Group group)
            {
                var subGroup = group.Groups.Append();
                subGroup.MInVotingMooN.ObjectValue = 1;
                subGroup.NumberOfDevicesWithinGroup.ObjectValue = 1;

                AppendTreeNode(subGroup, treeView1.SelectedNode, contextMenuStripGroup);

                TreeChanged();
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

        private void inputDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is SIF sif)
            {
                var subsystem = sif.Subsystems.AppendInputDevice();
                subsystem.MInVotingMooN.ObjectValue = 1;
                subsystem.NumberOfGroups.ObjectValue = 1;

                AppendTreeNode(subsystem, treeView1.SelectedNode, contextMenuStripSubsystem);
                TreeChanged();
            }
        }

        private void logicSolverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is SIF sif)
            {
                var subsystem = sif.Subsystems.AppendLogicSolver();
                subsystem.MInVotingMooN.ObjectValue = 1;
                subsystem.NumberOfGroups.ObjectValue = 1;

                AppendTreeNode(subsystem, treeView1.SelectedNode, contextMenuStripSubsystem);
                TreeChanged();
            }
        }

        private void finalElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is SIF sif)
            {
                var subsystem = sif.Subsystems.AppendFinalElement();
                subsystem.MInVotingMooN.ObjectValue = 1;
                subsystem.NumberOfGroups.ObjectValue = 1;

                AppendTreeNode(subsystem, treeView1.SelectedNode, contextMenuStripSubsystem);
                TreeChanged();
            }
        }

        private void setAsCrossVotingGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is Group group)
            {
                group.AllowAnyComponents.Value = true;
                var treeNodes = new List<TreeNode>();
                FindAllTreeNodes(treeView1.TopNode, treeNodes);
                var crossGroupsTreeNode = treeNodes.FirstOrDefault(x => x.Tag == group.GetSIF().CrossSubsystemGroups);

                if (crossGroupsTreeNode != null && crossGroupsTreeNode.Tag is CrossSubsystemGroups groups)
                {
                    groups.Add(group);
                    var childNode = new TreeNode(group.DisplayName(crossGroupsTreeNode));
                    childNode.ContextMenuStrip = contextMenuCrossGroup;
                    childNode.Tag = group;
                    crossGroupsTreeNode.Nodes.Add(childNode);
                }

                TreeChanged();
            }
        }

        private void removeFromCrossVotingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Group group)
            {
                group.AllowAnyComponents.Value = null;
                var crossGroups = group.GetSIF().CrossSubsystemGroups;
                crossGroups.Remove(group);
                treeView1.Nodes.Remove(treeView1.SelectedNode);
                TreeChanged();
            }
        }
    }
}