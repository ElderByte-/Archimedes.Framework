namespace Archimedes.Framework.Test.ContainerTest.Singletons
{
    class ServiceSingletonA
    {
        public ServiceSingletonA()
        {
            SingletonsTest.IncrementInstance();
        }
    }
}
