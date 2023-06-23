﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.CRM.Core.Configurations.ISettings
{
    public interface ICommissionSettings
    {
        decimal WithdrawCommissionPercentage { get; set; } 

        decimal DepositCommissionPercentage { get; set; }
    }
}