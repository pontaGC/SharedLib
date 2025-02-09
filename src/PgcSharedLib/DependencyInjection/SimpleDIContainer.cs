using System.Collections.Concurrent;
using System.Diagnostics;
using PgcSharedLib.Extensions;
using SimpleInjector;

namespace PgcSharedLib.DependencyInjection
{
    /// <summary>
    /// IoC Container using a SimpleInjector library.
    /// SimpleInjector: https://simpleinjector.org/index.html
    /// </summary>
    internal sealed class SimpleDIContainer : IDIContainer
    {
        #region Fields

        private static readonly IReadOnlyDictionary<InstanceLifeStyle, Lifestyle> LifeStyles =
            new ConcurrentDictionary<InstanceLifeStyle, Lifestyle>(
            [
                new KeyValuePair<InstanceLifeStyle, Lifestyle>(InstanceLifeStyle.Transient, Lifestyle.Transient),
                new KeyValuePair<InstanceLifeStyle, Lifestyle>(InstanceLifeStyle.Singleton, Lifestyle.Singleton),
            ]);

        // Simple Injector's Container is designed to work in multi-threaded applications
        private readonly Container container = new Container();

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers a pair of the interface type and the implementation type of interface.
        /// </summary>
        /// <typeparam name="TInterface">The interface or base type that can be used to retrieve the instances.</typeparam>
        /// <typeparam name="TImplementation">The implementation type of <c>TInterface</c>.</typeparam>
        /// <param name="lifeStyle">The life style of a registered instance. Default value is transient.</param>
        public void Register<TInterface, TImplementation>(InstanceLifeStyle lifeStyle = InstanceLifeStyle.Transient)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            this.container.Register<TInterface, TImplementation>(LifeStyles[lifeStyle]);
        }

        /// <summary>
        /// Registers a new instances of the implementation type which life style is transient.
        /// </summary>
        /// <typeparam name="TImplementation">The implementation type.</typeparam>
        public void Register<TImplementation>()
            where TImplementation : class
        {
            this.container.Register<TImplementation>();
        }

        /// <summary>
        /// Registers an implementation type to a collection of the interface.
        /// </summary>
        /// <typeparam name="TInterface">The interface or base type that can be used to retrieve the instances.</typeparam>
        /// <typeparam name="TImplementation">The implementation type of <c>TInterface</c>.</typeparam>
        /// <param name="lifeStyle">The life style of a registered instance. Default value is transient.</param>
        public void RegisterAll<TInterface, TImplementation>(InstanceLifeStyle lifeStyle = InstanceLifeStyle.Transient)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            this.container.Collection.Append<TInterface, TImplementation>(LifeStyles[lifeStyle]);
        }

        /// <summary>
        /// Registers a single instance that will be returned when an instance of type.
        /// </summary>
        /// <typeparam name="TInterface">The interface or base type that can be used to retrieve the instances.</typeparam>
        /// <param name="instance">The single instance.</param>
        public void RegisterInstance<TInterface>(TInterface instance)
            where TInterface : class
        {
            this.container.RegisterInstance(instance);
        }

        /// <summary>
        /// Registers a single concrete instance that will be returned when the type of it requested.
        /// </summary>
        /// <typeparam name="TService">The interface or base class type of the single concrete instance.</typeparam>
        public void RegisterSingleton<TService>()
            where TService : class
        {
            Debug.Assert(typeof(TService).IsInterfaceOrAbstractBaseClass(), $"{nameof(TService)} must be interface or abstract base class.");

            this.container.RegisterSingleton<TService>();
        }

        /// <inheritdoc />
        public void RegisterSingleton<TService>(Func<TService> serviceInstanceFactory)
            where TService : class
        {
            Debug.Assert(typeof(TService).IsAbstract , $"{nameof(TService)} must be interface or abstract base class.");

            if (serviceInstanceFactory is null)
            {
                throw new ArgumentNullException(nameof(serviceInstanceFactory));
            }

            this.container.RegisterSingleton(serviceInstanceFactory);
        }

        /// <summary>
        /// Resolves an instance that has been registered.
        /// </summary>
        /// <typeparam name="TInterface">The interface or base type that can be used to retrieve the instances.</typeparam>
        /// <returns>An instance of the implementation type has been registered.</returns>
        public TInterface Resolve<TInterface>() where TInterface : class
        {
            return this.container.GetInstance<TInterface>();
        }

        /// <summary>
        /// Gets all registered instances of the interface.
        /// </summary>
        /// <typeparam name="TInterface">The type of the requested interface.</typeparam>
        /// <returns>A sequence of instances of the requested interface</returns>
        public IEnumerable<TInterface> GetAllInstances<TInterface>()
            where TInterface : class
        {
            return this.container.GetAllInstances<TInterface>();
        }

        /// <summary>
        /// Releases all registered instances.
        /// </summary>
        public void Release()
        {
            this.container?.Dispose();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Release();
        }

        /// <summary>
        /// Verifies and diagnoses the registered instances in the container. Please call this method after all instances were registered.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws the invalid registered instance is found.</exception>
        public void Verify()
        {
            this.container.Verify();

            // All registration were guaranteed to be valid when debugging.
            this.container.Options.EnableAutoVerification = false;
            this.EnableAutoVerificationForDebug();
        }

        #endregion

        #region Private Methods

        [Conditional("DEBUG")]
        private void EnableAutoVerificationForDebug()
        {
            this.container.Options.EnableAutoVerification = true;
        }

        #endregion
    }
}
