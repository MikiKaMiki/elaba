namespace ABAValidatorAPI.Engine.Rules
{
    public class DescriptiveField4RuleSet : FieldRuleSetBase
    {
        protected override short FieldNumber => 4;

        public DescriptiveField4RuleSet()
        {
            _rules.Add(new DescriptiveField4Rule1());
        }

        //internal partial class DescriptiveField4Rule1 : RegexFieldRule
        //{
        //    protected override Regex Regex => DescriptiveChar21_23();

        //    public override string ErrorMessage => "Char 2-18, Size 17, Blank, Must be filled";

        //    public override bool IsValid(string line)
        //    {
        //        return Regex.IsMatch(line);
        //    }

        //    [GeneratedRegex(@"^.{20}[A-Z,&]{3}.*$")]
        //    private static partial Regex DescriptiveChar21_23();
        //}

        internal partial class DescriptiveField4Rule1 : LookupFieldRule<string>
        {
            public override string ErrorMessage => "Char 21-23; Size 3; Must be approved Financial Institution abbreviation";

            public override bool IsValid(string line)
            {
                var value = line.Substring(21, 3);

                return LookupList.Contains(value);
            }

            protected override IList<string> LookupList { get; } = [
                "ABA", "ABC", "ADC", "ADL", "ADV", "ADY", "ALX", "AMP", "ANZ", "APO",
                "ARA", "ASL", "AVE", "BAE", "BAL", "BAR", "BAU", "BBL", "BCA", "BCC",
                "BML", "BNO", "BNP", "BOC", "BOM", "BOT", "BPS", "BQL", "BSA", "BTA",
                "BWA", "BYB", "CAL", "CAP", "CBA", "CBL", "CCB", "CFC", "CLS", "CMA",
                "CMB", "CNA", "COM", "CRU", "CST", "CTI", "CUA", "CUS", "DBA", "DBL",
                "DBS", "ENC", "ETX", "FPX", "GBS", "GCB", "GNI", "GPA", "GSB", "GTW",
                "HAY", "HBA", "HBS", "HCC", "HIC", "HOM", "HPC", "HSB", "HUM", "IBA",
                "IBK", "ICB", "IMB", "ING", "INV", "JUD", "KEB", "LBA", "LCH", "MBL",
                "MCB", "MEB", "MET", "MMB", "MMP", "MSL", "NAB", "NEW", "NTA", "OCB",
                "ONE", "PCU", "PIB", "PNB", "PPB", "QCB", "QTM", "RAB", "RBA", "RBB",
                "RBC", "RCU", "ROK", "SBI", "SCB", "SCU", "SEL", "SGP", "SKY", "SMB",
                "SNX", "SPL", "SSB", "STG", "STH", "SUN", "T&C", "TBB", "TBT", "TCU",
                "UBS", "UFS", "UOB", "WBC", "WCU", "WSE", "YOU"
            ];
        }
    }
}
