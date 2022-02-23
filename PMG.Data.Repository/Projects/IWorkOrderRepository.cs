﻿using Application.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMG.Data.Repository.Projects
{
    public interface IWorkOrderRepository
    {
        Task<bool> SaveWorkOrder(WorkOrderDTO orderDTO);
        IQueryable<WorkOrderDTO> LoadAllWorkOrders();
        Task<bool> UpdateWorkOrder(WorkOrderDTO orderDTO);

    }
}
