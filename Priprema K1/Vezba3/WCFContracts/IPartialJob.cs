﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFContracts
{
    [ServiceContract]
    public interface IPartialJob
    {
        [OperationContract]
        int DoPartialCalculus(int from,int too);
    }
}
