using System;
using System.Collections.Generic;
using System.Text;

namespace Kickr
{
    public class FuncUriKeyGenerator : IUriKeyGenerator
    {
        private Func<Uri,string> _generator;

        public FuncUriKeyGenerator(Func<Uri, string> generator)
        {
            _generator = generator;
        }

        public string GenerateKey(Uri uri)
        {
            return _generator(uri);
        }
    }

    public class DefaultUriKeyGenerator : IUriKeyGenerator
    {
        public string GenerateKey(Uri uri)
        {
            return uri.ToString();
        }
    }
}
