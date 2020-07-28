using System.Threading.Tasks;
using Xunit;

namespace LittlePdf.Test.Functional
{
    public class FunctionalTests
    {
        [Fact]
        public async Task DocumentTest()
        {
            var document = new Document();
            var page1 = document.AddPage();
            var page2 = document.AddPage();
            page2.Rotate();

            await document.SaveAsync(@"C:\temp\doc.pdf");
        }
    }
}
