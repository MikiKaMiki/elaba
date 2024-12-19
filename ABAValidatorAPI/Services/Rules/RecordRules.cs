using System.Text.RegularExpressions;

namespace ABAValidatorAPI.Services.Rules
{
    public partial class RecordRules
    {
        [GeneratedRegex(@"^.{120}$")]
        private static partial Regex Line120();

        public KeyValuePair<Regex, string> DiscrptiveChar1 
        {
            get => new (DescriptiveChar1(), "Char 1, Size 1, Record type 0, Must be '0'"); 
        }

        public Dictionary<Regex, string> DescriptiveRules { get; } = new()
        {
            {Line120()               , "An ABA record is exactly 120 characters long (excluding new line characters)" },
            
            {DescriptiveChar1()      , "Char 1, Size 1, Record type 0, Must be '0'" },
            {DescriptiveChar2_18()   , "Char 2-18, Size 17, Blank, Must be filled" },
            {DescriptiveChar19_20()  , "Char 19-20, Size 2, Reel Sequence Number, Must be numeric commencing at 01. Right justified. Zero filled." },
            //todo: complete all rules
            //todo: split compex rules,
        };

        [GeneratedRegex(@"^0.*$")]
        private static partial Regex DescriptiveChar1();

        [GeneratedRegex(@"^.[ ]{17}.*$")]
        private static partial Regex DescriptiveChar2_18();

        [GeneratedRegex(@"^.{18}01.*$")]
        private static partial Regex DescriptiveChar19_20();


        public Dictionary<Regex, string> DetailRules { get; } = new()
        {
            {Line120()          , @"An ABA record is exactly 120 characters long (excluding new line characters)" },

            {DetailChar1()      , @"Char 1, Size 1, Record type 1, Must be '1'" },
            {DetailChar2_8()    , @"Char 2-8; Size 7; Bank/State/Branch Number; Must be numeric with hyphen in character position 5. Character positions 2 and 3 must equal valid Financial Institution number. Character position 4 must equal a valid state number (0-9). For credits to Employee Benefits Card accounts, field must always contain BSB 032-898" },
            {DetailChar9_17()   , @"Char 9-17; Size 9; Account number to be credited/debited; Numeric, hyphens and blanks only are valid. Must not contain all blanks (unless a credit card transaction) or zeros. Leading zeros which are part of a valid account number must be shown, e.g. 00-1234.
Where account number exceeds nine characters, edit out hyphens. Right justified, blank filled. For credits to Employee Benefits Card accounts, Account Number field must always be 999999" },
            //todo: complete all rules
            //todo: split compex rules
        };


        [GeneratedRegex(@"^1.*$")]
        public static partial Regex DetailChar1();

        /// <summary>
        /// todo: Non-Completed
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"^.\d{3}-\d{3}.*$")]
        private static partial Regex DetailChar2_8();

        /// <summary>
        /// todo: Non-Completed
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"^.{8}[0-9\- ]{9}.*$")]
        private static partial Regex DetailChar9_17();

        public Dictionary<Regex, string> TotalRules { get; } = new()
        {
            {Line120()         , @"An ABA record is exactly 120 characters long (excluding new line characters)" },

            {TotalChar1()      , @"Char 1, Size 1, Record type 7, Must be '7'" },
            {TotalChar2_8()    , @"Char 2-8; Size 7; BSB Format Filler; Must be '999-999'" },
            {TotalChar9_20()   , @"Char 9-20; Size 12; Blank; Must be blank filled" },
            //todo: complete all rules
            //todo: split compex rules
        };


        [GeneratedRegex(@"^7.*$")]
        private static partial Regex TotalChar1();

        /// <summary>
        /// todo: Non-Completed
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"^.999-999.*$")]
        private static partial Regex TotalChar2_8();

        /// <summary>
        /// todo: Non-Completed
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"^.{8}[0-9\- ]{9}.*$")]
        private static partial Regex TotalChar9_20();



        public Dictionary<Regex, string> GetRules(char recordType)
        {
            switch (recordType)
            {
                case '0':
                    {
                        return DescriptiveRules;
                    }
                case '1':
                    {
                        return DetailRules;
                    }
                case '7':
                    {
                        return TotalRules;
                    }
                default:
                    { 
                        throw new ArgumentOutOfRangeException(); 
                    }
            }
        }
    }
}
