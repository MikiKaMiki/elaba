namespace ABAValidatorAPI.Engine.Rules
{
    public abstract class LookupFieldRule<T> : IFieldRule
    {
        public abstract string ErrorMessage { get; }

        public abstract bool IsValid(string line);

        protected abstract IList<T> LookupList { get; }

    }
}
