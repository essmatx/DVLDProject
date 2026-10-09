$ErrorActionPreference = "Stop"
$oldDir = "C:\Users\Essmat Tarek\source\repos\DVLD"
$cleanDir = "C:\Users\Essmat Tarek\source\repos\DVLD_Clean"

# Step 1: Remove old contents from DVLD (skip .vs which is locked by VS)
$itemsToRemove = Get-ChildItem $oldDir -Exclude '.vs'
foreach ($item in $itemsToRemove) {
    Remove-Item $item.FullName -Recurse -Force
    Write-Host "Removed: $($item.Name)"
}

# Step 2: Copy new clean structure into DVLD (skip .vs from clean)
$itemsToCopy = Get-ChildItem $cleanDir -Exclude '.vs'
foreach ($item in $itemsToCopy) {
    $dest = Join-Path $oldDir $item.Name
    Copy-Item $item.FullName -Destination $dest -Recurse -Force
    Write-Host "Copied: $($item.Name)"
}

# Step 3: Remove the DVLD_Clean directory (no longer needed)
Remove-Item $cleanDir -Recurse -Force
Write-Host ""
Write-Host "=== Successfully applied clean 3-Tier structure to DVLD! ==="
