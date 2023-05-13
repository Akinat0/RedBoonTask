using System;
using System.Collections.Generic;
using UnityEngine;

namespace Trading.UI
{
    class ObjectPool<T> where T : new()
    {
        readonly Stack<T> stack = new Stack<T>();
        
        Func<T> OnCreate { get; }
        Action<T> OnGet { get; }
        Action<T> OnRelease { get; }

        int Count { get; set; }

        public ObjectPool(Func<T> onCreate, Action<T> onGet, Action<T> onRelease)
        {
            OnCreate = onCreate;
            OnGet = onGet;
            OnRelease = onRelease;
        }

        public T Get()
        {
            T element;
            if (stack.Count == 0)
            {
                element = OnCreate.Invoke();
                Count++;
            }
            else
            {
                element = stack.Pop();
            }

            OnGet?.Invoke(element);
            
            return element;
        }

        public void Release(T element)
        {
            if (stack.Count > 0 && ReferenceEquals(stack.Peek(), element))
                Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
            
            OnRelease?.Invoke(element);
            stack.Push(element);
        }
    }
}