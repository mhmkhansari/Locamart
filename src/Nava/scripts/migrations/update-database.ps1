param(
    [Parameter(Mandatory=$true)]
    [string]$MigrationName
)

$ProjectPath = "..\Nava\Infrastructure\Locamart.Nava.Adapter.Postgresql\Locamart.Nava.Adapter.Postgresql.csproj"
$StartupProjectPath = ".\..\Locamart.Host\Locamart.Host.csproj"
$DbContext = "LocamartNavaDbContext"

dotnet ef database update $MigrationName --project $ProjectPath --startup-project $StartupProjectPath --context $DbContext