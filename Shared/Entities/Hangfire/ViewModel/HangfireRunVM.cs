using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Hangfire.ViewModel
{
    public class HangfireRunVM
    {
        public string Job { get; set; }

        public string Fields { get; set; }
    }

    public class HangfireContractAboutToExpireRequest
    {
        public DateTime? ExpireDate { get; set; }
    }

    public class HangfireContractExpireRequest
    {
        public DateTime? ExpireDate { get; set; }
    }


    public class HangfireNewDiagnosisAlertRequest
    {
        public DateTime? AlertDate { get; set; }
        public int MonthsBuffer { get; set; }
        public List<int> CompaniesIds { get; set; }
    }

    public class HangfireDeleteExpiredAccountsRequest
    {
        public DateTime? ExpireDate { get; set; }

        public List<int> CompaniesIds { get; set; }

        public List<int> DocIdsToDelete { get; set; }
    }
}
