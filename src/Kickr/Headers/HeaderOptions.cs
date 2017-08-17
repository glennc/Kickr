using Microsoft.AspNetCore.Http;
using System;
namespace Kickr
{
    public class HeaderOptions
    {
        public IHeaderDictionary Headers { get; set; }

        public HeaderOptions()
        {
            Headers = new HeaderDictionary();
        }
    }
}
