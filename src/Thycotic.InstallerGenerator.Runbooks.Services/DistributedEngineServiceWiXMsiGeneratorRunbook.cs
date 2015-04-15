﻿using Thycotic.InstallerGenerator.Core.MSI.WiX;
using Thycotic.InstallerGenerator.Core.Steps;
using Thycotic.InstallerGenerator.MSI.WiX;

namespace Thycotic.InstallerGenerator.Runbooks.Services
{
    public class DistributedEngineServiceWiXMsiGeneratorRunbook : WiXMsiGeneratorRunbook
    {
        public const string DefaultArtifactName = "Thycotic.DistributedEngine.Service";

        public override void BakeSteps()
        {
            ArtifactName = GetArtifactFileName(DefaultArtifactName, Version);

            Steps = new IInstallerGeneratorStep[]
            {
                new ExternalProcessStep
                {
                    Name = "Heat process",
                    WorkingPath = WorkingPath,
                    ExecutablePath = ToolPaths.Heat,
                    Parameters = string.Format(@"
dir {0}
-nologo
-o output\Autogenerated.wxs 
-ag 
-sfrags 
-suid 
-cg main_component_group 
-t add_service_install.xsl 
-sreg 
-scom 
-srd 
-template fragment 
-dr INSTALLLOCATION", SourcePath)

                },
                new ExternalProcessStep
                {
                    Name = "Candle process",
                    WorkingPath = WorkingPath,
                    ExecutablePath = ToolPaths.Candle,
                    Parameters = string.Format(@"
-nologo 
-ext WixUtilExtension 
-dInstallerVersion={0} 
-out output\
output\AutoGenerated.wxs Product.wxs", Version)
                },
                new ExternalProcessStep
                {
                    Name = "Light process",
                    WorkingPath = WorkingPath,
                    ExecutablePath = ToolPaths.Light,
                    Parameters = string.Format(@"
-nologo
-b {0}
-ext WixUIExtension 
-ext WixUtilExtension 
-out {1}
output\AutoGenerated.wixobj output\Product.wixobj", SourcePath, ArtifactName)
                }
            };
        }
    }
}