# Reset-RabbitMqNodeCommand

```powershell
NAME
    Reset-RabbitMqNodeCommand
    
SYNOPSIS
    Returns a RabbitMQ node to its original state. Removes the node from any cluster it belongs to, removes all data from the management database, such as configured users and vhosts, and deletes all persistent messages.
    
    
SYNTAX
    Reset-RabbitMqNodeCommand [-Force <SwitchParameter>] [<CommonParameters>]
    
    
DESCRIPTION
    
    

PARAMETERS
    -Force <SwitchParameter>
        Gets or sets a value indicating whether to force reset and avoid prompting.
        
    -ForceReset <SwitchParameter>
        Gets or sets a value indicating whether to force reset and avoid prompting.
        
        This is an alias of the Force parameter.
        
    <CommonParameters>
        This cmdlet supports the common parameters: Verbose, Debug,
        ErrorAction, ErrorVariable, WarningAction, WarningVariable,
        OutBuffer, PipelineVariable, and OutVariable. For more information, see 
        about_CommonParameters (https:/go.microsoft.com/fwlink/?LinkID=113216). 
    
    ----------  EXAMPLE 1  ----------
    
    PS C:\>Reset-RabbitMqNodeCommand
    
REMARKS
    To see the examples, type: "get-help Reset-RabbitMqNodeCommand -examples".
    For more information, type: "get-help Reset-RabbitMqNodeCommand -detailed".
    For technical information, type: "get-help Reset-RabbitMqNodeCommand -full".
    For online help, type: "get-help Reset-RabbitMqNodeCommand -online"
```

