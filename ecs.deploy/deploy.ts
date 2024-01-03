import 'zx/globals';
import { $ } from 'zx';
import { SSMClient, GetParametersCommand } from '@aws-sdk/client-ssm';
import {
  ECSClient,
  DescribeTaskDefinitionCommand,
  RegisterTaskDefinitionCommand,
  RegisterTaskDefinitionCommandInput,
  DescribeServicesCommand,
  UpdateServiceCommand,
  TaskDefinition,
} from '@aws-sdk/client-ecs';
import * as dotenv from 'dotenv';
dotenv.config();
if (!process.env.AWS_PROFILE) throw new Error('AWS_PROFILE not set.');
if (!process.env.AWS_DEFAULT_REGION)
  throw new Error('AWS_DEFAULT_REGION not set.');
if (!process.env.WB_CLUSTER_NAME) throw new Error('WB_CLUSTER_NAME not set.');
if (!process.env.WB_SERVICE_NAME) throw new Error('WB_SERVICE_NAME not set.');
if (!process.env.SSM_PARAMETER_TASK_DEFINITION_NAME)
  throw new Error('SSM_PARAMETER_TASK_DEFINITION_NAME not set.');

const profile = process.env.AWS_PROFILE;
const region = process.env.AWS_DEFAULT_REGION;
const clusterName = process.env.WB_CLUSTER_NAME;
const serviceName = process.env.WB_SERVICE_NAME;
const ssmParameterTaskDefinitionName =
  process.env.SSM_PARAMETER_TASK_DEFINITION_NAME;

console.log(`Deploying using profile ${profile}`);
console.log(`- clusterName ${clusterName}`);
console.log(`- region ${region}`);
console.log(`- serviceName ${serviceName}`);
console.log(
  `- ssmParameterTaskDefinitionName ${ssmParameterTaskDefinitionName}`
);

const ssmClient = new SSMClient({
  region: region,
});
const ecsClient = new ECSClient({
  region: region,
});
// Attributes that are returned by DescribeTaskDefinition, but are not valid RegisterTaskDefinition inputs
const IGNORED_TASK_DEFINITION_ATTRIBUTES = [
  'compatibilities',
  'taskDefinitionArn',
  'requiresAttributes',
  'revision',
  'status',
  'registeredAt',
  'deregisteredAt',
  'registeredBy',
];
function isEmptyValue(value: any) {
  if (value === null || value === undefined || value === '') {
    return true;
  }
  if (Array.isArray(value)) {
    for (var element of value) {
      if (!isEmptyValue(element)) {
        // the array has at least one non-empty element
        return false;
      }
    }
    // the array has no non-empty elements
    return true;
  }
  if (typeof value === 'object') {
    for (var childValue of Object.values(value)) {
      if (!isEmptyValue(childValue)) {
        // the object has at least one non-empty property
        return false;
      }
    }
    // the object has no non-empty property
    return true;
  }
  return false;
}
function emptyValueReplacer(_: any, value: any) {
  if (isEmptyValue(value)) {
    return undefined;
  }
  if (Array.isArray(value)) {
    return value.filter((e) => !isEmptyValue(e));
  }
  return value;
}
function cleanNullKeys(obj: TaskDefinition): Partial<TaskDefinition> {
  return JSON.parse(JSON.stringify(obj, emptyValueReplacer));
}
function removeIgnoredAttributes(
  taskDef: Partial<TaskDefinition>
): Partial<TaskDefinition> {
  for (var attribute of IGNORED_TASK_DEFINITION_ATTRIBUTES) {
    if (taskDef[attribute as keyof TaskDefinition]) {
      delete taskDef[attribute as keyof TaskDefinition];
    }
  }
  return taskDef;
}
function validateProxyConfigurations(taskDef: any) {
  return (
    'proxyConfiguration' in taskDef &&
    taskDef.proxyConfiguration.type &&
    taskDef.proxyConfiguration.type == 'APPMESH' &&
    taskDef.proxyConfiguration.properties &&
    taskDef.proxyConfiguration.properties.length > 0
  );
}
function maintainValidObjects(
  taskDef: any
): RegisterTaskDefinitionCommandInput {
  if (validateProxyConfigurations(taskDef)) {
    taskDef.proxyConfiguration.properties.forEach(
      (property: any, index: number, arr: any[]) => {
        if (!('value' in property)) {
          arr[index].value = '';
        }
        if (!('name' in property)) {
          arr[index].name = '';
        }
      }
    );
  }
  if (taskDef && taskDef.containerDefinitions) {
    taskDef.containerDefinitions.forEach((container: any) => {
      if (container.environment) {
        container.environment.forEach(
          (property: any, index: number, arr: any[]) => {
            if (!('value' in property)) {
              arr[index].value = '';
            }
          }
        );
      }
    });
  }
  return taskDef;
}
const ssmResponse = await ssmClient.send(
  new GetParametersCommand({
    Names: [ssmParameterTaskDefinitionName],
    WithDecryption: false,
  })
);
console.log('BEGIN SSM Response');
console.log(ssmResponse);
console.log('END SSM Response');
const taskdefintionIdAndVersion = ssmResponse.Parameters?.find(
  (p) => p.Name === ssmParameterTaskDefinitionName
)?.Value;
const [taskDefinitionId, taskDefinitionVersion] =
  taskdefintionIdAndVersion?.split('/') || [];
if (!taskDefinitionId || !taskDefinitionVersion)
  throw new Error('Task definition not found in SSM.');
console.log(`taskdefintionIdAndVersion: ${taskdefintionIdAndVersion}`);
// First get the current task definition from AWS
const taskDefinitionResponse = await ecsClient.send(
  new DescribeTaskDefinitionCommand({
    taskDefinition: taskdefintionIdAndVersion,
  })
);
console.log(`taskDefinitionResponse: ${taskDefinitionResponse}`);
// Then get the current container definitions
const taskDefinition = taskDefinitionResponse.taskDefinition;
if (!taskDefinition) throw new Error('Task definition not found.');
const taskDefContents = maintainValidObjects(
  removeIgnoredAttributes(cleanNullKeys(taskDefinition))
);
const shortCommitSha = (await $`git rev-parse --short HEAD`).toString().trim();
taskDefContents.containerDefinitions?.forEach((container) => {
  console.log(container.image);
  if (container.image && container.image.includes('intranet-')) {
    const [repository] = container.image.split(':');
    container.image = `${repository}:${shortCommitSha}`;
    console.log(`Updated image to [${container.image}] for ${container.name}`);
  }
});
console.log('BEGIN UPDATED TASK DEFINITION');
console.log(JSON.stringify(taskDefContents, null, 2));
console.log('END UPDATED TASK DEFINITION');
// Register a new task definition with the same contents as the current one
// const registerResponse = await ecsClient.send(
//   new RegisterTaskDefinitionCommand(taskDefContents)
// );
// const taskDefinitionArn = registerResponse.taskDefinition?.taskDefinitionArn;
// const revision = registerResponse.taskDefinition?.revision;
// // Describe the currently running service
// const serviceResponse = await ecsClient.send(
//   new DescribeServicesCommand({
//     cluster: clusterName,
//     services: [serviceName],
//   })
// );
// // Update the service to use the new task definition
// const updateResponse = await ecsClient.send(
//   new UpdateServiceCommand({
//     cluster: clusterName,
//     service: serviceName,
//     taskDefinition: taskDefinitionArn,
//     forceNewDeployment: true,
//   })
// );
// const version = `${shortCommitSha}:${revision}`;
// console.log(`${serviceName} updated with version ${version}`);
