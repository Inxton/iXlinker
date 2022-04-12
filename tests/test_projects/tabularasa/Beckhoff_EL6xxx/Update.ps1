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
foreach ($ts in $TsList)
{
    $workdir = $ts.path.Replace("\Ts.tsproj","");
    $iXlinkerExe = ($workdir | split-path -parent | split-path -parent | split-path -parent | split-path -parent | split-path -parent) +  "\src\iXlinker\bin\Release\net5.0\iXlinker.exe"
    Start-Process -FilePath $iXlinkerExe -ArgumentList ("-t " + $ts.path) -Wait
    $AllFolders = New-Object System.Collections.Generic.List[string] 
    $AllFolders = Get-ChildItem -Path $workdir -Recurse -Directory -Force -ErrorAction SilentlyContinue | Select-Object FullName
    $DutFolders = New-Object System.Collections.Generic.List[string] 
    $DutFolders.Add($workdir + '\DUTs')
    $DutFolders.Add($workdir + '\DUTs\IO')
    $DutFolders.Add($workdir + '\DUTs\IO\Boxes')
    $DutFolders.Add($workdir + '\DUTs\IO\PdoEntries')
    $DutFolders.Add($workdir + '\DUTs\IO\PDOs')
    foreach ($folder in $AllFolders)
    {
        $delete = 1
        foreach ($dutfolder in $DutFolders)
        {
            if($folder.FullName.Equals($dutfolder))
            {
                $delete = 0
                break
            }
        }
        if($delete)
        {
            if (Test-Path $folder.FullName)
            { 
                Remove-Item $folder.FullName -Recurse -Force
            }
        }
    }
    $AllFiles = New-Object System.Collections.Generic.List[string] 
    $AllFiles = Get-ChildItem -Path $workdir -Recurse -File -Force -ErrorAction SilentlyContinue | Select-Object FullName
    $TestFiles = New-Object System.Collections.Generic.List[string] 
    $TestFiles.Add($workdir + '\Plc.plcproj')
    $TestFiles.Add($workdir + '\PlcTask.TcTTO')
    $TestFiles.Add($workdir + '\Ts.tsproj')
    $DutFiles = Get-ChildItem -Path ($workdir + "\DUTs") -Recurse -File 
    foreach ($DutFile in $DutFiles)
    {
        if((($DutFile | Get-Content | Select-String -pattern ("{attribute 'GeneratedUsingTerminal: " + $ts.type + "'}")).Matches.Success))
        {
            $TestFiles.Add($DutFile.FullName)
        }
    }
    foreach ($file in $AllFiles)
    {
        $delete = 1
        foreach ($testFile in $TestFiles)
        {
            if($file.FullName.Equals($testFile))
            {
                $delete = 0
                break
            }
        }
        if($delete)
        {
            if (Test-Path $file.FullName)
            { 
                Remove-Item $file.FullName -Force
            }
        }
    }
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