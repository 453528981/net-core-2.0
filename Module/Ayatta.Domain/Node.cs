using System;
using System.Linq;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    public class Node
    {
        public string Id { get; set; }
        public string Pid { get; set; }
        public string Text { get; set; }
        public bool IsParent { get; set; }
        public IDictionary<string,string> Props { get; set; }
        public List<Node> Nodes { get; set; }

        public Node()
        {
            Nodes = new List<Node>();
        }
    }
    public static partial class Extension
    {
        public static IList<Node> ToHierarchy(this IEnumerable<Node> data, string rootId)
        {
            Action<Node> addChildren = null;
            addChildren = (item =>
            {
                var children = data.Where(o => o.Pid == item.Id).ToList();
                if (children.Count > 0)
                {
                    item.IsParent = true;
                    item.Nodes.AddRange(children);
                    foreach (var child in children)
                    {
                        addChildren(child);
                    }
                }                              
            });

            var root = data.Where(o => o.Pid == rootId).ToList();
            root.ForEach(o => addChildren(o));
            return root;
        }
    }

}
