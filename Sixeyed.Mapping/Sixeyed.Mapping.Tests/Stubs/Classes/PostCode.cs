using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sixeyed.Mapping.Tests.Stubs
{
    public class PostCode
    {
        public string InwardCode { get; set; }
        public string OutwardCode { get; set; }

        public string Code
        {
            get { return Parse(this); }
            set
            {
                OriginalCode = value;
                var parts = value.Split(' ');
                if (parts.Length == 2)
                {
                    InwardCode = parts[0].Trim().ToUpper();
                    OutwardCode = parts[1].Trim().ToUpper();
                }
                else
                {
                    InwardCode = OutwardCode = string.Empty;
                }
            }
        }

        public string OriginalCode { get; private set; }

        public static string Parse(PostCode postCode)
        {
            if (string.IsNullOrEmpty(postCode.InwardCode) || string.IsNullOrEmpty(postCode.OutwardCode))
                return string.Empty;
            return string.Format("{0} {1}", postCode.InwardCode.Trim().ToUpper(), postCode.OutwardCode.Trim().ToUpper());
        }
    }
}
