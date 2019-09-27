namespace Gsafety.PTMS.Media.Utility
{
    public class ResettableParameters<TParameters>
        where TParameters : class, new()
    {
        readonly object _lock = new object();
        TParameters _parameters;

        public TParameters Parameters
        {
            get
            {
                lock (_lock)
                {
                    if (null == _parameters)
                        _parameters = new TParameters();

                    return _parameters;
                }
            }

            set { lock (_lock) _parameters = value; }
        }
    }
}
