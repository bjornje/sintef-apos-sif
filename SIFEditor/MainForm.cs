using Sintef.Apos.Sif;
using Sintef.Apos.Sif.Model;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using ComboBox = System.Windows.Forms.ComboBox;
using TextBox = System.Windows.Forms.TextBox;

namespace SIFEditor
{
    public partial class MainForm : Form
    {
        private string? _fileName = null;
        private Builder? _builder;
        private readonly string _text;
        private readonly List<PropertiesControl> _nodeDetails = new();
        private readonly PropertiesControl _propertieBlankPage = new();
        private readonly BindingList<ModelError> _filteredErrors = [];
        private readonly ToolStripItem _groupAddInputDevice;
        private readonly ToolStripItem _groupAddLogicSolver;
        private readonly ToolStripItem _groupAddFinalElement;

        public MainForm()
        {
            InitializeComponent();
            listBox1.ValueMember = "Message";
            listBox1.DataSource = _filteredErrors;

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            _text = $"APOS SIF Editor {version.Major}.{version.Minor}.{version.Build} UML Model {Definition.Version}";
            Text = _text;

            _propertieBlankPage.Width = splitContainer2.Panel1.Width;
            _propertieBlankPage.Height = splitContainer2.Panel1.Height;
            _propertieBlankPage.Dock = DockStyle.Fill;

            splitContainer2.Panel1.Controls.Add(_propertieBlankPage);

            _groupAddInputDevice = contextMenuStripGroup.Items.Find("inputDeviceToolStripMenuItem1", true).FirstOrDefault();
            _groupAddLogicSolver = contextMenuStripGroup.Items.Find("logicSolverToolStripMenuItem1", true).FirstOrDefault();
            _groupAddFinalElement = contextMenuStripGroup.Items.Find("finalElementToolStripMenuItem1", true).FirstOrDefault();
        }

        private void FilterModelErrors()
        {
            var selectedItem = listBox1.SelectedItem;
            var index = 0;
            var selectedIndex = -1;

            _filteredErrors.Clear();

            if (_builder == null)
            {
                return;
            }

            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag is Node node && node.GetType() != typeof(Root))
            {
                foreach (var error in _builder.Errors.Where(x => x.Node == node))
                {
                    if (error == selectedItem)
                    {
                        selectedIndex = index;
                    }

                    _filteredErrors.Add(error);
                    index++;
                }

                listBox1.SelectedIndex = selectedIndex;

                return;
            }

