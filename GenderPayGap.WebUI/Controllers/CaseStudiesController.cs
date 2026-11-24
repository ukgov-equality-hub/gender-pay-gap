using System.Collections.Concurrent;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using GenderPayGap.WebUI.ErrorHandling;
using GenderPayGap.WebUI.Models.CaseStudies;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GenderPayGap.WebUI.Controllers;

public class CaseStudiesController : Controller
{

    private static readonly ConcurrentDictionary<string, CaseStudyViewModel> CaseStudyViewModels = new();
    private static bool CaseStudyViewModelsLoaded = false;
    
    public CaseStudiesController(IWebHostEnvironment webHostEnvironment)
    {
        ReadCaseStudiesIfNeeded(webHostEnvironment.ContentRootPath);
    }
    
    [HttpGet("case-studies")]
    public IActionResult CaseStudies()
    {
        var viewModel = new CaseStudiesViewModel
        {
            CaseStudies = CaseStudyViewModels.Values.ToList()
        };

        return View("CaseStudies", viewModel);
    }
    
    [HttpGet("case-studies/{caseStudyId}")]
    public IActionResult CaseStudy(string caseStudyId)
    {
        if (!CaseStudyViewModels.TryGetValue(caseStudyId, out CaseStudyViewModel viewModel))
        {
            throw new PageNotFoundException();
        }
        
        return View("CaseStudy", viewModel);
    }

    
    private void ReadCaseStudiesIfNeeded(string contentRootPath)
    {
        if (!CaseStudyViewModelsLoaded)
        {
            string caseStudiesDirectory = Path.Combine(contentRootPath, "Content", "CaseStudies");

            foreach (string filePath in Directory.EnumerateFiles(caseStudiesDirectory, "*.md"))
            {
                CaseStudyViewModel caseStudyViewModel = CreateCaseStudyViewModelFromFile(filePath);
                CaseStudyViewModels.TryAdd(caseStudyViewModel.Slug, caseStudyViewModel);
            }

            CaseStudyViewModelsLoaded = true;
        }
    }

    private static CaseStudyViewModel CreateCaseStudyViewModelFromFile(string filePath)
    {
        string caseStudyMarkdownText = System.IO.File.ReadAllText(filePath).Replace("\r\n", "\n");
        if (!caseStudyMarkdownText.StartsWith("---\n", StringComparison.Ordinal))
        {
            throw new PageNotFoundException();
        }

        int closingDelimiter = caseStudyMarkdownText.IndexOf("\n---\n", 4, StringComparison.Ordinal);
        if (closingDelimiter < 0)
        {
            throw new PageNotFoundException();
        }

        string caseStudyYamlFrontMatterText = caseStudyMarkdownText[4..(closingDelimiter-1)];
        string caseStudyMainContentText = caseStudyMarkdownText[(closingDelimiter + 5)..];

        IDeserializer yamlDeserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        CaseStudyViewModel viewModel = yamlDeserializer.Deserialize<CaseStudyViewModel>(caseStudyYamlFrontMatterText);
        if (viewModel == null)
        {
            throw new PageNotFoundException();
        }

        MarkdownPipeline pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        string html = Markdown.ToHtml(caseStudyMainContentText, pipeline);
        
        IHtmlDocument document = new HtmlParser().ParseDocument(html);
        AddClassesToHtmlTags(document, "h2", "govuk-heading-l");
        AddClassesToHtmlTags(document, "p", "govuk-body");
        AddClassesToHtmlTags(document, "ul", "govuk-list", "govuk-list--bullet");
        AddClassesToHtmlTags(document, "a", "govuk-link");
        AddClassesToHtmlTags(document, "table", "govuk-table");
        AddClassesToHtmlTags(document, "thead", "govuk-table__head");
        AddClassesToHtmlTags(document, "tbody", "govuk-table__body");
        AddClassesToHtmlTags(document, "tr", "govuk-table__row");
        AddClassesToHtmlTags(document, "th", "govuk-table__header");
        AddClassesToHtmlTags(document, "td", "govuk-table__cell");
        ReplaceBlockquoteWithGovUkInsetText(document);
        viewModel.Html = document.Body.InnerHtml;
        
        viewModel.Slug = Path.GetFileNameWithoutExtension(filePath);
        
        return viewModel;
    }

    private static void AddClassesToHtmlTags(IHtmlDocument document, string selector, params string[] classes)
    {
        foreach (IElement paragraph in document.QuerySelectorAll(selector))
        {
            paragraph.ClassList.Add(classes);
        }
    }
    
    private static void ReplaceBlockquoteWithGovUkInsetText(IHtmlDocument document)
    {
        foreach (IElement blockquote in document.QuerySelectorAll("blockquote").ToList())
        {
            IElement paragraph = blockquote.QuerySelector("p");
            if (paragraph != null)
            {
                IElement div = document.CreateElement("div");
                div.ClassList.Add("govuk-inset-text");
                div.InnerHtml = paragraph.InnerHtml;
                blockquote.Parent?.ReplaceChild(div, blockquote);
            }
        }
    }

}
