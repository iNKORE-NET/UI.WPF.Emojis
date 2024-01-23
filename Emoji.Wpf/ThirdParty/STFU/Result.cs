//
//  Stfu — Sam’s Tiny Framework Utilities
//
//  Copyright © 2013–2022 Sam Hocevar <sam@hocevar.net>
//
//  This library is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

using System;

namespace Stfu
{
    public class ResultBase
    {
        internal ResultBase(string message)
        {
            m_message = message;
        }

        public bool IsError
            => m_message != null;

        public string Message
            => m_message;

        protected string m_message;
    }

    public class Result : ResultBase
    {
        /// <summary>
        /// Create a successful result object
        /// </summary>
        public Result()
            : base(null)
        {
        }

        public Result(string message)
            : base(message ?? "")
        {
        }

        public static readonly Result Ok = new Result();
        public static Result Error(string message) => new Result(message);
    }

    public class Result<T> : ResultBase
    {
        /// <summary>
        /// Create a successful result object
        /// </summary>
        /// <param name="val">The value stored in the result</param>
        public Result(T val)
            : base(null)
            => m_value = val;

        /// <summary>
        /// Create a failed result object with an error message
        /// </summary>
        /// <param name="val"></param>
        /// <param name="message"></param>
        public Result(T val, string message)
            : base(message ?? "")
            => m_value = val;

        public static implicit operator T(Result<T> val)
            => val.m_value;

        public static implicit operator Result<T>(T val)
            => new Result<T>(val);

        /// <summary>
        /// Create a typed result object from a generic result object. If one. If r.IsError is true,
        /// the result value is set to default(T). In practice, this will be null, or 0, or false.
        /// If r.IsError is false, we attempt to explicitly construct a T object instead.
        /// <param name="r"></param>
        /// </summary>
        public static implicit operator Result<T>(Result r)
        {
            try
            {
                if (!r.IsError)
                    return new Result<T>((T)Activator.CreateInstance(typeof(T))) { m_message = r.Message };
            }
            catch {}

            return new Result<T>(default(T)) { m_message = r.Message };
        }


        public static implicit operator Result<T>(ValueTuple<T, string> tuple)
            => new Result<T>(tuple.Item1, tuple.Item2);

        /// <summary>
        /// The value stored in the result object
        /// </summary>
        public T Value
            => m_value;

        private readonly T m_value;
    }
}
