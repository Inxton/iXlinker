$curDir = $MyInvocation.InvocationName.Replace($MyInvocation.MyCommand.Name,"")
$TsList = New-Object System.Collections.Generic.List[cls]
#Edit section =>
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL1809_0000_0018_01\Ts.tsproj', 'EL1809-0000-0018'))
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
