using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using GTRHRIS.Common.Classes;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using ColumnStyle = Infragistics.Win.UltraWinGrid.ColumnStyle;

namespace GTRHRIS.Attendence.FormEntry
{
    public partial class frmEmployeeList : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetails;
        private clsProcedure clsProc = new clsProcedure();

        private clsMain clM = new clsMain();
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private GTRHRIS.Common.FormEntry.frmMaster FM;

        public frmEmployeeList(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab,
                                Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                gridList.DisplayLayout.Bands[0].Columns["DesigId"].Hidden = true;
                gridList.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
                gridList.DisplayLayout.Bands[0].Columns["Empid"].Hidden = true;
                gridList.DisplayLayout.Bands[0].Columns["SubSectId"].Hidden = true;

                
                //Grid Width
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 80; //Employee code
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Width = 130; //Employee Name

                gridList.DisplayLayout.Bands[0].Columns["DesigName"].Width = 145; //Designation
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Width = 170; //Section
                gridList.DisplayLayout.Bands[0].Columns["SubSectName"].Width = 100; //SubSection
                gridList.DisplayLayout.Bands[0].Columns["Band"].Width = 90; //Band
                gridList.DisplayLayout.Bands[0].Columns["EmpType"].Width = 95; //Employee Type 
                gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Width = 95; //Employee Join Date 
                gridList.DisplayLayout.Bands[0].Columns["dtBirth"].Width = 95; //Employee Birth Date 
                gridList.DisplayLayout.Bands[0].Columns["EmpFather"].Width = 100; //Employee Father 
                gridList.DisplayLayout.Bands[0].Columns["EmpMother"].Width = 100; //Employee Mother
                gridList.DisplayLayout.Bands[0].Columns["Grade"].Width = 100; //Grade

                gridList.DisplayLayout.Bands[0].Columns["Sex"].Width = 75; //Sex
                gridList.DisplayLayout.Bands[0].Columns["Religion"].Width = 85; //Religion
                gridList.DisplayLayout.Bands[0].Columns["Line"].Width = 100; //Line 
                gridList.DisplayLayout.Bands[0].Columns["Floor"].Width = 100; //Floor 
                gridList.DisplayLayout.Bands[0].Columns["GS"].Width = 100; //GS
                gridList.DisplayLayout.Bands[0].Columns["BS"].Width = 100; //BS
                gridList.DisplayLayout.Bands[0].Columns["IsAllowOT"].Width = 50; //OT
                gridList.DisplayLayout.Bands[0].Columns["IsAllowPF"].Width = 50; //PF
                gridList.DisplayLayout.Bands[0].Columns["IsTrnDeduction"].Width = 50; //Transport
                gridList.DisplayLayout.Bands[0].Columns["BusStop"].Width = 120; //Transport

                //Caption
                gridList.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Code";
                gridList.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";

                gridList.DisplayLayout.Bands[0].Columns["DesigName"].Header.Caption = "Designation";
                gridList.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";
                gridList.DisplayLayout.Bands[0].Columns["SubSectName"].Header.Caption = "Sub Section";
                gridList.DisplayLayout.Bands[0].Columns["Band"].Header.Caption = "Band";
                gridList.DisplayLayout.Bands[0].Columns["EmpType"].Header.Caption = "Employee Type";
                gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Header.Caption = "Join Date";
                gridList.DisplayLayout.Bands[0].Columns["dtBirth"].Header.Caption = "Birth Date";
                gridList.DisplayLayout.Bands[0].Columns["EmpFather"].Header.Caption = "Father Name";
                gridList.DisplayLayout.Bands[0].Columns["EmpMother"].Header.Caption = "Mother Name";
                gridList.DisplayLayout.Bands[0].Columns["Grade"].Header.Caption = "Grade";

                gridList.DisplayLayout.Bands[0].Columns["Sex"].Header.Caption = "Sex";
                gridList.DisplayLayout.Bands[0].Columns["Religion"].Header.Caption = "Religion";
                gridList.DisplayLayout.Bands[0].Columns["Line"].Header.Caption = "Line";
                gridList.DisplayLayout.Bands[0].Columns["GS"].Header.Caption = "GS";
                gridList.DisplayLayout.Bands[0].Columns["BS"].Header.Caption = "BS";
                gridList.DisplayLayout.Bands[0].Columns["IsAllowOT"].Header.Caption = "OT";
                gridList.DisplayLayout.Bands[0].Columns["IsAllowPF"].Header.Caption = "PF";
                gridList.DisplayLayout.Bands[0].Columns["IsTrnDeduction"].Header.Caption = "Trans.";
                gridList.DisplayLayout.Bands[0].Columns["BusStop"].Header.Caption = "Bus Stoppage";

                gridList.DisplayLayout.Bands[0].Columns["dtJoin"].Format = "dd-MMM-yy";
                gridList.DisplayLayout.Bands[0].Columns["dtBirth"].Format = "dd-MMM-yy";

                //Select Full Row when click on any cell
                e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

              
                //gridList.Columns["Photo"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Image;
                this.gridList.DisplayLayout.Override.ActiveRowAppearance.BackColor = Color.DarkCyan;
                this.gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;
                //Selection Style Will Be Row Selector
                this.gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                this.gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                this.gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                // Set the scroll style to immediate so the rows get scrolled immediately
                // when the vertical scrollbar thumb is dragged.
                e.Layout.ScrollStyle = ScrollStyle.Immediate;

                // ScrollBounds of ScrollToFill will prevent the user from scrolling the
                // grid further down once the last row becomes fully visible.
                e.Layout.ScrollBounds = ScrollBounds.ScrollToFill;

                //Using Filter
                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmEmployeeList_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
                //prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlQuery = "Exec [prcGetEmployeeList] " + Common.Classes.clsMain.intComId + ", 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblgrid";               

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["tblGrid"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                clsCon = null;
            }
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEmployeeList_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = GTRHRIS.Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            GTRHRIS.Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            uTab = null;
            FM = null;
            clsProc = null;
        }

