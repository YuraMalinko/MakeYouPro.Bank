﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeYouPro.Bourse.Rates.Dal.Interfaces;
using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal.Repository
{
    public class RurRepository: IRubRepository
    {
        private static Context _context;
        public RurRepository()
        {
            _context = new Context();
        }
        public RUBDto AddRur(RUBDto rur)
        {
            _context.MainRUR.Add(rur);
            _context.SaveChanges();

            return rur;
        }
    }
}
