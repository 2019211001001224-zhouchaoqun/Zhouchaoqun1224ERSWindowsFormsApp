using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zhouchaoqun1224ERSWindowsFormsApp
{
    public partial class EmployeeRecordsForm : Form
    {
        private TreeNode tvRootNode;

        public EmployeeRecordsForm()
        {
            InitializeComponent();
            PopulateTreeView();
            initalizeListView();
        }
        private void PopulateTreeView()
        {
            statusBarPanel1.Tag = "Refreshing Employee Code. Pls wait...";
            this.Cursor = Cursors.WaitCursor;
            treeView1.Nodes.Clear();
            tvRootNode = new TreeNode("Employee Records");
            this.Cursor = Cursors.Default;
            treeView1.Nodes.Add(tvRootNode);

            TreeNodeCollection nodeCollection=tvRootNode.Nodes;
            XmlTextReader reader = new XmlTextReader("C:\\Users\\chenchen\\source\\repos\\Zhouchaoqun1224ERSWindowsFormsApp\\Zhouchaoqun1224ERSWindowsFormsApp\\EmpRec.xml");
            reader .MoveToElement();
            try
            {
                while (reader.Read())
                {
                    if(reader.HasAttributes && reader .NodeType== XmlNodeType.Element)
                    {
                        reader.MoveToContent();//<EmpRecordsData>
                        reader.MoveToContent();//<Ecode>
                        reader.MoveToAttribute("Id");//Id="0001"
                        string strval = reader.Value;//E0001
                        reader.Read ();
                        reader.Read();
                        if(reader.Name=="Dept")
                        {
                            reader.Read();
                        }
                        TreeNode EcodeNode = new TreeNode(strval);
                        nodeCollection.Add(EcodeNode);
                    }
                }
                statusBarPanel1.Tag = "click on an employee code to see their record.";
            }
            catch (XmlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        protected void initalizeListView()
        {
            listView1.Clear();
            listView1.Columns.Add("Employee Name",255,HorizontalAlignment.Left);
            listView1.Columns.Add("DateofJoin", 70, HorizontalAlignment.Right);
            listView1.Columns.Add("Gread", 105, HorizontalAlignment.Left);
            listView1.Columns.Add("Salary", 105, HorizontalAlignment.Left);
        }
        protected void PopulateListView(TreeNode crrNode)
        {
            initalizeListView();
            XmlTextReader listRead = new XmlTextReader("C:\\Users\\chenchen\\source\\repos\\Zhouchaoqun1224ERSWindowsFormsApp\\Zhouchaoqun1224ERSWindowsFormsApp\\EmpRec.xml");
            listRead.MoveToElement();
            while (listRead.Read())
            {
                string strNodeName;
                string strNodePath;
                string name;
                string gread;
                string doj;
                string sal;
                string[] strItemsArr = new string[4];
                listRead.MoveToFirstAttribute();
                strNodeName = listRead.Value;
                strNodePath = crrNode.FullPath.Remove(0,17);
                if (strNodeName == strNodePath)
                {
                    ListViewItem lvi;
                    listRead.MoveToNextAttribute();
                    name = listRead.Value;
                    lvi = listView1.Items.Add(name);

                    listRead.Read();
                    listRead.Read();

                    listRead.MoveToFirstAttribute();
                    doj = listRead.Value;
                    lvi.SubItems.Add(doj);

                    listRead.MoveToNextAttribute();
                    gread = listRead.Value;
                    lvi.SubItems.Add(gread);

                    listRead.MoveToNextAttribute();
                    sal = listRead.Value;
                    lvi.SubItems.Add(sal);

                    listRead.MoveToNextAttribute();
                    listRead.MoveToElement();
                    listRead.ReadString();  
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode currNode = e.Node;
            if (tvRootNode == currNode)
            {
                statusBarPanel1.Text = "Double Click the Employee Records.";
                return;
            }
            else
            {
                statusBarPanel1.Text = "Click an Employee code to view Individual record.";
            }
            PopulateListView(currNode);
        }
    }
}
