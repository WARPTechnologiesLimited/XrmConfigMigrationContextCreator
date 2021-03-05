namespace WARP.XrmConfigMigrationFixtureCreator.Tests
{
    using FakeXrmEasy;

    using Microsoft.Xrm.Sdk.Query;

    using WARP.XrmConfigMigrationContextCreator;

    using Xunit;

    // ReSharper disable once InconsistentNaming
    public class Context_Tests
    {
        private const string Folder = "data";

        [Fact]
        public void CanFillEntitiesFromFolder()
        {
            var fakedContext = new XrmFakedContext();
            var manager = new ContextManager(fakedContext);
            manager.FillContext(Folder, false);

            Assert.True(fakedContext.Data.Count > 0);
        }

        [Fact]
        public void CanFillEntitiesAndRelationshipsFromFolder()
        {
            var fakedContext = new XrmFakedContext();
            var manager = new ContextManager(fakedContext);
            manager.FillContext(Folder);

            // query an intersect entity.
            var service = fakedContext.GetOrganizationService();
            var query = new QueryExpression("opportunitycompetitors")
            {
                ColumnSet = new ColumnSet()
            };
            var m2ms = service.RetrieveMultiple(query);

            Assert.True(fakedContext.Data.Count > 0);
            Assert.True(m2ms.Entities.Count > 0);
        }
    }
}
