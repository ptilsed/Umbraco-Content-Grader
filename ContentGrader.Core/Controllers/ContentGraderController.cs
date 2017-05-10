using ContentGrader.Core.Analysers;
using ContentGrader.Core.Models;
using System.Web.Http;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace ContentGrader.Core.Controllers
{
    [PluginController("ContentGrader")]
    public class ContentGraderController : UmbracoAuthorizedJsonController
    {
        [HttpPost]
        public TextStatistics GradeContent([FromBody]string content)
        {
         return TextStatisticAnalyser.Calculate(content);
        }
    }
}
