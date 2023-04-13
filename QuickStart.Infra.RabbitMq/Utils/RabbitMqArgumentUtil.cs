using QuickStart.Infra.RabbitMq.ConfigOptions;
using QuickStart.Infra.RabbitMq.Enums;

namespace QuickStart.Infra.RabbitMq.Utils
{
    internal sealed class RabbitMqArgumentUtil
    {
        /// <summary>
        /// Get exchange or queue additional arguments from configuration.
        /// </summary>
        /// <param name="lstArgumentOptions"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IDictionary<string, object>? ConvertToRabbitMqArguments(List<ArgumentOptions> lstArgumentOptions) 
        {
            if (lstArgumentOptions == null || lstArgumentOptions.Count == 0) 
            {
                return null;
            }

            var arguments = new Dictionary<string, object>();
            foreach (var argumentOpts in lstArgumentOptions) 
            {
                if (argumentOpts != null && !string.IsNullOrWhiteSpace(argumentOpts.Key) && argumentOpts.Value != null) 
                {
                    var val = argumentOpts.Value;
                    if (argumentOpts.Type == ArgumentTypeEnum.INT)
                    {
                        if (int.TryParse(argumentOpts.Value.ToString(), out int res) == false) 
                        {
                            throw new ArgumentException("Argument must be a integer number.", nameof(argumentOpts.Value));
                        }
                        val = res;
                    }
                    else if (argumentOpts.Type == ArgumentTypeEnum.LONG)
                    {
                        if (long.TryParse(argumentOpts.Value.ToString(), out long res) == false)
                        {
                            throw new ArgumentException("Argument must be a long integer number.", nameof(argumentOpts.Value));
                        }
                        val = res;
                    }
                    arguments[argumentOpts.Key] = val;
                }
            }
            return arguments;
        }
    }
}
