using SQLPLUS.Builder.BuildServices;
using SQLPLUS.Builder.ConfigurationModels;
using SQLPlusExtension.Models;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace SQLPlusExtension
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {

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

            if(buildDefinition.BuildOptions.IncludeModels == false || buildDefinition.BuildOptions.IncludeServices == false)
            {
                projectInformation.SetUpForIndividualProjects();
            }

            SQLPlusConfigurationWindowViewModel dataContext = new SQLPlusConfigurationWindowViewModel(
                configurationService,
                projectInformation,
                buildDefinition,
                databaseConnection,
                vsProject);

            SQLPlusConfigurationWindow configurationWindow = new SQLPlusConfigurationWindow(dataContext);
           
            var configurationResult = await configurationWindow.ShowDialogAsync(WindowStartupLocation.CenterOwner);

        }
    }
}
