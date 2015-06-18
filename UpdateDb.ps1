$srv = new-object Microsoft.SqlServer.Management.Smo.Server("(local)")
$res = new-object Microsoft.SqlServer.Management.Smo.Restore


$res.Devices.AddDevice("C:\AdventureWorks2012Backup.bak", [Microsoft.SqlServer.Management.Smo.DeviceType]::File)
$res.Database = "FPL_Dev"
$res.NoRecovery = $TRUE
$res.SqlRestore($srv)