using System;
using System.Collections.Generic;
using System.Text;

namespace WhiteLagoon.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IVillaRepository Villa { get; }
        void Save();
    }
}
