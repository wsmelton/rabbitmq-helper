# Get-RabbitMqInstaller

```powershell
NAME
    Get-RabbitMqInstaller
    
SYNOPSIS
    Downloads RabbitMq
    
    
SYNTAX
    Get-RabbitMqInstaller [-OfflineRabbitMqInstallerPath] <string> [<CommonParameters>]
    
    Get-RabbitMqInstaller [-OfflineRabbitMqInstallerPath] <string> [-Force <SwitchParameter>] [-PrepareForOfflineInstall <SwitchParameter>] [-UseThycoticMirror <SwitchParameter>] [<CommonParameters>]
    
    Get-RabbitMqInstaller [-Force <SwitchParameter>] [-UseThycoticMirror <SwitchParameter>] [<CommonParameters>]
    
    
DESCRIPTION
    
    

PARAMETERS
    -OfflineRabbitMqInstallerPath <string>
        Gets or sets the offline rabbit mq installer path.
        
    -OfflinePath <string>
        Gets or sets the offline rabbit mq installer path.
        
        This is an alias of the OfflineRabbitMqInstallerPath parameter.
        
    -Force <SwitchParameter>
        Gets or sets a value indicating whether to force download even the file exists.
        
    -ForceDownload <SwitchParameter>
        Gets or sets a value indicating whether to force download even the file exists.
        
        This is an alias of the Force parameter.
        
    -UseThycoticMirror <SwitchParameter>
        Gets or sets a value indicating whether to use the Thycotic Mirror during download.
        
    -Mirror <SwitchParameter>
        Gets or sets a value indicating whether to use the Thycotic Mirror during download.
        
        This is an alias of the UseThycoticMirror parameter.
        
    -PrepareForOfflineInstall <SwitchParameter>
        Gets or sets whether to prepare for offline install.
        
    <CommonParameters>
        This cmdlet supports the common parameters: Verbose, Debug,
        ErrorAction, ErrorVariable, WarningAction, WarningVariable,
        OutBuffer, PipelineVariable, and OutVariable. For more information, see 
        about_CommonParameters (https:/go.microsoft.com/fwlink/?LinkID=113216). 
    
    ----------  EXAMPLE 1  ----------
    
    Download from rabbitmq's web site
    PS C:\>Get-RabbitMqInstaller
    
    ----------  EXAMPLE 2  ----------
    
    Download from Thycotic's mirror web site
    PS C:\>Get-RabbitMqInstaller -UseThycoticMirror
    
    ----------  EXAMPLE 3  ----------
    
    Force download from Thycotic's mirror web site even if the file already exists
    PS C:\>Get-RabbitMqInstaller -UseThycoticMirror -Force
    
REMARKS
    To see the examples, type: "get-help Get-RabbitMqInstaller -examples".
    For more information, type: "get-help Get-RabbitMqInstaller -detailed".
    For technical information, type: "get-help Get-RabbitMqInstaller -full".
    For online help, type: "get-help Get-RabbitMqInstaller -online"
```

