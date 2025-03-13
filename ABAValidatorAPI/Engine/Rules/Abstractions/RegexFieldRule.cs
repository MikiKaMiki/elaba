using System.Text.RegularExpressions;

namespace ABAValidatorAPI.Engine.Rules
{
    public abstract class RegexFieldRule : IFieldRule
    {
        protected abstract Regex Regex { get; }

        public abstract string ErrorMessage { get; }

        public abstract bool IsValid(string line);
    }
}
