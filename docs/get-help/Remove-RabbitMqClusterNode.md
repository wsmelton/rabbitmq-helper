# Remove-RabbitMqClusterNode

```powershell
NAME
    Remove-RabbitMqClusterNode
    
SYNOPSIS
    Removes a node from the current nodes cluster. Use when a node is not responsive and/or cannot leave the cluster
    
    
SYNTAX
    Remove-RabbitMqClusterNode [-StrictHostname] <string> [<CommonParameters>]
    
    
DESCRIPTION
    
    

PARAMETERS
    -StrictHostname <string>
        Gets or sets name of the other node. Not the FQDN. Has to match exactly what the target machine thinks its name is, including case.
        
    <CommonParameters>
        This cmdlet supports the common parameters: Verbose, Debug,
        ErrorAction, ErrorVariable, WarningAction, WarningVariable,
        OutBuffer, PipelineVariable, and OutVariable. For more information, see 
        about_CommonParameters (https:/go.microsoft.com/fwlink/?LinkID=113216). 
    
    ----------  EXAMPLE 1  ----------
    
    PS C:\>Remove-RabbitMqClusterNode UnresponsiveNode1
    
REMARKS
    To see the examples, type: "get-help Remove-RabbitMqClusterNode -examples".
    For more information, type: "get-help Remove-RabbitMqClusterNode -detailed".
    For technical information, type: "get-help Remove-RabbitMqClusterNode -full".
    For online help, type: "get-help Remove-RabbitMqClusterNode -online"
```

