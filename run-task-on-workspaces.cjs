// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/* eslint-disable header/header */

if (process.argv.length < 3) {
    console.log('You have to specify what workspace task to run on all')
    console.log('\nUsage: run-task-on-workspaces [task] [arguments]');
    console.log('\nExamples of tasks: build|test|ci');
    process.exit(1);
}

const path = require('path');
const fs = require('fs');
const spawnSync = require('child_process').spawnSync;
const editJsonFile = require('edit-json-file');
const rootPackageJson = require('./package.json')
const glob = require('glob').sync;

const workspaces = {};


const distFolder = `dist${path.sep}`
for (const workspaceDef of rootPackageJson.workspaces) {
    console.log(`Getting packages for workspace definition '${workspaceDef}' \n`);

    const packagePattern = workspaceDef.endsWith('package.json')
        ? workspaceDef
        : `${workspaceDef.replace(/\/$/, '')}/package.json`;
    const files = glob(packagePattern, { cwd: process.cwd() });

    const packages = files
        .filter(_ => !_.includes(distFolder) && !_.includes('node_modules'))
        .sort((a, b) => a.length - b.length);
    packages.forEach(_ => {
        let localPackage;
        try {
            localPackage = JSON.parse(fs.readFileSync(_).toString());
        } catch (error) {
            console.error(`Could not read workspace package '${_}':`, error.message);
            process.exit(1);
        }

        workspaces[localPackage.name] = path.dirname(_);
        console.log(`Including workspace '${localPackage.name}' at '${workspaces[localPackage.name]}'`);
    });
}

console.log('');

const task = process.argv[2];
const args = process.argv.slice(3, process.argv.length);

console.log(`Performing '${task}' on workspaces`);

if (args.length > 0) {
    console.log(`  Using args : ${args}`);
}

console.log('');

const workspaceNames = Object.keys(workspaces);

function updateDependencyVersionsFromLocalWorkspaces(file, packageJson, version) {
    const dependencyFields = Object.keys(packageJson).filter(_ => _.endsWith('dependencies') || _.endsWith('Dependencies'));
    for (let field of dependencyFields) {
        let actualField = field;
        const dependencies = packageJson[field] ?? {};
        const fileDependencies = file.get(field);

        for (let dependencyName of Object.keys(dependencies)) {
            if (workspaceNames.includes(dependencyName)) {
                console.log(`Updating workspace ${field} '${dependencyName}' to version ${version}`);
                const actualDependencyName = dependencyName.replace(/\./g, '\\.');
                actualField = field.replace(/\./g, '\\.');
                fileDependencies[actualDependencyName] = version;
            }
        }

        file.set(actualField, fileDependencies);
    }
}

for (const workspaceName in workspaces) {
    const workspaceRelativeLocation = workspaces[workspaceName];
    const workspaceAbsoluteLocation = path.join(process.cwd(), workspaceRelativeLocation);
    const packageJsonFile = path.join(workspaceAbsoluteLocation, 'package.json');

    if (fs.existsSync(packageJsonFile)) {
        const file = editJsonFile(packageJsonFile, { stringify_width: 4 });
        const packageJson = file.toObject();
        if (task === 'publish-version') {
            if (packageJson.private === true) {
                console.log(`Skipping private workspace '${workspaceName}' at '${workspaceRelativeLocation}'`);
                continue;
            }

            if (args.length === 1) {
                const version = args[0];
                file.set('version', version);
                updateDependencyVersionsFromLocalWorkspaces(file, packageJson, version);
                file.save();

                const targetReadMe = path.join(workspaceAbsoluteLocation, 'README.md');

                if (!fs.existsSync(targetReadMe)) {
                    fs.copyFileSync(path.join(process.cwd(), "README.md"), targetReadMe);
                }

                console.log(`Publishing workspace '${workspaceName}' at '${workspaceRelativeLocation}'`);
                const result = spawnSync('npm', ['publish'], { cwd: workspaceAbsoluteLocation, shell: false });
                console.log(result.stdout.toString());
                if (result.status !== 0) {
                    console.log(`Error publishing workspace '${workspaceName}'`);
                    process.exit(1);
                }
            }
        } else {

            if (!packageJson.scripts || !Object.hasOwn(packageJson.scripts, task)) {
                console.log(`Skipping workspace '${workspaceName}' - no script with name '${task}'`);
                continue;
            }

            console.log(`Workspace '${workspaceName}' at '${workspaceRelativeLocation}'`);

            const result = spawnSync('yarn', [task], { cwd: workspaceAbsoluteLocation, shell: false });
            if (result.error) {
                console.log(`Error running task '${task}' on workspace '${workspaceName}':`,
                    result.error.message);
                console.error(result.error.stack);
                process.exit(1);
            }
            console.log(result.stdout.toString());
            if (result.status !== 0) {
                console.log(`Error running task '${task}' on workspace '${workspaceName}'`);
                console.log(result.stderr.toString());
                process.exit(1);
            }
        }
    }
}
