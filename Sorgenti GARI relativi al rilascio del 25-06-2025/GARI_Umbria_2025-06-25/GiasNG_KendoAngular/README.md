# Gias Kendo Grid

This library was generated with [Angular CLI](https://github.com/angular/angular-cli) version 14.2.0.

This project was created to extract the `gias-kendo-grid` library into an NPM package. `gias-kendo-grid` is a wrapper around the Kendo Grid, initially developed within the `GiasNG` project. The main goal is to abstract as many common and reusable grid behaviors as possible, making them available for use across different projects.

## Features

- Kendo grid wrapper tailored for Gias applications
- Easy integration into any Angular project

## Installation

Currently, the library must be installed manually. Follow these steps:

1. Copy the `.tgz` file to your target Angular project and add the dependency in `package.json`:

```
"gias-kendo-grid": "file:src/lib/gias-kendo-grid-1.0.0.tgz"
```

Make sure to update the version number as needed.

2. Before installing, remove existing `node_modules` and the lock file to ensure a clean install:

```
rm -r node_modules/
rm package-lock.json
```

3. Install the dependency:

```
npm install gias-kendo-grid
npm install
```

4. Import the required modules and components from the library into your Angular project modules.

```
import { GiasKendoGridModule } from 'gias-kendo-grid';

@NgModule({
  imports: [
    GiasKendoGridModule,
    // other imports
  ],
})
export class SomeModule {}
```

## Contributing

To contribute to the Gias Kendo Grid:

1. Make your changes or add new components following Angular best practices.
2. Update the `version` in `package.json` based on the type of change (see below).
3. Run:

```
npm run build-library
npm run pack-lib
```

4. Test the generated `.tgz` file by installing it in another Angular project.
5. Keep all development work on a separate branch until the changes have been validated. Once confirmed, merge them into the `dev` branch.

We follow [Semantic Versioning (SemVer)](https://semver.org/) using the `MAJOR.MINOR.PATCH` format:

- **PATCH** (`x.x.1`): Bug fixes or minor internal changes  
- **MINOR** (`x.1.0`): Backward-compatible new features or components  
- **MAJOR** (`1.0.0`): Breaking changes or major refactors

**Examples**:  
`"version": "1.2.0"` → `"1.2.1"` // PATCH  
`"version": "1.2.0"` → `"1.3.0"` // MINOR  
`"version": "1.2.0"` → `"2.0.0"` // MAJOR

## Changelog

### 29/05/2025 Version

1. Updated `gis-ui-kit.tgz`
2. Removed `listview` component
