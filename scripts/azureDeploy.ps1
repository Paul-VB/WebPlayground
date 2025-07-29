# Stop on errors
$ErrorActionPreference = "Stop"

Write-Host "Building the project..."
npm run build

$currentBranch = git branch --show-current
Write-Host "Current branch: $currentBranch"

# Get last commit message from current branch
$lastCommitMsg = git log -1 --pretty=%B
Write-Host "Last commit message from $currentBranch`: $lastCommitMsg"

Write-Host "Switching to AzureLive branch..."
git checkout AzureLive

Write-Host "Rebasing AzureLive onto $currentBranch..."
git rebase $currentBranch

Write-Host "Adding and committing any changes (if needed)..."
git add -A
try {
    git commit -m "$lastCommitMsg"
} catch {
    Write-Host "No changes to commit."
}

Write-Host "Pushing AzureLive branch (force-with-lease)..."
git push --force-with-lease origin AzureLive

Write-Host "Switching back to $currentBranch..."
git checkout $currentBranch

Write-Host "Deployment to AzureLive completed successfully."
