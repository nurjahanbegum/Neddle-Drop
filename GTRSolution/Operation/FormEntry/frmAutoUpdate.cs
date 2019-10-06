using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using SharpCompress.Common;
using SharpCompress.Reader;


namespace GTRHRIS.Operation.FormEntry
{
    public partial class frmAutoUpdate : Form
    {

        private string FileLocation = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\") + 0).ToString();  //@"C:\Program Files\Microsoft\Regency_Setup";
        
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        WebClient webClient;    // Our WebClient that will be doing the downloading for us
        Stopwatch sw = new Stopwatch();
        private DataRow dr;
        private string OV, NV,FileType;
        public frmAutoUpdate(ref string strOV,string strNV,string strFileType)
        {
            InitializeComponent();
            OV = strOV;
            NV = strNV;
            FileType = strFileType;
        }

        private void frmAutoUpdate_Load(object sender, EventArgs e)
        {
            lvlOV.Text = "Old Version " + OV;
            lvlNV.Text = "New Version " + NV;
            //MessageBox.Show(Properties.Settings.Default.AUId);
            btnNotnow.Visible = true;
            if(FileType=="1")
            {
                btnNotnow.Visible = false;
            }
        }

        private void btnNotnow_Click(object sender, EventArgs e)
        {

            Application.Exit();
            //this.Close();

        }
        public void DownloadFile(string urlAddress, string location)
        {
            using (webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                // webClient.DownloadProgressChanged
                try
                {
                    // The variable that will be holding the url address
                    Uri URL;

                    // Make sure the url starts with "http://"
                    if (!urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                        URL = new Uri("http://" + urlAddress);
                    else
                        URL = new Uri(urlAddress);

                    // Start the stopwatch which we will be using to calculate the download speed
                    sw.Start();

                    // Start downloading the file
                    webClient.DownloadFileAsync(URL, location);
                    //webClient.UploadFileAsync();


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            try
            {
                // Calculate download speed and output it to label3
                if (labelPerc.Text != (Convert.ToDouble(e.BytesReceived) / 1024 / sw.Elapsed.TotalSeconds).ToString("0"))
                    labelSpeed.Text = (Convert.ToDouble(e.BytesReceived) / 1024 / sw.Elapsed.TotalSeconds).ToString("0.00") + " kb/s";

                // Update the progressbar percentage only when the value is not the same (to avoid updating the control constantly)
                if (ultraProgressBar1.Value != e.ProgressPercentage)

                    ultraProgressBar1.Value = e.ProgressPercentage;

                // Show the percentage on our label (update only if the value isn't the same to avoid updating the control constantly)
                if (labelPerc.Text != e.ProgressPercentage.ToString() + "%")
                    labelPerc.Text = e.ProgressPercentage.ToString() + "%";

                // Update the label with how much data have been downloaded so far and the total size of the file we are currently downloading
                labelDownloaded.Text = (Convert.ToDouble(e.BytesReceived) / 1024 / 1024).ToString("0.00") + " Mb's" + "  /  " + (Convert.ToDouble(e.TotalBytesToReceive) / 1024 / 1024).ToString("0.00") + " Mb's";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            sw.Reset();
            if (e.Cancelled == true)
            {
                File.Delete(saveFileDialog.FileName);       // Delete the incomplete file if the download is canceled
                MessageBox.Show("Canceled");
            }
            else
              
            PrcExtruct(saveFileDialog.FileName, saveFileDialog.FileName);
            if (MessageBox.Show("Download Complete!!!" + Environment.NewLine + Environment.NewLine + "Are You Restart Your Software ?", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                this.Close();
            }
            else
            {
                Application.Exit();
            }
        }
        private void PrcExtruct(string urlSource, string urlDestination)
        {
            try
            {

                string asd = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\") + 1).ToString() + "GTRHRISOld.exe";
              
                if (File.Exists(asd))
                {
                    File.Delete(asd);
                }
                if (dr["FileFormate"].ToString().ToUpper() == "EXE".ToUpper() || dr["FileFormate"].ToString().ToUpper() == "ALL".ToUpper())
                {
                    System.IO.File.Move(Application.ExecutablePath,
                                        Application.ExecutablePath.Substring(0,Application.ExecutablePath.LastIndexOf("\\") +1) + "GTRHRISOld.exe");
                }
                using (Stream stream = File.OpenRead(Application.StartupPath + "/" + dr["FileSourceName"].ToString().Substring(dr["FileSourceName"].ToString().LastIndexOf("/") + 1)))
                {
                    var reader = ReaderFactory.Open(stream);
                    Properties.Settings.Default.AUId = dr["AutoDownload_ID"].ToString();
                    Properties.Settings.Default.Save();
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory)
                        {
                            Console.WriteLine(reader.Entry.FilePath);
                            reader.WriteEntryToDirectory(Application.StartupPath + "/",
                                                         ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);

                           
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return;
            }
        }

        //private void btnUpgrade_Click(object sender, EventArgs e)
        //{
        //    //this.WindowState = FormWindowState.Minimized;
        //    GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
        //    System.Data.DataSet dsAutoupdate = new System.Data.DataSet();

        //    try
        //    {
        //        string sqlQuery =
        //            "Select top(1) AutoDownload_ID,LuserID,FileType,FileFormate, FileName, FilePath, FileSourceName, IsDownLoad, VershionNo From tbl_AutoUpdate where IsDownLoad=0  Order by AutoDownload_ID Desc";

        //        clsCon.GTRFillDatasetWithSQLCommand(ref dsAutoupdate, sqlQuery);

        //        dsAutoupdate.Tables[0].TableName = "AutoUpdate";
        //        if (dsAutoupdate.Tables["AutoUpdate"].Rows.Count > 0)
        //        {
        //            dr = dsAutoupdate.Tables[0].Rows[0];
        //            if (dr["VershionNo"].ToString() != OV)
        //            {
        //                DownloadFile(dr["FileSourceName"].ToString(), Application.StartupPath + "/" + dr["FileSourceName"].ToString().Substring(dr["FileSourceName"].ToString().LastIndexOf("/") + 1));
        //            }
        //            else
        //            {
        //                MessageBox.Show("You Already updatet");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        clsCon = null;

        //    }
        //}

        //private void btnUpgrade_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        string asd = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\") + 1).ToString() + "GTRHRISOld.exe";

        //        if (File.Exists(asd))
        //        {
        //            File.Delete(asd);
        //        }
        //        if (dr["FileFormate"].ToString().ToUpper() == "EXE".ToUpper() || dr["FileFormate"].ToString().ToUpper() == "ALL".ToUpper())
        //        {
        //            System.IO.File.Move(Application.ExecutablePath,
        //                                Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\") + 1) + "GTRHRISOld.exe");
        //        }
        //        using (Stream stream = File.OpenRead(Application.StartupPath + "/" + dr["FileSourceName"].ToString().Substring(dr["FileSourceName"].ToString().LastIndexOf("/") + 1)))
        //        {
        //            var reader = ReaderFactory.Open(stream);
        //            Properties.Settings.Default.AUId = dr["AutoDownload_ID"].ToString();
        //            Properties.Settings.Default.Save();
        //            while (reader.MoveToNextEntry())
        //            {
        //                if (!reader.Entry.IsDirectory)
        //                {
        //                    Console.WriteLine(reader.Entry.FilePath);
        //                    reader.WriteEntryToDirectory(Application.StartupPath + "/",
        //                                                 ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);


        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.Message);
        //        return;
        //    }
        //}

        private void btnUpgrade_Click(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Minimized;
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            System.Data.DataSet dsAutoupdate = new System.Data.DataSet();

            try
            {

                if (File.Exists(FileLocation + "\\Debug.rar"))
                {
                    File.Delete(FileLocation + "\\Debug.rar");
                }
                
                
                string fileName = "Debug.rar";
                string sourcePath = @"\\YABDPAYROLL\GTR Exe Update";  //@"Z:\Regency";
                string targetPath = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\") + 0).ToString(); //@"C:\Program Files\Microsoft\Regency_Setup";

                // Use Path class to manipulate file and directory paths. 
                string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                string destFile = System.IO.Path.Combine(targetPath, fileName);

                // To copy a folder's contents to a new location: 
                // Create a new target folder, if necessary. 
                if (!System.IO.Directory.Exists(targetPath))
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                }

                // To copy a file to another location and  
                // overwrite the destination file if it already exists.
                System.IO.File.Copy(sourceFile, destFile, true);

                // To copy all the files in one directory to another directory. 
                // Get the files in the source folder. (To recursively iterate through 
                // all subfolders under the current directory, see 
                // "How to: Iterate Through a Directory Tree.")
                // Note: Check for target path was performed previously 
                //       in this code example. 
                if (System.IO.Directory.Exists(sourcePath))
                {
                    string[] files = System.IO.Directory.GetFiles(sourcePath);

                    // Copy the files and overwrite destination files if they already exist. 
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        fileName = System.IO.Path.GetFileName(s);
                        destFile = System.IO.Path.Combine(targetPath, fileName);
                        System.IO.File.Copy(s, destFile, true);
                    }
                }
                else
                {
                    Console.WriteLine("Source path does not exist!");
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

            if (File.Exists(FileLocation + "\\GTRHRISOLD.exe"))
            {
                File.Delete(FileLocation + "\\GTRHRISOLD.exe");
            }


            System.IO.File.Move(Application.ExecutablePath,
                                Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\") + 1) + "GTRHRISOld.exe");



            if (File.Exists(FileLocation + "\\GTRHRIS.exe"))
            {
                File.Delete(FileLocation + "\\GTRHRIS.exe");
            }


            //string path = @"C:\Program Files\Microsoft\Regency_Setup\Debug.rar";
            //string spath = @"C:\Program Files\Microsoft\Regency_Setup";

            string path = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\") + 0).ToString() + "\\Debug.rar";
            string spath = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\") + 0).ToString();

            using (Stream stream = File.OpenRead(path))
            {
                var reader = ReaderFactory.Open(stream);
                Properties.Settings.Default.Save();
                reader.WriteAllToDirectory(spath, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);

            }

            Application.Exit();
        }


    }
}
