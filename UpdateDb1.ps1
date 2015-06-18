$srv = new-object Microsoft.SqlServer.Management.Smo.Server("(local)")
$res = new-object Microsoft.SqlServer.Management.Smo.Restore
$backup = new-object Microsoft.SqlServer.Management.Smo.Backup

$backup.Devices.AddDevice("C:\AdventureWorks2012Backup.bak", [Microsoft.SqlServer.Management.Smo.DeviceType]::File)
$backup.Database = "AdventureWorks2012"
$backup.Action = [Microsoft.SqlServer.Management.Smo.BackupActionType]::Database
$backup.Initialize = $TRUE
$backup.SqlBackup($srv)
$srv.Databases["AdventureWorks2012"].Drop()

$res.Devices.AddDevice("C:\AdventureWorks2012Backup.bak", [Microsoft.SqlServer.Management.Smo.DeviceType]::File)
$res.Database = "AdventureWorks2012"
$res.NoRecovery = $TRUE
$res.SqlRestore($srv)