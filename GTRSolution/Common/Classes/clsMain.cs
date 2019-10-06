using System;
using System.Collections;
using System.Reflection;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GTRLibrary;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;

namespace GTRHRIS.Common.Classes
{
    public class clsMain
    {
        #region Variable
        public static System.Data.DataSet dsConfigure;

        public static string AppTitle = "GT Solution";      //Store Application Title
        public static string AppPath = "";      //Store Application path

        public static Int16 intCurrency = 1;   //Default Currecny

        public static Int32 intUserId = 0;        //Store User Id
        public static string strUser = "";          //Store User Name
        public static string strUserCode = "";          //Store User Code

        public static Int32 intSGroupId = 0;      // Store User Sub Group Id
        public static string strSGroupName = "";  // Store User Sub Group Name

        public static Int32 intGroupId = 0;       //Store User Group Id
        public static string strGroupName = "";   //Store User Group Name

        public static string strComputerName = "";  //Computer Name
        public static string strMacAddress = "";    //Mac Address
        public static string strIPAddress = "";     //IP Address

        public static string strTranDate = "";      //Transaction Date

        public static string strRelationalId = "0"; // For Supplier, Customer, Product List Will Connect With Transaction Form
        public static string IsApprubed = "0"; // For Trnsfer Location Form
        public static bool boolDirectConvertQ2WO = false;

        public static Int16 intComId = 1;           // Store Sister Concer / Company Id
        public static string strComName = "";       // Store Sister Concer / Company Name

        public static string strValidation = "";    // Software Validation
        public static string strValidBT = "";       // Software Validation Button

        public static string strValidationDB = "";   // Software Validation
        public static string strValidBTDB = "";      // Software Validation Button

        //public static string dbACC = "";       // Database Inventory
        //public static string dbIMS = "";       // Database Inventory
        //public static string dbCOM = "";       // Database Inventory
        //public static string dbSYS = "";       // Database Inventory
        //public static string dbHRIS = "";       // Database HRIS
        //public static string dbTLOG = "";       // Database Tran



        //Report Name For Convert to PDF
        public static string strReportName = "";
        public static string strExtension = "";
        public static string strFormat = "";



        //public static DateTime FirstDayOfMonthFromDateTime = DateTime.Now;       // Store Sister Concer / Company Name
        //public static DateTime LastDayOfMonthFromDateTime = DateTime.Now;       // Store Sister Concer / Company Name



        #region Picture Path variable
        public static string strPicPathStore = @"Z:\Com\Pics\Store";
        public static string strPicPathCmps = @"Z:\Com\Pics\Cmps";
        public static string strPicPathIcon = @"Z:\Com\Pics\Icon";
        #endregion

        #region Report Related Variable
        
        public static string strReportPathMain="";
        public static string strDSNMain="";
        public static string strQueryMain="";

        public static Int16 intHasSubReport = 0;
        public static string strRelationalField = "";
        public static string strDSNSub = "";
        public static string strQuerySub = "";

        #endregion

        //To store the list of opened form
        public static ArrayList openForm = new ArrayList();

        //To store the list of Menu
        public static ArrayList alMnuFrmName = new ArrayList();
        public static ArrayList alMnuImgName = new ArrayList();

        #region Barcode related variable
        public static string[] strBarcode = new string[2];
        #endregion

        #endregion Variable

        #region Constant

        public const string cnstGTRDateFormat = "dd MMM yyyy";  // Date Format

        #endregion Constant        
        
        Infragistics.Win.Appearance app1 = new Infragistics.Win.Appearance();

        //public DateTime FirstDayOfMonthFromDateTime(DateTime dateTime)
        //{
        //    return new DateTime(dateTime.Year, dateTime.Month, 1);
        //}

        //public DateTime LastDayOfMonthFromDateTime(DateTime dateTime)
        //{
        //    DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
        //    return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        //}
        
        public void GTRFormatFocus(ref UltraTextEditor obj)
        {
            app1.BackColor = System.Drawing.Color.Violet;//.PowderBlue;
            obj.Appearance = app1;
        }

