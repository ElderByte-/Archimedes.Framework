namespace Archimedes.Framework.ContextEnvironment.Properties
{
    public interface IPropertySource
    {
        /// <summary>
        /// Load the properties from this source
        /// </summary>
        /// <returns></returns>
        PropertyStore Load();
    }
}
