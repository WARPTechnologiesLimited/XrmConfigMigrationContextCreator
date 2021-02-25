# XrmConfigMigrationContextCreator
Utility assembly for populating a FakeXrmEasy context with the data.xml from a Common Data Service Configuration Migration export.

## Prerequisites
A Visual Studio project utilising the [FakeXrmEasy](https://dynamicsvalue.com/home) framework.

## Installation
Simply reference the WARP.XrmConfigMigrationContextCreator.dll assembly in your project.

## Usage
```cs
const string Folder = "data";
var fakedContext = new XrmFakedContext();
var manager = new ContextManager(fakedContext);
manager.FillContext(Folder);
```
`fakedContext` will be populated with the entities in the data extract and available for use in your unit tests.