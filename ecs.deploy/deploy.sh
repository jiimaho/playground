##########################################################################################
# This script is only intended for local use. It is not used by the CI/CD pipeline.
###########################################################################################

export AWS_PROFILE=wdev
export AWS_DEFAULT_REGION=eu-west-1
export WB_CLUSTER_NAME=cluster-dev
export WB_SERVICE_NAME=volvowallbox-provision-api
export SSM_PARAMETER_TASK_DEFINITION_NAME=/jim/volvowallbox/provisionapi/apptask/definition

npm install
npm run deploy