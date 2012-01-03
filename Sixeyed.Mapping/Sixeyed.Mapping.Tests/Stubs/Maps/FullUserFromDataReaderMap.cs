

namespace Sixeyed.Mapping.Tests.Stubs.Maps
{
    public class FullUserFromDataReaderMap : DataReaderMap<User>
    {
        /// <summary>
        /// Default constructor, initialises mapping
        /// </summary>
        public FullUserFromDataReaderMap()
        {
            this.AutoMapUnspecifiedTargets = false;
            Specify(ColumnName.Id, t => t.Id);
            Specify(ColumnName.FirstName, t => t.FirstName);
            Specify(ColumnName.LastName, t => t.LastName);
            Specify(ColumnName.AddressLine1, t => t.Address.Line1);
            Specify(ColumnName.AddressLine2, t => t.Address.Line2);
            Specify(ColumnName.PostCode, t => t.Address.PostCode.Code);            
        }

        private struct ColumnName
        {            
            public const string Id = "UserId";
            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string AddressLine1 = "Line1";
            public const string AddressLine2 = "Line2";
            public const string PostCode = "PostCode";
        }
    }
}

