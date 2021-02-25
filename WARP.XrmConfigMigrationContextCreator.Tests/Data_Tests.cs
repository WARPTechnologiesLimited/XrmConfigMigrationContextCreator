using WARP.XrmConfigMigrationContextCreator;
using Xunit;

namespace WARP.XrmConfigMigrationFixtureCreator.Tests
{
    // ReSharper disable once InconsistentNaming
    public class Data_Tests
    {
        [Fact]
        public void CanReadFile()
        {
            const string DataFile = @"data\data.xml";
            const string SchemaFile = @"data\data_schema.xml";
            var dataFileReader = new DataFileReader(DataFile, SchemaFile);

            Assert.True(dataFileReader.ReadDataFile());
            Assert.NotEmpty(dataFileReader.GetEntities());
            Assert.NotEmpty(dataFileReader.GetRelationships());
        }
    }
}
