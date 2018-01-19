using Microsoft.AppCenter.Analytics;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCenterFestival.Logger
{
    public class AppCenterLogger : ILoggerFacade
    {
        public void Log(string message, Category category, Priority priority)
        {
            Analytics.TrackEvent(message, new Dictionary<string, string>
            {
                ["category"] = category.ToString(),
                ["priority"] = priority.ToString(),
            });
        }
    }
}
