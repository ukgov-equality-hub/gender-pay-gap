using GenderPayGap.WebUI.Models.CaseStudies;
using Microsoft.AspNetCore.Mvc;

namespace GenderPayGap.WebUI.Controllers;

public class CaseStudiesController : Controller
{

    [HttpGet("case-studies")]
    public IActionResult CaseStudies()
    {
        var viewModel = new CaseStudiesViewModel();
        
        return View("CaseStudies", viewModel);
    }

    [HttpGet("case-studies/{caseStudyId}")]
    public IActionResult CaseStudy(string caseStudyId)
    {
        return View("CaseStudy");
    }
}
