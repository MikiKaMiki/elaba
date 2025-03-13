

namespace ABAValidatorAPI.Engine
{
    public class AbaValidationContext
    {
        public int DetailRecordCount { get; set; }


        public int DetailRecordCreditTotal { get; set; }


        public int DetailRecordDebitTotal { get; set; }

        public DetailRecordValidationContext? CurrentDetailRecordValidationContext { get; private set; }

        internal void InjectNewDetailRecordValidationContext()
        {
            CurrentDetailRecordValidationContext = new();
        }

        internal void EjectDetailRecordValidationContext()
        {
            CurrentDetailRecordValidationContext = null;
        }
    }
}
