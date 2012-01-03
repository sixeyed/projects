

namespace Sixeyed.Mapping.Tests.Stubs.Maps
{
    public class FullUserFromOrderedCsvMap : CsvMap<User>
    {
        /// <summary>
        /// Default constructor, initialises mapping
        /// </summary>
        public FullUserFromOrderedCsvMap()
        {
            this.AutoMapUnspecifiedTargets = true;
            Specify(FieldIndex.AddressLine1, t => t.Address.Line1);
            Specify(FieldIndex.AddressLine2, t => t.Address.Line2);
            //...
        }

        private struct FieldIndex
        {
            public const int AddressLine1 = 6;
            public const int AddressLine2 = 7;
            //...
        }
    }
}
