# F-03: line length limit is 100 characters (Legacy, bin, obj are skipped)
$files = Get-ChildItem -Recurse -Filter *.cs |
    Where-Object { $_.FullName -notmatch '[\\/](bin|obj|Legacy)[\\/]' }

$bad = foreach ($f in $files) {
    $lines = [System.IO.File]::ReadAllLines($f.FullName, [System.Text.Encoding]::UTF8)
    for ($i = 0; $i -lt $lines.Length; $i++) {
        if ($lines[$i].Length -gt 100) {
            "$($f.FullName):$($i + 1) (length $($lines[$i].Length))"
        }
    }
}

if ($bad) {
    Write-Host 'Found lines longer than 100 characters:'
    $bad | ForEach-Object { Write-Host $_ }
    exit 1
}

Write-Host 'OK: all lines are 100 characters or shorter'
exit 0
