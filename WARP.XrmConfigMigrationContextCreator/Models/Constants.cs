// <copyright file="Constants.cs" company="WARP Technologies Limited">
// Copyright © WARP Technologies Limited
// </copyright>

namespace WARP.XrmConfigMigrationContextCreator.Models
{
    /// <summary>
    /// Container for constants.
    /// </summary>
    internal static class Constants
    {
        /// <summary> Gets name for entity element. </summary>
        public const string EntityElementName = "entity";

        /// <summary> Gets name for name attribute. </summary>
        public const string NameAttributeName = "name";

        /// <summary> Gets name for type attribute. </summary>
        public const string TypeAttributeName = "type";

        /// <summary> Gets name for lookup type attribute. </summary>
        public const string LookuptypeAttributeName = "lookupType";

        /// <summary> Gets name for enitites element. </summary>
        public const string EntitiesElementName = "entities";

        /// <summary> Gets name for record element. </summary>
        public const string RecordElementName = "record";

        /// <summary> Gets name for field element. </summary>
        public const string FieldElementName = "field";

        /// <summary> Gets name for value attribute. </summary>
        public const string ValueAttributeName = "value";

        /// <summary> Gets name for M-M element. </summary>
        public const string ManyToManyElementName = "m2mrelationship";

        /// <summary> Gets name for m-m name attribute. </summary>
        public const string ManyToManyRelatiohsipNameAttributeName = "m2mrelationshipname";

        /// <summary> Gets default name for data.xml file. </summary>
        public const string DefaultDataFileName = "data.xml";

        /// <summary> Gets default name for data_schema.xml file. </summary>
        public const string DefaultSchemaFileName = "data_schema.xml";
    }
}
