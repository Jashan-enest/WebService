using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.ServiceModel;
using System.ServiceModel.Web;
using Microsoft.WindowsAzure.Diagnostics.Management;

namespace WebRole1
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {

            String wadConnectionString = "Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString";

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue(wadConnectionString));

            RoleInstanceDiagnosticManager roleInstanceDiagnosticManager = cloudStorageAccount.CreateRoleInstanceDiagnosticManager(RoleEnvironment.DeploymentId, RoleEnvironment.CurrentRoleInstance.Role.Name, RoleEnvironment.CurrentRoleInstance.Id);
            DiagnosticMonitorConfiguration diagnosticMonitorConfiguration = roleInstanceDiagnosticManager.GetCurrentConfiguration();

            diagnosticMonitorConfiguration.Directories.ScheduledTransferPeriod = TimeSpan.FromMinutes(5d);

            //Diagnostics logs
            diagnosticMonitorConfiguration.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1d);

            //Diagnostics Infrastructure logs
            diagnosticMonitorConfiguration.DiagnosticInfrastructureLogs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1d);

            //Windows Event Logs
            diagnosticMonitorConfiguration.WindowsEventLog.DataSources.Add("Application!*");
            diagnosticMonitorConfiguration.WindowsEventLog.DataSources.Add("System!*");
            diagnosticMonitorConfiguration.WindowsEventLog.ScheduledTransferPeriod = TimeSpan.FromMinutes(5d);

            //---------->Add performance counter monitoring for configured counters
            //Processor Usage Counter
            PerformanceCounterConfiguration performanceCounterConfiguration = new PerformanceCounterConfiguration();
            performanceCounterConfiguration.CounterSpecifier = @"\Processor(_Total)\% Processor Time";
            performanceCounterConfiguration.SampleRate = System.TimeSpan.FromSeconds(10d);
            diagnosticMonitorConfiguration.PerformanceCounters.DataSources.Add(performanceCounterConfiguration);
            diagnosticMonitorConfiguration.PerformanceCounters.ScheduledTransferPeriod = TimeSpan.FromMinutes(1d);
            roleInstanceDiagnosticManager.SetCurrentConfiguration(diagnosticMonitorConfiguration);
            //Processor Usage Counter
            PerformanceCounterConfiguration performanceCounterConfigurationMemory = new PerformanceCounterConfiguration();
            performanceCounterConfigurationMemory.CounterSpecifier = @"\Memory\Available Mbytes";
            performanceCounterConfigurationMemory.SampleRate = System.TimeSpan.FromSeconds(10d);
            diagnosticMonitorConfiguration.PerformanceCounters.DataSources.Add(performanceCounterConfigurationMemory);
            diagnosticMonitorConfiguration.PerformanceCounters.ScheduledTransferPeriod = TimeSpan.FromMinutes(1d);
            roleInstanceDiagnosticManager.SetCurrentConfiguration(diagnosticMonitorConfiguration);

            //Crash Dumps
            CrashDumps.EnableCollection(true);

            return base.OnStart();
        }
    }
}
