using System;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinToolbars;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using System.Drawing;
using System.IO;
using System.Media;

namespace GTRHRIS.Common.FormEntry
{
    public partial class frmMaster : Form
    {
        private System.Data.DataSet dsFirst;
        private System.Data.DataSet dsLoad;
        private System.Data.DataSet dsList;

        private ArrayList alMenuImage = new ArrayList();
        clsProcedure clsproc=new clsProcedure();

        FormClass.clsMaster clsForm;
        System.Data.DataSet dsMaster;

        Form frm;
        Form frmParent = new Form();

        public frmMaster()
        {
            //Constructor Default
            InitializeComponent();
        }

        public frmMaster(frmLogin frm)
        {
            //Constructor With Reference of Login form
            InitializeComponent();
            frmParent = frm;
        }

        private void frmMaster_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to exit ?", "gtSolution", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            if (Common.Classes.clsMain.intUserId > 0)
            {
                //Upate user log
                Common.Classes.clsMain clsMain = new Common.Classes.clsMain();
                clsMain.prcLogout();
                clsMain = null;
            }

            this.Dispose();
        }

        private void frmMaster_Load(object sender, EventArgs e)
        {
            Common.Classes.clsMain.AppPath = Environment.CurrentDirectory;

            //Showing Login Form at the loading time of master form
            frmLogin frm = new frmLogin(this);
            
            frm.MdiParent = this;
           // ultraDockManager1.Visible = true;
            frm.Show();
        }
        private void prcAutoUpdate()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            System.Data.DataSet dsAutoupdate = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Select top(1) AutoDownload_ID,LuserID, FileName, FilePath,FileType, FileSourceName, IsDownLoad, VershionNo From tbl_AutoUpdate where IsDownLoad=0  Order by AutoDownload_ID Desc";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsAutoupdate, sqlQuery);

                dsAutoupdate.Tables[0].TableName = "AD";
                if (dsAutoupdate.Tables["AD"].Rows.Count > 0)
                {
                    DataRow dr = dsAutoupdate.Tables["AD"].Rows[0];
                    //string result = Consinment.FormEntry.Form1.ShowBox("Do you want to exit?", "Exit");
                    // Consinment.FormEntry.Form1 frm = new Consinment.FormEntry.Form1(ref ultraProgressBar1, labelPerc, labelDownloaded, labelSpeed);\
                    string str = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();


                    if (dr["IsDownLoad"].ToString().ToUpper() == "1" || dr["VershionNo"].ToString() == str)
                    {
                        return;
                    }
                    else
                    {


                        if (dr["Filepath"].ToString().ToUpper() == "EXE".ToUpper() ||
                            dr["Filepath"].ToString().ToUpper() == "ALL".ToUpper())
                        {



                            Operation.FormEntry.frmAutoUpdate frm = new Operation.FormEntry.frmAutoUpdate(ref str, dr["VershionNo"].ToString(), dr["FileType"].ToString());
                            //frm.MdiParent = this;
                            frm.StartPosition = FormStartPosition.CenterScreen;
                            //  frm.MdiParent = this;
                            frm.Show();
                            this.WindowState = FormWindowState.Minimized;

                        }
                        else
                        {
                            str = "This";
                            Operation.FormEntry.frmAutoUpdate frm = new Operation.FormEntry.frmAutoUpdate(ref str, dr["VershionNo"].ToString(), dr["FileType"].ToString());
                            //frm.MdiParent = this;
                            frm.StartPosition = FormStartPosition.CenterScreen;

                            //  frm.MdiParent = this;
                            frm.Show();
                            this.WindowState = FormWindowState.Minimized;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                clsCon = null;

            }
        }

        public void prcConfigureForm()
        {
            //Clear & Reset Toolbar & Ribbon
            toolMan.Tools.Clear();
            toolMan.Ribbon.Reset();

            dsMaster = new System.Data.DataSet();
            clsForm = new FormClass.clsMaster();

            //Retrieve Data From Database Through this function
            clsForm.prcGetData(ref dsMaster);

            string strCaption = "Loading Form";
            try
            {
                //Form property
                //this.Text = "GT Solution";
                this.WindowState = FormWindowState.Maximized;

                //Begin toolbar manager updation
                this.toolMan.BeginUpdate();

                //Initialize Ribbon
                this.prcInitializeRibbon();

                //Load Context Menu
                this.prcCreateApplicationMenu();

                //Initialize Tab Container
                prcInitTab();
                //Auto Update
                prcAutoUpdate();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, strCaption);
            }
            finally
            {
                //end toolbar manager updation
                this.toolMan.EndUpdate();
            }
        }

