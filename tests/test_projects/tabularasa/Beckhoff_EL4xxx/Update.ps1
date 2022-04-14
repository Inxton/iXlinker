$curDir = $MyInvocation.InvocationName.Replace($MyInvocation.MyCommand.Name,"")
$TsList = New-Object System.Collections.Generic.List[cls]
#Edit section =>
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL4004_0000_0020_01\Ts.tsproj', 'EL4004-0000-0020'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL4024_0000_0021_01\Ts.tsproj', 'EL4024-0000-0021'))
# <= Edit section
Import-Module $curDir\..\iXlinkerUpdateCommon.psm1
foreach ($ts in $TsList)
{
    Generate -path $ts.path  -type $ts.type
}
class cls {
   [string] $path
   [string] $type

   cls(
     [string] $_path,
     [string] $_type
    ) {
      $this.path = $_path
      $this.type = $_type
    }
}
