// <copyright file="DataFileReader.cs" company="WARP Technologies Limited">
// Copyright © WARP Technologies Limited
// </copyright>

namespace WARP.XrmConfigMigrationContextCreator
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using Microsoft.Xrm.Sdk;

    using WARP.XrmConfigMigrationContextCreator.Models;

    /// <summary>
    /// Class for reading the data.xml file.
    /// </summary>
    public class DataFileReader
    {
        private readonly string dataFilePath;

        private readonly SchemaHelper schemaHelper;

        private List<ManyToManyRelationshipStore> relationshipStores;

        private List<Entity> entities;

        /// <summary>
        /// Initialises a new instance of the <see cref="DataFileReader"/> class.
        /// </summary>
        /// <param name="dataFilePath">Path to the data.xml file.</param>
        /// <param name="schemaFilePath">Path to the data_schema.xml file.</param>
        public DataFileReader(string dataFilePath, string schemaFilePath)
        {
            this.dataFilePath = dataFilePath;
            this.schemaHelper = new SchemaHelper(schemaFilePath);
        }

        /// <summary>
        /// Reads the data.xml file.
        /// </summary>
        /// <returns>True if the file was read.</returns>
        public bool ReadDataFile()
        {
            this.entities = new List<Entity>();
            this.relationshipStores = new List<ManyToManyRelationshipStore>();

            var entityName = string.Empty;
            Entity entity = null;
            ManyToManyRelationshipStore relationshipStore = null;
            var isActivityPointer = false;

            using (var reader = XmlReader.Create(this.dataFilePath))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (reader.Name)
                            {
                                case Constants.EntityElementName:
                                    // started a new record type.
                                    entityName = reader.GetAttribute(Constants.NameAttributeName);
                                    break;
                                case Constants.RecordElementName:
                                    // started a new record.
                                    entity = new Entity(entityName, Guid.Parse(reader.GetAttribute("id")));
                                    break;
                                case Constants.FieldElementName:
                                    // a field with value.
                                    if (isActivityPointer)
                                    {
                                        // TODO.
                                    }
                                    else
                                    {
                                        var attributeName = reader.GetAttribute(Constants.NameAttributeName);
                                        var value = reader.GetAttribute(Constants.ValueAttributeName);
                                        var crmValue = this.schemaHelper.GenerateAttributeValue(value, entityName, attributeName);

                                        if (crmValue != null)
                                        {
                                            // add the field to the entity.
                                            entity?.Attributes.Add(attributeName, crmValue);
                                        }
                                    }

                                    break;
                                case Constants.ManyToManyElementName:
                                    // a m-m relationship.
                                    // TODO: Is this ok for the relationshipName? Calling it the same as the intersect entity.
                                    // The name of the xml attribute "m2mrelationshipname" is misleading.
                                    // The value is actually the name of the intersect entity. Not the relationship name.
                                    // Example: systemuser M-M with role.
                                    // Actual metadata: Intersect entity = systemuserroles. Relationship SchemaName = systemuserroles_association.
                                    // XML Attribute m2mrelationshipname: Value = systemuserroles.
                                    var relationshipName = reader.GetAttribute(Constants.ManyToManyRelatiohsipNameAttributeName);
                                    relationshipStore = new ManyToManyRelationshipStore(
                                                            relationshipName,
                                                            reader.GetAttribute(Constants.ManyToManyRelatiohsipNameAttributeName),
                                                            entityName,
                                                            $"{entityName}id",
                                                            reader.GetAttribute("targetentityname"),
                                                            reader.GetAttribute("targetentitynameidfield"))
                                                            {
                                                                Entity1Id = Guid.Parse(reader.GetAttribute("sourceid")),
                                                            };
                                    break;
                                case "targetid":
                                    var id = reader.ReadElementContentAsString();
                                    relationshipStore.AddRelationship(id);
                                    break;
                                case Constants.ActivityPointerRecordsElementName:
                                    // starting an activity pointer / party list
                                    isActivityPointer = true;

                                    // TODO: Handle an activity.
                                    break;
                            }

                            break;

                        case XmlNodeType.EndElement:
                            switch (reader.Name)
                            {
                                case Constants.RecordElementName:
                                    // finished this entity. Add to the collection.
                                    this.entities.Add(entity);
                                    break;
                                case Constants.ManyToManyElementName:
                                    this.relationshipStores.Add(relationshipStore);
                                    break;
                                case Constants.ActivityPointerRecordsElementName:
                                    isActivityPointer = false;

                                    // TODO: Handle the activity.
                                    break;
                            }

                            break;

                        case XmlNodeType.None:
                        case XmlNodeType.Attribute:
                        case XmlNodeType.Text:
                        case XmlNodeType.CDATA:
                        case XmlNodeType.EntityReference:
                        case XmlNodeType.Entity:
                        case XmlNodeType.ProcessingInstruction:
                        case XmlNodeType.Comment:
                        case XmlNodeType.Document:
                        case XmlNodeType.DocumentType:
                        case XmlNodeType.DocumentFragment:
                        case XmlNodeType.Notation:
                        case XmlNodeType.Whitespace:
                        case XmlNodeType.SignificantWhitespace:
                        case XmlNodeType.EndEntity:
                        case XmlNodeType.XmlDeclaration:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the entities read from the data.xml file.
        /// </summary>
        /// <returns>List of Entity objects.</returns>
        public List<Entity> GetEntities()
        {
            if (this.entities == null)
            {
                this.ReadDataFile();
            }

            return this.entities;
        }

        /// <summary>
        /// Gets the m-m relationships read from the data.xml file.
        /// </summary>
        /// <returns>List of RelationshipStore objects.</returns>
        public List<ManyToManyRelationshipStore> GetRelationships()
        {
            if (this.relationshipStores == null)
            {
                this.ReadDataFile();
            }

            return this.relationshipStores;
        }
    }
}
