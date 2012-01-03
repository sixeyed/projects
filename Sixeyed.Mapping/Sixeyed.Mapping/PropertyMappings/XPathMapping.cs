using System;
using System.Collections;
using System.Linq.Expressions;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Property mapping using an XPath query over an XML input as source
    /// </summary>
    [Serializable]
    public class XPathMapping : PropertyMapping<string>
    {      
        /// <summary>
        /// Constructor with source XPath query and target accessor
        /// </summary>
        /// <param name="sourceQuery"></param>
        /// <param name="targetExpression"></param>
        public XPathMapping(string sourceQuery, LambdaExpression targetExpression) : base(sourceQuery, targetExpression) { }

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
                var result = sourceDocument.Root.XPathEvaluate(Source);
                if (result is IEnumerable)
                {
                    var enumerator = ((IEnumerable)result).GetEnumerator();
                    enumerator.MoveNext();
                    var node = enumerator.Current;
                    if (node is XElement)
                    {
                        value = ((XElement)node).Value;
                    }
                    else if (node is XAttribute)
                    {
                        value = ((XAttribute)node).Value;
                    }
                }
            }
            return value;
        }
    }
}