using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GaoJD.Club.Core.Utility
{
    public class EncryptHelper
    {
        //默认密钥初始化向量
        private static byte[] DESIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encriptString">待加密的字符串</param>
        /// <param name="encKey">加密密钥,要求为8字节</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string Encrypt(string encriptString, string encKey)
        {
            if (encKey.Trim().Length != 8)
                encKey = "87654321";

            try
            {
                byte[] bKey = Encoding.GetEncoding("GB2312").GetBytes(encKey.Substring(0, 8));
                byte[] bIV = DESIV;
                byte[] bEncContent = Encoding.GetEncoding("GB2312").GetBytes(encriptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                //用指定的 Key 和初始化向量 (IV) 创建对称数据加密标准 (DES) 加密器对象
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write);
                cStream.Write(bEncContent, 0, bEncContent.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray()).Replace("+", "%2B");
                // return Encoding.GetEncoding("GB2312").GetString(mStream.ToArray());
            }
            catch
            {
                return encriptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8字节,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string Decrypt(string decryptString, string decryptKey)
        {
            decryptString = decryptString.Replace("%2B", "+");
            if (decryptKey.Trim().Length != 8)
                decryptKey = "87654321";
            try
            {
                byte[] bKey = Encoding.GetEncoding("GB2312").GetBytes(decryptKey);
                byte[] bIV = DESIV;
                byte[] bDecContent = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write);
                cStream.Write(bDecContent, 0, bDecContent.Length);
                cStream.FlushFinalBlock();
                return Encoding.GetEncoding("GB2312").GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }

        #region DES加解密用于URL传参
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encriptString">待加密的字符串</param>
        /// <param name="encKey">加密密钥,要求为8字节</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptURL(string encriptString, string encKey)
        {
            if (encKey.Trim().Length != 8)
                encKey = "87654321";

            try
            {
                byte[] bKey = Encoding.GetEncoding("GB2312").GetBytes(encKey.Substring(0, 8));
                byte[] bIV = DESIV;
                byte[] bEncContent = Encoding.GetEncoding("GB2312").GetBytes(encriptString);



                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                //用指定的 Key 和初始化向量 (IV) 创建对称数据加密标准 (DES) 加密器对象
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write);
                cStream.Write(bEncContent, 0, bEncContent.Length);
                cStream.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in mStream.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                ret.ToString();
                return ret.ToString();
            }
            catch
            {
                return encriptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8字节,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptURL(string decryptString, string decryptKey)
        {
            if (decryptKey.Trim().Length != 8)
                decryptKey = "87654321";
            try
            {
                byte[] bKey = Encoding.GetEncoding("GB2312").GetBytes(decryptKey);
                byte[] bIV = DESIV;
                byte[] bDecContent = new byte[decryptString.Length / 2];

                for (int x = 0; x < decryptString.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(decryptString.Substring(x * 2, 2), 16));
                    bDecContent[x] = (byte)i;
                }

                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write);
                cStream.Write(bDecContent, 0, bDecContent.Length);
                cStream.FlushFinalBlock();
                return Encoding.GetEncoding("GB2312").GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
        #endregion

        #region 获取MD5的值
        /// <summary>
        /// 获取MD5得值，没有转换成Base64的
        /// </summary>
        /// <param name="sDataIn">需要加密的字符串</param>
        /// <param name="move">偏移量</param>
        /// <returns>sDataIn加密后的字符串</returns>
        public static string GetMD5(string sDataIn)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] byt, bytHash;
            byt = System.Text.Encoding.UTF8.GetBytes(sDataIn);
            bytHash = md5.ComputeHash(byt);
            md5.Clear();
            string sTemp = "";

            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("x").PadLeft(2, '0');
            }
            return sTemp;
        }
        #endregion

        /// <summary>
        /// DES加密算法
        /// </summary>
        /// <param name="content">待加密的内容</param>
        /// <param name="key">向量key</param>
        /// <param name="iv">向量iv</param>
        /// <returns>加密后的字符串</returns>
        public static string DESEncrypt(string content, string key, string iv = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("加密key不能为空");
            }
            byte[] rgbKey = GetLegalKey(key);
            byte[] rgbIv = null;
            if (string.IsNullOrEmpty(iv))
            {
                rgbIv = rgbKey;
            }
            else
            {
                rgbIv = GetLegalKey(iv);
            }
            using (MemoryStream stream = new MemoryStream())
            {
                DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();
                desCryptoServiceProvider.Mode = CipherMode.ECB;
                using (CryptoStream cryptoStream = new CryptoStream(stream, desCryptoServiceProvider.CreateEncryptor(rgbKey, rgbIv), CryptoStreamMode.Write))
                {
                    byte[] contentArray = Encoding.UTF8.GetBytes(content);
                    cryptoStream.Write(contentArray, 0, contentArray.Length);
                    cryptoStream.FlushFinalBlock();
                }
                return Convert.ToBase64String(stream.ToArray());
            }
        }
        /// <summary>
        /// DES解密算法
        /// </summary>
        /// <param name="encryptedStr">加密后的字符串</param>
        /// <param name="key">向量key</param>
        /// <param name="iv">向量iv</param>
        /// <returns>解密后的字符串</returns>
        public static string DESDecrypt(string encryptedStr, string key, string iv = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("解密key不能为空");
            }
            byte[] rgbKey = GetLegalKey(key);
            byte[] rgbIv = null;
            if (string.IsNullOrEmpty(iv))
            {
                rgbIv = rgbKey;
            }
            else
            {
                rgbIv = GetLegalKey(iv);
            }
            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(encryptedStr)))
            {
                DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();
                desCryptoServiceProvider.Mode = CipherMode.ECB;
                using (CryptoStream cryptoStream = new CryptoStream(stream, desCryptoServiceProvider.CreateDecryptor(rgbKey, rgbIv), CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cryptoStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
        /// <summary>
        /// 新疆接口 DES解密算法
        /// </summary>
        /// <param name="encryptedStr">加密后的字符串</param>
        /// <param name="key">向量key</param>
        /// <param name="iv">向量iv</param>
        /// <returns>解密后的字符串</returns>
        public static string XinJiangDESDecrypt(string encryptedStr, string key, string iv = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("解密key不能为空");
            }
            byte[] rgbKey = GetLegalKey(key);
            byte[] rgbIv = null;
            if (string.IsNullOrEmpty(iv))
            {
                rgbIv = rgbKey;
            }
            else
            {
                rgbIv = GetLegalKey(iv);
            }
            byte[] arrB = Encoding.UTF8.GetBytes(encryptedStr);
            int iLen = arrB.Length;
            // 两个字符表示一个字节，所以字节数组长度是字符串长度除以2
            byte[] arrOut = new byte[iLen / 2];
            for (int i = 0; i < iLen; i = (i + 2))
            {
                String strTmp = new String(Encoding.UTF8.GetChars(arrB, i, 2));
                arrOut[i / 2] = (byte)Convert.ToInt32(strTmp, 16);
            }
            using (MemoryStream stream = new MemoryStream(arrOut))
            {
                DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();
                desCryptoServiceProvider.Mode = CipherMode.ECB;

                using (CryptoStream cryptoStream = new CryptoStream(stream, desCryptoServiceProvider.CreateDecryptor(rgbKey, rgbIv), CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cryptoStream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        /// <summary>
        /// 获取合法的向量值
        /// </summary>
        /// <param name="key">key或者iv</param>
        /// <returns>合法的向量值</returns>
        private static byte[] GetLegalKey(string key)
        {
            if (key.Length > 8)
            {
                key = key.Substring(0, 8);
            }
            else if (key.Length < 8)
            {
                key = key.PadRight(8, '-');
            }
            return Encoding.UTF8.GetBytes(key);
        }
        /// <summary>
        /// 将字符串使用base64算法加密
        /// </summary>
        /// <param name="sourceString">待加密的字符串</param>
        /// <param name="ens">System.Text.Encoding 对象，如创建中文编码集对象：System.Text.Encoding.GetEncoding(54936)</param>
        /// <returns></returns>
        public static string EncodingForString(string sourceString, System.Text.Encoding ens)
        {
            try
            {
                return Convert.ToBase64String(ens.GetBytes(sourceString));
            }
            catch
            {
                return sourceString;
            }
        }
        /// <summary>
        /// 将字符串使用base64算法加密
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string EncodingForString(string sourceString)
        {
            try
            {
                return EncodingForString(sourceString, System.Text.Encoding.GetEncoding(54936));
            }
            catch
            {
                return sourceString;
            }
        }
      

        #region 广东接口使用的hex和aes的加密和解密
        #region Hex处理后的十六进制字符串
        /// <summary>
        /// Hex返回处理后的十六进制字符串
        /// </summary>
        /// <param name="mStr"></param>
        /// <returns></returns>
        public static byte[] DecodeHex(string mStr)
        {
            try
            {
                char[] data = mStr.ToCharArray();
                int len = data.Length;
                byte[] buffer = new byte[len >> 1];
                // two characters form the hex value.
                for (int i = 0, j = 0; j < len; i++)
                {
                    int f = Convert.ToInt32(data[j].ToString(), 16) << 4;
                    j++;
                    f = f | Convert.ToInt32(data[j].ToString(), 16);
                    j++;
                    buffer[i] = (byte)(f & 0xFF);
                }
                return buffer;
            }
            catch (Exception)
            {

                return Encoding.UTF8.GetBytes(mStr); ;
            }

        }

        /// <summary>
        ///  Hex返回十六进制转换后的字符串
        /// </summary>
        /// <param name="mHex"></param>
        /// <returns></returns>
        public static string EncodeHexString(byte[] HexValue)
        {
            try
            {
                byte[] data = HexValue;
                char[] toDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
                int l = data.Length;
                char[] charArray = new char[l << 1];
                // two characters form the hex value.
                for (int i = 0, j = 0; i < l; i++)
                {
                    charArray[j++] = toDigits[(0xF0 & data[i]) >> 4];
                    charArray[j++] = toDigits[0x0F & data[i]];
                }
                return new string(charArray);
            }
            catch (Exception)
            {

                return HexValue.ToString();
            }

        }

        #endregion
        #region AES加密/解密

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="str">待加密字符串</param>
        ///<param name="str">加密key</param>
        /// <returns>加密后字符串</returns>
        public static byte[] Encrypt_AES(string str, byte[] strAesKey)
        {
            try
            {
                byte[] encrypted;
                using (RijndaelManaged rijAlg = new RijndaelManaged())
                {
                    rijAlg.Key = strAesKey;
                    rijAlg.IV = strAesKey;
                    rijAlg.Mode = CipherMode.ECB;
                    rijAlg.Padding = PaddingMode.PKCS7;
                    ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                //Write all data to the stream.
                                swEncrypt.Write(str);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }
                return encrypted;
            }
            catch (Exception)
            {

                return strAesKey;
            }

        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="str">待解密字符串</param>
        /// <param name="strAesKey">加密key</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt_AES(byte[] str, byte[] strAesKey)
        {
            try
            {
                string plaintext = null;
                using (RijndaelManaged rijAlg = new RijndaelManaged())
                {
                    rijAlg.Key = strAesKey;
                    rijAlg.IV = strAesKey;
                    rijAlg.Mode = CipherMode.ECB;
                    rijAlg.Padding = PaddingMode.PKCS7;
                    ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                    using (MemoryStream msDecrypt = new MemoryStream(str))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {

                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                }
                return plaintext;
            }
            catch
            {

                return null;
            }

        }


        #endregion

        #endregion


        #region 福建接口des算法

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string DESEnCode(string pToEncrypt, string sKey)
        {
            // string pToEncrypt1 = HttpContext.Current.Server.UrlEncode(pToEncrypt);   
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.GetEncoding("UTF-8").GetBytes(pToEncrypt);

            //建立加密对象的密钥和偏移量    
            //原文使用ASCIIEncoding.ASCII方法的GetBytes方法    
            //使得输入密码必须输入英文文本    
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string DESDeCode(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();

            // return HttpContext.Current.Server.UrlDecode(System.Text.Encoding.Default.GetString(ms.ToArray()));
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }


        #endregion


        #region AES 加密和解密    wsa add

        /// <summary>
        ///  AES 加密，使用对象：用户中心统一认证
        /// </summary>
        /// <param name="original">要加密的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns></returns>
        public static string AESEncrypt(string original, string key, string iv)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(original);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// AES 解密，使用对象：用户中心统一认证
        /// </summary>
        /// <param name="original">被解密的密文</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns></returns>
        public static string AESDecrypt(string original, string key, string iv)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = Convert.FromBase64String(original);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        #endregion


        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(string source)
        {
            return Base64Encode(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string Base64Encode(Encoding encodeType, string source)
        {
            string encode = string.Empty;
            byte[] bytes = encodeType.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }
        #region 内蒙古哈希加密
        public static string NeiMeng_HMACSHA1Hasher(string HashText, string EncryptKey)
        {
            byte[] HmacKey = System.Text.Encoding.UTF8.GetBytes(EncryptKey);
            byte[] HmacData = System.Text.Encoding.UTF8.GetBytes(HashText);
            HMACSHA1 Hmac = new HMACSHA1(HmacKey);
            CryptoStream cs = new CryptoStream(Stream.Null, Hmac, CryptoStreamMode.Write);
            cs.Write(HmacData, 0, HmacData.Length);
            cs.Close();
            byte[] Result = Hmac.Hash;
            StringBuilder sb = new StringBuilder();
            foreach (byte b in Result)
            {
                // 以十六进制格式格式化
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        /// hmacSha1算法加密（生成长度40），转成大写
        /// </summary>
        /// <param name="encryptText">加密明文</param>
        /// <param name="encryptKey">加密密钥</param>
        /// <returns></returns>
        public static string NeiMeng_hmacSha1(string encryptText, string encryptKey)
        {
            HMACSHA1 myHMACSHA1 = new HMACSHA1(Encoding.Default.GetBytes(encryptKey));
            byte[] RstRes = myHMACSHA1.ComputeHash(Encoding.Default.GetBytes(encryptText));

            StringBuilder EnText = new StringBuilder();
            foreach (byte Byte in RstRes)
            {
                EnText.AppendFormat("{0:x2}", Byte);
            }
            return EnText.ToString();
        }

        #endregion

        /// <summary>
        /// 哈希加密
        /// </summary>
        /// <param name="HashText">待加密文本</param>
        /// <param name="EncryptKey">加密key</param>
        /// <returns></returns>
        public static string EncryptHasher(string HashText, string EncryptKey)
        {
            byte[] HmacKey = System.Text.Encoding.UTF8.GetBytes(EncryptKey);
            byte[] HmacData = System.Text.Encoding.UTF8.GetBytes(HashText);
            HMACSHA1 Hmac = new HMACSHA1(HmacKey);
            CryptoStream cs = new CryptoStream(Stream.Null, Hmac, CryptoStreamMode.Write);
            cs.Write(HmacData, 0, HmacData.Length);
            cs.Close();
            byte[] Result = Hmac.Hash;
            StringBuilder sb = new StringBuilder();
            foreach (byte b in Result)
            {
                // 以十六进制格式格式化
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
