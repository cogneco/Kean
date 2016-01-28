using System;

namespace Kean.Collection.Tree
{
    public interface INode<T>
    {
        ITree<T> Tree { get; }
        bool IsRoot { get; }

        bool HasParent { get; }
        bool HasChild { get; }
        bool HasPrevious { get; }
        bool HasNext { get; }
        bool IsFirst { get; }
        bool IsLast { get; }

        INode<T> Parent { get; }
        INode<T> Child { get; }
        INode<T> Previous { get; }
        INode<T> Next { get; }
        INode<T> First { get; }
        INode<T> Last { get; }

        ITree<T> Cut();

        int Count { get; }
        int Depth { get; }
    }
}
