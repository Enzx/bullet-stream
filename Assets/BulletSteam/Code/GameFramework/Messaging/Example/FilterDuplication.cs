namespace BulletSteam.GameFramework.Messaging.Example
{
    public class FilterDuplication<TMessage> : Filter<TMessage>
    {
        TMessage previousMessage;

        public override bool Apply(TMessage message)
        {
            var ret = previousMessage != null && false == previousMessage.Equals(message);
            previousMessage = message;
            return ret;
        }
    }
}