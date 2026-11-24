using GenderPayGap.WebUI.Tests.TestHelpers;

namespace GenderPayGap.WebUI.Tests.CodeQualityTests
{
    [TestFixture]
    public class DoNotAllowHtmlDotRawTests
    {

        [Test]
        public void DoNotAllowHtmlDotRawInCode()
        {
            var excludedFilesAndFolders = new List<string>
            {
                Path.Combine("GenderPayGap.UnitTests", "GenderPayGap.WebUI.Tests", "CodeQualityTests", "DoNotAllowHtmlDotRawTests.cs"), // This file!
                Path.Combine("GenderPayGap.WebUI", "bin"), // Output folder
                Path.Combine("GenderPayGap.WebUI", "obj"), // Output folder
            };

            // Arrange
            string rootCodeFolder = CodeQualityTestHelpers.GetRootCodeFolder();

            // Pre-Act Assert (to check we're running the test on the right folder)
            Assert.That(File.Exists(Path.Combine(rootCodeFolder, "GenderPayGap.sln")), $"We expect to find a file [GenderPayGap.sln] in the root folder [{rootCodeFolder}]");

            // More Arrange
            string searchPattern = "*.cs*" /* We want to find .cs and .cshtml files */;
            FileInfo[] files = new DirectoryInfo(rootCodeFolder).GetFiles(searchPattern, SearchOption.AllDirectories);

            // Pre-Act Assert (to check again that we're running the test on the right folder)
            Assert.Greater(files.Length, 400, "We expect there to be >400 .cs/.cshtml files");

            // Act
            var failedFiles = new List<string>();
            foreach (FileInfo fileInfo in files)
            {
                string filePath = fileInfo.FullName;
                string filePathSuffix = filePath.Replace(rootCodeFolder, "");

                if (CodeQualityTestHelpers.FileIsExcluded(filePathSuffix, excludedFilesAndFolders))
                {
                    continue;
                }

                string fileText = File.ReadAllText(filePath);
                if (fileText.Contains("Html.Raw"))
                {
                    failedFiles.Add(filePathSuffix);
                }
            }

            // Assert
            if (failedFiles.Any())
            {
                Assert.Fail($"The following {failedFiles.Count} files contain a call to Html.Raw, which is not allowed:\n- {string.Join("\n- ", failedFiles)}\n");
            }
        }
    }
}
