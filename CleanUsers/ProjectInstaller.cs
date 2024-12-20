using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using CleanUsers;
namespace CleanUsers
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;
        Service1 service1;
        public ProjectInstaller()
        {
            InitializeComponent();

            // Instantiate installers for process and service.
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            // The service runs under the system account.
            processInstaller.Account = ServiceAccount.LocalSystem;

            // The service is started manually.
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.Description = "Clean Active Users Service";
            
            // ServiceName must be identical to the Windows Service name.
            serviceInstaller.ServiceName = "CleanupActiveUserService";
            this.AfterInstall += new InstallEventHandler(serviceInstaller1_AfterInstall);

            // Add installers to collection. Order is not important.
            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);

        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
            ServiceController serviceController = new ServiceController(serviceInstaller.ServiceName);
            try
            {
                serviceController.Start();
            }
            catch (Exception ex)
            {
                // Log or handle the error as needed

             service1.WriteErrorLog(ex.ToString(), "ErrorLog.txt");
                     }

        }

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
