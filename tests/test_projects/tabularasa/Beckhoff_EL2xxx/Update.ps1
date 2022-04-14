$curDir = $MyInvocation.InvocationName.Replace($MyInvocation.MyCommand.Name,"")
$TsList = New-Object System.Collections.Generic.List[cls]
#Edit section =>
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL2014_0000_0017_01\Ts.tsproj', 'EL2014-0000-0017'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL2014_0000_0017_02\Ts.tsproj', 'EL2014-0000-0017'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL2014_0000_0017_03\Ts.tsproj', 'EL2014-0000-0017'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL2032_0000_0018_01\Ts.tsproj', 'EL2032-0000-0018'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL2034_0000_0018_01\Ts.tsproj', 'EL2034-0000-0018'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL2044_0000_0016_01\Ts.tsproj', 'EL2044-0000-0016'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL2044_0000_0016_02\Ts.tsproj', 'EL2044-0000-0016'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL2044_0000_0016_03\Ts.tsproj', 'EL2044-0000-0016'))
$TsList.Add([cls]::new($curDir + 'Beckhoff_EL2809_0000_0018_01\Ts.tsproj', 'EL2809-0000-0018'))
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