using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Property mapping using a named element field within an XML input as source
    /// </summary>
    [Serializable]
    public class XElementMapping : PropertyMapping<XName>
    {
        /// <summary>
        /// Constructor with source element name and target property
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="target"></param>
        public XElementMapping(XName sourceName, PropertyInfo target) : base(sourceName, target) { }

        /// <summary>
        /// Constructor with source element name and target property
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="targetExpression"></param>
        public XElementMapping(XName sourceName, LambdaExpression targetExpression) : base(sourceName, targetExpression) { }

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
                XDocument sourceDocument = source as XDocument;
                var element = sourceDocument.Root.Descendants(Source).FirstOrDefault() as XElement;
                if (element != null)
                {
                    value = element.Value;
                }
            }
            return value;
        }
    }
}