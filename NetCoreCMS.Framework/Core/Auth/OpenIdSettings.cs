using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Auth
{
    [Serializable]
    public class OpenIdSettings
    {
        public string FacebookAppId { get; set; }
        public string FacebookAppSecret { get; set; }
        public string GoogleClientId { get; set; }
        public string GoogleClientSecret { get; set; }

        public string MicrosoftAppId { get; set; }
        public string MicrosoftAppPassword { get; set; }

        public string TwitterConsumerKey { get; set; }
        public string TwitterCustomerSecret { get; set; }
    }
}
