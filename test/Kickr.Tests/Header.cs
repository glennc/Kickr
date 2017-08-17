using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Kickr.Tests
{
    public class Header
    {
        [Fact]
        public void headers_override()
        {
            var services = new ServiceCollection()
                                .AddOptions()
                                .AddHeaders(p => p.AddHeaders(o => o.Headers.Add("Key1", ""))
                                                  .AddHeaders(o => o.Headers.Add("Key2", ""))
                                                  .AddHeaders("http://Uri", o => o.Headers.Add("Key3", "")))
                                .BuildServiceProvider();


            var monitor = services.GetRequiredService<IOptionsMonitor<HeaderOptions>>();
            Assert.Equal(3, monitor.Get("http://Uri").Headers.Count);
            Assert.Equal(2, monitor.CurrentValue.Headers.Count);
        }
    }
}
