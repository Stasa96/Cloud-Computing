using Common;
using StorageHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole
{
    public class InternalJobServerProvider : IFindFilm
    {
        public Film FindFilm(string name)
        {
            Film f = null;
            try
            {
                f = WorkerRole.tableHelper.GetOneFilm(name);
                f.Cnt++;
                WorkerRole.tableHelper.AddOrReplaceFilm(f);
            }
            catch { }

            return f;
        }
    }
}
