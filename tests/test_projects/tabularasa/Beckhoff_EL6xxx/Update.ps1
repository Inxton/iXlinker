$curDir = $MyInvocation.InvocationName.Replace($MyInvocation.MyCommand.Name,"")
$TsList = New-Object System.Collections.Generic.List[cls]
#Edit section =>
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL6001_0000_0020_01\Ts.tsproj', 'EL6001-0000-0020'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL6001_0000_0020_02\Ts.tsproj', 'EL6001-0000-0020'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL6001_0000_0020_03\Ts.tsproj', 'EL6001-0000-0020'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL6001_0000_0020_04\Ts.tsproj', 'EL6001-0000-0020'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL6001_0000_0020_05\Ts.tsproj', 'EL6001-0000-0020'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL6002_0000_0019_01\Ts.tsproj', 'EL6002-0000-0019'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL6002_0000_0019_02\Ts.tsproj', 'EL6002-0000-0019'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL6002_0000_0019_03\Ts.tsproj', 'EL6002-0000-0019'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL6224_0000_0021_01\Ts.tsproj', 'EL6224-0000-0021'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL6224_0000_0021_02\Ts.tsproj', 'EL6224-0000-0021'))
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