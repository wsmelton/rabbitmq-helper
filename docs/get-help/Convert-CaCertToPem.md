# Convert-CaCertToPem

```powershell
NAME
    Convert-CaCertToPem
    
SYNOPSIS
    Converts a Certificate Authority cert to a pem.
    
    
SYNTAX
    Convert-CaCertToPem [-CaCertPath] <string> [<CommonParameters>]
    
    
DESCRIPTION
    The Convert-CaCertToPem cmdlet converts a Certificate Authority cert to a pem.
    
    The pem file will be located in the Thycotic RabbitMq Site Connector folder.
    

PARAMETERS
    -CaCertPath <string>
        Gets or sets the ca cert path.
        
    <CommonParameters>
        This cmdlet supports the common parameters: Verbose, Debug,
        ErrorAction, ErrorVariable, WarningAction, WarningVariable,
        OutBuffer, PipelineVariable, and OutVariable. For more information, see 
        about_CommonParameters (https:/go.microsoft.com/fwlink/?LinkID=113216). 
    
    ----------  EXAMPLE 1  ----------
    
    PS C:\>
    Convert-CaCertToPem -CaCertPath "$PSScriptRoot\..\Examples\sc.cer" -Verbose
    
REMARKS
    To see the examples, type: "get-help Convert-CaCertToPem -examples".
    For more information, type: "get-help Convert-CaCertToPem -detailed".
    For technical information, type: "get-help Convert-CaCertToPem -full".
    For online help, type: "get-help Convert-CaCertToPem -online"
```

