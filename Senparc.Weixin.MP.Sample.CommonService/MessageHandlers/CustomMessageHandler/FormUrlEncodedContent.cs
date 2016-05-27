using System.Collections.Generic;

namespace Senparc.Weixin.MP.Sample.CommonService.CustomMessageHandler
{
    internal class FormUrlEncodedContent
    {
        private List<KeyValuePair<string, string>> values;

        public FormUrlEncodedContent(List<KeyValuePair<string, string>> values)
        {
            this.values = values;
        }
    }
}