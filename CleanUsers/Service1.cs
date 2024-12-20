using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Services;
using CleanUsers.CleanActveUsers; 


namespace CleanUsers
{
    public partial class Service1 : ServiceBase
    {
        private System.Timers.Timer timer;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Initialize the timer
            timer = new System.Timers.Timer();
            timer.Interval = 600000; // 10 minute intervals
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Start();
        }


        private void OnElapsedTime(object sender, ElapsedEventArgs e)
        {
            try
            {
                // Create a web service client

                CleanActveUsers.Service serviceClient = new CleanActveUsers.Service();
                
                   
                serviceClient.CleanActiveUser(); //CleanActiveUser

                serviceClient.RecordLoginStats(); 
                            
            }
            catch (Exception ex)
            {
                // Log the error 
                WriteErrorLog(ex.ToString(), "ErrorLog.txt");

            }   }

        protected override void OnStop()
        {
            timer.Stop();
            timer.Dispose();
        }



  
        public void WriteErrorLog(string Message, string FileName)
        {

            //Uncomment below to disable writing any log information of errors into the log file.
            StreamWriter streamWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" + FileName + ".txt", true);
            try
            {
                streamWriter.WriteLine(DateTime.Now.ToString() + ": " + Message);
                streamWriter.Flush();
                streamWriter.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                }
            }
        }




        //[RunInstaller(true)]
        //public class ProjectInstaller : Installer
        //{
        //    private ServiceProcessInstaller processInstaller;
        //    private ServiceInstaller serviceInstaller;

        //    public ProjectInstaller()
        //    {
           
        //        processInstaller = new ServiceProcessInstaller
        //        {
        //            Account = ServiceAccount.LocalSystem
        //        };

        //        serviceInstaller = new ServiceInstaller
        //        {
        //            ServiceName = "CleanActiveUserService",
        //            DisplayName = "CleanActiveUserService",
        //            Description = "A service that cleans up active users periodically.",
        //            StartType = ServiceStartMode.Manual
        //        };

        //        Installers.Add(processInstaller);
        //        Installers.Add(serviceInstaller);
        //    }
        //}

    }
}
