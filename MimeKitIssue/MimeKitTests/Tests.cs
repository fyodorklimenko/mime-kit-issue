using MimeKit;
using MimeKit.Cryptography;
using NUnit.Framework;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MimeKitTests
{
    public class Tests
    {
        [Test]
        [TestCase("cr_eol.txt")]
        [TestCase("crlf_eol.txt")]
        [TestCase("lf_eol.txt")]
        [TestCase("mixed_eol.txt")]
        public async Task GetStream_CreateMimePart_WriteMimePart_LoadMimePart_CompareMimeParts(string fileName)
        {
            var stream = await GetAssetStreamAsync(fileName);
            var m1 = new MimePart
            {
                Content = new MimeContent(stream)
            };

            await m1.WriteToAsync(GetAssetFilePath($"{fileName}__m1.txt"));
            var entity = await MimeEntity.LoadAsync(GetAssetFilePath($"{fileName}__m1.txt"));
            var m2 = (MimePart)entity;

            FileAssert.AreEqual(stream, m1.Content.Stream);
            FileAssert.AreEqual(m1.Content.Stream, m2.Content.Stream);
        }

        [Test]
        [TestCase("cr_eol.txt")]
        [TestCase("crlf_eol.txt")]
        [TestCase("lf_eol.txt")]
        [TestCase("mixed_eol.txt")]
        public async Task Encrypt_VS_Decrypt(string fileName)
        {
            var stream = await GetAssetStreamAsync(fileName);

            var context = new TestSecureContext(
                await GetFromPemFileAsync<X509Certificate>("pem.pem", "password"),
                EncryptionAlgorithm.TripleDes,
                await GetFromPemFileAsync<RsaPrivateCrtKeyParameters>("pem.pem", "password"));

            var encrypted = ApplicationPkcs7Mime.Encrypt(
                context,
                new List<MailboxAddress>
                {
                    new MailboxAddress(string.Empty)

                },
                new MimePart
                {
                    Content = new MimeContent(stream)
                });

            var decrypted = encrypted.Decrypt(context);

            Assert.IsInstanceOf<MimePart>(decrypted);
            FileAssert.AreEqual(stream, ((MimePart)decrypted).Content.Stream);
        }

        private static string GetAssetFilePath(string fileName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "Assets", fileName);
        }

        private static async Task<Stream> GetAssetStreamAsync(string fileName)
        {
            var result = new MemoryStream();
            var stream = File.OpenRead(GetAssetFilePath(fileName));
            await stream.CopyToAsync(result);
            await stream.DisposeAsync();
            return result;
        }

        private static async Task<TResult> GetFromPemFileAsync<TResult>(string fileName, string password)
        {
            var path = GetAssetFilePath(fileName);
            var stringReader = new StringReader(await File.ReadAllTextAsync(path));
            var pemReader = new PemReader(stringReader, new TestPasswordFinder(password));

            var @object = pemReader.ReadObject();

            while (@object != null)
            {
                if (@object is TResult result)
                {
                    return result;
                }

                @object = pemReader.ReadObject();
            }

            throw new Exception();
        }
    }
}