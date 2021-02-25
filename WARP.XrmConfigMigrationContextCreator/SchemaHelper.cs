// <copyright file="SchemaHelper.cs" company="WARP Technologies Limited">
// Copyright © WARP Technologies Limited
// </copyright>

namespace WARP.XrmConfigMigrationContextCreator
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;

    using Microsoft.Xrm.Sdk;

    using WARP.XrmConfigMigrationContextCreator.Models;

    /// <summary>
    /// Class for generating xUnit fixtures from CRM Config Migration Utility exports.
    /// </summary>
    public class SchemaHelper
    {
        private XElement schema;

        /// <summary>
        /// Initialises a new instance of the <see cref="SchemaHelper"/> class.
        /// </summary>
        public SchemaHelper()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SchemaHelper"/> class.
        /// </summary>
        /// <param name="filePath">Path to data_schema.xml file.</param>
        public SchemaHelper(string filePath)
        {
            this.schema = XElement.Load(filePath);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SchemaHelper"/> class.
        /// </summary>
        /// <param name="schema">data_schema.xml as an XElement.</param>
        public SchemaHelper(XElement schema)
        {
            this.schema = schema;
        }

        /// <summary>
        /// Loads the data_schema.xml file.
        /// </summary>
        /// <param name="filePath">Path to the data_schema.xml file.</param>
        /// <returns>True if the file was read and has elements.</returns>
        public bool LoadSchemaFile(string filePath)
        {
            this.schema = XElement.Load(filePath);

            return this.schema.HasElements;
        }

        /// <summary>
        /// Gets the XElement for a given entity and fieldname.
        /// </summary>
        /// <param name="entityName">Entity Logical Name.</param>
        /// <param name="fieldName">Field Name in the entitiy.</param>
        /// <returns>XElement containing the metadata for that field.</returns>
        public XElement GetFieldSchemaElement(string entityName, string fieldName)
        {
            var entities = from el in this.schema.Elements(Constants.EntityElementName)
                where (string)el.Attribute(Constants.NameAttributeName) == entityName
                select el;

            var entity = entities.Single();

            var fields = from el in entity.Descendants("field")
                where (string)el.Attribute(Constants.NameAttributeName) == fieldName
                select el;

            return fields.Single();
        }

        /// <summary>
        /// Generates a CRM Attribute value for a given value, entitiyname and fieldname.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="entityName">Entity Logical Name.</param>
        /// <param name="fieldName">Field Name.</param>
        /// <returns>A CRM typed value object.</returns>
        public dynamic GenerateAttributeValue(string value, string entityName, string fieldName)
        {
            var el = this.GetFieldSchemaElement(entityName, fieldName);
            return this.GenerateAttributeValue(value, el);
        }

        /// <summary>
        /// Generates a CRM Attribute value for a given value and field metadata.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="fieldSchema">Metadata for the field.</param>
        /// <returns>A CRM typed value object.</returns>
        public dynamic GenerateAttributeValue(string value, XElement fieldSchema)
        {
            switch (fieldSchema.Attribute("type")?.Value)
            {
                case "string":
                    return value;
                case "guid":
                    return Guid.Parse(value);
                case "bool":
                    return bool.Parse(value);
                case "datetime":
                    return DateTime.Parse(value);
                case "money":
                    decimal moneyDecimal;
                    moneyDecimal =
                        decimal.Parse(
                            char.IsSymbol(value[0])
                                ? value.Substring(2).Replace(",", string.Empty)
                                : value.Replace(",", string.Empty), CultureInfo.InvariantCulture);

                    return moneyDecimal;
                case "decimal":
                    return decimal.Parse(value.Replace(",", string.Empty), CultureInfo.InvariantCulture);
                case "number":
                    return int.Parse(value.Replace(",", string.Empty), CultureInfo.InvariantCulture);
                case "float":
                    return double.Parse(value.Replace(",", string.Empty), CultureInfo.InvariantCulture);
                case "partylist":
                    // TODO
                    return null;
                case "imagedata":
                    return null;
                case "optionsetvaluecollection":
                    IList<OptionSetValue> list = (from option in value.Replace(" ", string.Empty)
                            .Replace("[-1", string.Empty).Replace("-1]", string.Empty)
                            .Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToList()
                        select new OptionSetValue(option)).ToList();
                    return new OptionSetValueCollection(list);
                case "status":
                case "state":
                case "optionsetvalue":
                    return new OptionSetValue(int.Parse(value.Replace(",", string.Empty)));
                case "entityreference":
                    var lookupType = fieldSchema.Attribute(Constants.LookuptypeAttributeName)?.Value;
                    return new EntityReference(lookupType, Guid.Parse(value));
                default:
                    return null;
            }
        }
    }
}
