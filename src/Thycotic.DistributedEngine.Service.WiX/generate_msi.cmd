@rem the first parameter is the version number like "1.0.0.1"
@rem the second parameter is the path e.g. ..\ServiceHost\bin\Release
copy icon.ico %2
"M:\development\repos\distributedengine\src\Thycotic.InstallerGenerator.Core\lib\WiX\heat.exe" dir %2 -o Output\Autogenerated.wxs -ag -sfrags -suid -cg main_component_group -t add_service_install.xsl -sreg -scom -srd -template fragment -dr INSTALLLOCATION
"M:\development\repos\distributedengine\src\Thycotic.InstallerGenerator.Core\lib\WiX\candle.exe" -nologo -ext WixUtilExtension -dInstallerVersion=%1 Output\AutoGenerated.wxs Product.wxs -out Output\ -arch x64
"M:\development\repos\distributedengine\src\Thycotic.InstallerGenerator.Core\lib\WiX\light.exe" -b %2 -ext WixUIExtension -ext WixUtilExtension -nologo Output\AutoGenerated.wixobj Output\Product.wixobj -out Output\Thycotic.DistributedEngine.Service.%1.msi