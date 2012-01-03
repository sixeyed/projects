using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Sixeyed.Mapping.Extensions;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping.MappingStrategies
{
    /// <summary>
    /// Mapping stratgey for mapping from a <see cref="XDocument"/> input 
    /// </summary>
    /// <remarks>
    /// Source is an xdcoument, index for looking up field mappings is string - 
    /// representing the name of an element or attribute xobject in the source
    /// </remarks>
    public class XDocumentMappingStrategy : MappingStrategy<XDocument, XObject, string>
    {
        protected override void CreateMappings(XDocument source, Type targetType, IMatchingStrategy<string> matching)
        {
            var objects = new List<XObject>();
            var nodes = (from node in source.Root.DescendantNodesAndSelf()
                             where node.NodeType == XmlNodeType.Element
                             select node as XObject);
            foreach (var node in nodes)
            {
                objects.Add(node);
                objects.AddRange(from attr in ((XElement)node).Attributes()
                                 select attr as XObject);
            }            

            var targetProperties = targetType.GetAccessiblePublicInstanceProperties();
            foreach (var target in targetProperties)
            {
                foreach (XObject obj in objects)
                {
                    XName name = null;
                    XElement element = obj as XElement;
                    if (element != null)
                        name = element.Name;
                    else
                    {
                        XAttribute attribute = obj as XAttribute;
                        name = attribute.Name;
                    }

                    if (matching.IsMatch(target, name.LocalName))
                    {
                        AddMapping(CreateMapping(obj, target));
                        break;
                    }
                }
            }
        }

        protected override IPropertyMapping CreateMapping(XObject sourceItem, PropertyInfo target)
        {
            XElement element = sourceItem as XElement;
            if (element != null)
                return new XElementMapping(element.Name, target);
            else
            {
                XAttribute attribute = sourceItem as XAttribute;
                return new XAttributeMapping(attribute.Name, target);
            }
        }
    }
}
