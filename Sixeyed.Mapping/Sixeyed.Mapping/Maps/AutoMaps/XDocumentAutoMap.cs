using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Linq;
using Sixeyed.Mapping.Exceptions;
using Sixeyed.Mapping.MatchingStrategies;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Automap for populating an object from an XML source, populated as <see cref="XDocument"/>
    /// </summary>
    /// <remarks>
    /// By default, uses <see cref="SimpleNameMatchingStrategy"/>, matching target properties which have
    /// similar names to source XML element or attribute names. Ignores case and non-aplhanumeric characters
    /// </remarks>
    /// <typeparam name="TTarget">Type of target object</typeparam>
    public class XDocumentAutoMap<TTarget> : XDocumentMap<TTarget>
        where TTarget : class, new()
    {
        /// <summary>
        /// Whether to automatically matach & map unspecified target properties
        /// </summary>
        /// <remarks>
        /// Required by map base, in AutoMap always returns true
        /// </remarks>
        public override bool AutoMapUnspecifiedTargets
        {
            get { return true; }
            set { } //do nothing
        }

        /// <summary>
        /// Specify the caching strategy for the map, used to cache the property match between source and target
        /// </summary>
        /// <typeparam name="TCachingStrategy"></typeparam>
        /// <returns></returns>
        public new XDocumentAutoMap<TTarget> Cache<TCachingStrategy>()
            where TCachingStrategy : ICachingStrategy, new()
        {
            return (XDocumentAutoMap<TTarget>)base.Cache<TCachingStrategy>();
        }

        /// <summary>
        /// Specify the matching strategy for the map, used to match source and target properties
        /// </summary>
        /// <typeparam name="TMatchingStrategy"></typeparam>
        /// <returns></returns>
        public new XDocumentAutoMap<TTarget> Matching<TMatchingStrategy>()
            where TMatchingStrategy : IMatchingStrategy<string>, new()
        {
            return (XDocumentAutoMap<TTarget>)base.Matching<TMatchingStrategy>();
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <param name="nodeType">Type of the source node - one of Element or Attribute</param>
        /// <param name="sourceNodeName">Name of the source node to get</param>
        /// <param name="targetPropertyExpression">Accessor for the target property to set</param>
        /// <returns></returns>
        public new XDocumentAutoMap<TTarget> Specify(XmlNodeType nodeType, XName sourceNodeName, Expression<Func<TTarget, object>> targetPropertyExpression)
        {
            return (XDocumentAutoMap<TTarget>)base.Specify(nodeType, sourceNodeName, targetPropertyExpression);
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <param name="xPathQuery">XPath query for the source node to get, starting from root</param>
        /// <param name="targetPropertyExpression">Accessor for the target property to set</param>
        /// <returns></returns>
        public new XDocumentAutoMap<TTarget> Specify(string xPathQuery, Expression<Func<TTarget, object>> targetPropertyExpression)
        {
            return (XDocumentAutoMap<TTarget>)base.Specify(xPathQuery, targetPropertyExpression);
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
        public new XDocumentAutoMap<TTarget> Specify<TInput, TOutput>(XmlNodeType nodeType, XName sourceNodeName, Expression<Func<TTarget, TOutput>> targetPropertyExpression, Func<TInput, TOutput> conversion)
        {
            return (XDocumentAutoMap<TTarget>)base.Specify<TInput, TOutput>(nodeType, sourceNodeName, targetPropertyExpression, conversion);
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
        public new XDocumentAutoMap<TTarget> Specify<TInput, TOutput>(string xPathQuery, Expression<Func<TTarget, TOutput>> targetPropertyExpression, Func<TInput, TOutput> conversion)
        {
            return (XDocumentAutoMap<TTarget>)base.Specify<TInput, TOutput>(xPathQuery, targetPropertyExpression, conversion);
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <param name="mappingAction">Action to populate target property value</param>
        /// <returns></returns>
        public new XDocumentAutoMap<TTarget> Specify(Action<XDocument, TTarget> mappingAction)
        {
            return (XDocumentAutoMap<TTarget>)base.Specify(mappingAction);
        }

        /// <summary>
        /// Populate an existing target object from a source document
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void PopulateTarget(XDocument source, TTarget target)
        {
            var map = new XDocumentAutoMap<TTarget>();
            map.Populate(source, target);
        }

        /// <summary>
        /// Create a target object populated from a source document
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TTarget CreateTarget(XDocument source)
        {
            var map = new XDocumentAutoMap<TTarget>();
            return map.Create(source);
        }

        /// <summary>
        /// Create a collection of target objects from a source document
        /// </summary>
        /// <param name="sourceList"></param>
        /// <returns></returns>
        public static List<TTarget> CreateTargetList(XDocument sourceList)
        {
            var map = new XDocumentAutoMap<TTarget>();
            return map.CreateList(sourceList);
        }

        /// <summary>
        /// Create a collection of target object populated from a source document
        /// </summary>
        /// <remarks>
        /// Takes a collection of source documents for compatibility, but can only read from a single document
        /// </remarks>
        /// <exception cref="MappingException">Thrown if ThrowMappingExceptions is true and more than one document is passed in</exception>
        /// <returns></returns>
        public override List<TTarget> CreateList(IList<XDocument> sourceList)
        {
            if (sourceList.Count != 1)
            {
                if (ThrowMappingExceptions)
                {
                    throw new MappingException("XDocumentAutoMap.CreateList: cannot use zero or multiple XDocuments, pass a single XDocument into CreateList");
                }
            }
            var list = new List<TTarget>();
            var sourceNodes = sourceList[0].Root.Nodes();
            foreach (var sourceNode in sourceNodes)
            {
                var sourceDocument = new XDocument();
                sourceDocument.Add(sourceNode);
                list.Add(Create(sourceDocument));
            }
            return list;
        }
    }
}