            foreach (var error in _builder.Errors)
            {
                if (error == selectedItem)
                {
                    selectedIndex = index;
                }

                _filteredErrors.Add(error);
                index++;
            }

            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag is Node node2 && node2.GetType() != typeof(Root))
            {
                listBox1.SelectedIndex = selectedIndex;
            }
            else
            {
                listBox1.SelectedIndex = -1;
            }
        }

        private void RefreshTreeView()
        {
            var updateNeeded = false;
            var treeNodes = new List<TreeNode>();

            FindAllTreeNodes(treeView1.TopNode, treeNodes);

            foreach (var treeNode in treeNodes)
            {
                if (treeNode.Tag is not Node node)
                {
                    continue;
                }

                var text = node.NodeDisplayName(treeNode.Parent);
                if (treeNode.Text != text)
                {
                    treeNode.Text = text;
                    updateNeeded = true;
                }

            }

            if (updateNeeded)
            {
                treeView1.Refresh();
            }

            _builder?.SIFs.PushAttributes();
        }

        private void AddRoot(Root root)
        {
            treeView1.Nodes.Clear();

            var node = new TreeNode(root.NodeDisplayName(new TreeNode()));
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
                var childNode = new TreeNode(sif.NodeDisplayName(node));
                childNode.ContextMenuStrip = contextMenuStripSIF;
                childNode.Tag = sif;
                node.Nodes.Add(childNode);

                AddGroups(sif.Groups, childNode);
                AddDocuments(sif.Documents, childNode);
            }
        }

        private void AddDocuments(Documents documents, TreeNode node)
        {
            var childNode = new TreeNode(documents.NodeDisplayName(node));
            childNode.ContextMenuStrip = contextMenuStripDocuments;
            childNode.Tag = documents;
            node.Nodes.Add(childNode);

            foreach (var document in documents)
            {
                AddDocument(document, childNode);
            }
        }

        private void AddDocument(Document document, TreeNode node)
        {
            var childNode = new TreeNode(document.NodeDisplayName(node));
            childNode.ContextMenuStrip = contextMenuStripDocument;
            childNode.Tag = document;
            node.Nodes.Add(childNode);
        }

        private void AddGroups(Groups groups, TreeNode node)
        {
            foreach (var group in groups)
            {
                var childNode = new TreeNode(group.NodeDisplayName(node));
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
                var childNode = new TreeNode(component.NodeDisplayName(node));
                childNode.ContextMenuStrip = contextMenuStripSISComponent;
                childNode.Tag = component;
                node.Nodes.Add(childNode);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is not ModelError error)
            {
                return;
            }

            var list = new List<TreeNode>();
            FindAllTreeNodes(treeView1.TopNode, list);

            var result = list.FirstOrDefault(x => x.Tag == error.Node);


            if (result == null)
            {
                return;
            }

            treeView1.SelectedNode = result;
            result.EnsureVisible();

            if (error.Attribute != null && splitContainer2.Panel1.Controls[0] is PropertiesControl properties)
            {
                properties.Focus(error.Attribute.Name);
            }
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
            var allTreeNodes = new List<TreeNode>();
            FindAllTreeNodes(treeView1.TopNode, allTreeNodes);
            foreach (var item in allTreeNodes)
            {
                item.BackColor = Color.Transparent;
            }

            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Tag is not Node node)
            {
                return;
            }

            _ignoreTextChanged = true;

            treeView1.SelectedNode.BackColor = Color.PaleTurquoise;

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

            FilterModelErrors();

            _ignoreTextChanged = false;
        }

        private void Control_TextChanged(object? sender, EventArgs e)
        {
            if (_ignoreTextChanged) return;

            if (sender is TextBox textBox && textBox.Tag is IAttribute attributeType)
            {
                if (string.IsNullOrEmpty(textBox.Text)) attributeType.StringValue = null;
                else attributeType.StringValue = textBox.Text;
            }
            else if (sender is ComboBox comboBox && comboBox.Tag is IAttribute attributeType2)
            {
                var valueBefore = attributeType2.ObjectValue;

                if (string.IsNullOrEmpty(comboBox.Text)) attributeType2.StringValue = null;
                else attributeType2.StringValue = comboBox.Text;

                //if (attributeType2.Owner is Group group && attributeType2 == group.Subsystem)
                //{
                //    var change = CrossVotingGroupChange(valueBefore as bool?, group.Subsystem.Value);
                //    if (change == 1)
                //    {
                //        var treeNodes = new List<TreeNode>();
                //        FindAllTreeNodes(treeView1.TopNode, treeNodes);
                //        var crossGroupsTreeNode = treeNodes.FirstOrDefault(x => x.Tag == group.GetSIF().CrossSubsystemGroups);

                //        if (crossGroupsTreeNode != null && crossGroupsTreeNode.Tag is CrossSubsystemGroups groups)
                //        {
                //            groups.Add(group);
                //            var childNode = new TreeNode(group.NodeDisplayName(crossGroupsTreeNode));
                //            childNode.ContextMenuStrip = contextMenuCrossGroup;
                //            childNode.Tag = group;
                //            crossGroupsTreeNode.Nodes.Add(childNode);
                //        }
                //    }
                //    else if (change == -1)
                //    {
                //        var treeNodes = new List<TreeNode>();
                //        FindAllTreeNodes(treeView1.TopNode, treeNodes);
                //        var crossGroupsTreeNode = treeNodes.FirstOrDefault(x => x.Tag == group.GetSIF().CrossSubsystemGroups);
                //        if (crossGroupsTreeNode != null && crossGroupsTreeNode.Tag is CrossSubsystemGroups groups)
                //        {
                //            var groupTreeNode = FindSubNode(crossGroupsTreeNode, group);
                //            if (groupTreeNode.Tag == group)
                //            {
                //                groups.Remove(group);
                //                crossGroupsTreeNode.Nodes.Remove(groupTreeNode);
                //            }
                //        }
                //    }
                //}
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


        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Focus();

            if (_builder != null && _builder.HasChanges)
            {
                var result = MessageBox.Show("Save changes?", "APOS SIF Editor", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Cancel)
                {
                    return;
                }

                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                    return;
                }

            }

            _fileName = null;
            _builder = new Builder();

            FilterModelErrors();

            AddRoot(_builder.SIFs.Parent);
            addSifToolStripMenuItem_Click(sender, e);

            splitContainer2.Panel1.Controls.Clear();
            splitContainer2.Panel1.Controls.Add(_propertieBlankPage);

            TreeChanged();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Focus();

            var finalMessage = string.Empty;

            using var openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "AML-files|*.aml";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _fileName = openFileDialog.FileName;

                _builder = new Builder();

                try
                {
                    _builder.LoadFromFile(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    if (!TryRecoverFile(openFileDialog.FileName))
                    {
                        return;
                    }

                    finalMessage = $"There is a problem with this file. By extracting only the InstanceHierarchy part of the file and injecting it into a valid APOS AML template it was possible to load into the editor. Error message: {ex.Message}";
                }

                FilterModelErrors();

                var root = _builder.Roots.FirstOrDefault();
                if (root != null)
                {
                    AddRoot(root);
                }

                splitContainer2.Panel1.Controls.Clear();
                splitContainer2.Panel1.Controls.Add(_propertieBlankPage);

                TreeChanged();

                if (finalMessage != string.Empty)
                {
                    MessageBox.Show(finalMessage, "Load CAEX file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private bool TryRecoverFile(string fileName)
        {
            var text = File.ReadAllText(fileName);

            var startTag = text.IndexOf("<InstanceHierarchy ");
            var endTag = text.IndexOf("</InstanceHierarchy>");

            if (startTag < 0)
            {
                MessageBox.Show("InstanceHierarchy start tag not found.", "Load CAEX file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (endTag < startTag)
            {
                MessageBox.Show("InstanceHierarchy end tag not found.", "Load CAEX file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var recoveredText = RecoverFile(text.Substring(startTag, endTag - startTag + 20));

            try
            {
                using (var stream = GenerateStreamFromString(recoveredText))
                {
                    _builder.LoadFromStream(stream);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Load CAEX file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private string RecoverFile(string instanceHierarchy)
        {
            var startTag = Definition.Model.IndexOf("<SystemUnitClassLib ");

            return $"{Definition.Model.Substring(0, startTag)}{instanceHierarchy}{Definition.Model.Substring(startTag)}";
        }

        private bool _hasChanges;
        private void TreeChanged()
        {

            saveToolStripMenuItem.Enabled = _builder != null;
            saveAsToolStripMenuItem.Enabled = _builder != null;
            if (_builder == null)
            {
                return;
            }

            RefreshTreeView();

            var hasChanges = _builder.HasChanges;
            if (hasChanges || (_hasChanges != hasChanges))
            {
                _builder.Validate();
                FilterModelErrors();
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
                var sif = root.SIFs.Append();
                var sifTreeNode = AppendTreeNode(sif, treeView1.TopNode, contextMenuStripSIF);

                //InputDeviceSubsystem
                var inputDeviceGroup = sif.Groups.Append();
                inputDeviceGroup.Subsystem.Value = v_subsystem.input_device_subsystem;
                inputDeviceGroup.MInVotingMooN.ObjectValue = 1;
                inputDeviceGroup.NInVotingMooN.ObjectValue = 1;

                var inputDeviceGroupTreeNode = AppendTreeNode(inputDeviceGroup, sifTreeNode, contextMenuStripGroup);

                var initiatorComponent = inputDeviceGroup.Components.Append();
                AppendTreeNode(initiatorComponent, inputDeviceGroupTreeNode, contextMenuStripSISComponent);

                //LogicSolverSubsystem
                var logicSolverGroup = sif.Groups.Append();
                logicSolverGroup.Subsystem.Value = v_subsystem.logic_solver_subsystem;
                logicSolverGroup.MInVotingMooN.ObjectValue = 1;
                logicSolverGroup.NInVotingMooN.ObjectValue = 1;

                var logicSolverGroupTreeNode = AppendTreeNode(logicSolverGroup, sifTreeNode, contextMenuStripGroup);

                var solverComponent = logicSolverGroup.Components.Append();
                AppendTreeNode(solverComponent, logicSolverGroupTreeNode, contextMenuStripSISComponent);

                //FinalElementSubsystem
                var finalElementGroup = sif.Groups.Append();
                finalElementGroup.Subsystem.Value = v_subsystem.final_element_subsystem;
                finalElementGroup.MInVotingMooN.ObjectValue = 1;
                finalElementGroup.NInVotingMooN.ObjectValue = 1;

                var finalElementGroupTreeNode = AppendTreeNode(finalElementGroup, sifTreeNode, contextMenuStripGroup);

                var finalComponent = finalElementGroup.Components.Append();
                AppendTreeNode(finalComponent, finalElementGroupTreeNode, contextMenuStripSISComponent);

                //Documents
                var documentsTreeNode = AppendTreeNode(sif.Documents, sifTreeNode, contextMenuStripDocuments);
                var document = sif.Documents.Append();
                AppendTreeNode(document, documentsTreeNode, contextMenuStripDocument);

                TreeChanged();
            }
        }

        private static TreeNode AppendTreeNode(Node tag, TreeNode parent, ContextMenuStrip? contextMenuStrip)
        {
            var treeNode = new TreeNode(tag.NodeDisplayName(parent));
            treeNode.ContextMenuStrip = contextMenuStrip;
            treeNode.Tag = tag;
            parent.Nodes.Add(treeNode);
            parent.Expand();
            return treeNode;
        }

        private static void AdjustMenuItemEnabled(ContextMenuStrip menuStrip, string key, bool isEnabled)
        {
            var item = menuStrip.Items.Find(key, true).FirstOrDefault();
            if (item == null)
            {
                return;
            }

            item.Enabled = isEnabled;
        }

        private static void AdjustMenuItemVisible(ContextMenuStrip menuStrip, string key, bool isVisible)
        {
            var item = menuStrip.Items.Find(key, true).FirstOrDefault();
            if (item == null)
            {
                return;
            }

            item.Visible = isVisible;
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
                if (e.Node?.Tag is SafetyInstrumentedFunction sif)
                {
                    AdjustMenuItemEnabled(e.Node.ContextMenuStrip, "inputDeviceToolStripMenuItem", sif.InputDevice == null);
                    AdjustMenuItemEnabled(e.Node.ContextMenuStrip, "logicSolverToolStripMenuItem", sif.LogicSolver == null);
                    AdjustMenuItemEnabled(e.Node.ContextMenuStrip, "finalElementToolStripMenuItem", sif.FinalElement == null);
                    AdjustMenuItemEnabled(e.Node.ContextMenuStrip, "compositeToolStripMenuItem", sif.Composite == null);
                }
                else if (e.Node?.Tag is Group group)
                {
                    if (group.Subsystem.Value == v_subsystem.composite_subsystem)
                    {
                        _groupAddInputDevice.Visible = true;
                        _groupAddLogicSolver.Visible = true;
                        _groupAddFinalElement.Visible = true;
                    }
                    else
                    {
                        _groupAddInputDevice.Visible = false;
                        _groupAddLogicSolver.Visible = false;
                        _groupAddFinalElement.Visible = false;
                    }
                }

                treeView1.SelectedNode = e.Node;
            }
        }

        private void addComponentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is Group group && !_groupAddInputDevice.Visible)
            {
                var component = group.Components.Append();
                AppendTreeNode(component, treeView1.SelectedNode, contextMenuStripSISComponent);
                TreeChanged();
            }
        }

        private void removeComponentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is SISDeviceRequirements component && component.Parent is Group group)
            {
                group.Remove(component);
                treeView1.Nodes.Remove(treeView1.SelectedNode);
                TreeChanged();
            }
        }

        private void removeComponentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is Group group)
            {
                if (group.Parent is Group parentGroup)
                {
                    parentGroup.Remove(group);
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                    TreeChanged();
                }
                else if (group.Parent is SafetyInstrumentedFunction sif)
                {
                    sif.Groups.Remove(group);
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                    TreeChanged();
                }
            }
        }

        private void removeSubsystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is Group subsystem && subsystem.Parent is SafetyInstrumentedFunction sif)
            {
                sif.Remove(subsystem);
                treeView1.Nodes.Remove(treeView1.SelectedNode);
                TreeChanged();
            }
        }

        private void removeSIFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is SafetyInstrumentedFunction sif && sif.Parent is Root root)
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
            AppendLine(sb, "Path", "SIFID", "Component name", "Attribute name", "Attribute value", "Attribute type", "Attribute requirement");

            foreach (var sif in _builder.SIFs)
            {
                foreach (var attribute in sif.Attributes)
                {
                    AppendLine(sb, sif.GetPath(sif.Parent), sif.SIFID.Value, "", attribute.Name, attribute.StringValue, attribute.TypeDisplayName(out _), attribute.RequirementText());

                }

                foreach (var group in sif.GetAllGroups())
                {
                    AppendGroup(sb, sif, group);
                }

                foreach (var document in sif.Documents)
                {
                    foreach (var attribute in document.Attributes)
                    {
                        AppendLine(sb, document.GetPath(sif.Parent), sif.SIFID.Value, "", attribute.Name, attribute.StringValue, attribute.TypeDisplayName(out _), attribute.RequirementText());
                    }
                }
            }

            File.WriteAllText(fileName, sb.ToString());
        }

        private static void AppendGroup(StringBuilder sb, SafetyInstrumentedFunction sif, Group group)
        {
            foreach (var attribute in group.Attributes)
            {
                AppendLine(sb, group.GetPath(sif.Parent), sif.SIFID.Value, "", attribute.Name, attribute.StringValue, attribute.TypeDisplayName(out _), attribute.RequirementText());
            }

            foreach (var component in group.Components)
            {
                foreach (var attribute in component.Attributes)
                {
                    AppendLine(sb, component.GetPath(sif.Parent), sif.SIFID.Value, component.TagName.Value, attribute.Name, attribute.StringValue, attribute.TypeDisplayName(out _), attribute.RequirementText());
                }
            }
        }

        private static void AppendLine(StringBuilder sb, string col1, string col2, string col3, string col4, string col5, string col6, string col7)
        {
            sb.AppendLine($"{col1}\t{col2}\t{col3}\t{col4}\t{col5}\t{col6}\t{col7}");
        }

        private void addGroupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is Group group)
            {
                var subGroup = group.Groups.Append();
                subGroup.MInVotingMooN.ObjectValue = 1;
                subGroup.NInVotingMooN.ObjectValue = 1;

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
            if (treeView1.SelectedNode?.Tag is SafetyInstrumentedFunction sif)
            {
                var subsystem = sif.Groups.AppendInputDevice();
                subsystem.VoteWithinGroup(1, 1);

                AppendTreeNode(subsystem, treeView1.SelectedNode, contextMenuStripGroup);
                TreeChanged();
            }
        }

        private void logicSolverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is SafetyInstrumentedFunction sif)
            {
                var subsystem = sif.Groups.AppendLogicSolver();
                subsystem.VoteWithinGroup(1, 1);

                AppendTreeNode(subsystem, treeView1.SelectedNode, contextMenuStripGroup);
                TreeChanged();
            }
        }

        private void finalElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is SafetyInstrumentedFunction sif)
            {
                var subsystem = sif.Groups.AppendFinalElement();
                subsystem.VoteWithinGroup(1, 1);

                AppendTreeNode(subsystem, treeView1.SelectedNode, contextMenuStripGroup);
                TreeChanged();
            }
        }

        private void compositeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is SafetyInstrumentedFunction sif)
            {
                var subsystem = sif.Groups.AppendComposite();
                subsystem.VoteWithinGroup(1, 1);

                AppendTreeNode(subsystem, treeView1.SelectedNode, contextMenuStripGroup);
                TreeChanged();
            }
        }

        private void toolStripMenuItemAddDocument_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Documents documents)
            {
                var document = documents.Append();
                AppendTreeNode(document, treeView1.SelectedNode, contextMenuStripDocument);
                TreeChanged();
            }
        }

        private void toolStripMenuItemUMLModel_Click(object sender, EventArgs e)
        {
            var modelForm = new UMLModelForm();
            modelForm.Show();
        }

        private void addDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is Documents documents)
            {
                var document = documents.Append();
                AppendTreeNode(document, treeView1.SelectedNode, contextMenuStripDocument);
                TreeChanged();
            }
        }

        private void removeDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is Document document && document.Parent is Documents documents)
            {
                documents.Remove(document);
                treeView1.Nodes.Remove(treeView1.SelectedNode);
                TreeChanged();
            }
        }

        private void toolStripMenuItemAddAttribute_Click(object sender, EventArgs e)
        {
            var form = new AddAttributeForm();

            if (form.ShowDialog() == DialogResult.OK)
            {
                Definition.TryAddAttribute(form.AttributeName, form.RefAttributeType, form.IsOrderedList, form.Target);

                _builder?.PushAttributes();

                treeView1_AfterSelect(null, null);
            }
        }

        private void inputDeviceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is Group group)
            {
                var component = group.Components.AppendInputDevice();
                AppendTreeNode(component, treeView1.SelectedNode, contextMenuStripSISComponent);
                TreeChanged();
            }
        }

        private void logicSolverToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is Group group)
            {
                var component = group.Components.AppendLogicSolver();
                AppendTreeNode(component, treeView1.SelectedNode, contextMenuStripSISComponent);
                TreeChanged();
            }
        }

        private void finalElementToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is Group group)
            {
                var component = group.Components.AppendFinalElement();
                AppendTreeNode(component, treeView1.SelectedNode, contextMenuStripSISComponent);
                TreeChanged();
            }
        }
    }
}