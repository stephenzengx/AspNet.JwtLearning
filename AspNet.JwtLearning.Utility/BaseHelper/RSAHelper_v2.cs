using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AspNet.JwtLearning.Utility
{
    public class RSAHelper_v2
    {
        public static string BaseDir = AppDomain.CurrentDomain.BaseDirectory;

        //设置[公/私钥]文件路径
        public static string PublicKeyPath = Path.Combine(BaseDir, "KeyFile\\public.key");
        public static string PrivateKeyPath = Path.Combine(BaseDir, "KeyFile\\private.key");

        //Rsa 1024  pkcs#1 (前端js加密也要加 begin-end头)
        public static string PubKey
        {
            get
            {
                return File.ReadAllText(PublicKeyPath);
            }
        }

        public static string PrivKey
        {
            get
            {
                return File.ReadAllText(PrivateKeyPath);
            }
        }

        #region 解析
        private static RSAParameters CreateRsaFromPublicKey(string publicKey)
        {
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] x509key;
            byte[] seq = new byte[15];
            int x509size;

            var tmp = publicKey.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace("\r\n", "");

            x509key = Convert.FromBase64String(tmp);
            x509size = x509key.Length;

            using (var mem = new MemoryStream(x509key))
            {
                using (var binr = new BinaryReader(mem))
                {
                    byte bt = 0;
                    ushort twobytes = 0;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130)
                        binr.ReadByte();
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();
                    else
                        return new RSAParameters();

                    seq = binr.ReadBytes(15);
                    if (!CompareBytearrays(seq, SeqOID))
                        return new RSAParameters();

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8103)
                        binr.ReadByte();
                    else if (twobytes == 0x8203)
                        binr.ReadInt16();
                    else
                        return new RSAParameters();

                    bt = binr.ReadByte();
                    if (bt != 0x00)
                        return new RSAParameters();

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130)
                        binr.ReadByte();
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();
                    else
                        return new RSAParameters();

                    twobytes = binr.ReadUInt16();
                    byte lowbyte = 0x00;
                    byte highbyte = 0x00;

                    if (twobytes == 0x8102)
                        lowbyte = binr.ReadByte();
                    else if (twobytes == 0x8202)
                    {
                        highbyte = binr.ReadByte();
                        lowbyte = binr.ReadByte();
                    }
                    else
                        return new RSAParameters();
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    int modsize = BitConverter.ToInt32(modint, 0);

                    int firstbyte = binr.PeekChar();
                    if (firstbyte == 0x00)
                    {
                        binr.ReadByte();
                        modsize -= 1;
                    }

                    byte[] modulus = binr.ReadBytes(modsize);

                    if (binr.ReadByte() != 0x02)
                        return new RSAParameters();
                    int expbytes = (int)binr.ReadByte();
                    byte[] exponent = binr.ReadBytes(expbytes);
                    var rsaKeyInfo = new RSAParameters
                    {
                        Modulus = modulus,
                        Exponent = exponent
                    };
                    return rsaKeyInfo;
                }

            }
        }

        private static RSAParameters CreateRsaFromPrivateKey(string privateKey)
        {
            string tmp = privateKey.Replace("\r\n", "")
                .Replace("-----END RSA PRIVATE KEY-----", "")
                .Replace("-----BEGIN RSA PRIVATE KEY-----", "");
            var privateKeyBits = Convert.FromBase64String(tmp);
            var RSAparams = new RSAParameters();

            using (var binr = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    throw new Exception("Unexpected value read binr.ReadUInt16()");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new Exception("Unexpected version");

                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("Unexpected value read binr.ReadByte()");

                RSAparams.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.D = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.P = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Q = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DP = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DQ = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }

            return RSAparams;
        }


        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte();
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }

        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }
        #endregion

        //RSA 公钥加密
        public static string Encrypt(string context)
        {
            UTF8Encoding ByteConverter = new UTF8Encoding();
            byte[] DataToEncrypt = ByteConverter.GetBytes(context);

            var rsa = RSA.Create();
            rsa.ImportParameters(CreateRsaFromPublicKey(PubKey));

            byte[] bytes = rsa.Encrypt(DataToEncrypt, RSAEncryptionPadding.Pkcs1);
            string str = Convert.ToBase64String(bytes);

            return str;
        }

        //RSA 私钥解密
        public static string Decrypt(string context)
        {
            byte[] DataToDecrypt = Convert.FromBase64String(context);

            var rsa = RSA.Create();
            rsa.ImportParameters(CreateRsaFromPrivateKey(PrivKey));

            byte[] bytes = rsa.Decrypt(DataToDecrypt, RSAEncryptionPadding.Pkcs1);
            UTF8Encoding ByteConverter = new UTF8Encoding();
            string str = ByteConverter.GetString(bytes);

            return str;
        }
    }
}
