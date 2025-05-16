<#
.SYNOPSIS
    Adds a new EF Core code-first migration with a timestamped name.

.PARAMETER Name
    (Optional) A custom migration name. If omitted, a name of the form "Migration_YYYYMMdd_HHmmss" is used.

.PARAMETER Project
    (Optional) Path to the target project (.csproj) containing your DbContext. Defaults to current directory’s first .csproj.

.PARAMETER StartupProject
    (Optional) Path to the startup project if different (e.g. your WebUI). Defaults to the same as -Project.

.PARAMETER Context
    (Optional) The DbContext class name, if you need to target a specific context.

.EXAMPLE
    .\Add-EfMigration.ps1

    # Auto-names migration and uses the only .csproj in cwd.

.EXAMPLE
    .\Add-EfMigration.ps1 -Name "AddOrdersTable" -Project "./Data/MyApp.Data.csproj" -StartupProject "./WebUI/MyApp.Web.csproj" -Context "AppDbContext"
#>

[CmdletBinding()]
param(
    [string]$Name,
    [string]$Project,
    [string]$StartupProject,
    [string]$Context
)

function Get-FirstCsproj {
    Get-ChildItem -Path . -Filter *.csproj -Recurse -ErrorAction SilentlyContinue |
    Select-Object -First 1 -ExpandProperty FullName
}

# Determine project
if (-not $Project) {
    $Project = Get-FirstCsproj
    if (-not $Project) {
        Write-Error "No .csproj found. Please specify -Project path."
        exit 1
    }
}

# Default startup to project if not provided
if (-not $StartupProject) {
    $StartupProject = $Project
}

# Generate migration name if not provided
if (-not $Name) {
    $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
    $Name = "Migration_$timestamp"
}

# Build the EF CLI command
$efArgs = @("migrations", "add", $Name, "--project", "`"$Project`"", "--startup-project", "`"$StartupProject`"")

if ($Context) {
    $efArgs += @("--context", $Context)
}

# Execute
Write-Host "Adding EF Core migration '$Name'..."
$exitCode = dotnet ef @efArgs

if ($LASTEXITCODE -ne 0) {
    Write-Error "dotnet ef exited with code $LASTEXITCODE"
    exit $LASTEXITCODE
}

Write-Host "Migration '$Name' added successfully."