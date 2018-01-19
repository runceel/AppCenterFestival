using Microsoft.AppCenter.Analytics;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCenterFestival.Logger
{
    public interface IAppCenterLogger
    {
        void TrackEvent(string name, params (string key, string value)[] properties);
    }

    public class AppCenterLogger : IAppCenterLogger
    {
        public void TrackEvent(string name, params (string key, string value)[] properties)
        {
            Analytics.TrackEvent(name, properties.ToDictionary(x => x.key, x => x.value));
        }
    }
}
