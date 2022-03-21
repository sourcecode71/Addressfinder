﻿using Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMG.Data.Repository.PayInvoice
{
    public interface IInvoiceRepository
    {
        Task<List<InvoiceDTO>> GetAllInvoice();
        Task<List<InvoiceDTO>> GetPendingInvoice();
        Task<List<InvoiceDTO>> GetWorkOrderByInvoice();
        Task<List<InvoiceDTO>> GetProjectByInvoice();
        Task<bool> SaveInvoice(InvoiceDTO dTO);
        Task<bool> SavePayBill(PaymentDto dTO);
        Task<List<PaymentDto>> GetAllPayment();
    }
}
