using Archimedes.Patterns.CommandLine;

namespace Archimedes.Framework.ContextEnvironment.Properties
{
    public class CommandLinePropertySource : IPropertySource
    {
        private readonly string[] _commandLineArgs;

        public CommandLinePropertySource(string[] commandLineArgs)
        {
            _commandLineArgs = commandLineArgs;
        }

        public PropertyStore Load()
        {
            var cmdProps = new PropertyStore();

            if (_commandLineArgs != null && _commandLineArgs.Length > 0)
            {
                var argsCmd = CommandLineParser.ParseCommandLineArgs(_commandLineArgs);
                cmdProps.Merge(argsCmd.ToParameterMap());
            }
            return cmdProps;
        }
    }
}
