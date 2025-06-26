namespace BulletSteam.GameFramework.Messaging
{
    public class CompositeFilter<TMessage, TFilter> : Filter<TMessage> where TFilter : Filter<TMessage>
    {
        private readonly TFilter[] _filters;

        public CompositeFilter(params TFilter[] filters)
        {
            _filters = filters;
        }

        public override bool Apply(TMessage message)
        {
            for (int i = 0; i < _filters.Length; i++)
            {
                if (_filters[i].Apply(message) == false) return false;
            }

            return true;
        }
    }
}