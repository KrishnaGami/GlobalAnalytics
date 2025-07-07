pipeline {
    agent any

    environment {
        DOTNET_ROOT = "C:\\Program Files\\dotnet"
        PATH = "${DOTNET_ROOT};${env.PATH}"
    }

    stages {
        stage('Checkout') {
            steps {
                git url: 'https://github.com/KrishnaGami/GlobalAnalytics.git', branch: 'main'
            }
        }

        stage('Restore') {
            steps {
                bat 'dotnet restore GlobalAnalytics.API/GlobalAnalytics.API.csproj'
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build GlobalAnalytics.API/GlobalAnalytics.API.csproj --configuration Release --no-restore'
            }
        }

        stage('Test') {
            steps {
                bat 'dotnet test GlobalAnalytics.Test/GlobalAnalytics.Test.csproj --no-build --verbosity normal'
            }
        }

        stage('Publish') {
            steps {
                bat 'dotnet publish GlobalAnalytics.API/GlobalAnalytics.API.csproj --configuration Release --output ./publish'
            }
        }

        stage('Docker Build') {
            steps {
                bat 'docker build --progress=plain -t globalanalytics-api -f GlobalAnalytics.Deployment/Dockerfile .'
            }
        }

        stage('Docker Run') {
			steps {
				// Optional cleanup
				bat 'docker rm -f globalanalytics-api || exit 0'

				// Run on free port
				bat 'docker run -d -p 8081:80 --name globalanalytics-api globalanalytics-api'
			}
		}

    }

    post {
        always {
            echo 'Pipeline execution completed.'
        }
        failure {
            echo 'Pipeline failed!'
        }
    }
}
