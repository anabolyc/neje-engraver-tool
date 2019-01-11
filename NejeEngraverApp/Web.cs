using System.IO;
using System.Net;
using System.Text;

namespace NejeEngraverApp
{
    internal class Web
    {
        public static string getWebSource(string url)
        {
            string result;
            try
            {
                Stream stream = new WebClient().OpenRead(url);
                string arg_27_0 = new StreamReader(stream, Encoding.GetEncoding("utf-8")).ReadToEnd();
                stream.Close();
                result = arg_27_0;
            }
            catch
            {
                result = "";
            }
            return result;
        }
    }
}
