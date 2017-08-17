using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kickr.Headers
{
    public class CommonHeaderConfigureOptions<TOptions> : IConfigureNamedOptions<TOptions>, IConfigureOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the options.</param>
        /// <param name="action">The action to register.</param>
        public CommonHeaderConfigureOptions(string name, Action<TOptions> action)
        {
            Name = name;
            Action = action;
        }

        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The configuration action.
        /// </summary>
        public Action<TOptions> Action { get; }

        /// <summary>
        /// Invokes the registered configure Action if the name matches.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public virtual void Configure(string name, TOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            // Always run this one.
            //if (Name == null || name == Name)
            //{
                Action?.Invoke(options);
            //}
        }

        public void Configure(TOptions options) => Configure(Options.DefaultName, options);
    }
}
