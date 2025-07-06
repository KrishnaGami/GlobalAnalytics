pipeline {
    agent any

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = "1"
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/KrishnaGami/GlobalAnalytics.git'
            }
        }

        stage('Restore') {
            steps {
                sh 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build --configuration Release'
            }
        }

        stage('Test') {
            steps {
                sh 'dotnet test --no-build --verbosity normal'
            }
        }

        stage('Publish') {
            steps {
                sh 'dotnet publish GlobalAnalytics.API/GlobalAnalytics.API.csproj -c Release -o ./publish'
            }
        }

        stage('Docker Build') {
            steps {
                sh 'docker build -t globalanalytics-api -f GlobalAnalytics.API/Dockerfile .'
            }
        }

        stage('Docker Run') {
            steps {
                sh 'docker run -d -p 8080:80 globalanalytics-api'
            }
        }
    }
}
