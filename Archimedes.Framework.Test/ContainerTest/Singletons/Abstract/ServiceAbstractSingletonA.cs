namespace Archimedes.Framework.Test.ContainerTest.Singletons.Abstract
{
    abstract class ServiceAbstractSingletonA
    {
        public ServiceAbstractSingletonA()
        {
            AbstractSingletonsTest.IncrementInstance();
        }
    }
}
