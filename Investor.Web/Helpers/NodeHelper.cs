using System;
using System.Collections.Generic;
using System.Linq;
using umbraco.interfaces;
using umbraco.NodeFactory;

namespace Investor.Helpers
{
    public class NodeHelper
    {
        public Node GetRootNode(INode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node", "GetRootNode(null)");
            }

            if (node.Level == 1)
            {
                var rootNode = node as Node;

                if (rootNode == null)
                {
                    throw new Exception("rootNode is not right type");
                }

                return rootNode;
            }

            return GetRootNode(node.Parent);
        }

        public static IEnumerable<Node> FindChildren(Node currentNode, Func<Node, bool> predicate)
        {
            var result = new List<Node>();
            var nodes = currentNode.Children.OfType<Node>().Where(predicate);

            if (nodes.Count() != 0)
            {
                result.AddRange(nodes);
            }

            foreach (var child in currentNode.Children.OfType<Node>())
            {
                nodes = FindChildren(child, predicate);

                if (nodes.Count() != 0)
                {
                    result.AddRange(nodes);
                }
            }

            return result;
        }
    }
}