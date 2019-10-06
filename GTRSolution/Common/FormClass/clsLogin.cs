using System.Data;
using GTRLibrary;

namespace GTRHRIS.Common.FormClass
{
    internal class clsLogin
    {
        clsProcedure clsProc =new clsProcedure();
        public clsLogin()
        {
        }
        
        public void prcGetLoginDetails(ref System.Data.DataSet ds, string userName, string userPassword)
        {
            
            clsConnection clsCon = new clsConnection();

            string sqlQuery = "Select *, getdate() as 'LoginDate' from viewLogin_User where IsInactive=0 and LUserName='" + clsProc.GTREncryptWord(userName) + "' and LUserPass='" + clsProc.GTREncryptWord(userPassword) + "'";
            clsCon.GTRFillDatasetWithSQLCommand(ref ds, sqlQuery);
            ds.Tables[0].TableName = "Login";
        }
        
    }
}