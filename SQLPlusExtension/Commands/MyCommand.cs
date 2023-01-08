using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.RpcContracts.Build;
using Microsoft.VisualStudio.Shell.Interop;
using Newtonsoft.Json;
using SQLPLUS.Builder;
using SQLPLUS.Builder.BuildServices;
using SQLPLUS.Builder.ConfigurationModels;
using SQLPLUS.Builder.DataCollectors;
using SQLPLUS.Builder.Render;
using SQLPLUS.Builder.Render.Common;
using SQLPlusExtension.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using VSLangProj;

namespace SQLPlusExtension
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {
        
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            //TODO: Check the user is registered and logged in, if not show the login screen.


            Project vsProject = await VS.Solutions.GetActiveProjectAsync();
            if(vsProject is null)
            {
                await VS.MessageBox.ShowAsync("Could not determine the active project");
                return;
            }

            ProjectInformation projectInformation = new ProjectInformation(vsProject.Name, Path.GetDirectoryName(vsProject.FullPath));
            ConfigurationService configurationService = new ConfigurationService(projectInformation);

            List<string> createdConfigurationFiles = configurationService.CreateConfigurationDirectoriesAndFiles();
            if(createdConfigurationFiles.Count != 0)
            {
                await vsProject.AddExistingFilesAsync(createdConfigurationFiles.ToArray());
                await vsProject.SaveAsync();
            }
            
            var databaseConnection = configurationService.GetDatabaseConnection();
            var buildDefinition = configurationService.GetBuildDefinition();

            SQLPlusConfigurationWindowViewModel dataContext = new SQLPlusConfigurationWindowViewModel(
                configurationService,
                projectInformation,
                buildDefinition,
                databaseConnection,
                vsProject);

            SQLPlusConfigurationWindow window = new SQLPlusConfigurationWindow(dataContext);
           
            var result = await window.ShowDialogAsync(WindowStartupLocation.CenterOwner);

        }

    }
}
