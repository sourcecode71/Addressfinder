﻿using Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMG.Data.Repository.Projects
{
    public interface IProjects
    {
        string  GetProjectNumber(ProjectDto projectDto);
        string GetPmBudgetNumber(ProjectApprovalDto projectDto);
        Task<List<ProjectDto>> GetProjectBySearch(string SearchTag);
        Task<bool> SaveProjectApproval(ProjectApprovalDto approvalDto);
        Task<List<ProjectApprovalDto>> LoadProjectBudgetAcitivies(string projectName);
    }
}
