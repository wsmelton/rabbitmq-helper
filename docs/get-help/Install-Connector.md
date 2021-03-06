# Install-Connector

```powershell
NAME
    Install-Connector
    
SYNOPSIS
    Installs the site connector.
    
    
SYNTAX
    Install-Connector -Credential <PSCredential> -OfflineErlangInstallerPath <string> -OfflineRabbitMqInstallerPath <string> [-AgreeErlangLicense <SwitchParameter>] [-AgreeRabbitMqLicense <SwitchParameter>] [<CommonParameters>]
    
    Install-Connector -CaCertPath <string> -Credential <PSCredential> -Hostname <string> -PfxCredential <PSCredential> -PfxPath <string> -UseTls <SwitchParameter> [-AgreeErlangLicense <SwitchParameter>] [-AgreeRabbitMqLicense <SwitchParameter>] 
    [-OfflineErlangInstallerPath <string>] [-OfflineRabbitMqInstallerPath <string>] [-UseThycoticMirror <SwitchParameter>] [<CommonParameters>]
    
    Install-Connector -Credential <PSCredential> [-AgreeErlangLicense <SwitchParameter>] [-AgreeRabbitMqLicense <SwitchParameter>] [-ForceDownload <SwitchParameter>] [-UseThycoticMirror <SwitchParameter>] [<CommonParameters>]
    
    
DESCRIPTION
    The Install-Connector cmdlet is designed to make the installation of a non-TLS and TLS site connector.
    
    It will install both Erlang and RabbitMq provided that the appropriate parameters are supplied.
    
    The cmdlet requires that a basic user also be created. This user is strictly for putting and pulling messages from RabbitMq.
    

PARAMETERS
    -AgreeRabbitMqLicense <SwitchParameter>
        Gets or sets the agree rabbit mq license. If omitted, the user will not be prompted to agree to the license.
        
    -AgreeErlangLicense <SwitchParameter>
        Gets or sets the agree Erlang license. If omitted, the user will not be prompted to agree to the license.
        
    -OfflineErlangInstallerPath <string>
        Gets or sets the offline Erlang installer path. If omitted, the installer will be downloaded.
        
    -OfflineRabbitMqInstallerPath <string>
        Gets or sets the offline RabbitMq installer path to use. If omitted, the installer will be downloaded.
        
    -ForceDownload <SwitchParameter>
        Gets or sets a value indicating whether force download (even they already exist) the pre-requisites. This value has no effect when using an offline installer.
        
    -Force <SwitchParameter>
        Gets or sets a value indicating whether force download (even they already exist) the pre-requisites. This value has no effect when using an offline installer.
        
        This is an alias of the ForceDownload parameter.
        
    -UseThycoticMirror <SwitchParameter>
        Gets or sets a value indicating whether to use the Thycotic Mirror even if the file exists.
        
    -Mirror <SwitchParameter>
        Gets or sets a value indicating whether to use the Thycotic Mirror even if the file exists.
        
        This is an alias of the UseThycoticMirror parameter.
        
    -Credential <PSCredential>
        Gets or sets the name of the rabbit mq user.
        
    -UseTls <SwitchParameter>
        Gets or sets whether to use TLS or not.
        
    -Hostname <string>
        Gets or sets the hostname or FQDN of the server which will host the RabbitMq node.
        
    -SubjectName <string>
        Gets or sets the hostname or FQDN of the server which will host the RabbitMq node.
        
        This is an alias of the Hostname parameter.
        
    -FQDN <string>
        Gets or sets the hostname or FQDN of the server which will host the RabbitMq node.
        
        This is an alias of the Hostname parameter.
        
    -CaCertPath <string>
        Gets or sets the CA certificate path. This certificate is use to establish the trust chain to the CA.
        
    -PfxPath <string>
        Gets or sets the PFX path. This could be a self-signed or a certificate from a public CA.
        
        If self-signed, the certificate should be installed on all client/engine machines. It does NOT to be installed on the RabbitMq node.
        
    -PfxCredential <PSCredential>
        Gets or set the credential for the PFX. Username part is ignored.
        
    <CommonParameters>
        This cmdlet supports the common parameters: Verbose, Debug,
        ErrorAction, ErrorVariable, WarningAction, WarningVariable,
        OutBuffer, PipelineVariable, and OutVariable. For more information, see 
        about_CommonParameters (https:/go.microsoft.com/fwlink/?LinkID=113216). 
    
    ----------  EXAMPLE 1  ----------
    
    The most basic use case to install RabbitMq is to have a single node without using encryption.
    This is generally useful during development or during POC stages.
    To do so, you could use the following:
    PS C:\>Install-Connector -agreeErlangLicense -agreeRabbitMqLicense
    
REMARKS
    To see the examples, type: "get-help Install-Connector -examples".
    For more information, type: "get-help Install-Connector -detailed".
    For technical information, type: "get-help Install-Connector -full".
    For online help, type: "get-help Install-Connector -online"
```

