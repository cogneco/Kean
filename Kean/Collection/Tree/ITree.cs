using System;

namespace Kean.Collection.Tree
{
    public interface ITree<T>
    {
        INode<T> Root { get; }

        bool Contains(INode<T> node);
        bool Contains(T item);
        bool Remove(T item);

        ITree<T> Cut(T item);
        INode<T> this[T item] { get; }

    }
}
