﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public interface IUnitOfWork
    {
        void Save();
    }
}
