namespace ABAValidatorAPI.Engine.Rules
{
    /// <summary>
    /// Guaranties that the innter rule will be executed only if the parent rule is successfull executed.
    /// </summary>
    public interface IFieldCompositRule
    {
        /// <summary>
        /// Dependent rules.
        /// If parent rule is not matched the Internal will not be checked.
        /// </summary>
        public IList<IFieldRule> InteralRules { get; }
    }
}
