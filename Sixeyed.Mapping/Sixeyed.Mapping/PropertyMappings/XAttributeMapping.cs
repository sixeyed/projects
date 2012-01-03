using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Property mapping using a named attribute field within an XML input as source
    /// </summary>
    [Serializable]
    public class XAttributeMapping : PropertyMapping<XName>
    {
        /// <summary>
        /// Constructor with source attribute name and target property
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="target"></param>
        public XAttributeMapping(XName sourceName, PropertyInfo target) : base(sourceName, target) { }

        /// <summary>
        /// Constructor with source attribute name and target accessor
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="targetExpression"></param>
        public XAttributeMapping(XName sourceName, LambdaExpression targetExpression) : base(sourceName, targetExpression) { }

        /// <summary>
        /// Whether the source field can be read 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override bool CanRead(object source)
        {
            return (source is XDocument);
        }

        protected override object GetValueInternal(object source)
        {
            object value = null;
            if (CanRead(source))
            {
                var sourceDocument = source as XDocument;
                var attribute = sourceDocument.Root.Attributes(Source).FirstOrDefault() as XAttribute;
                if (attribute != null)
                {
                    value = attribute.Value;
                }
            }
            return value;
        }
    }
}