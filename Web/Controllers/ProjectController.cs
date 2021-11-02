using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApiService _apiService;

        public ProjectController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index(int id)
        {
            ProjectPageDetails projectPage = new ProjectPageDetails();

            ProjectModel project = _apiService.CallGetProject(id).Result;
            projectPage.Project = project;
            projectPage.AllEmployees = _apiService.CallGetEmployeesNames().Result;

            return View(projectPage);
        }
    }
}