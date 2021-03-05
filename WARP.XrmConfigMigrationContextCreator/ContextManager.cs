// <copyright file="ContextManager.cs" company="WARP Technologies Limited">
// Copyright © WARP Technologies Limited
// </copyright>

namespace WARP.XrmConfigMigrationContextCreator
{
    using System.IO;
    using System.Linq;

    using FakeXrmEasy;

    using Microsoft.Xrm.Sdk;

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
        /// <param name="fillRelationships">Indicates whether to fill the M2M relationships.</param>
        /// <returns>Populated context.</returns>
        public XrmFakedContext FillContext(string unpackedDataFolderPath, bool fillRelationships = true)
        {
            var dataFilePath = Path.Combine(unpackedDataFolderPath, Constants.DefaultDataFileName);
            var schemaFilePath = Path.Combine(unpackedDataFolderPath, Constants.DefaultSchemaFileName);

            return this.FillContext(dataFilePath, schemaFilePath, fillRelationships);
        }

        /// <summary>
        /// Fills the context with the contents of unpacked CRM Configuration Migration Data Utility output.
        /// </summary>
        /// <param name="dataFilePath">Path to the data.xml file.</param>
        /// <param name="schemaFilePath">Path to the data_schema.xml file.</param>
        /// <param name="fillRelationships">Indicates whether to fill the M2M relationships.</param>
        /// <returns>Populated context.</returns>
        public XrmFakedContext FillContext(string dataFilePath, string schemaFilePath, bool fillRelationships = true)
        {
            var dataFileReader = new DataFileReader(dataFilePath, schemaFilePath);
            var entities = dataFileReader.GetEntities();
            this.FakedContext.Initialize(entities);
            if (fillRelationships)
            {
                this.FillManyToManyRelationships(dataFileReader);
            }

            return this.FakedContext;
        }

        private void FillManyToManyRelationships(DataFileReader dataFileReader)
        {
            var service = this.FakedContext.GetOrganizationService();
            try
            {
                dataFileReader.GetRelationships().ForEach(
                    rs =>
                        {
                            // add the relationship to the context metadata.
                            this.FakedContext.AddRelationship(rs.RelationshipName, rs.XrmFakedRelationship);

                            // Generate List of target EntityReferences.
                            var targetReferences = rs.Relationships.Select(id => new EntityReference(rs.Entity2LogicalName, id)).ToList();

                            // Associate with the source entity.
                            service.Associate(rs.Entity1LogicalName, rs.Entity1Id, new Relationship(rs.RelationshipName), new EntityReferenceCollection(targetReferences));
                        });
            }
            catch
            {
                // most likely here because the target entity isn't in the export.
            }
        }
    }
}