        public void GTRFormatFocus(ref UltraComboEditor obj)
        {
            app1.BackColor = System.Drawing.Color.PowderBlue;
            obj.Appearance = app1;
        }

        public void GTRFormatFocus(ref UltraCombo obj)
        {
            app1.BackColor = System.Drawing.Color.PowderBlue;
            obj.Appearance = app1;
        }

        public void GTRFormatLeave(ref UltraTextEditor obj)
        {
            app1.BackColor = System.Drawing.Color.White;
            obj.Appearance = app1;
        }

        public void GTRFormatLeave(ref UltraComboEditor obj)
        {
            app1.BackColor = System.Drawing.Color.White;
            obj.Appearance = app1;
        }

        public void GTRFormatLeave(ref UltraCombo obj)
        {
            app1.BackColor = System.Drawing.Color.White;
            obj.Appearance = app1;
        }

        //Move Like Tab When User Press Enter
        public void GTRTabMove(Int16 KeyCode)
        {
            if (KeyCode == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        //Select all data in the textbox
        public void GTRGotFocus(ref UltraTextEditor txt)
        {
            txt.SelectAll();
        }

        public void GTRSelectText(ref UltraTextEditor txt)
        {
            if (!String.IsNullOrEmpty(txt.Text))
            {
                double dblOut;
                if (double.TryParse(txt.Text.ToString(), out dblOut) == true)
                {
                    txt.Text = double.Parse(txt.Text.ToString()).ToString();
                }
                txt.SelectionStart = 0;
                txt.SelectionLength = txt.Text.Length;
            }
        }

        #region Reflection
        public object GTRMakeFormNameAsObject(string strLocation, string strForm, ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            object obj = null;
            Type oType = Assembly.GetExecutingAssembly().GetType(strLocation + strForm);
            Type[] parameterTypes = new Type[] { typeof(Infragistics.Win.UltraWinTabControl.UltraTabControl).MakeByRefType(), typeof(Common.FormEntry.frmMaster) };
            ConstructorInfo constructor = oType.GetConstructor(parameterTypes);
            if (oType != null)
            {
                obj = constructor.Invoke(new Object[] { utab, fm });
            }
            return obj;
        }
        public Form GTRMakeFormNameAsForm(string strFormName)
        {
            Form frm = null;
            Type oType = Assembly.GetExecutingAssembly().GetType(strFormName);
            if (oType != null)
            {
                frm = ((Form)(Activator.CreateInstance(oType)));
            }
            return frm;
        }
        #endregion Reflection

        #region User Login Log
        public void prcLogin()
        {
            clsConnection clsCon = new clsConnection();
            try
            {
                int Result = 0;
                string sqlQuery = "";
                sqlQuery = "Insert Into tblLogin_Activity_Log (LUserId, LoginDate, LoginStartTime, LoginPCName, LoginPCIP, LoginPCMac) "
                    + " Select " + clsMain.intUserId + ", convert(varchar,GETDATE(),107), GETDATE(), '" + clsMain.strComputerName + "', '" + Common.Classes.clsMain.strIPAddress + "', '" + Common.Classes.clsMain.strMacAddress + "'";
                Result = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
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
        public void prcLogout()
        {
            clsConnection clsCon = new clsConnection();
            try
            {
                int Result = 0;
                string sqlQuery = "";
                sqlQuery = "Update tblLogin_Activity_Log Set LoginEndTime = GETDATE() Where SLNo = "
                    + "(Select MAX(SLNo) From tblLogin_Activity_Log Where LUserId = " + Common.Classes.clsMain.intUserId + ")";
                Result = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
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
        #endregion

        #region Load Combo

        public void GTRLoadCombo(ref Infragistics.Win.UltraWinEditors.UltraComboEditor cbo, string sqlQuery, string DisplayMember, string ValueMember)
        {
            clsConnection clsCon = new clsConnection();
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();

                clsCon.GTRFillDatasetWithSQLCommand(ref ds, sqlQuery);
                cbo.DataSource = ds.Tables[0];

                cbo.DisplayMember = DisplayMember;
                cbo.ValueMember = ValueMember;
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

        public void GTRLoadCombo(ref Infragistics.Win.UltraWinGrid.UltraCombo cbo, string sqlQuery, string DisplayMember, string ValueMember)
        {
            clsConnection clsCon = new clsConnection();
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();

                clsCon.GTRFillDatasetWithSQLCommand(ref ds, sqlQuery);
                cbo.DataSource = ds.Tables[0];

                cbo.DisplayMember = DisplayMember;
                cbo.ValueMember = ValueMember;
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

        #endregion Load Combo
        
        #region Form Open Related Transaction (frmMaster Form Tab Control)
        public static Boolean fncExistOpenForm(Form frm)
        {
            //fncExistOpenForm call to add list to arraylist byDefault
            return fncExistOpenForm(frm, "Add");
        }

        //Static function to access direct from caller function
        public static Boolean fncExistOpenForm(Form frm, string flag)
        {
            //creating object of this class
            clsMain com = new clsMain();

            //Find the index no of searching form
            int index = fncFindOpenFormIndex(frm);

            Boolean bln = false;
            if (index >= 0)
            {
                if (flag != "Add")
                {
                    //flag : Remove
                    com.prcRemoveOpenForm(index);
                }
                bln = true;
            }
            else
            {
                if (flag == "Add")
                {
                    //flag : Add
                    com.prcAddOpenForm(frm);
                }
            }
            return bln;
        }

        private void prcAddOpenForm(Form frm)
        {
            //add object to arraylist
            openForm.Add(frm);
        }

        private void prcRemoveOpenForm(int index)
        {
            //remove object from arraylist
            openForm.RemoveAt(index);
        }

        //searching & return index no of provided form object
        public static int fncFindOpenFormIndex(Form frm)
        {
            string strFindFormName = frm.Name.ToString();
            int index = -1;
            for (int i = 0; i < openForm.Count; i++)
            {
                string value = ((Form)openForm[i]).Name.ToString();
                if (value == strFindFormName)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        #endregion Form Open Related Transaction (frmMaster Form Tab Control)

        #region configuration
        public static void SetConfiguration()
        {
            dsConfigure = new System.Data.DataSet();
           clsConnection clscon = new clsConnection();
            try
            {
                string SQLQuery = "Select moduleId, flagName, flagValue from tblConfiguration";
                clscon.GTRFillDatasetWithSQLCommand(ref dsConfigure, SQLQuery);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                clscon = null;
            }

        }
        public static string GetConfiguration(Int32 moduleId, string flagName)
        {
            string returnValue="";
            DataRow[] dr = dsConfigure.Tables[0].Select("moduleId ="+moduleId+" and flagName = '"+ flagName +"'");
            foreach (DataRow dr2 in dr)
            {
                returnValue = dr2["flagValue"].ToString() ;
            }
            return returnValue;
        }
        #endregion

        #region Menu Image
        public static void prcAddMenuName(string strMenuFrmName, string strMenuImageName)
        {
            //add object to arraylist
            alMnuFrmName.Add(strMenuFrmName);
            alMnuImgName.Add(strMenuImageName);
        }

        //searching & return index no of opening menu
        public static int fncFindMenuImageIndex(string strMenuFrmName)
        {
            string strFindFormName = strMenuFrmName;
            int index = -1;
            for (int i = 0; i < openForm.Count; i++)
            {
                string value = ((Form)openForm[i]).Name.ToString();
                if (value == strFindFormName)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        #endregion Menu Image




        #region
        public string fncEncrypt(String txt)
        {
            int encCode = 15;
            int i;
            string str = "";

            byte[] asciiBytes = Encoding.ASCII.GetBytes(txt);
            foreach (byte b in asciiBytes)
            {
                str += Convert.ToChar(b + encCode).ToString();
            }
            return str;
        }

        public string fncDecrypt(String txt)
        {
            int encCode = 15;
            int i;
            string str = "";

            byte[] asciiBytes = Encoding.ASCII.GetBytes(txt);
            foreach (byte b in asciiBytes)
            {
                str += Convert.ToChar(b - encCode).ToString();
            }
            return str;
        }
        #endregion
    }
}