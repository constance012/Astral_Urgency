@echo off
REM --- Configuration ---
set "TARGET_FOLDER=P:\Unity Projects\Astral_Urgency\Astral Urgency\Assets"
set "LINK_NAME=0_CstSharedResources"
set "LINK_TARGET=P:\Unity Projects\Astral_Urgency\CST_Unity_Shared_Resources\0_CstSharedResources"

REM --- Change to the target folder ---
cd /d "%TARGET_FOLDER%"
if errorlevel 1 (
    echo Failed to cd into %TARGET_FOLDER%
    exit /b 1
)

REM --- Remove old symbolic link if it exists ---
if exist "%LINK_NAME%" (
    echo Removing old symbolic link: %LINK_NAME%
    del "%LINK_NAME%"
)

REM --- Create new symbolic directory link ---
echo Creating new symbolic link: %LINK_NAME% -> %LINK_TARGET%
mklink /d "%LINK_NAME%" "%LINK_TARGET%"