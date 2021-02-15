using System.Collections;
using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

namespace EFZ.Domain.BusinessLogic
{
    class Cert
    {
        #region Attributes

        private readonly string _path = "";
        private string _password = "";
        private AsymmetricKeyParameter _akp;
        private X509Certificate[] _chain;

        #endregion

        #region Accessors
        public X509Certificate[] Chain
        {
            get { return _chain; }
            set { _chain = value; }
        }

        public AsymmetricKeyParameter Akp => _akp;

        public string Path => _path;

        public string Password
        {
            get => _password;
            set => _password = value;
        }
        #endregion

        #region Helpers

        private void ProcessCert()
        {
            string alias = null;

            //First we'll read the certificate file
            var certFile = new FileStream(this.Path, FileMode.Open, FileAccess.Read);
            

            var pk12 = new Pkcs12Store(certFile, this._password.ToCharArray());

            //then Iterate throught certificate entries to find the private key entry
            var i = pk12.Aliases;
            foreach (var pk12Aliase in pk12.Aliases)
            {
                alias = ((string)pk12Aliase);
                if (pk12.IsKeyEntry(alias))
                    break;
            }

            this._akp = pk12.GetKey(alias).Key;
            X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
            this._chain = new X509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
                _chain[k] = ce[k].Certificate;


            certFile.Dispose();

        }
        

        private void ProcessCertPem()
        {
            //First we'll read the certificate file
            var certFile = new FileStream(this.Path, FileMode.Open, FileAccess.Read);

            PemReader pemReader = new PemReader(new StreamReader(certFile));
            byte[] x509Data = pemReader.ReadPemObject().Content;

            var parser = new X509CertificateParser();
            var certificate =  parser.ReadCertificate(x509Data);
            this._akp = certificate.GetPublicKey();
            //then Iterate throught certificate entries to find the private key entry
            this._chain = new X509Certificate[1];
            this._chain[0] = certificate;
 

            certFile.Dispose();

        }


        #endregion

        #region Constructors
        public Cert()
        { }
        public Cert(string path, bool isPem = false)
        {
            this._path = path; 
            if(isPem)
                this.ProcessCertPem();
            else
            {
                this.ProcessCert();
            }
        }
        public Cert(string path, string password)
        {
            this._path = path;
            this.Password = password;
            this.ProcessCert();
        }
        #endregion

    }
}