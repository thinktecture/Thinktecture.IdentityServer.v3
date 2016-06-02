﻿/*
 * Copyright 2014, 2015 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Logging;
using IdentityServer3.Core.Services;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace IdentityServer3.Core.Configuration.Hosting
{
    internal class LogProviderExceptionLogger : IExceptionLogger
    {
        private readonly static ILog Logger = LogProvider.GetCurrentClassLogger();

        public async Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            if (context.Request != null)
            {
                var mesage = string.Format("Unhandled exception accessing: {0}", context.Request.RequestUri.AbsolutePath);
                Logger.ErrorException(mesage, context.Exception);
            }
            else
            {
                Logger.ErrorException("Unhandled exception", context.Exception);
            }

            var env = context.Request.GetOwinEnvironment();
            var events = env.ResolveDependency<IEventService>();
            await events.RaiseUnhandledExceptionEventAsync(context.Exception);
        }
    }
}