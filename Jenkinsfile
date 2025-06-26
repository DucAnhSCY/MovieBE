
pipeline {
    agent any
    stages {        stage('clone'){
            steps {
                echo 'Cloning source code'
                git branch:'master', url: 'https://github.com/DucAnhSCY/MovieBookingTicket.git'
                // Clean any existing build artifacts and cache files thoroughly
                bat 'if exist bin rmdir /s /q bin'
                bat 'if exist obj rmdir /s /q obj'
                bat 'if exist publish rmdir /s /q publish'
                // Remove any potential cache files that might cause path conflicts
                bat 'if exist obj\\Debug\\net8.0\\rpswa.dswa.cache.json del /f /q obj\\Debug\\net8.0\\rpswa.dswa.cache.json'
                bat 'if exist obj\\Release\\net8.0\\rpswa.dswa.cache.json del /f /q obj\\Release\\net8.0\\rpswa.dswa.cache.json'
            }
        } // end clone
        
        stage('restore package') {
            steps {
                echo 'Restore package'
                // Clean first to avoid any cache issues
                bat 'dotnet clean gitnetcore.csproj || echo "Clean completed"'
                bat 'dotnet restore gitnetcore.csproj'
            }
        }        
        stage ('build') {
            steps {
                echo 'build project netcore'
                bat 'dotnet build gitnetcore.csproj --configuration Release --no-restore'
            }
        }
        
        stage ('tests') {
            steps{
                echo 'running test...'
                bat 'dotnet test gitnetcore.csproj --configuration Release --verbosity normal'
            }
        }
          stage ('publish to temp folder') {
            steps{
                echo 'Publishing...'
                // Clean the publish directory first and remove any conflicting cache files
                bat 'if exist publish rmdir /s /q publish'
                bat 'if exist obj\\Debug\\net8.0\\rpswa.dswa.cache.json del /f /q obj\\Debug\\net8.0\\rpswa.dswa.cache.json'
                bat 'if exist obj\\Release\\net8.0\\rpswa.dswa.cache.json del /f /q obj\\Release\\net8.0\\rpswa.dswa.cache.json'
                bat 'dotnet clean gitnetcore.csproj --configuration Release'
                bat 'dotnet publish gitnetcore.csproj -c Release -o publish --no-restore --force'
            }
        }
          stage ('Copy to IIS folder') {
            steps {
                echo 'Copy to running folder'
                // Stop IIS first to avoid file locks
                bat 'iisreset /stop'
                bat 'if not exist "c:\\wwwroot\\myproject" mkdir "c:\\wwwroot\\myproject"'
                bat 'xcopy "%WORKSPACE%\\publish\\*" "c:\\wwwroot\\myproject\\" /E /Y /I /R'
                bat 'iisreset /start'
            }
        }
        
        stage('Deploy to IIS') {
            steps {
                powershell '''
                    # Tạo website nếu chưa có
                    Import-Module WebAdministration
                    if (-not (Get-Website -Name "MySite" -ErrorAction SilentlyContinue)) {
                        New-Website -Name "MySite" -Port 81 -PhysicalPath "c:\\wwwroot\\myproject"
                    }
                '''
            }
        } // end deploy iis
    } // end stages
} // end pipeline
