using System;
using System.Data;

namespace GTRHRIS.Common.FormClass
{
    internal class clsMaster
    {
        public void prcGetData(ref System.Data.DataSet dsMenu)
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            clsCon.GTRFillDatasetWithSQLCommand(ref dsMenu, "Exec prcGetMenuPermission " + (Int32)Classes.clsMain.intUserId);

            dsMenu.Tables[0].TableName = "Module";
            dsMenu.Tables[1].TableName = "Group";
            dsMenu.Tables[2].TableName = "Menu";
            dsMenu.Tables[3].TableName = "Company";
        }
    }
}