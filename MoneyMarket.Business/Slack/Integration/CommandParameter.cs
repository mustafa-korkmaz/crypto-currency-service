using System;
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

        /// <summary>
        /// dbo.responses.depth property
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// tp validate decimal or integer parameters
        /// </summary>
        public bool IsNumber { get; set; }
    }
}
