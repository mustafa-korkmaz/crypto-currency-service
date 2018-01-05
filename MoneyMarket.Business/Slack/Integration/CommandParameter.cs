﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyMarket.Business.Slack.Integration
{
    public class CommandParameter
    {
        public IEnumerable<string> ParameterSet { get; set; }

        public string ParameterValue { get; set; }

        public int Depth { get; set; }
    }
}