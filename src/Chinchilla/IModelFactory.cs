using System;

namespace Chinchilla
{
    /// <summary>
    /// The model factory is responsible for creating and tracking model references
    /// </summary>
    public interface IModelFactory : IDisposable
    {
        /// <summary>
        /// Creates a model reference
        /// </summary>
        IModelReference CreateModel();

        /// <summary>
        /// Creates a model reference with a tag
        /// </summary>
        IModelReference CreateModel(string tag);
    }
}