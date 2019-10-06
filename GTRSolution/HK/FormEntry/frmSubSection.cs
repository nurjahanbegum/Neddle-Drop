using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Collections;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using GTRLibrary;
using System.Windows.Forms;

namespace GTRHRIS.HK.FormEntry
{
    public partial class frmSubSection : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmSubSection(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmSubSection_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetail = null;
            uTab = null;
            FM = null;
            clsProc = null;
        }

        private void txtId_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        private void txtNameBangla_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16) e.KeyCode);
        }

        
        private void txtId_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        private void txtNameBangla_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16) e.KeyChar);
        }

        public void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string SqlQuery = "Exec prcGetSubSection '" + Common.Classes.clsMain.intComId + "',0";
                clsCon.GTRFillDatasetWithSQLCommand( ref dsList, SqlQuery );
                dsList.Tables[0].TableName = "SubSection";
                dsList.Tables[1].TableName = "Section";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["SubSection"];
            }
            catch (Exception ex)
            {
                throw(ex);
            }
            finally
            {
                clsCon = null;
            }
        }

        public void prcDisplayDetails( string strParam)
        {
            clsConnection clsCon = new clsConnection();
            dsDetail = new System.Data.DataSet();
            try
            {
                string SqlQuery = "Exec prcGetSubSection '" + Common.Classes.clsMain.intComId + "'," + Int32.Parse(strParam) + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetail, SqlQuery);
                dsDetail.Tables[0].TableName = "details";
                DataRow dr;

                if (dsDetail.Tables["details"].Rows.Count > 0)
                {
                    dr = dsDetail.Tables["details"].Rows[0];
                    txtId.Text = dr["SubSectID"].ToString();
                    txtName.Text = dr["SubSectName"].ToString();
                    txtNameB.Text = dr["SubSectNameB"].ToString();
                    txtSlNo.Text = dr["SLNO"].ToString();
                    cboSect.Text = dr["SectId"].ToString();
                   // txtNameB.Text = dr["DesigNameB"].ToString();


                    btnSave.Text = "&Update";
                    btnDelete.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                clsCon = null;
            }

        }

        public void prcLoadCombo()
        {

            cboSect.DataSource = null;
            cboSect.DataSource = dsList.Tables["Section"];

        }

        public void prcClearData()
        {
            txtId.Text = "";
            txtNameB.Text = "";
            txtName.Text = "";
            txtSlNo.Text = "0";
            cboSect.Text = "";

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false ;
        }
        public Boolean fncBlank()
        {
            if (this.txtName .Text.Length == 0)
            {
                MessageBox.Show("Please provide Sub Section Name.");
                txtName.Focus();
                return true;
            }

            if (this.cboSect.Text.Length == 0)
            {
                MessageBox.Show("Please provide Section Name.");
                cboSect.Focus();
                return true;
            }
            
            return false;
        }

        private void frmSubSection_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
                prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fncBlank())
            {
                return;
            }
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            Int32 NewId = 0;

            try
            {
                if (btnSave.Text.ToString()!= "&Save")
                {
                    //Update     
                    sqlQuery = " Update tblCat_SubSection  Set SubSectName ='" + txtName.Text.ToString() + "', SubSectNameB='" + txtNameB.Text.ToString() + "' , SLNO= '" + txtSlNo.Text.ToString() + "',SectId = '" + cboSect.Value.ToString() + "',SectName ='" + cboSect.Text.ToString() + "' ";
                    sqlQuery += " Where SubSectID = " + Int32.Parse(txtId.Text);
                    arQuery.Add(sqlQuery); 

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                    arQuery.Add(sqlQuery);

                    sqlQuery = " Update S Set S.SectName = D.SectName from tblCat_SubSection S,tblCat_Section D Where S.SectId = D.SectId and S.SubSectID = " + Int32.Parse(txtId.Text) + "";
                    arQuery.Add(sqlQuery);

                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Succefully");
                }
                else
                {
                    sqlQuery = "Select Isnull(Max(SubSectID),0)+1 As NewId from tblCat_SubSection ";
                    NewId = clsCon.GTRCountingData(sqlQuery);
                    //Insert to Table
                    sqlQuery = "Insert Into tblCat_SubSection(SLNo, SubSectID, aId,ComId,SubSectName, SubSectNameB,SectId,SectName, PCName, LUserId) ";
                    sqlQuery = sqlQuery + " Values ('" + txtSlNo.Text.ToString() + "'," + NewId + ", " + NewId + ",'" + Common.Classes.clsMain.intComId + "', '" + txtName.Text.ToString() + "', '" + txtNameB.Text.ToString() + "','" + cboSect.Value.ToString() + "','" + cboSect.Text.ToString() + "','" + Common.Classes.clsMain.strComputerName + "','" + Common.Classes.clsMain.intUserId + "' )";
                    int add = arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);

                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Succefully");
                }
                prcClearData();
                txtName.Focus();

                prcLoadList();
                prcLoadCombo();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message );
            }
            finally
            {
                arQuery = null;
                clsCon = null;
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Do you want to delete Sub Section information of [" + txtName.Text + "]", "",
                                System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "";

                //Delete Data
                sqlQuery = "Delete from tblCat_SubSection  Where SubSectID  = " + Int32.Parse(txtId.Text);
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                arQuery.Add(sqlQuery);
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Deleted Successfully.");

                prcClearData();
                txtName.Focus();

                prcLoadList();
                // prcLoadCombo();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                arQuery = null;
                clsCon = null;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            gridList.DisplayLayout.Bands[0].Columns["SubSectID"].Hidden = true;
            gridList.DisplayLayout.Bands[0].Columns["SLNo"].Hidden = true;
            gridList.DisplayLayout.Bands[0].Columns["SectID"].Hidden = true;

            //Set Caption
            gridList.DisplayLayout.Bands[0].Columns["SubSectName"].Header.Caption = "SubSection";
            gridList.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
           
            //Set Width
            gridList.DisplayLayout.Bands[0].Columns["SubSectName"].Width = 235;
            gridList.DisplayLayout.Bands[0].Columns["SectName"].Width = 165;

            //Change alternate color
            gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            this.gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            this.gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            this.gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            prcClearData();
            prcDisplayDetails(gridList.ActiveRow.Cells[0].Value.ToString());
        }



        private void txtNameB_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtSlNo_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboSect_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboSect.DisplayLayout.Bands[0].Columns["SectName"].Width = cboSect.Width;
            cboSect.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
            cboSect.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            cboSect.DisplayMember = "SectName";
            cboSect.ValueMember = "SectId";
        }
        
    }
}
