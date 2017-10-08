using System;
using System.Linq;
using System.Collections.Generic;

namespace Ayatta
{
    public interface IHierarchyEntity<T> : IEntity<T>
    {
        T ParentId { get; set; }
        string Path { get; set; }
        List<IHierarchyEntity<T>> Children { get; set; }
        int Depth { get; }
        bool HasChildren { get; }
    }

    public abstract class HierarchyEntity<T>
    {
        public T Id { get; set; }
        public T ParentId { get; set; }
        public string Path { get; set; }
        public List<IHierarchyEntity<T>> Children { get; set; }

        public virtual int Depth => string.IsNullOrEmpty(Path) ? 0 : Path.Split(',').Length;

        public bool HasChildren => Children != null && Children.Count > 0;


        protected HierarchyEntity()
        {
            Children = new List<IHierarchyEntity<T>>();
        }
    }

    public static class HierarchyEntityExtension
    {
        public static IList<IHierarchyEntity<int>> ToHierarchy(this IList<IHierarchyEntity<int>> data, int parentId = 0)
        {
            Action<IHierarchyEntity<int>> addChildren = null;
            addChildren = (item =>
            {
                var children = data.Where(o => o.ParentId == item.Id).ToList();
                if (children.Count > 0)
                {
                    item.Children.AddRange(children);
                    foreach (var child in children)
                    {
                        addChildren(child);
                    }
                }
            });

            var root = data.Where(o => o.ParentId == parentId).ToList();
            root.ForEach(o => addChildren(o));
            return root;
        }

        public static IList<IHierarchyEntity<string>> ToHierarchy(this IList<IHierarchyEntity<string>> data, string parentId)
        {
            Action<IHierarchyEntity<string>> addChildren = null;
            addChildren = (item =>
            {
                var children = data.Where(o => o.ParentId == item.Id).ToList();
                if (children.Count > 0)
                {
                    item.Children.AddRange(children);
                    foreach (var child in children)
                    {
                        addChildren(child);
                    }
                }
            });
            var root = data.Where(o => o.ParentId == parentId).ToList();
            root.ForEach(o => addChildren(o));
            return root;
        }
    }
}