﻿using Gsafety.PTMS.Media.RTSP.Common.Extensions.Generic.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsafety.PTMS.Media.RTSP.Common.Collections.Generic
{
    /// <summary>
    /// Provides an implementation of LinkedList in which entries are only removed from the Next Node.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentLinkedQueue<T>
    {
        #region Nested Types

        /// <summary>
        /// A node which has a reference to the next node.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        internal sealed class Node
        {
            internal const Node Null = null;

            public T Value;

            public Node Next;

            //Create and have no value, Deleted, Has Value
            //Flags, Allocated, Deleted, Stored

            public Node(ref T data)
            {
                this.Value = data;
            }
        }

        #endregion

        #region Statics

        //Could make methods to call Set on LinkedListNode<T> using reflection.

        internal static System.Reflection.ConstructorInfo Constructor;

        internal static System.Reflection.PropertyInfo ListProperty, NextProperty, PreviousProperty, ValueProperty;

        static ConcurrentLinkedQueue()
        {
            //implementations may change the name
            //var fields = typeof(LinkedListNode<T>).GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

            //System.TypedReference tr = __makeref(obj, T);
            //fields[0].SetValueDirect(tr, obj);

            //or the ctor
            var ctors = typeof(LinkedListNode<T>).GetConstructors(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

            Constructor = ctors.LastOrDefault();
            //ctors[0].Invoke(new object[] { list, value });

            //Could also use properties but it's a tad slower...
            var props = typeof(LinkedListNode<T>).GetProperties();

            ListProperty = props.Where(p => p.Name == "List").FirstOrDefault();

            NextProperty = props.Where(p => p.Name == "Next").FirstOrDefault();

            PreviousProperty = props.Where(p => p.Name == "Previous").FirstOrDefault();

            ValueProperty = props.Where(p => p.Name == "Value").FirstOrDefault();

            //props[0].SetMethod.Invoke(obj, new object[] { list });
            //props[1].SetMethod.Invoke(obj, new object[] { next });
            //props[2].SetMethod.Invoke(obj, new object[] { prev });
            //props[3].SetMethod.Invoke(obj, new object[] { value });
        }

        public static void Circular(ConcurrentLinkedQueue<T> queue)
        {
            //queue.Last.Next = queue.First.Previous = queue.Last;

            //new LinkedListNode<T>(queue.Last.Value)
            //{
            //    //List = null, //internal constructor...
            //    //Next = null,
            //    //Last = null,
            //    //Previous = null,
            //    Value = default(T)
            //};

            if (queue == null) return;

            ////If the queue was empty
            //if (queue.IsEmpty)
            //{
            //    queue.Last = new LinkedListNode<T>(default(T));

            //    queue.First = new LinkedListNode<T>(default(T));
            //}

            ////First.Previous = queue.Last
            //PreviousProperty.SetValue(queue.First, queue.Last);

            ////Last.Next = queue.First
            //NextProperty.SetValue(queue.Last, queue.First);
        }

        #endregion

        #region Fields

        //Using AddLast / AddFirst
        //readonly System.Collections.Generic.LinkedList<T> LinkedList = new LinkedList<T>();

        //Using TryEnqueue / TryDequeue
        //readonly System.Collections.Concurrent.ConcurrentQueue<T> ConcurrentQueue = new System.Collections.Concurrent.ConcurrentQueue<T>();

        /// <summary>
        /// Cache of first and last nodes as they are added to the list.
        /// </summary>
        internal Node First, Last;

        /// <summary>
        /// The count of contained nodes
        /// </summary>
        long m_Count = 0;

        //Todo
        //Capacity, ICollection

        #endregion

        #region Properties

        /// <summary>
        /// Indicates how many elements are contained
        /// </summary>
        public long Count
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            //get { return System.Threading.Thread.VolatileRead(ref m_Count); }
            get { return m_Count; }
        }

        /// <summary>
        /// Indicates if no elements are contained.
        /// </summary>
        public bool IsEmpty
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return Count.Equals(0); }
        }

        #endregion

        #region Constrcutor

        /// <summary>
        /// Constructs a LinkedQueue
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ConcurrentLinkedQueue()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Try to Dequeue an element
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <remarks>Space Complexity S(5), Time Complexity O(2)</remarks>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryDequeue(out T t)
        {
            //Take a read on the First or the last node.
            //This can probably be reduced to a switch on Count which when < 0 uses Last instead.

            //Keep the read node on the stack
            Node onStack;

            //VolatileRead, Compare, Call
            if (Count <= 0 || Object.ReferenceEquals(onStack = First ?? Last, Node.Null))
            {
                //Store
                t = default(T);

                //Return
                return false;
            }

            //Load Store
            t = onStack.Value;

            //Exchange
            System.Threading.Interlocked.Exchange<Node>(ref First, onStack.Next);

            //Decrement count
            System.Threading.Interlocked.Decrement(ref m_Count);

            //Return
            return true;
        }

        /// <summary>
        /// Try to peek at the first element
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <remarks>Space Complexity S(1), Time Complexity O(1)</remarks>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryPeek(ref T t)
        {
            if (Object.ReferenceEquals(First, Node.Null)) return false;

            t = First.Value;

            return true;
        }

        /// <summary>
        /// Enqueue an element
        /// </summary>
        /// <param name="t"></param>
        public void Enqueue(T t) { TryEnqueue(ref t); }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        /// <remarks>Space Complexity S(2), Time Complexity O(2)</remarks>
        public bool TryEnqueue(ref T t)
        {
            Node newNode = new Node(ref t);

            if (Object.ReferenceEquals(First, Node.Null)) Last = First = newNode;
            else Last = Last.Next = newNode;

            System.Threading.Interlocked.Increment(ref m_Count);

            return true;
        }

        /// <summary>
        /// Sets First and Last to null and Calls Clear on the LinkedList.
        /// </summary>
        /// <remarks>Space Complexity S(5), Time Complexity O(Count)</remarks>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Clear(bool all, out Node head, out Node tail)
        {
            if (false == all)
            {
                head = First;

                tail = Last;

                First = Last = Node.Null;
            }
            else
            {
                System.Threading.Interlocked.Exchange(ref First, Last);

                head = tail = System.Threading.Interlocked.Exchange(ref Last, Node.Null);
            }

            System.Threading.Interlocked.Exchange(ref m_Count, 0);
        }

        public void Clear(bool all = true)
        {
            Node First, Last;

            Clear(all, out First, out Last);
        }

        #endregion

    }
}