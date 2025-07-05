param(
    [Parameter(Mandatory=$true)]
    [string]$MigrationName
)

$ProjectPath = "..\Liam\Infrastructure\Locamart.Liam.Adapter.Postgresql\Locamart.Liam.Adapter.Postgresql.csproj"
$StartupProjectPath = ".\..\Locamart.Host\Locamart.Host.csproj"
$DbContext = "LiamDbContext"

dotnet ef migrations add $MigrationName --project $ProjectPath --startup-project $StartupProjectPath --context $DbContext