using Microsoft.Xrm.Sdk;
using System;

using Xunit;

namespace WARP.XrmConfigMigrationFixtureCreator.Tests
{
    using WARP.XrmConfigMigrationContextCreator;

    /// <summary>
    /// Tests for the SchemaHelper.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class Schema_Tests
    {
        [Fact]
        public void CanReadFile()
        {
            const string XmlPath = @"data\data_schema.xml";
            var schemaHelper = new SchemaHelper();
            Assert.True(schemaHelper.LoadSchemaFile(XmlPath));
        }

        [Fact]
        public void CanQuerySchema()
        {
            var schemaHelper = new SchemaHelper(@"data\data_schema.xml");
            var typeName = schemaHelper.GetFieldSchemaElement("account", "primarycontactid");

            Assert.Equal("entityreference", typeName.Attribute("type")?.Value);
        }

        [Fact]
        public void GenerateCrmFieldValue()
        {
            const string EntityName = "account";

            var schemaHelper = new SchemaHelper(@"data\data_schema.xml");

            // Lookup
            var fieldSchema = schemaHelper.GetFieldSchemaElement(EntityName, "primarycontactid");
            var value = schemaHelper.GenerateAttributeValue(Guid.NewGuid().ToString(), fieldSchema);
            Assert.IsType<EntityReference>(value);
            Assert.Equal("contact", ((EntityReference)value).LogicalName);

            // Option Set
            fieldSchema = schemaHelper.GetFieldSchemaElement(EntityName, "accountcategorycode");
            value = schemaHelper.GenerateAttributeValue("3", fieldSchema);
            Assert.IsType<OptionSetValue>(value);

            // Money
            fieldSchema = schemaHelper.GetFieldSchemaElement(EntityName, "creditlimit");
            value = schemaHelper.GenerateAttributeValue("£3,000.00", fieldSchema);
            Assert.IsType<decimal>(value);

            // bool
            fieldSchema = schemaHelper.GetFieldSchemaElement(EntityName, "creditonhold");
            value = schemaHelper.GenerateAttributeValue("True", fieldSchema);
            Assert.IsType<bool>(value);
            Assert.True((bool)value);
            value = schemaHelper.GenerateAttributeValue("False", fieldSchema);
            Assert.IsType<bool>(value);
            Assert.False((bool)value);

            // DateTime
            fieldSchema = schemaHelper.GetFieldSchemaElement(EntityName, "createdon");
            value = schemaHelper.GenerateAttributeValue("18/06/2019 00:00:00", fieldSchema);
            Assert.IsType<DateTime>(value);
        }
    }
}
