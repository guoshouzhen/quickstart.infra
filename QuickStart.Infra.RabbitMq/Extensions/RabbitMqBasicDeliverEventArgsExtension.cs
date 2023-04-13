using QuickStart.Infra.RabbitMq.Utils;
using RabbitMQ.Client.Events;
using System.Text;

namespace QuickStart.Infra.RabbitMq.Extensions
{
    public static class RabbitMqBasicDeliverEventArgsExtension
    {
        /// <summary>
        /// Get the string message from a BasicDeliverEventArgs instance.
        /// </summary>
        /// <param name="basicDeliverEventArgs"></param>
        /// <returns></returns>
        public static string GetMessage(this BasicDeliverEventArgs basicDeliverEventArgs)
        {
            basicDeliverEventArgs.EnsureNotNull();
            return Encoding.UTF8.GetString(basicDeliverEventArgs.Body.ToArray());
        }

        /// <summary>
        ///  Get the string message from a BasicDeliverEventArgs instance and deserialize with specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="basicDeliverEventArgs"></param>
        /// <returns></returns>
        public static T? GetPayLoad<T>(this BasicDeliverEventArgs basicDeliverEventArgs)
        {
            basicDeliverEventArgs.EnsureNotNull();
            return JsonUtil.Json2Object<T>(Encoding.UTF8.GetString(basicDeliverEventArgs.Body.ToArray()));
        }

        /// <summary>
        /// Ensure channel not null.
        /// </summary>
        /// <param name="basicDeliverEventArgs"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static BasicDeliverEventArgs EnsureNotNull(this BasicDeliverEventArgs basicDeliverEventArgs)
        {
            if (basicDeliverEventArgs == null)
            {
                throw new ArgumentNullException("BasicDeliverEventArgs params must be not null.", nameof(basicDeliverEventArgs));
            }
            return basicDeliverEventArgs;
        }
    }
}
