using MimeKit;
using MimeKit.Cryptography;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System;
using System.IO;

namespace MimeKitTests
{
    public class TestSecureContext : BouncyCastleSecureMimeContext
    {
        private readonly X509Certificate _certificate;
        private readonly EncryptionAlgorithm _algorithm;
        private readonly RsaPrivateCrtKeyParameters _rsaPrivateCrtKeyParameters;

        public TestSecureContext(
            X509Certificate certificate,
            EncryptionAlgorithm algorithm,
            RsaPrivateCrtKeyParameters rsaPrivateCrtKeyParameters)
        {
            _certificate = certificate ?? throw new ArgumentNullException(nameof(certificate));
            _algorithm = algorithm;
            _rsaPrivateCrtKeyParameters = rsaPrivateCrtKeyParameters ??
                                          throw new ArgumentNullException(nameof(rsaPrivateCrtKeyParameters));
        }

        public override bool CanSign(MailboxAddress signer)
        {
            throw new NotImplementedException();
        }

        public override bool CanEncrypt(MailboxAddress mailbox)
        {
            throw new NotImplementedException();
        }

        public override void Import(Stream stream, string password)
        {
            throw new NotImplementedException();
        }

        public override void Import(X509Certificate certificate)
        {
            throw new NotImplementedException();
        }

        public override void Import(X509Crl crl)
        {
            throw new NotImplementedException();
        }

        protected override X509Certificate GetCertificate(IX509Selector selector)
        {
            throw new NotImplementedException();
        }

        protected override AsymmetricKeyParameter GetPrivateKey(IX509Selector selector)
        {
            return _rsaPrivateCrtKeyParameters;
        }

        protected override HashSet GetTrustedAnchors()
        {
            throw new NotImplementedException();
        }

        protected override IX509Store GetIntermediateCertificates()
        {
            throw new NotImplementedException();
        }

        protected override IX509Store GetCertificateRevocationLists()
        {
            throw new NotImplementedException();
        }

        protected override DateTime GetNextCertificateRevocationListUpdate(X509Name issuer)
        {
            throw new NotImplementedException();
        }

        protected override CmsRecipient GetCmsRecipient(MailboxAddress mailbox)
        {
            return new CmsRecipient(_certificate)
            {
                EncryptionAlgorithms = new[] { _algorithm }
            };
        }

        protected override CmsSigner GetCmsSigner(MailboxAddress mailbox, DigestAlgorithm digestAlgo)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateSecureMimeCapabilities(X509Certificate certificate, EncryptionAlgorithm[] algorithms, DateTime timestamp)
        {
            throw new NotImplementedException();
        }
    }
}
