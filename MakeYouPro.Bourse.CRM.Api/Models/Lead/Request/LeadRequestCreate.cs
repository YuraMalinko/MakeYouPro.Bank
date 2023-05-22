﻿using MakeYouPro.Bank.CRM.Models.Lead.Request;

namespace MakeYouPro.Bourse.CRM.Api.Models.Lead.Request
{
    public class LeadRequestCreate:LeadRequestBase
    {
        public string Email { get; set; }

        public string Citizenship { get; set; }

        public string PassportSeries { get; set; }

        public string PassportNumber { get; set; }

        public string Comment { get; set; }

    }
}
