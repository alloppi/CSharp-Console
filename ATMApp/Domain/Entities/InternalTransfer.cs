using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.Domain.Entities
{
    public class InternalTransfer
    {
        public decimal TransferAmount { get; set; }
        public long ReceiptBankAccountNumber { get; set; }
        public string ReceiptBankAccountName { get; set; }
    }
}
