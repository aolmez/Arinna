﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arinna.Data.Model.Interfaces
{
    public interface IEntity<TIdType>
    {
        TIdType Id { get; set; }
    }
}
