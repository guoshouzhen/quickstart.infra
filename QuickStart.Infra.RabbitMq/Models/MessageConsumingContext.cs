using RabbitMQ.Client.Events;

namespace QuickStart.Infra.RabbitMq.Models
{
    /// <summary>
    /// Message Consuming context model.
    /// </summary>
    public class MessageConsumingContext
    {
        /// <summary>
        /// Source received message.
        /// </summary>
        public BasicDeliverEventArgs Message { get; private set; }
        /// <summary>
        /// Message Acknowledged action.
        /// </summary>
        private readonly Action<BasicDeliverEventArgs> _ackAction;
        /// <summary>
        /// Message rejected action.
        /// </summary>
        private readonly Action<BasicDeliverEventArgs, bool> _rejectAction;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ackAction"></param>
        /// <param name="rejectAction"></param>
        public MessageConsumingContext(BasicDeliverEventArgs message, Action<BasicDeliverEventArgs> ackAction, Action<BasicDeliverEventArgs, bool> rejectAction)
        {
            Message = message;
            _ackAction = ackAction;
            _rejectAction = rejectAction;
        }

        /// <summary>
        /// Acknowledge current message.
        /// </summary>
        public void AcknowledgeMessage() 
        {
            _ackAction(Message);
        }

        /// <summary>
        /// Reject current message.
        /// </summary>
        /// <param name="isReQueue">If deliver the rejected message into queue, so can be consumed by other consumer.</param>
        public void RejectMessage(bool isReQueue = false) 
        {
            _rejectAction(Message, isReQueue);
        }
    }
}
