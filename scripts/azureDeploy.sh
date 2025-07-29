#!/bin/bash

# Exit immediately if a command exits with a non-zero status
set -e

# Build the project
echo "Building the project..."
npm run build

# Note the current branch
CURRENT_BRANCH=$(git branch --show-current)
echo "Current branch: $CURRENT_BRANCH"

# Checkout the "prod" branch called AzureLive
echo "Switching to AzureLive branch..."
git checkout AzureLive

# Merge the current branch into AzureLive
echo "Merging $CURRENT_BRANCH into AzureLive..."
git merge $CURRENT_BRANCH --no-edit

# Commit the changes
echo "Committing the changes..."
git add -A
git commit -m "Deploying changes from $CURRENT_BRANCH to AzureLive"

# Push the changes
echo "Pushing to AzureLive..."
git push origin AzureLive

# Switch back to the original branch
echo "Switching back to $CURRENT_BRANCH..."
git checkout $CURRENT_BRANCH

echo "Deployment to AzureLive completed successfully."
