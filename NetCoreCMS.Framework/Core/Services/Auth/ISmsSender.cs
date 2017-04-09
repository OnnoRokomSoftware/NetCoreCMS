/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using System.Threading.Tasks;

namespace NetCoreCMS.Framework.Core.Services.Auth
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
