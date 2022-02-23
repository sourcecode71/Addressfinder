﻿using Application.DTOs;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMG.Data.Repository.Projects
{
    public class WorkOrderRepository : IWorkOrderRepository
    {
        private readonly DataContext _context;
        public WorkOrderRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public IQueryable<WorkOrderDTO> LoadAllWorkOrders()
        {
            try
            {
                IQueryable<WorkOrderDTO> wrkList = (from w in _context.WorkOrder
                                            join p in _context.Projects on w.ProjectId equals p.Id
                                            where p.IsBudgetApproved == true
                                            orderby w.SetDate descending
                                            select new WorkOrderDTO
                                            {
                                                Id = w.Id,
                                                ProjectId = w.ProjectId,
                                                ClinetName = p.Client,
                                                ProjectName = p.Name,
                                                ApprovedBudget = w.ApprovedBudget,
                                                Comments = w.Comments,
                                                OTDescription = w.OTDescription,
                                                ProjectNo = w.ProjectNo,
                                                ProjectYear = p.Year,
                                                WorkOrderNo = w.WorkOrderNo,
                                                ProjectBudget = p.Budget,
                                                ApprovedDateStr = w.ApprovalDate.ToString("MM/dd/yyyy")

                                            }).Take(50);

                return wrkList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> SaveWorkOrder(WorkOrderDTO dTO)
        {
            try
            {
                string OTNo = GetProjectNumber(dTO);

                var pmWorkOrder = new WorkOrder
                {
                    Id = Guid.NewGuid(),
                    WorkOrderNo = OTNo,
                    ProjectNo = dTO.ProjectNo,
                    ProjectId = dTO.ProjectId,
                    ApprovedBudget = dTO.ApprovedBudget,
                    ApprovalDate = DateTime.Now,
                    Comments = dTO.Comments,
                    OTDescription = dTO.OTDescription
                };

                _context.Add(pmWorkOrder);

                var Status = await _context.SaveChangesAsync();

                return Status == 1;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<bool> UpdateWorkOrder(WorkOrderDTO dTO)
        {

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                    WorkOrder workOrder = await _context.WorkOrder.FirstOrDefaultAsync(p => p.Id == dTO.WorkOrderId);

                    HisWorkOrder hisWork = new HisWorkOrder
                        {
                            Id = Guid.NewGuid(),
                            ApprovalDate = workOrder.ApprovalDate,
                            ApprovedBudget = workOrder.ApprovedBudget,
                            Comments = workOrder.Comments,
                            IsDeleted = workOrder.IsDeleted,
                            OTDescription = workOrder.OTDescription,
                            ProjectId = workOrder.ProjectId,
                            ProjectNo = workOrder.ProjectNo,
                            SetDate = workOrder.SetDate,
                            SetUser = workOrder.SetUser,
                            WorkOrderId = workOrder.Id,
                            WorkOrderNo = workOrder.WorkOrderNo,
                        };

                        _context.HisWorkOrder.Add(hisWork);

                        workOrder.UpdateDate = DateTime.Now;
                        workOrder.UpdateUser = "admin";
                        workOrder.Comments = dTO.Comments;
                        workOrder.ApprovedBudget = dTO.ApprovedBudget;
                        workOrder.ProjectId = dTO.ProjectId;
                        workOrder.ProjectNo = dTO.ProjectNo;

                       int state = await _context.SaveChangesAsync();
                       await transaction.CommitAsync();

                        return state == 1;

                    }
                    catch (Exception ex)
                    {
                       await transaction.RollbackAsync();
                        throw ex;
                    }
                }
        }

        private string GetProjectNumber(WorkOrderDTO dTO)
        {
            var PmOTCount = _context.WorkOrder.Where(P => P.ProjectNo == dTO.ProjectNo).Count() + 1;
            string ProjectNumber = string.Format("{0}{1}{2}", dTO.ProjectNo,"OT", PmOTCount.ToString("00"));
            return ProjectNumber;
        }
    }


 
}
