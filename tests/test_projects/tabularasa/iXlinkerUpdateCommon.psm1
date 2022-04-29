function Generate{
    param([string] $path,[string]$type)
        $workdir = $path.Replace("\Ts.tsproj","");
        $iXlinkerExe = ($workdir | split-path -parent | split-path -parent | split-path -parent | split-path -parent | split-path -parent) +  "\src\iXlinker\bin\Release\net5.0\iXlinker.exe"
        Start-Process -Wait -FilePath $iXlinkerExe -ArgumentList ("-t " + $path)
        $AllFolders = New-Object System.Collections.Generic.List[string] 
        $AllFolders = Get-ChildItem -Path $workdir -Recurse -Directory -Force -ErrorAction SilentlyContinue | Select-Object FullName
        $DutFolders = New-Object System.Collections.Generic.List[string] 
        $DutFolders.Add($workdir + '\DUTs')
        $DutFolders.Add($workdir + '\DUTs\IO')
        $DutFolders.Add($workdir + '\DUTs\IO\Boxes')
        $DutFolders.Add($workdir + '\DUTs\IO\Devices')
        $DutFolders.Add($workdir + '\DUTs\IO\PdoEntries')
        $DutFolders.Add($workdir + '\DUTs\IO\PDOs')
        $DutFolders.Add($workdir + '\DUTs\IO\Topology')
        $TmpFolders = Get-ChildItem -Path ($workdir) -Directory | Where-Object { $_.Name.StartsWith('_') } |Select-Object FullName
        foreach ($tmpFolder in $TmpFolders)
        {
            Remove-Item $tmpFolder.FullName -Force -Recurse
        }
        if(!($type.Equals('*')))
        {
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
                if((($DutFile | Get-Content | Select-String -pattern ("{attribute 'GeneratedUsingTerminal: " + $type + "'}")).Matches.Success))
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
        $EmptyFolders = Get-ChildItem -Path ($workdir + '\DUTs\IO') -Directory | Where-Object { $_.GetFileSystemInfos().Count -eq 0 } |Select-Object FullName
        foreach ($emptyFolder in $EmptyFolders)
        {
            Remove-Item $emptyFolder.FullName -Force
        }
        $exclude_ext = @(".tsproj",".plcproj", ".TcTTO", ".TcGVL", ".TcDUT")
        $files2delete = Get-ChildItem -File $workdir -Recurse | Where-Object { $exclude_ext -notcontains $_.Extension }
        foreach ($file2delete in $files2delete)
        {
            Remove-Item $file2delete.FullName -Force
        }
    }