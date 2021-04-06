using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
namespace InventorySolution
{
    public partial class Record : Form
    {
        public Record()
        {
            InitializeComponent();
        }
        List<string> liHeader = new List<string>();
        List<string> liDummyHeader = new List<string>();
        List<string> liSubHeader = new List<string>();
        List<string> liData = new List<string>();
        OleDbConnection dbConnection;
        private void Record_Load(object sender, EventArgs e)
        {
            try
            {

            
            LoadHeader();
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void LoadHeader()
        {
            try
            {

            
            cboHeader.Text = "";
            cboHeader.Items.Clear();
            dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+txtFilePath.Text.Trim()+"\\Inventory.accdb");
            dbConnection.Open();
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM HEADER", dbConnection);
            da.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                liHeader.Add(dr[1].ToString());
                cboHeader.Items.Add(dr[0].ToString() + "-" + dr[1].ToString());
            }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void LoadSubHeader()
        {
            try
            { 
            cboSubHeader.Text = "";
            cboSubHeader.Items.Clear();
            cboData.Text = "";
            cboData.Items.Clear();
            dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+txtFilePath.Text.Trim()+"\\Inventory.accdb");
            DataSet ds1 = new DataSet();
            OleDbDataAdapter da1 = new OleDbDataAdapter("SELECT * FROM SUBHEADER  WHERE HEADERID=" + cboHeader.Text.Split('-')[0], dbConnection);
            da1.Fill(ds1);
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                liSubHeader.Add(dr[1].ToString());
                cboSubHeader.Items.Add(dr[0].ToString() + "-" + dr[1].ToString());
            }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void LoadData()
        {
            try
            {

            
            cboData.Text = "";
            cboData.Items.Clear();
            dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+ txtFilePath.Text.Trim() +"\\Inventory.accdb");
            DataSet ds2 = new DataSet();
            OleDbDataAdapter da2 = new OleDbDataAdapter("SELECT * FROM DATATBL WHERE SUBHEADERID=+" + cboSubHeader.Text.Split('-')[0], dbConnection);
            da2.Fill(ds2);
            foreach (DataRow dr in ds2.Tables[0].Rows)
            {
                liData.Add(dr[1].ToString());
                cboData.Items.Add(dr[0].ToString() + "-" + dr[1].ToString());
            }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

            
            LoadData();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {

            


            if ((cboHeader.Text.Trim() != "") && (cboSubHeader.Text.Trim() == ""))
            {
                InsertHeader();
                
            }
            else if ((cboHeader.Text.Trim() != "") && (cboSubHeader.Text.Trim() != "") &&(cboData.Text.Trim()==""))
            {
                InsertSubHeader();
                            }
            else if ((cboSubHeader.Text.Trim() != "") && (cboData.Text.Trim() != ""))
            {
                InsertData();
                
            }


            }
            catch (Exception)
            {

                throw;
            }



        }

        public void InsertHeader()
        {
            try
            {

            
            if (cboHeader.Text!="")
            {
                bool recexists = HeaderRecExists();
                if (!recexists)
                {
                    dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+ txtFilePath.Text.Trim() +"\\Inventory.accdb;Persist Security Info=True");
                    OleDbCommand oleDbCommand = dbConnection.CreateCommand();
                    dbConnection.Open();
                   
                    oleDbCommand.CommandText = "INSERT INTO HEADER([HeaderText])VALUES('" + cboHeader.Text.Trim() + "')";
                    oleDbCommand.Connection = dbConnection;
                    int ro = oleDbCommand.ExecuteNonQuery();
                    MessageBox.Show(cboHeader.Text.Trim() + " Record Inserted");
                    dbConnection.Close();
                    LoadHeader();
                }
                else
                {
                    MessageBox.Show("Record Already Exists");
                }
            
            }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateHeader()
        {
            try
            {

            
            if (cboHeader.Text != "")
            {

                if (MessageBox.Show("Are you sure to update record? " + cboHeader.Text.Trim(), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+ txtFilePath.Text.Trim()+"\\Inventory.accdb;Persist Security Info=True");
                    OleDbCommand oleDbCommand = dbConnection.CreateCommand();
                    dbConnection.Open();

                    oleDbCommand.CommandText = "UPDATE HEADER SET [HeaderText]='" + cboHeader.Text.Trim().Split('-')[1] + "' WHERE ID=" + cboHeader.Text.Split('-')[0];
                    oleDbCommand.Connection = dbConnection;
                    int ro = oleDbCommand.ExecuteNonQuery();
                    MessageBox.Show(cboHeader.Text.Trim() + " Record Updated");
                    dbConnection.Close();
                    LoadHeader();
                }

            }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateSubHeader()
        {
            try
            {

            

            if (MessageBox.Show("Are you sure to update record? " + cboSubHeader.Text.Trim(), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+txtFilePath.Text.Trim()+"\\Inventory.accdb;Persist Security Info=True");
                OleDbCommand oleDbCommand = dbConnection.CreateCommand();
                dbConnection.Open();

                oleDbCommand.CommandText = "UPDATE SUBHEADER SET [SUBHEADERNAME]='" + cboSubHeader.Text.Trim().Split('-')[1] + "' WHERE SUBHEADERID=" + cboSubHeader.Text.Split('-')[0];
                oleDbCommand.Connection = dbConnection;
                int ro = oleDbCommand.ExecuteNonQuery();
                MessageBox.Show(cboSubHeader.Text.Trim() + " Record Updated");
                dbConnection.Close();
                LoadSubHeader();

            }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateData()
        {
            try
            {

            

            if (MessageBox.Show("Are you sure to Update record? " + cboData.Text.Trim(), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+txtFilePath.Text.Trim()+ "\\Inventory.accdb; Persist Security Info = True");
                OleDbCommand oleDbCommand = dbConnection.CreateCommand();
                dbConnection.Open();

                oleDbCommand.CommandText = "UPDATE DATATBL SET [DATATEXT]='" + cboData.Text.Trim().Split('-')[1] + "' WHERE DATAID=" + cboData.Text.Split('-')[0];
                oleDbCommand.Connection = dbConnection;
                int ro = oleDbCommand.ExecuteNonQuery();
                MessageBox.Show(cboData.Text.Trim() + " Record Updated");
                dbConnection.Close();
                LoadData();

            }
            }
            catch (Exception)
            {

                throw;
            }

        }


        public void DeleteHeader()
        {
            try
            {

            
            if (cboHeader.Text != "")
            {
                
                if (MessageBox.Show("Are you sure to delete record? " + cboHeader.Text.Trim(), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+txtFilePath.Text.Trim()+"\\Inventory.accdb;Persist Security Info=True");
                    OleDbCommand oleDbCommand = dbConnection.CreateCommand();
                    dbConnection.Open();

                    oleDbCommand.CommandText = "DELETE FROM HEADER  WHERE ID=" + cboHeader.Text.Split('-')[0];
                    oleDbCommand.Connection = dbConnection;
                    int ro = oleDbCommand.ExecuteNonQuery();
                    MessageBox.Show(cboHeader.Text.Trim() + " Record Deleted");
                    dbConnection.Close();
                    LoadHeader();

                }
            }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteSubHeader()
        {
            try
            {

            
            if (MessageBox.Show("Are you sure to delete record? " + cboSubHeader.Text.Trim(), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+txtFilePath.Text.Trim()+"\\Inventory.accdb;Persist Security Info=True");
                OleDbCommand oleDbCommand = dbConnection.CreateCommand();
                dbConnection.Open();

                oleDbCommand.CommandText = "DELETE FROM SUBHEADER  WHERE SUBHEADERID=" + cboSubHeader.Text.Split('-')[0];
                oleDbCommand.Connection = dbConnection;
                int ro = oleDbCommand.ExecuteNonQuery();
                MessageBox.Show(cboSubHeader.Text.Trim() + " Record Deleted");
                dbConnection.Close();
                LoadSubHeader();

            }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void DeleteData()
        {

            try
            {

            
            if (MessageBox.Show("Are you sure to delete record? " + cboData.Text.Trim(), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+txtFilePath.Text.Trim()+"\\Inventory.accdb;Persist Security Info=True");
                OleDbCommand oleDbCommand = dbConnection.CreateCommand();
                dbConnection.Open();

                oleDbCommand.CommandText = "DELETE FROM DATATBL WHERE DATAID=" + cboData.Text.Split('-')[0];
                oleDbCommand.Connection = dbConnection;
                int ro = oleDbCommand.ExecuteNonQuery();
                dbConnection.Close();
                    MessageBox.Show(cboData.Text.Trim() + " Record Deleted");
                    LoadData();
            }


            }
            catch (Exception)
            {

                throw;
            }

        }



        public bool HeaderRecExists()
        {
            try
            {

           

            bool recyes = false;
            string cmd = "";
            DataSet ds1 = new DataSet();
            dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+txtFilePath.Text.Trim()+"\\Inventory.accdb;Persist Security Info=True");
            dbConnection.Open();
            try
            {
                if (cboHeader.Text.Split('-').Length >= 2)
                {
                    //  cmd = "SELECT * FROM HEADER  WHERE HEADERTEXT='" + cboHeader.Text.Trim().Split('-')[1] + "'";
                    recyes = true;
                    return recyes;
                }
                else
                {
                    cmd = "SELECT * FROM HEADER  WHERE HEADERTEXT='" + cboHeader.Text.Trim() + "'";

                }
            }
            catch (Exception)
            {


            }
            
            OleDbDataAdapter da1 = new OleDbDataAdapter(cmd, dbConnection);
            da1.Fill(ds1);
            if (ds1.Tables[0].Rows.Count>1)
            {
                recyes = true;
            }

            return recyes;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SubHeaderRecExists()
        {

            bool recyes = false;
            string cmd = "";
            DataSet ds1 = new DataSet();
            dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+txtFilePath.Text.Trim()+"\\Inventory.accdb;Persist Security Info=True");
            dbConnection.Open();
            try
            {
                if (cboSubHeader.Text.Split('-').Length >= 2)
                {
                    //cmd = "SELECT * FROM SUBHEADER  WHERE SUBHEADERNAME='" + cboSubHeader.Text.Trim().Split('-')[1] + "'";
                    recyes = true;
                    return recyes;
                }
                else
                {
                    cmd = "SELECT * FROM SUBHEADER  WHERE SUBHEADERNAME='" + cboSubHeader.Text.Trim() + "'";

                }
            }
            catch (Exception)
            {


            }
            OleDbDataAdapter da1 = new OleDbDataAdapter(cmd, dbConnection);
            da1.Fill(ds1);
            if (ds1.Tables[0].Rows.Count > 1)
            {
                recyes = true;
            }

            return recyes;

        }

        public bool DataRecExists()
        {
            bool recyes = false;
            string cmd = "";
            dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+txtFilePath.Text.Trim()+"\\Inventory.accdb;Persist Security Info=True");
            dbConnection.Open();
            try
            {
                if (cboData.Text.Split('-').Length >= 2)
                {
                    //cmd = "SELECT * FROM DATATBL  WHERE DATATEXT='" + cboData.Text.Trim().Split('-')[1] + "'";
                    recyes = true;
                    return recyes;
                }
                else
                {
                    cmd = "SELECT * FROM DATATBL  WHERE DATATEXT='" + cboData.Text.Trim() + "'";

                }
            }
            catch (Exception)
            {


            }
            DataSet ds1 = new DataSet();
            OleDbDataAdapter da1 = new OleDbDataAdapter(cmd, dbConnection);
            da1.Fill(ds1);
            if (ds1.Tables[0].Rows.Count > 1)
            {
                recyes = true;
            }

            return recyes;

        }

        public void InsertSubHeader()
        {
            try
            {

            

            if ((cboSubHeader.Text != "") &&(cboHeader.Text!=""))
            {
                bool recexists = SubHeaderRecExists();
                if (!recexists)
                {
                    dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+txtFilePath.Text.Trim()+"\\Inventory.accdb;Persist Security Info=True");
                    OleDbCommand oleDbCommand = dbConnection.CreateCommand();
                    dbConnection.Open();
                    oleDbCommand.CommandText = "INSERT INTO SUBHEADER([SUBHEADERNAME],[HEADERID])VALUES('" + cboSubHeader.Text.Trim() + "'," + cboHeader.Text.Split('-')[0] + ")";
                    oleDbCommand.Connection = dbConnection;
                    int ro = oleDbCommand.ExecuteNonQuery();
                    dbConnection.Close();
                    MessageBox.Show(cboSubHeader.Text.Trim() + " Record Inserted");
                    LoadSubHeader();
                }
                else
                {
                    MessageBox.Show("Record Already Exists");
                }
            }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void InsertData()
        {
            try
            {

            
            if ((cboSubHeader.Text != "")&& (cboData.Text != ""))
            {
                bool recexists = DataRecExists();
                if (!recexists)
                {
                    dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+txtFilePath.Text.Trim()+"\\Inventory.accdb;Persist Security Info=True");
                    OleDbCommand oleDbCommand = dbConnection.CreateCommand();
                    dbConnection.Open();
                    oleDbCommand.CommandText = "INSERT INTO DATATBL([DATATEXT],[SUBHEADERID])VALUES('" + cboData.Text.Trim() + "'," + cboSubHeader.Text.Split('-')[0] + ")";
                    oleDbCommand.Connection = dbConnection;
                    int ro = oleDbCommand.ExecuteNonQuery();
                    MessageBox.Show(cboData.Text.Trim() + " Record Inserted");
                    dbConnection.Close();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Record Already Exists");
                }
            }
            }
            catch (Exception)
            {

                throw;
            }
        }



        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {

            
            dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+txtFilePath.Text.Trim()+"\\Inventory.accdb;Persist Security Info=True");
            OleDbCommand oleDbCommand = dbConnection.CreateCommand();
            dbConnection.Open();
            oleDbCommand.CommandText = "DELETE FROM SUBHEADER; ";
            oleDbCommand.Connection = dbConnection;
            int ro = oleDbCommand.ExecuteNonQuery();
            dbConnection.Close();
            dbConnection= new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+txtFilePath.Text.Trim()+"\\Inventory.accdb;Persist Security Info=True");
            OleDbCommand oleDbCommand1 = dbConnection.CreateCommand();
            dbConnection.Open();
            oleDbCommand1.CommandText = "DELETE FROM HEADER ";
            oleDbCommand1.Connection = dbConnection;
            int ro1 = oleDbCommand1.ExecuteNonQuery();
            dbConnection.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }
                    
        private void cboHeader_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            LoadSubHeader();
        }

        private void cboData_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if ((cboHeader.Text.Trim() != "") && (cboSubHeader.Text.Trim() == ""))
            {
                UpdateHeader();

            }
            else if ((cboHeader.Text.Trim() != "") && (cboSubHeader.Text.Trim() != "") && (cboData.Text.Trim() == ""))
            {
                UpdateSubHeader();
            }
            else if ((cboSubHeader.Text.Trim() != "") && (cboData.Text.Trim() != ""))
            {
                UpdateData();

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if ((cboHeader.Text.Trim() != "") && (cboSubHeader.Text.Trim() == ""))
            {
                DeleteHeader();

            }
            else if ((cboHeader.Text.Trim() != "") && (cboSubHeader.Text.Trim() != "") && (cboData.Text.Trim() == ""))
            {
                DeleteSubHeader();
            }
            else if ((cboSubHeader.Text.Trim() != "") && (cboData.Text.Trim() != ""))
            {
                DeleteData();

            }
        }

        private void Record_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((Form)this.MdiParent).Controls["btnNext"].Enabled = true;


        }

        private void btnExportExl_Click(object sender, EventArgs e)
        {
            try
            {

            

            string cmd = "";
            dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+txtFilePath.Text.Trim()+"\\Inventory.accdb;Persist Security Info=True");
            dbConnection.Open();
            DataSet ds1 = new DataSet();

            cmd = "SELECT HEADERTEXT FROM HEADER WHERE ID=" + cboHeader.Text.Split('-')[0]; //,SubHeaderName,DATATEXT,SUBHEADER,DATATBL WHERE ID=HEADERID AND SUBHEADER.SUBHEADERID=HEADERID AND DATAID=SUBHEADER.SUBHEADERID AND ID=" + cboHeader.Text.Split('-')[0];
            OleDbDataAdapter da1 = new OleDbDataAdapter(cmd, dbConnection);
            da1.Fill(ds1);

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Data");
            DataSet ds2 = new DataSet();
            string cmd2 = "SELECT SUBHEADERID, SUBHEADERNAME FROM SUBHEADER WHERE HEADERID=" + cboHeader.Text.Split('-')[0]; //,SubHeaderName,DATATEXT,SUBHEADER,DATATBL WHERE ID=HEADERID AND SUBHEADER.SUBHEADERID=HEADERID AND DATAID=SUBHEADER.SUBHEADERID AND ID=" + cboHeader.Text.Split('-')[0];
            OleDbDataAdapter da2 = new OleDbDataAdapter(cmd2, dbConnection);
            da2.Fill(ds2);

            if (ds2.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds2.Tables[0].Rows)
                {
                    dataTable.Rows.Add(dr["SubHeaderName"].ToString());
                    DataSet ds3 = new DataSet();
                    string cmd3 = "SELECT DATATEXT FROM DATATBL WHERE SUBHEADERID=" + dr["SUBHEADERID"].ToString();//ds2.Tables[0].Rows[0].ItemArray[0]; //,SubHeaderName,DATATEXT,SUBHEADER,DATATBL WHERE ID=HEADERID AND SUBHEADER.SUBHEADERID=HEADERID AND DATAID=SUBHEADER.SUBHEADERID AND ID=" + cboHeader.Text.Split('-')[0];
                    OleDbDataAdapter da3 = new OleDbDataAdapter(cmd3, dbConnection);
                    da3.Fill(ds3);

                    foreach (DataRow dr1 in ds3.Tables[0].Rows)
                    {
                        dataTable.Rows.Add("     "+dr1["DATATEXT"].ToString());

                    }

                }
            }
            

            //if (ds2.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow dr in ds2.Tables[0].Rows)
            //    {
            //        //if (cboSubHeader.Text.Split('-')[0].Contains(dr["SUBHEADERID"].ToString()))
            //        //{
            //            DataSet ds3 = new DataSet();
            //            string cmd3 = "SELECT DATATEXT FROM DATATBL WHERE SUBHEADERID=" + dr["SUBHEADERID"].ToString();//ds2.Tables[0].Rows[0].ItemArray[0]; //,SubHeaderName,DATATEXT,SUBHEADER,DATATBL WHERE ID=HEADERID AND SUBHEADER.SUBHEADERID=HEADERID AND DATAID=SUBHEADER.SUBHEADERID AND ID=" + cboHeader.Text.Split('-')[0];
            //            OleDbDataAdapter da3 = new OleDbDataAdapter(cmd3, dbConnection);
            //            da3.Fill(ds3);

            //            foreach (DataRow dr1 in ds3.Tables[0].Rows)
            //            {
            //                dataTable.Rows.Add(dr1["DATATEXT"].ToString());

            //            }
            //        //}
            //    }             
                

            //}

                        
               

                var lines = new List<string>();

                string[] columnNames = dataTable.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName).
                                                  ToArray();

                var header = string.Join(",", columnNames);
                lines.Add(header);

                var valueLines = dataTable.AsEnumerable()
                                   .Select(row => string.Join(",", row.ItemArray));
                lines.AddRange(valueLines);

                File.WriteAllLines(txtFilePath.Text.Trim()+"\\"+ cboHeader.Text.Split('-')[1] + DateTime.Now.ToString("yyyyMMddHHmmss")+".csv", lines);
                MessageBox.Show("File Created in Path = "+txtFilePath.Text.Trim());

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
