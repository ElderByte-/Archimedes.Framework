namespace Archimedes.Framework.DI.Config
{
    /// <summary>
    ///
    /// </summary>
    public interface IComponentPostProcessor
    {
        /// <summary>
        /// Apply this post processor to the given new component before component initialisation callbacks.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="componentName"></param>
        /// <returns>The component (or wraped proxy of it) to subsequently use</returns>
        object postProcessBeforeInitialisation(object component, string componentName);

        /// <summary>
        /// Apply this post processor to the given new component after component initialisation callbacks.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="componentName"></param>
        /// <returns>The component (or wraped proxy of it) to subsequently use</returns>
        object postProcessAfterInitialisation(object component, string componentName);

    }
}
