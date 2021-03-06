﻿using System;
using System.IO;
using System.Management.Automation;
using System.Threading;
using Thycotic.RabbitMq.Helper.Logic;
using Thycotic.RabbitMq.Helper.Logic.IO;
using Thycotic.RabbitMq.Helper.Logic.Workflow;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Downloads RabbitMq
    /// </summary>
    /// <para type="synopsis">Downloads RabbitMq</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Get-DownloadLocations</para>
    /// <para type="link">Get-ErlangInstaller</para>
    /// <para type="link">Install-RabbitMq</para>
    /// <example>
    ///     <para>Download from rabbitmq's web site</para>
    ///     <para>PS C:\></para> 
    ///     <code>Get-RabbitMqInstaller</code>
    /// </example>
    /// <example>
    ///     <para>Download from Thycotic's mirror web site</para>
    ///     <para>PS C:\></para> 
    ///     <code>Get-RabbitMqInstaller -UseThycoticMirror</code>
    /// </example>
    /// <example>
    ///     <para>Force download from Thycotic's mirror web site even if the file already exists</para>
    ///     <para>PS C:\></para> 
    ///     <code>Get-RabbitMqInstaller -UseThycoticMirror -Force</code>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "RabbitMqInstaller")]
    public class GetRabbitMqInstallerCommand : Cmdlet
    {
        /// <summary>
        ///     The rabbit mq installer path
        /// </summary>
        public static readonly string RabbitMqInstallerPath = Path.Combine(Path.GetTempPath(), string.Format("rabbitMq{0}.exe", InstallationConstants.RabbitMq.Version));

        /// <summary>
        ///     Gets or sets the offline rabbit mq installer path.
        /// </summary>
        /// <value>
        ///     The offline rabbit mq installer path.
        /// </value>
        /// <para type="description">Gets or sets the offline rabbit mq installer path.</para>
        [Parameter(
             Position = 0,
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Offline)]
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParameterSets.Prepare)]
        [Alias("OfflinePath")]
        public string OfflineRabbitMqInstallerPath { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to force download even the file exists.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [force download]; otherwise, <c>false</c>.
        /// </value>
        /// <para type="description">Gets or sets a value indicating whether to force download even the file exists.</para>
        [Parameter(
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Online)]
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParameterSets.Prepare)]
        [Alias("ForceDownload")]
        public SwitchParameter Force { get; set; }


        /// <summary>
        ///     Gets or sets a value indicating whether to use the Thycotic Mirror during download.
        /// </summary>
        /// <value>
        ///     <c>true</c> if mirror will be used; otherwise, <c>false</c>.
        /// </value>
        /// <para type="description">Gets or sets a value indicating whether to use the Thycotic Mirror during download.</para>
        [Parameter(
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Online)]
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParameterSets.Prepare)]
        [Alias("Mirror")]
        public SwitchParameter UseThycoticMirror { get; set; }

        /// <summary>
        /// Gets or sets whether to prepare for offline install.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [force download]; otherwise, <c>false</c>.
        /// </value>
        /// <para type="description">Gets or sets whether to prepare for offline install.</para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParameterSets.Prepare)]
        public SwitchParameter PrepareForOfflineInstall { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">Installer does not exist</exception>
        protected override void ProcessRecord()
        {
            var activityId = ActivityIdProvider.GetNextActivityId();
            if (!string.IsNullOrWhiteSpace(OfflineRabbitMqInstallerPath))
            {
                if (PrepareForOfflineInstall)
                {
                    WriteDebug($"Preparing offline installer path {OfflineRabbitMqInstallerPath}");

                    if (Force)
                        WriteVerbose("Forcing download");

                    WriteVerbose("Downloading Erlang");

                    var downloader = new PrerequisiteDownloader();

                    var downloadUrl = UseThycoticMirror
                        ? InstallationConstants.RabbitMq.ThycoticMirrorDownloadUrl
                        : InstallationConstants.RabbitMq.DownloadUrl;

                    downloader.Download(CancellationToken.None, new Uri(downloadUrl, UriKind.Absolute),
                        OfflineRabbitMqInstallerPath, InstallationConstants.RabbitMq.InstallerChecksum, Force, 5,
                        WriteDebug, WriteVerbose, (s, exception) => throw exception,
                        progress =>
                        {
                            WriteProgress(new ProgressRecord(activityId, "RabbitMq download in progress", "Downloading")
                            {
                                PercentComplete = progress.ProgressPercentage,
                                RecordType = progress.ProgressPercentage == 100 ? ProgressRecordType.Completed : ProgressRecordType.Processing
                            });
                        });

                }
                else
                {
                    WriteVerbose(string.Format("Using offline installer path {0}", OfflineRabbitMqInstallerPath));

                    if (!File.Exists(OfflineRabbitMqInstallerPath))
                    {
                        throw new FileNotFoundException("Installer does not exist");
                    }

                    if (PrerequisiteDownloader.CalculateSha512(OfflineRabbitMqInstallerPath) !=
                        InstallationConstants.RabbitMq.InstallerChecksum)
                    {
                        throw new FileNotFoundException("Installer checksum does not match");
                    }

                    if (File.Exists(RabbitMqInstallerPath))
                        File.Delete(RabbitMqInstallerPath);

                    File.Copy(OfflineRabbitMqInstallerPath, RabbitMqInstallerPath);
                }
            }
            else
            {
                if (Force)
                    WriteVerbose("Forcing download");

                WriteVerbose("Downloading RabbitMq");

                var downloader = new PrerequisiteDownloader();

                var downloadUrl = UseThycoticMirror
                    ? InstallationConstants.RabbitMq.ThycoticMirrorDownloadUrl
                    : InstallationConstants.RabbitMq.DownloadUrl;

                downloader.Download(CancellationToken.None, new Uri(downloadUrl, UriKind.Absolute),
                    RabbitMqInstallerPath, InstallationConstants.RabbitMq.InstallerChecksum, Force, 5, WriteDebug, WriteVerbose, (s, exception) => throw exception,
                    progress =>
                    {
                        WriteProgress(new ProgressRecord(activityId, "RabbitMq download in progress", "Downloading")
                        {
                            PercentComplete = progress.ProgressPercentage,
                            RecordType = progress.ProgressPercentage == 100 ? ProgressRecordType.Completed : ProgressRecordType.Processing
                        });
                    });

            }
        }

        private static class ParameterSets
        {
            public const string Offline = "Offline";
            public const string Online = "Online";
            public const string Prepare = "Prepare";
        }
    }
}