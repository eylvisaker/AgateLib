pipeline {
    agent {
        label 'windows'
    }

    options {
        skipDefaultCheckout(true)
    }

    environment {
        NUGET_APIKEY = credentials('nuget-api-key')
    }

    stages {
        stage('Checkout') {
            steps {
                deleteDir()
                checkout scm
            }
        }
        stage('Compile and Test') {
            steps {
                powershell './build.ps1 Test -build-number $env:BUILD_NUMBER -branch-name $env:BRANCH_NAME'
            }
        }
        stage('Build') {
            steps {
                powershell './build.ps1 Build -build-number $env:BUILD_NUMBER -branch-name $env:BRANCH_NAME --skip'
            }
        }
        stage('Package') {
            steps {
                powershell './build.ps1 Pack -build-number $env:BUILD_NUMBER -branch-name $env:BRANCH_NAME --skip'
            }
        }
        stage('Archive Artifacts') {
            steps {
                archiveArtifacts artifacts: 'artifacts/**'
            }
        }
    }

    post {
        always {
            nunit testResultsPattern: 'artifacts/**/*.xml'
        }
        failure {
            script {
                mail (
                  to: 'eylvisaker@gmail.com',
                  subject: "b.vt: FAILED BUILD - AgateLib ${env.BRANCH_NAME} ${env.BUILD_NUMBER}",
                  body: "AgateLib branch ${env.BRANCH_NAME} failed to build."
                )
            }
        }
        fixed {
            script {
                mail (
                  to: 'eylvisaker@gmail.com',
                  subject: "b.vt: FIXED BUILD - AgateLib ${env.BRANCH_NAME} ${env.BUILD_NUMBER}",
                  body: "AgateLib branch ${env.BRANCH_NAME} is fixed now."
                )
            }
        }
        success {
            script {
                cleanWs()
            }
        }
    }
}
