[<img src="https://raw.githubusercontent.com/garethbirduk/GradientSoftware.SCARS-Core/main/resources/icon.png" width="25" height="25">](https://github.com/garethbirduk/GradientSoftware.SCARS-Core)
[![main](https://github.com/garethbirduk/GradientSoftware.SCARS-Core/actions/workflows/main.yml/badge.svg)](https://github.com/garethbirduk/GradientSoftware.SCARS-Core/actions)
[![coverage](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/garethbirduk/GIST_ID/raw/code-coverage.json)](https://garethbirduk.github.io/GradientSoftware.SCARS-Core)

# Template for dotnet projects

- [ ] GH_APIKEY - Generate this in developer settings including package creation / deletion<br>
- [ ] GIST_AUTH_TOKEN - Generate this in developer settings including gist creation
- [ ] GISTID - Create a gist and get the GISTID in this step
- [ ] SCARS-Core - Repo name (without GradientSoftware.) e.g. Utils

## Generate coverage gist
https://gist.github.com/
- [ ] Set Gist description - eg code-coverage-utils.json
- [ ] Set Gist filename - eg code-coverage-utils.json
- [ ] Set Gist content - eg code-coverage-utils.json
- [ ] Note the GISTID

# Folder level Search and Replace in files
- [ ] SCARS-Core
- [ ] GISTID - note GISTID is the mask; GIST_ID is the reference to the environment variable which must remain as GIST_ID not the value!

# Rename files and folders
- [] /SCARS-Core
- [] /SCARS-Core/SCARS-Core.csproj
- [] /SCARS-Core.Test
- [] /SCARS-Core.Test/SCARS-Core.Test.csproj
- [] /SCARS-Core.sln
      
# Set environment variables
https://github.com/garethbirduk/GradientSoftware.SCARS-Core/settings/secrets/actions
- [ ] GH_APIKEY
- [ ] GIST_AUTH_TOKEN
- [ ] GIST_ID

# Set rules
https://github.com/garethbirduk/GradientSoftware.SCARS-Core/settings/rules
- [ ] Main protection

# Github pages
https://github.com/garethbirduk/GradientSoftware.SCARS-Core/settings/pages
- [ ] Build and deployment source: Github Actions
- [ ] Configure static page then Cancel. Not sure this is required.

