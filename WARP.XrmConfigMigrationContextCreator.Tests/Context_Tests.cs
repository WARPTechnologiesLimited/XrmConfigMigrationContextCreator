namespace WARP.XrmConfigMigrationFixtureCreator.Tests
{
    using FakeXrmEasy;

    using WARP.XrmConfigMigrationContextCreator;

    using Xunit;

    // ReSharper disable once InconsistentNaming
    public class Context_Tests
    { 
        [Fact]
        public void CanFillContextFromFolder()
        {
            const string Folder = "data";
            var fakedContext = new XrmFakedContext();
            var manager = new ContextManager(fakedContext);
            manager.FillContext(Folder);

            Assert.True(manager.FakedContext.Data.Count > 1);
        }
    }
}
