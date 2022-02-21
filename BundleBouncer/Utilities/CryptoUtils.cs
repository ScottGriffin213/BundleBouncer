using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleBouncer.Utilities
{
    internal class CryptoUtils
    {
        internal static PgpPublicKey bytesToPubkey(byte[] bytes)
        {
            using(var ms = new MemoryStream(bytes))
            using(var decoder = PgpUtilities.GetDecoderStream(ms))
            return new PgpPublicKeyRing(decoder).GetPublicKey();
        }
        /**
        * verify the signature in in against the file fileName.
        */
        internal static bool VerifySignature(
            string fileName,
            Stream inputStream,
            PgpPublicKey publicKey)
        {
            inputStream = PgpUtilities.GetDecoderStream(inputStream);

            PgpObjectFactory pgpFact = new PgpObjectFactory(inputStream);
            PgpSignatureList p3;
            PgpObject o = pgpFact.NextPgpObject();
            if (o is PgpCompressedData)
            {
                PgpCompressedData c1 = (PgpCompressedData)o;
                pgpFact = new PgpObjectFactory(c1.GetDataStream());

                p3 = (PgpSignatureList)pgpFact.NextPgpObject();
            }
            else
            {
                p3 = (PgpSignatureList)o;
            }

            Stream dIn = File.OpenRead(fileName);
            PgpSignature sig = p3[0];
            sig.InitVerify(publicKey);

            int ch;
            while ((ch = dIn.ReadByte()) >= 0)
            {
                sig.Update((byte)ch);
            }

            dIn.Close();

            return sig.Verify();
        }
    }
}