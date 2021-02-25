// <copyright file="ManyToManyRelationshipStore.cs" company="WARP Technologies Limited">
// Copyright © WARP Technologies Limited
// </copyright>

namespace WARP.XrmConfigMigrationContextCreator.Models
{
    using System;
    using System.Collections.Generic;
    using FakeXrmEasy;

    /// <summary>
    /// Container for M-M relationships.
    /// </summary>
    public class ManyToManyRelationshipStore
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ManyToManyRelationshipStore"/> class.
        /// </summary>
        /// <param name="relationshipName">Name of the relationship as used in CRM.</param>
        /// <param name="intersectEntity">Name of the intersect entity.</param>
        /// <param name="entity1LogicalName">Name of Entity 1.</param>
        /// <param name="entity1Attribute">Name of keyfield of Entity 1.</param>
        /// <param name="entity2LogicalName">Name of Entity 2.</param>
        /// <param name="entity2Attribute">Name of keyfield of Entity 2.</param>
        public ManyToManyRelationshipStore(string relationshipName, string intersectEntity, string entity1LogicalName, string entity1Attribute, string entity2LogicalName, string entity2Attribute)
        {
            this.RelationshipName = relationshipName;
            this.IntersectEntity = intersectEntity;
            this.Entity1LogicalName = entity1LogicalName;
            this.Entity1Attribute = entity1Attribute;
            this.Entity2LogicalName = entity2LogicalName;
            this.Entity2Attribute = entity2Attribute;

            this.XrmFakedRelationship = new XrmFakedRelationship
                                            {
                                                Entity1Attribute = entity1Attribute,
                                                Entity1LogicalName = entity1LogicalName,
                                                Entity2Attribute = entity2Attribute,
                                                Entity2LogicalName = entity2LogicalName,
                                                IntersectEntity = intersectEntity,
                                            };

            this.Relationships = new List<Guid>();
        }

        /// <summary>
        /// Gets or sets the ID of Entity 1.
        /// </summary>
        public Guid Entity1Id { get; set; }

        /// <summary>
        /// Gets or sets name of the intersect entity.
        /// </summary>
        public string IntersectEntity { get; set; }

        /// <summary>
        /// Gets or sets name of Entity 1.
        /// </summary>
        public string Entity1LogicalName { get; set; }

        /// <summary>
        /// Gets or sets name of keyfield of Entity 1.
        /// </summary>
        public string Entity1Attribute { get; set; }

        /// <summary>
        /// Gets or sets name of Entity 2.
        /// </summary>
        public string Entity2LogicalName { get; set; }

        /// <summary>
        /// Gets or sets name of keyfield of Entity 2.
        /// </summary>
        public string Entity2Attribute { get; set; }

        /// <summary>
        /// Gets or sets name of the relationship as used in CRM.
        /// </summary>
        public string RelationshipName { get; set; }

        /// <summary>
        /// Gets the fake relationship metadata.
        /// </summary>
        public XrmFakedRelationship XrmFakedRelationship { get; }

        /// <summary>
        /// Gets or sets list of relationship IDs.
        /// </summary>
        public List<Guid> Relationships { get; set; }

        /// <summary>
        /// Adds a relationship to the list.
        /// </summary>
        /// <param name="entity2Guid">ID of Entity 2.</param>
        public void AddRelationship(Guid entity2Guid)
        {
            this.Relationships.Add(entity2Guid);
        }

        /// <summary>
        /// Adds a relationship to the list.
        /// </summary>
        /// <param name="entity2Id">ID of Entity 2.</param>
        public void AddRelationship(string entity2Id)
        {
            this.Relationships.Add(Guid.Parse(entity2Id));
        }
    }
}
