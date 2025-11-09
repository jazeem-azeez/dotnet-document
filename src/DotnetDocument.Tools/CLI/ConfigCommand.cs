using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetDocument.Tools.CLI
{
    /// <summary>
    /// The config command class
    /// </summary>
    internal static class ConfigCommand
    {
        /// <summary>
        /// The config description
        /// </summary>
        private const string ConfigDescription = "Prints the tool configuration file.";

        /// <summary>
        /// The default description
        /// </summary>
        private const string DefaultDescription = "Prints the default documentation config";

        /// <summary>
        /// The config file path description
        /// </summary>
        private const string ConfigFilePathDescription =
            "Set the config file path used to define documentation templates.";

        /// <summary>
        /// Creates the handler
        /// </summary>
        /// <param name="handler">The handler</param>
        /// <returns>The config command</returns>
        internal static Command Create(Handler handler)
        {
            var configOption = new Option<string>("--config", ConfigFilePathDescription)
            {
                Aliases = { "-c" }
            };

            var defaultOption = new Option<bool>("--default", DefaultDescription);

            var configCommand = new Command("config", ConfigDescription)
            {
                configOption,
                defaultOption
            };

            configCommand.SetHandler(handler, defaultOption, configOption);

            return configCommand;
        }

        /// <summary>
        /// The handler
        /// </summary>
        internal delegate Task<int> Handler(bool @default, string config,
            CancellationToken cancellationToken);
    }
}
