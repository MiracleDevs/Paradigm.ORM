using System;

namespace Paradigm.ORM.DataExport.Export
{
    public interface IExporter : IDisposable
    {
        void Export();
    }
}