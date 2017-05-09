using ContentGrader.Core.Analysers;
using ContentGrader.Core.Models;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace ContentGrader.Core.Controllers
{
    [PluginController("ContentGrader")]
    public class ContentGraderController : UmbracoAuthorizedJsonController
    {
        public TextStatistics GradeContent(string content)
        {
            return TextStatisticAnalyser.Calculate(content);
        }
    }
}