        private void prcInitializeRibbon()
        {
            string strCaption = "Initializing Ribbon";
            try
            {
                //Configure Ribbon 
                this.toolMan.Ribbon.Visible = true;
                this.toolMan.DockWithinContainer = this;
                this.toolMan.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
                
                //Setup Style Of Ribbon
                toolMan.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2010;
                toolMan.Ribbon.FileMenuStyle = Infragistics.Win.UltraWinToolbars.FileMenuStyle.ApplicationMenu2010;

                //ImageList 
                this.toolMan.ImageListLarge = ilLarge;
                this.toolMan.ImageListSmall = ilSmall;

                // Create Basic Ribbon Tab
                foreach (DataRow dr in dsMaster.Tables["Module"].Rows)
                {
                    strCaption = "Creating Tab";
                    RibbonTab rt = new RibbonTab(dr["moduleId"].ToString(), dr["moduleCaption"].ToString());
                    this.toolMan.Ribbon.Tabs.Add(rt);
                }

                //Creating Ribbon Group
                foreach (DataRow dr in dsMaster.Tables["Group"].Rows)
                {
                    strCaption = "Creating Group";
                    RibbonGroup rg = new RibbonGroup(dr["mMenuGroupId"].ToString(), dr["mMenuGroupCaption"].ToString());
                    this.toolMan.Ribbon.Tabs[dr["moduleId"].ToString()].Groups.Add(rg);
                }

                #region Creating Button
                //Creating Button
                foreach (DataRow dr in dsMaster.Tables["Menu"].Rows)
                {
                    strCaption = "Creating Button : "+dr["MenuCaption"].ToString();
                    #region Form Based Button
                    if (Int16.Parse(dr["isFormBased"].ToString()) != 0)
                    {
                        ButtonTool btn = new ButtonTool(dr["menuId"].ToString());
                        btn.SharedProps.Caption = dr["menuCaption"].ToString();

                        if (Int16.Parse(dr["menuImageExist"].ToString()) == 0)
                        {
                            //Picture does not Exist for menu
                            this.toolMan.Tools.Add(btn);
                            this.toolMan.Ribbon.Tabs[dr["moduleId"].ToString()].Groups[dr["mMenuGroupId"].ToString()].Tools.Add(btn);
                        }
                        else
                        {
                            //Picture Exist for menu
                            alMenuImage.Add(dr["menuId"].ToString());

                            //Picture Exist for menu
                            prcFillImageList(dr["menuImageName"].ToString(), Int16.Parse(dr["menuImageSize"].ToString()));
                            Common.Classes.clsMain.alMnuFrmName.Add(dr["frmName"].ToString());

                            if (Int16.Parse(dr["menuImageSize"].ToString()) != 2)
                            {
                                //Small Image
                                btn.SharedProps.AppearancesSmall.Appearance.Image = alMenuImage.Count - 1;//Common.Classes.clsMain.alMnuFrmName.Count - 1;
                            }
                            else
                            {
                                //Large Image
                                btn.SharedProps.AppearancesLarge.Appearance.Image = alMenuImage.Count - 1;
                            }
                            btn.SharedProps.DisplayStyle = ToolDisplayStyle.TextOnlyInMenus;

                            #region Normal Menu [No DropDown Menu]
                            if (Int16.Parse(dr["IsDropDown"].ToString()) == 0)
                            {
                                this.toolMan.Tools.Add(btn);

                                //Set Large Image for tool
                                ToolBase tb = this.toolMan.Ribbon.Tabs[dr["moduleId"].ToString()].Groups[dr["mMenuGroupId"].ToString()].Tools.AddTool(dr["MenuId"].ToString());
                                tb.CustomizedCaption = dr["menuCaption"].ToString();
                                if (Int16.Parse(dr["menuImageSize"].ToString()) == 2)
                                {
                                    tb.InstanceProps.PreferredSizeOnRibbon = RibbonToolSize.Large;
                                }
                            }
                            #endregion Normal Menu [No DropDown Menu]

                            #region DropDown Menu
                            if (Int16.Parse(dr["IsDropDown"].ToString()) == 1)
                            {
                                if (Int16.Parse(dr["DropDownParentId"].ToString()) == 0)
                                {
                                    Infragistics.Win.Appearance ap1 = new Infragistics.Win.Appearance();
                                    ap1.Image = alMenuImage.Count - 1;

                                    PopupMenuTool pmt = new PopupMenuTool(dr["menuId"].ToString());
                                    pmt.DropDownArrowStyle = DropDownArrowStyle.Segmented;
                                    pmt.SharedPropsInternal.Caption = dr["menuCaption"].ToString();
                                    pmt.SharedPropsInternal.Category = "DROPDOWNPARENT";        //Used to stop working when click on the main menu
                                    pmt.SharedPropsInternal.ToolTipText = "Select menu from list";
                                    pmt.InstanceProps.PreferredSizeOnRibbon = RibbonToolSize.Large;

                                    //Setup Appearance
                                    pmt.SharedPropsInternal.AppearancesSmall.Appearance = ap1;

                                    toolMan.Tools.Add(pmt);
                                    toolMan.Ribbon.Tabs[dr["moduleId"].ToString()].Groups[dr["mMenuGroupId"].ToString()].Tools.AddTool(pmt.Key.ToString());
                                }
                                else
                                {
                                    StateButtonTool sbt = new StateButtonTool(dr["menuId"].ToString());
                                    
                                    sbt.SharedPropsInternal.Caption = dr["menuCaption"].ToString();
                                    sbt.CustomizedImage = ilSmall.Images[alMenuImage.Count - 1];

                                    toolMan.Tools.Add(sbt);

                                    ((PopupMenuTool)(this.toolMan.Ribbon.Tabs[dr["moduleId"].ToString()].Groups[dr["mMenuGroupId"].ToString()].Tools[dr["DropdownParentId"].ToString()])).Tools.AddRange(new ToolBase[] { sbt });
                                }
                            }
                            #endregion DropDown Menu
                        }
                    }
                    #endregion Form Based Button

                    #region Combo Button
                    if ((dr["ContainerType"]).ToString() == "Combo")
                    {
                        //Crate Combobox tool
                        ComboBoxTool cbo = new ComboBoxTool("cbo" + dr["menuName"].ToString());

                        //Create ValueList For Loading Combo Data
                        ValueList valueList1 = new ValueList(0);
                        valueList1.DisplayStyle = Infragistics.Win.ValueListDisplayStyle.DisplayText;
                        valueList1.PreferredDropDownSize = new System.Drawing.Size(0, 0);

                        string strDefault = "";
                        foreach (DataRow dr3 in dsMaster.Tables["Company"].Rows)
                        {
                            if (Int16.Parse(dr3["isDefault"].ToString()) != 0)
                            {
                                strDefault = dr3["comId"].ToString();
                            }
                            ValueListItem v1 = new ValueListItem();
                            v1.DataValue = dr3["comId"].ToString();
                            v1.DisplayText = dr3["comName"].ToString();

                            valueList1.ValueListItems.Add(v1);
                        }
                        cbo.ValueList = valueList1;

                        this.toolMan.Tools.AddRange(new ToolBase[] { cbo });
                        ToolBase tbCompany = this.toolMan.Ribbon.Tabs[dr["moduleId"].ToString()].Groups[dr["mMenuGroupId"].ToString()].Tools.AddTool(cbo.Key);
                        //ToolBase tbCompany = ((RibbonGroup)dr["mMenuGroupId"]).Tools.AddTool(cbo.Key);

                        //Set Default Value
                        if (strDefault.Length > 0)
                        {
                            ((ComboBoxTool)toolMan.Tools[cbo.Key]).Value = Int16.Parse(strDefault);
                        }
                    }
                    #endregion Combo Button
                }
                #endregion Creating Button
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, strCaption);
            }
        }

        private void prcCreateButton(RibbonTab rt, RibbonGroup rtg, string strContainerFlag)
        {
            try
            {
                foreach (DataRow dr in dsMaster.Tables["Menu"].Rows)
                {
                    if (Int16.Parse(dr["isFormBased"].ToString()) != 0)
                    {
                        ButtonTool btn = new ButtonTool(dr["menuId"].ToString());
                        btn.SharedProps.Caption = dr["menuCaption"].ToString();

                        //Picture does not Exist for menu
                        if (Int16.Parse(dr["menuImageExist"].ToString()) == 0)
                        {
                            this.toolMan.Tools.Add(btn);
                            rtg.Tools.AddTool(dr["menuId"].ToString());
                        }
                        else
                        {
                            alMenuImage.Add(dr["menuId"].ToString());

                            //Picture Exist for menu
                            prcFillImageList(dr["menuImageName"].ToString(), Int16.Parse(dr["menuImageSize"].ToString()));
                            Common.Classes.clsMain.alMnuFrmName.Add(dr["frmName"].ToString());

                            if (Int16.Parse(dr["menuImageSize"].ToString()) != 2)
                            {
                                //Small Image
                                btn.SharedProps.AppearancesSmall.Appearance.Image = alMenuImage.Count - 1;//Common.Classes.clsMain.alMnuFrmName.Count - 1;
                            }
                            else
                            {
                                //Large Image
                                btn.SharedProps.AppearancesLarge.Appearance.Image = alMenuImage.Count - 1;
                            }
                            btn.SharedProps.DisplayStyle = ToolDisplayStyle.ImageAndText;
                            this.toolMan.Tools.Add(btn);

                            //Set Large Image
                            ToolBase tb = rtg.Tools.AddTool(dr["MenuId"].ToString());
                            tb.CustomizedCaption = dr["menuCaption"].ToString();
                            if (Int16.Parse(dr["menuImageSize"].ToString()) == 2)
                            {
                                tb.InstanceProps.PreferredSizeOnRibbon = RibbonToolSize.Large;
                            }
                        }
                    }

                    if (strContainerFlag == "Combo")
                    {
                        //Crate Combobox tool
                        ComboBoxTool cbo = new ComboBoxTool("cbo" + dr["menuName"].ToString());

                        //Create ValueList For Loading Combo Data
                        ValueList valueList1 = new ValueList(0);
                        valueList1.DisplayStyle = Infragistics.Win.ValueListDisplayStyle.DisplayText;
                        valueList1.PreferredDropDownSize = new System.Drawing.Size(0, 0);

                        string strDefault = "";
                        foreach (DataRow dr3 in dsMaster.Tables["Company"].Rows)
                        {
                            if (Int16.Parse(dr3["isDefault"].ToString()) != 0)
                            {
                                strDefault = dr3["comId"].ToString();
                            }
                            ValueListItem v1 = new ValueListItem();
                            v1.DataValue = dr3["comId"].ToString();
                            v1.DisplayText = dr3["comName"].ToString();

                            valueList1.ValueListItems.Add(v1);
                        }
                        cbo.ValueList = valueList1;

                        this.toolMan.Tools.AddRange(new ToolBase[] { cbo });
                        ToolBase tbCompany = rtg.Tools.AddTool(cbo.Key);
                        //Set Default Value
                        if (strDefault.Length > 0)
                        {
                            ((ComboBoxTool)toolMan.Tools[cbo.Key]).Value = Int16.Parse(strDefault);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Creating Button");
            }
        }

        private void prcFillImageList(string strImageName, int isLargeImage)
        {

            // Get all the icon files in the current directory.
            Common.Classes.clsMain.alMnuImgName.Add(Common.Classes.clsMain.strPicPathIcon + @"\" + strImageName);
            Icon newIcon = new Icon(Common.Classes.clsMain.strPicPathIcon + @"\" + strImageName);

            ilSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
            ilSmall.ImageSize = new System.Drawing.Size(16, 16);
            ilSmall.Images.Add(newIcon);

            ilLarge.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            ilLarge.ImageSize = new System.Drawing.Size(32, 32);
            ilLarge.Images.Add(newIcon);
        }

        private void prcCreateApplicationMenu()
        {
            // create a Button Tool that is displayed in Navigation menu
            ButtonTool btnLogout = new ButtonTool("Logout");
            btnLogout.SharedProps.Caption = "&Logout";
            this.toolMan.Tools.Add(btnLogout);

            ButtonTool btnExit = new ButtonTool("Exit");
            btnExit.SharedProps.Caption = "E&xit";
            this.toolMan.Tools.Add(btnExit);

            //this.toolMan.Tools.Add(applicationmenu2010ContainerTool);
            ApplicationMenu2010 appMan = this.toolMan.Ribbon.ApplicationMenu2010;

            // Add Button tool to the Navigation Menu of Office 2010 style application menu
            appMan.NavigationMenu.Tools.AddTool("Logout");
            appMan.NavigationMenu.Tools.AddTool("Exit");

            //ToolBase tbExit = appMan.NavigationMenu.Tools.AddTool("Exit");
            //tbExit.CustomizedCaption = "E&xit";
            //tbExit.InstanceProps.PreferredSizeOnRibbon = RibbonToolSize.Large;
        }

        private void toolMan_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.SharedPropsInternal.Category.Equals("DROPDOWNPARENT"))
            {
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                switch (e.Tool.Key)
                {
                    case "Logout":
                        toolMan.Tools.Clear();
                        toolMan.Ribbon.Reset();

                        //Upate user log
                        Common.Classes.clsMain CM = new Common.Classes.clsMain();
                        CM.prcLogout();
                        CM = null;

                        Common.Classes.clsMain.intComId = 0;
                        Common.Classes.clsMain.intUserId = 0;

                        prcResetTab();

                        frmLogin frm = new frmLogin(this);
                        frm.MdiParent = this;
                        frm.Show();

                        return;
                        break;
                    case "Exit":
                        this.Close();
                        return;
                        break;
                }
                //MessageBox.Show(e.Tool.Key);
                prcHideOpenForm();
                
                Classes.clsMain clsMain = new Classes.clsMain();
                DataRow[] dr = dsMaster.Tables["Menu"].Select("MenuId=" + e.Tool.Key);
                foreach (DataRow dr2 in dr)
                {
                    if (dr2["menuName"].ToString().Trim() == "Reset")
                    {
                        prcConfigureForm();
                        break;
                    }

                    frm = (Form)clsMain.GTRMakeFormNameAsObject("GTRHRIS." + dr2["frmLocation"] + ".", dr2["frmName"].ToString(), ref utcMain, this);
                    if (Classes.clsMain.fncExistOpenForm(frm) == false)
                    {
                        frm.Dock = System.Windows.Forms.DockStyle.Fill;
                        frm.MdiParent = this;
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.Show();

                        prcAddTab(frm.Name.ToString(), frm.Text.ToString());
                        //utcMain.Tabs[utcMain.Tabs.VisibleTabsCount - 1].Active = true;
                        utcMain.Tabs[utcMain.Tabs.VisibleTabsCount - 1].Selected = true;
                        //utcMain.Tabs[frm.Name].Active = true;
                    }
                    else
                    {
                        prcShowOpenForm(frm.Name.ToString());
                    }
                    break;
                }
                clsMain = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public void prcExecuteChildForm(string frmLocation, string frmName)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                prcHideOpenForm();

                Classes.clsMain clsMain = new Classes.clsMain();
                frm = (Form)clsMain.GTRMakeFormNameAsObject("GTRHRIS." + frmLocation + ".", frmName, ref utcMain, this);
                if (Classes.clsMain.fncExistOpenForm(frm) == false)
                {
                    frm.Dock = System.Windows.Forms.DockStyle.Fill;
                    frm.MdiParent = this;//.mdiPanel;
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.Show();

                    prcAddTab(frm.Name.ToString(), frm.Text.ToString());
                    utcMain.Tabs[utcMain.Tabs.VisibleTabsCount - 1].Selected = true;
                }
                else
                {
                    prcShowOpenForm(frm.Name.ToString());
                }
                clsMain = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void prcAddTab(string str1, string str2)
        {
            utcMain.Tabs.Add(str1, str2);
        }

        public void prcRemoveTab(Form frm)
        {
            try
            {
                int index = Common.Classes.clsMain.fncFindOpenFormIndex(frm);
                utcMain.Tabs.RemoveAt(index);
            }
            catch (Exception)
            {
            }
        }

        private void prcShowOpenForm(string str)
        {
            Form[] frm = this.MdiChildren;
            foreach (Form cfrm in frm)
            {
                if (cfrm.Name == str)
                {
                   
                    cfrm.Visible = true;
                    cfrm.StartPosition = FormStartPosition.CenterScreen;
                    cfrm.BringToFront();
                }
            }
        }

        private void prcHideOpenForm()
        {
            Form[] frm = this.MdiChildren;
            foreach (Form cfrm in frm)
            {
                cfrm.Visible = false;
            }
        }

        private void utcMain_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                string str = "";
                str = utcMain.ActiveTab.Key;
                prcHideOpenForm();
                prcShowOpenForm(str);
            }
            catch (Exception)
            {
            }
        }

        private void prcInitTab()
        {
            this.utcMain.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.StateButtons;
            this.utcMain.TabButtonStyle = Infragistics.Win.UIElementButtonStyle.ButtonSoft;
            this.utcMain.TabOrientation = Infragistics.Win.UltraWinTabs.TabOrientation.BottomLeft;
            this.utcMain.ShowTabListButton = DefaultableBoolean.True;
            this.utcMain.TabPadding = new System.Drawing.Size(20, 1);
            this.utcMain.TabSize = new System.Drawing.Size(20, 30);

            this.utcMain.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.utcMain.Visible = true;
        }

        private void toolMan_ToolValueChanged(object sender, ToolEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "cboSC":
                    string comId = ((ComboBoxTool)e.Tool).Value.ToString();
                    string comName = ((ComboBoxTool)e.Tool).Text.ToString();

                    Common.Classes.clsMain.intComId = Int16.Parse(comId);
                    Common.Classes.clsMain.strComName = comName;
                    this.Text = Common.Classes.clsMain.AppTitle + " || " + comName + "|| ";
                    break;
            }
        }

        private void prcResetTab()
        {
            try
            {
                for (int i = this.MdiChildren.Length - 1; i >= 0; i--)
                {
                    MdiChildren[i].Close();
                }
                utcMain.Tabs.Clear();
                Common.Classes.clsMain.openForm.Clear();

                utcMain.Visible = false;

                //Instruction
                dsFirst = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void prcShowReport(string FormCaption)
        {

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Classes.clsMain clsMain = new Classes.clsMain();
                frm = (Form)clsMain.GTRMakeFormNameAsObject("GTRHRIS.Common.FormEntry.", "frmrptViewer", ref utcMain, this);
                frm.WindowState = FormWindowState.Minimized;
                frm.Show();
                frm = null;
                clsMain = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            //Cursor.Current = Cursors.WaitCursor;
            //try
            //{
            //    prcHideOpenForm();

            //    Classes.clsMain clsMain = new Classes.clsMain();
            //    frm = (Form)clsMain.GTRMakeFormNameAsObject("GTRHRIS.Common.FormEntry.", "frmrptViewer", ref utcMain, this);
            //    if (Classes.clsMain.fncExistOpenForm(frm) == false)
            //    {
            //        frm.Dock = System.Windows.Forms.DockStyle.Fill;
            //        frm.MdiParent = this;//.mdiPanel;
            //        frm.Text = FormCaption;
            //        frm.StartPosition = FormStartPosition.CenterScreen;
            //        frm.Show();

            //        prcAddTab(frm.Name.ToString(), frm.Text.ToString());
            //        utcMain.Tabs[utcMain.Tabs.VisibleTabsCount - 1].Selected = true;
            //    }
            //    else
            //    {
            //        prcShowOpenForm(frm.Name.ToString());
            //    }
            //    clsMain = null;
            //    frm = null;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
            //finally
            //{
            //    Cursor.Current = Cursors.Default;
            //}
        }

        private void utcMain_TabClosing(object sender, TabClosingEventArgs e)
        {
            MdiChildren[utcMain.ActiveTab.Index].Close();
        }
    }
}