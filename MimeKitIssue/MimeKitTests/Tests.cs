using MimeKit;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;

namespace MimeKitTests
{
    public class Tests
    {
        [Test]
        public async Task Test()
        {
            var stream = await GetStream("mixed_eol.txt");
            var m1 = new MimePart
            {
                Content = new MimeContent(stream)
            };

            await m1.WriteToAsync(GetFilePath("m1.txt"));
            var entity = await MimeEntity.LoadAsync(GetFilePath("m1.txt"));
            var m2 = (MimePart)entity;

            FileAssert.AreEqual(stream, m1.Content.Stream);
            FileAssert.AreEqual(m1.Content.Stream, m2.Content.Stream);
        }

        private static async Task<Stream> GetStream(string fileName)
        {
            var result = new MemoryStream();
            var stream = File.OpenRead(GetFilePath(fileName));
            await stream.CopyToAsync(result);
            await stream.DisposeAsync();
            return result;
        }

        private static string GetFilePath(string fileName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), fileName);
        }
    }
}