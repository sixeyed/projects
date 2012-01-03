

namespace Sixeyed.Mapping.Tests.Stubs.Maps
{
    public class FullUserFromCsvMap : CsvMap<User>
    {
        /// <summary>
        /// Default constructor, initialises mapping
        /// </summary>
        public FullUserFromCsvMap()
        {
            this.AutoMapUnspecifiedTargets = false;
            Specify(FieldIndex.Id, t => t.Id);
            Specify(FieldIndex.FirstName, t => t.FirstName);
            Specify(FieldIndex.LastName, t => t.LastName);
            Specify(FieldIndex.DateOfBirth, t => t.DateOfBirth);
            Specify(FieldIndex.AddressLine1, t => t.Address.Line1);
            Specify<string, string>(FieldIndex.AddressLine2, t => t.Address.Line2, x=> x.ToLower());
            //...
        }

        private struct FieldIndex
        {            
            public const int Id = 1;
            public const int FirstName = 2;
            public const int LastName = 3;
            public const int DateOfBirth = 4;
            public const int AddressLine1 = 5;
            public const int AddressLine2 = 6;
            //...
        }
    }
}
