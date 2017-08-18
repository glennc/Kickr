using System;
using System.Collections.Generic;
using System.Text;

namespace Kickr
{
    public interface IUriKeyGenerator
    {
        string GenerateKey(Uri uri);
    }
}
