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
                                            join cm in _context.Company on w.CompanyId equals cm.Id
                                            join cl in _context.Clients on p.ClientId equals cl.Id into gj
                                            from xx in gj.DefaultIfEmpty()
                                            where p.BudgetApprovedStatus != 2
                                            orderby w.SetDate descending
                                            select new WorkOrderDTO
                                            {
                                                Id = w.Id,
                                                ProjectId = w.ProjectId,
                                                ClinetName = xx.Name,
                                                ProjectName = p.Name,
                                                OriginalBudget = w.OriginalBudget,
                                                ApprovedBudget = w.ApprovedBudget,
                                                ConsecutiveWork = w.ConsWork,
                                                Comments = w.Comments,
                                                OTDescription = w.OTDescription,
                                                ProjectNo = w.ProjectNo,
                                                ProjectYear = p.Year,
                                                WorkOrderNo = w.WorkOrderNo,
                                                ProjectBudget = p.Budget,
                                                StartDateStr = w.StartDate.ToString("MM/dd/yyyy"),
                                                EndDateStr = w.EndDate.ToString("MM/dd/yyyy"),
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
                string OTNo = GetWorkOrderNumber(dTO);

                var pmWorkOrder = new WorkOrder
                {
                    Id = Guid.NewGuid(),
                    WorkOrderNo = OTNo,
                    ConsWork = dTO.ConsecutiveWork,
                    ProjectId = dTO.ProjectId,
                    CompanyId = new Guid(dTO.CompanyId),
                    OriginalBudget = dTO.OriginalBudget,
                    StartDate = dTO.StartDate,
                    EndDate = dTO.EndDate,
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
                    WorkOrder workOrder = await _context.WorkOrder.FirstOrDefaultAsync(p => p.Id == new Guid(dTO.WorkOrderId) );

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


        public IQueryable<WorkOrderDTO> GetFilteredWorkOrder(string strOT)
        {
            try
            {
                var wrkODT = from wrk in _context.WorkOrder
                             join prj in _context.Projects on wrk.ProjectId equals prj.Id
                             where wrk.OTDescription.Contains(strOT) 
                             orderby wrk.ApprovalDate descending
                             select new WorkOrderDTO
                             {
                                 Id = wrk.Id,
                                 ApprovedBudget = wrk.ApprovedBudget,
                                 OTDescription = wrk.OTDescription,
                                 ApprovedDate = wrk.ApprovalDate,
                                 ProjectId = wrk.ProjectId,
                                 ProjectNo = wrk.ProjectNo,
                                 WorkOrderNo = wrk.WorkOrderNo,
                                 ProjectName = prj.Name,
                                 ClinetName = prj.Name,
                                 ProjectYear = prj.Year,
                                 Comments = wrk.Comments
                             };
                return wrkODT;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<bool> SaveInvoice(InvoiceDTO invDTO)
        {
            try
            {
                var invData = new Invoice
                {
                    Id = Guid.NewGuid(),
                    WorkOrderId = new Guid(invDTO.WorkOrderId),
                    WorkOrderNo = invDTO.WorkNo,
                    ProjectId = new Guid(invDTO.ProjectId),
                    PartialBill = invDTO.PartialBill,
                    InvoiceBill = invDTO.InvoiceBill,
                    InvoiceDate = invDTO.InvoiceDate,
                    InvoiceNumber = invDTO.InvoiceNumber,
                    Remarks = invDTO.Remarks,
                    SetUser = invDTO.SetUser,
                    SetDate = DateTime.Now
                };

                _context.Invoice.Add(invData);

                var State = await _context.SaveChangesAsync();

                return State==1;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<InvoiceDTO> GetAllInvoices()
        {
            var invDTO = from inv in _context.Invoice
                         join wrk in _context.WorkOrder on inv.WorkOrderId equals wrk.Id
                         join prj in _context.Projects on wrk.ProjectId equals prj.Id
                         orderby inv.Id descending
                         select new InvoiceDTO
                         {
                             Id = inv.Id.ToString(),
                             WorkOrderId = inv.WorkOrderId.ToString(),
                             WorkNo = inv.WorkOrderNo,
                             OTName = wrk.OTDescription,
                             ProjectName = prj.Name,
                             PartialBill = inv.PartialBill,
                             InvoiceBill = inv.InvoiceBill,
                             InvoiceNumber = inv.InvoiceNumber,
                             InvoiceDate = inv.InvoiceDate,
                             Remarks = inv.Remarks
                         };
            return invDTO;

        }

        private string GetWorkOrderNumber(WorkOrderDTO dTO)
        {
            DateTime CurrentDate = DateTime.Now;

            string Day = CurrentDate.Day.ToString("00");
            string Month = CurrentDate.Month.ToString("00");
            string Year = CurrentDate.Year.ToString();
            string ProjectNumber = string.Format("{0}{1}{2}", Day, Month, Year);

            var PmOTCount = _context.WorkOrder.Where(P => P.ProjectNo == dTO.ProjectNo).Count() + 1;
            
            string workOrderNo = string.Format("{0}{1}{2}", ProjectNumber, "OT", PmOTCount.ToString("00"));
            return workOrderNo;
        }

       
    }


 
}
