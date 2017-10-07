using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.IoC
{
    /// <summary>
    /// Implement this interface at Service and Repository for registering them into IoC container as Transient.
    /// If you do not implement any of three scope interface Ncc will register your repository or service as Transient by default.
    /// </summary>
    public interface ITransient
    {
    }
}
