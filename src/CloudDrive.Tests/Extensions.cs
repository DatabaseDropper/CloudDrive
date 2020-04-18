using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace CloudDrive.Tests
{
    public static class Extensions
    {
        public static StringContent ToPOSTableJSON(this object o)
        {
            return new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
        }
    }
}
