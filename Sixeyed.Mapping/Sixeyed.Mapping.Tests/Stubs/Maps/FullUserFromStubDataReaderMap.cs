

namespace Sixeyed.Mapping.Tests.Stubs.Maps
{
    public class FullUserFromStubDataReaderMap : DataReaderMap<User>
    {
        /// <summary>
        /// Default constructor, initialises mapping
        /// </summary>
        public FullUserFromStubDataReaderMap()
        {
            this.AutoMapUnspecifiedTargets = true;
            Specify(ColumnName.Id, t => t.Id);           
        }

        private struct ColumnName
        {            
            public const string Id = "USERID";
        }
    }
}

