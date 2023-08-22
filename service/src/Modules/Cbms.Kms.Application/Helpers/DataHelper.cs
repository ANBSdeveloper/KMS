using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Cbms.Kms.Application.Helpers
{
    public class DataHelper
    {
        public static DateTime VNDateTime_UTCNow
        {
            get
            {
                return DateTime.UtcNow.AddHours(7);
            }
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static DataSet ConvertToDataSet<T>(IList<T> list)
        {
            DataSet dsFromDtStru = new DataSet();
            DataTable table = new DataTable();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            foreach (T item in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyInfo prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }
                table.Rows.Add(row);
            }
            dsFromDtStru.Tables.Add(table);
            return dsFromDtStru;
        }

        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static DateTime UnixTimestampToDateTime(long unixTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimeStampInTicks = unixTime * TimeSpan.TicksPerMillisecond;
            return new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc).AddHours(7);
        }

        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            //dateTime = dateTime.AddHours(-7);
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimeStampInTicks = (dateTime.ToUniversalTime() - unixStart).Ticks;
            return unixTimeStampInTicks / TimeSpan.TicksPerMillisecond;
        }

        public static string GetSHA1Hash(params string[] arrParams)
        {
            string Input = "";
            for (int i = 0; i < arrParams.Length; i++)
            {
                Input = Input + " " + arrParams[i];
            }
            var x = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(Input);

            bs = x.ComputeHash(bs);

            var s = new System.Text.StringBuilder();

            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }

            return s.ToString();
        }


        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string CreateMD5Sig(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static long GetTimeStamp(DateTime date)
        {
            return (long)(date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
        }

        public static long GetTimeStampToSeconds(DateTime date)
        {
            return (long)(date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

		public static decimal RemoveDec(string decString)
		{
			decimal grossAmount = 0;
			grossAmount = decimal.Parse(decString, CultureInfo.InvariantCulture);
			int iAmount = (int)grossAmount;
			return iAmount;
		}

		public static string SubRight(string value, int length)
		{
			if (String.IsNullOrEmpty(value)) return string.Empty;

			return value.Length <= length ? value : value.Substring(value.Length - length);
		}

		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}

		public static string Base64Decode(string base64EncodedData)
		{
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}
	}

    public static class TripleDES
    {
        private static readonly string ENCRYPTIONKEY = "BzSS4WAYl6bWRpK5mITvaRFT1krG5Ujm";


        public static string Encrypt(string OriginalData)
        {
            return Encrypt(OriginalData, true);
        }

        public static string Encrypt(string OriginalData, bool UseHashing, string key = "")
        {
            byte[] KeyDataBytes;
            if (key == "")
                key = ENCRYPTIONKEY;
            if (!UseHashing)
            {

                KeyDataBytes = UTF8Encoding.UTF8.GetBytes(key);
            }
            else
            {
                using (MD5CryptoServiceProvider mMD5 = new MD5CryptoServiceProvider())
                {
                    KeyDataBytes = mMD5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    mMD5.Clear();
                }
            }

            using (TripleDESCryptoServiceProvider mTripleDES = new TripleDESCryptoServiceProvider())
            {
                mTripleDES.Key = KeyDataBytes;
                mTripleDES.Mode = CipherMode.ECB;
                mTripleDES.Padding = PaddingMode.PKCS7;

                byte[] OriginalDataBytes = UTF8Encoding.UTF8.GetBytes(OriginalData);
                byte[] EncryptedDataBytes = mTripleDES.CreateEncryptor().TransformFinalBlock(OriginalDataBytes
                                                                                             , 0
                                                                                             , OriginalDataBytes.Length);

                mTripleDES.Clear();

                return Convert.ToBase64String(EncryptedDataBytes
                                              , 0
                                              , EncryptedDataBytes.Length);
            }
        }

        public static string Decrypt(string EncryptedData)
        {
            return Decrypt(EncryptedData, true);
        }

        public static string Decrypt(string EncryptedData, bool UseHashing, string key = "")
        {
            byte[] KeyDataBytes;
            if (key == "")
                key = ENCRYPTIONKEY;
            if (!UseHashing)
            {
                KeyDataBytes = UTF8Encoding.UTF8.GetBytes(key);
            }
            else
            {
                using (MD5CryptoServiceProvider mMD5 = new MD5CryptoServiceProvider())
                {
                    KeyDataBytes = mMD5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    mMD5.Clear();
                }
            }

            using (TripleDESCryptoServiceProvider mTripleDES = new TripleDESCryptoServiceProvider())
            {
                mTripleDES.Key = KeyDataBytes;
                mTripleDES.Mode = CipherMode.ECB;
                mTripleDES.Padding = PaddingMode.PKCS7;

                byte[] EncryptedDataBytes = Convert.FromBase64String(EncryptedData.Replace(' ', '+'));
                byte[] OriginalDataBytes = mTripleDES.CreateDecryptor().TransformFinalBlock(EncryptedDataBytes
                                                                                            , 0
                                                                                            , EncryptedDataBytes.Length);

                mTripleDES.Clear();

                return UTF8Encoding.UTF8.GetString(OriginalDataBytes);
            }
        }        

    }
}