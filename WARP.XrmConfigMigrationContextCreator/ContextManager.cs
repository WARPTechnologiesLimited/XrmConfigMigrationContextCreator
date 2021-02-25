// <copyright file="ContextManager.cs" company="WARP Technologies Limited">
// Copyright © WARP Technologies Limited
// </copyright>

namespace WARP.XrmConfigMigrationContextCreator
{
    using System;
    using System.IO;

    using FakeXrmEasy;

    using WARP.XrmConfigMigrationContextCreator.Models;

    /// <summary>
    /// Class for handling the population of the Fake context.
    /// </summary>
    public class ContextManager
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ContextManager"/> class.
        /// </summary>
        /// <param name="fakedContext">The FakeXrmEasy context to fill.</param>
        public ContextManager(XrmFakedContext fakedContext)
        {
            this.FakedContext = fakedContext;
        }

        /// <summary>
        /// Gets the FakeXrmEasy context.
        /// </summary>
        public XrmFakedContext FakedContext { get; private set; }

        /// <summary>
        /// Fills the context with the contents of unpacked CRM Configuration Migration Data Utility output.
        /// </summary>
        /// <param name="unpackedDataFolderPath">Path to the unpacked files folder.</param>
        /// <returns>Populated context.</returns>
        public XrmFakedContext FillContext(string unpackedDataFolderPath)
        {
            var dataFilePath = Path.Combine(unpackedDataFolderPath, Constants.DefaultDataFileName);
            var schemaFilePath = Path.Combine(unpackedDataFolderPath, Constants.DefaultSchemaFileName);

            return this.FillContext(dataFilePath, schemaFilePath);
        }

        /// <summary>
        /// Fills the context with the contents of unpacked CRM Configuration Migration Data Utility output.
        /// </summary>
        /// <param name="dataFilePath">Path to the data.xml file.</param>
        /// <param name="schemaFilePath">Path to the data_schema.xml file.</param>
        /// <returns>Populated context.</returns>
        public XrmFakedContext FillContext(string dataFilePath, string schemaFilePath)
        {
            var dataFileReader = new DataFileReader(dataFilePath, schemaFilePath);
            var entities = dataFileReader.GetEntities();
            this.FakedContext.Initialize(entities);

            // TODO: Handle the M-M relationships.
            // this.FillManyToManyRelationships();
            return this.FakedContext;
        }

        private void FillManyToManyRelationships()
        {
            // TODO: Fill the M-M relationships from the data.
            // https://dynamicsvalue.com/get-started/nn-relationships
            // Problem: How to determine the relationship name from information provided. Doesn't appear to be in the data.xml file.
            // May need to spoof a relationship name per entity-entity combination. Maybe the intersect entity name is good enough.
            throw new NotImplementedException("This functionality is yet to be written.");
        }
    }
}
