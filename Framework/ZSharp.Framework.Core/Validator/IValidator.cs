﻿using System;
using System.Collections.Generic;

namespace ZSharp.Framework.Validator
{
    /// <summary>
    /// The entity validator base contract
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Perform validation and return if the entity state is valid
        /// </summary>
        /// <typeparam name="T">The type of entity to validate</typeparam>
        /// <param name="item">The instance to validate</param>
        /// <returns>True if entity state is valid</returns>
        bool IsValid<T>(T item) where T : class;

        /// <summary>
        /// Return the collection of errors if entity state is not valid
        /// </summary>
        /// <typeparam name="TEntity">The type of entity</typeparam>
        /// <returns>A collection of validation errors</returns>
        IEnumerable<string> GetInvalidMessages();
    }
}
