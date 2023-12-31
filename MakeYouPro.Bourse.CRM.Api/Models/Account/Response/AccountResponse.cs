﻿using MakeYouPro.Bourse.CRM.Api.Models.Lead.Response;
using MakeYouPro.Bourse.CRM.Core.Enums;

namespace MakeYouPro.Bourse.CRM.Models.Account.Response
{
    public class AccountResponse
    {
        public int Id { get; set; }

        public LeadResponseMinInfo Lead { get; set; }

        //  public LeadResponseBase Lead { get; set; }

        public DateTime DateCreate { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public AccountStatusEnum Status { get; set; }

        public string? Comment { get; set; }
    }
}
