namespace BulletSteam.GameFramework.Messaging.Example
{
    public class FilterEvenNumbers : Filter<TextMessage>
    {
        public override bool Apply(TextMessage textMessage)
        { 
            int x = int.Parse((string)textMessage.Text);
            return (x & 1) == 1;
        }
    }
}