        private void ultraButton2_Click(object sender, EventArgs e)
        {
            if(gridList.Rows.Count==0)
            {
                return;
            }
            clsMain.strRelationalId = gridList.ActiveRow.Cells["EmpID"].Value.ToString();
            FM.prcExecuteChildForm("Attendence.FormEntry","frmEmployee");
        
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            if (gridList.Rows.Count == 0)
            {
                return;
            }
            clsMain.strRelationalId = gridList.ActiveRow.Cells["EmpID"].Value.ToString();
            FM.prcExecuteChildForm("Attendence.FormEntry", "frmEmployee");
        }

        private void GridToToExcel_InitializeColumn(object sender, InitializeColumnEventArgs e)
        {
            try
            {
                if (e.Column.DataType == typeof(System.DateTime?) && e.Column.Format != null)
                {
                    e.ExcelFormatStr = e.Column.Format.Replace("tt", "AM/PM");
                }
                else
                {
                    e.ExcelFormatStr = e.Column.Format;
                }
            }
            catch (Exception ex)
            {
                //ExceptionFramework.ExceptionPolicy.HandleException(ex, "DefaultPolicy");
            }
        }


        private void btnExcel_Click(object sender, EventArgs e)
        {
            DialogResult dlgRes =
            MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = "Employee List.xls" + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

            dlgSurveyExcel.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult dlgResSaveFile = dlgSurveyExcel.ShowDialog();
            if (dlgResSaveFile == DialogResult.Cancel)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            Application.DoEvents();
            UltraGridExcelExporter GridToToExcel = new UltraGridExcelExporter();
            GridToToExcel.FileLimitBehaviour = FileLimitBehaviour.TruncateData;
            GridToToExcel.InitializeColumn += new InitializeColumnEventHandler(GridToToExcel_InitializeColumn);
            GridToToExcel.Export(gridList, dlgSurveyExcel.FileName);

            MessageBox.Show("Download complete.");
        }

    }
}


      