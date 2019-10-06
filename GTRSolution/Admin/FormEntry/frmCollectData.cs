using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;

namespace GTRHRIS.Admin.FormEntry
{
    public partial class frmCollectData : Form
    {
        private string strFileLoc = Common.Classes.clsMain.AppPath + @"\Download\";
        private string strFileLocMove = Common.Classes.clsMain.AppPath + @"\DownloadDone\";
        private string strFileNameWithLoc = "";
        private string strFileName = "";

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        clsProcedure clsProc = new clsProcedure();
        Common.FormEntry.frmMaster FM;

        public frmCollectData(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (fncBlank())
                {
                    return;
                }

                prcImportData();
                prcCollectData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmCollectData_Load(object sender, EventArgs e)
        {
            try
            {
                prcClearData();

                //Filling Listview
                prcCollectData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void prcCollectData()
        {
            ListView1.Items.Clear();

            DirectoryInfo myDirectory = new DirectoryInfo(strFileLoc);
            FileSystemInfo[] myReceivedFiles = myDirectory.GetFiles("*.txt;");
            string[] files = Directory.GetFiles(strFileLoc);

            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                ListViewItem item = new ListViewItem(fileName);
                item.Tag = file;
                ListView1.Items.Add(item);
            }
        }

        private void frmCollectData_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            clsProc = null;
            FM = null;
            uTab = null;
        }

        private void prcClearData()
        {
            this.btnSave.Text = "&Process";
        }

        private Boolean fncBlank()
        {
            if(ListView1.SelectedItems[0].Text=="")
            {
                MessageBox.Show("Please select a file name from the list.");
                ListView1.Focus();
                return true;
            }
            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
            prcCollectData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void prcGetListviewData()
        {
            if (ListView1.SelectedItems.Count > 0)
            {
                ListViewItem selected = ListView1.SelectedItems[0];
                strFileName = ListView1.SelectedItems[0].Text.ToString() + ".txt";

                strFileNameWithLoc = strFileLoc + strFileName;
                //System.Diagnostics.Process.Start(Common.Classes.clsMain.AppPath + @"\Download\" + filename);
            }
        }

        private void prcImportData()
        {
            //Getting Selected File Information From List
            prcGetListviewData();

            clsConnection clsCon = new clsConnection();
            ArrayList arQuery = new ArrayList();
            string sqlQuery = "";

            try
            {
                using (StreamReader sr = new StreamReader(File.Open(strFileNameWithLoc, FileMode.Open)))
                {
                    string line = "";
                    while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                    {
                        string[] parts = line.Split(new string[] {":"}, StringSplitOptions.None);
                        string strTime = parts[3] + ":" + parts[4] + ":" + parts[5];
                        string strStNo = DateTime.Parse(parts[2].ToString()).ToString("dd:MM:yyyy").Replace(":", "");
                        strStNo += DateTime.Parse(strTime.ToString()).ToString("HH:mm:ss").Replace(":", "");
                        sqlQuery =
                            string.Format(
                                " INSERT INTO tblRawData ( DeviceNo, cardno, dtPunchDate, dtPunchTime, stNo, PCName, LUserId) " +
                                " VALUES ('{0}','{1}','{2}','{3}', '{4}', '{5}', '{6}')", parts[0], parts[1], parts[2], strTime, double.Parse(strStNo), clsProc.GTRGetComputerName(), Common.Classes.clsMain.intUserId);
                        arQuery.Add(sqlQuery);
                    }
                }

                sqlQuery =
                    "Update tblRawdata Set EmpId = B.EmpId, IsNew = 1 From tblRawdata AS A Inner Join tblEmp_Info As B On A.CardNo = B.CardNo Where IsNew = 0 ";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                //Moving Data To Another Folder
                prcTransferDataFile();

                MessageBox.Show("Data collection completed successfully.");
            }
            catch (Exception ex)
            {
                throw(ex);
            }
            finally
            {
                arQuery = null;
                clsCon = null;
            }
        }

        private void prcTransferDataFile()
        {
            string fileLocMove = strFileLocMove+strFileName;
            if (File.Exists(strFileNameWithLoc))
            {
                File.Move(strFileNameWithLoc, fileLocMove);
            }
        }
    }
}
