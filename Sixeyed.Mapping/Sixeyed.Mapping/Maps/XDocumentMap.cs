using System;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Linq;
using Sixeyed.Mapping.MappingStrategies;
using Sixeyed.Mapping.MatchingStrategies;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Base class for populating an object from an XML source, populated as <see cref="XDocument"/>
    /// </summary>
    /// <remarks>
    /// By default, uses <see cref="SimpleNameMatchingStrategy"/>, matching target properties which have
    /// similar names to source XML element or attribute names. Ignores case and non-aplhanumeric characters
    /// </remarks>
    /// <typeparam name="TTarget">Type of target object</typeparam>
    public abstract class XDocumentMap<TTarget> : Map<XDocument, TTarget, string>
        where TTarget : class, new()
    {
        protected override IMappingStrategy<string> GetDefaultMappingStrategy()
        {
            return new XDocumentMappingStrategy();
        }

        protected override IMatchingStrategy<string> GetDefaultMatchingStrategy()
        {
            return new SimpleNameMatchingStrategy();
        }

        /// <summary>
        /// Specify the caching strategy for the map, used to cache the property match between source and target
        /// </summary>
        /// <typeparam name="TCachingStrategy"></typeparam>
        /// <returns></returns>
        public new XDocumentMap<TTarget> Cache<TCachingStrategy>()
            where TCachingStrategy : ICachingStrategy, new()
        {
            return (XDocumentMap<TTarget>)base.Cache<TCachingStrategy>();
        }

        /// <summary>
        /// Specify the matching strategy for the map, used to match source and target properties
        /// </summary>
        /// <typeparam name="TMatchingStrategy"></typeparam>
        /// <returns></returns>
        public new XDocumentMap<TTarget> Matching<TMatchingStrategy>()
            where TMatchingStrategy : IMatchingStrategy<string>, new()
        {
            return (XDocumentMap<TTarget>)base.Matching<TMatchingStrategy>();
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <param name="nodeType">Type of the source node - one of Element or Attribute</param>
        /// <param name="sourceNodeName">Name of the source node to get</param>
        /// <param name="targetPropertyExpression">Accessor for the target property to set</param>
        /// <returns></returns>
        public XDocumentMap<TTarget> Specify(XmlNodeType nodeType, XName sourceNodeName, Expression<Func<TTarget, object>> targetPropertyExpression)
        {
            var mapping = GetNodeMapping(nodeType, sourceNodeName, targetPropertyExpression);
            MappingStrategy.AddMapping(mapping);
            return this;
        }
       
        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <param name="xPathQuery">XPath query for the source node to get, starting from root</param>
        /// <param name="targetPropertyExpression">Accessor for the target property to set</param>
        /// <returns></returns>
        public XDocumentMap<TTarget> Specify(string xPathQuery, Expression<Func<TTarget, object>> targetPropertyExpression)
        {
            var mapping = new XPathMapping(xPathQuery, targetPropertyExpression);
            MappingStrategy.AddMapping(mapping);
            return this;
        }

        /// <summary>
        /// Specify an explicit property mapping with a value conversion
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown if XmlNodeType is not Element or Attribute
        /// </exception>
        /// <typeparam name="TInput">Source property type</typeparam>
        /// <typeparam name="TOutput">Target property type</typeparam>
        /// <param name="nodeType">Type of the source node - one of Element or Attribute</param>
        /// <param name="sourceNodeName">Name of the source node to get</param>
        /// <param name="targetPropertyExpression">Accessor for the target property to set</param>
        /// <param name="conversion">Conversion function to apply to the source value</param>
        /// <returns></returns>
        public XDocumentMap<TTarget> Specify<TInput, TOutput>(XmlNodeType nodeType, XName sourceNodeName, Expression<Func<TTarget, TOutput>> targetPropertyExpression, Func<TInput, TOutput> conversion)
        {
            var mapping = GetNodeMapping(nodeType, sourceNodeName, targetPropertyExpression);
            mapping.SetConversion<TInput, TOutput>(conversion);
            MappingStrategy.AddMapping(mapping);
            return this;            
        }

        /// <summary>
        /// Specify an explicit property mapping with a value conversion
        /// </summary>
        /// <typeparam name="TInput">Source property type</typeparam>
        /// <typeparam name="TOutput">Target property type</typeparam>
        /// <param name="xPathQuery">XPath query for the source node to get, starting from root</param>
        /// <param name="targetPropertyExpression">Accessor for the target property to set</param>
        /// <param name="conversion">Conversion function to apply to the source value</param>
        /// <returns></returns>
        public XDocumentMap<TTarget> Specify<TInput, TOutput>(string xPathQuery, Expression<Func<TTarget, TOutput>> targetPropertyExpression, Func<TInput, TOutput> conversion)
        {
            var mapping = new XPathMapping(xPathQuery, targetPropertyExpression);
            mapping.SetConversion<TInput, TOutput>(conversion);
            MappingStrategy.AddMapping(mapping);
            return this;
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <param name="mappingAction">Action to populate target property value</param>
        /// <returns></returns>
        public new XDocumentMap<TTarget> Specify(Action<XDocument, TTarget> mappingAction)
        {
            return (XDocumentMap<TTarget>)base.Specify(mappingAction);
        }

        private IPropertyMapping GetNodeMapping(XmlNodeType nodeType, XName sourceNodeName, LambdaExpression targetPropertyExpression)
        {
            IPropertyMapping mapping = null;
            if (nodeType != XmlNodeType.Element && nodeType != XmlNodeType.Attribute)
            {
                throw new ArgumentException(string.Format("XmlNodeType must be Element or Attribute; {0} not supported", nodeType), "nodeType");
            }
            switch (nodeType)
            {
                case XmlNodeType.Attribute:
                    mapping = new XAttributeMapping(sourceNodeName, targetPropertyExpression);
                    break;
                case XmlNodeType.Element:
                    mapping = new XElementMapping(sourceNodeName, targetPropertyExpression);
                    break;
            }
            return mapping;
        }
    }
}
