namespace Archimedes.Framework.ContextEnvironment.Properties
{
    public interface IPropertySource
    {
        /// <summary>
        /// Load the properties from this source
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PropertySourceException">Thrown when there was a problem loading this property source</exception>
        PropertyStore Load();
    }
}
