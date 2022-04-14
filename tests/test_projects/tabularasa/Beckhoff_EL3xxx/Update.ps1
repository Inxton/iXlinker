$curDir = $MyInvocation.InvocationName.Replace($MyInvocation.MyCommand.Name,"")
$TsList = New-Object System.Collections.Generic.List[cls]
#Edit section =>
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL3024_0000_0019_01\Ts.tsproj', 'EL3024-0000-0019'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL3024_0000_0019_02\Ts.tsproj', 'EL3024-0000-0019'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL3024_0000_0019_03\Ts.tsproj', 'EL3024-0000-0019'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL3064_0000_0020_01\Ts.tsproj', 'EL3064-0000-0020'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL3064_0000_0020_02\Ts.tsproj', 'EL3064-0000-0020'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL3064_0000_0020_03\Ts.tsproj', 'EL3064-0000-0020'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL3164_0000_0020_01\Ts.tsproj', 'EL3164-0000-0020'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL3164_0000_0020_02\Ts.tsproj', 'EL3164-0000-0020'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL3164_0000_0020_03\Ts.tsproj', 'EL3164-0000-0020'))
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