﻿using Models.Models;
using Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.IRepositories
{
    internal interface INotificationRepository:IBaseRepositories<Notification>
    {
    }
}

namespace Models
{
    public interface INotificationRepository
    {
    }
}