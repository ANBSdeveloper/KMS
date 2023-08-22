using System;
using System.IO;
using System.Text;

namespace Cbms.Kms.Domain.Helpers
{
    public static class FunctionHelper
	{
		public static void WriteLogFile(string strFuncName, string strMsg, string LOG_FILE_DIR, string LOG_FILE, string fName)
		{
			if (!Directory.Exists(LOG_FILE_DIR))
			{
				Directory.CreateDirectory(LOG_FILE_DIR);
			}

			string strDate = String.Format("{0:yyyy/MM/dd}", DateTime.Now).Replace("/", "");
			string filename = string.Format(LOG_FILE, strDate + "_" + fName);
			System.IO.StreamWriter sw = new System.IO.StreamWriter(new FileStream(filename, FileMode.Append), Encoding.UTF8);
			//System.IO.StreamWriter sw = System.IO.File.AppendText(filename);
			try
			{
				string logLine = System.String.Format("{0:G}: {1}.", System.DateTime.Now, "[" + strFuncName + " - " + strMsg + "] ");
				sw.WriteLine(logLine);
				sw.WriteLine("-------------------------------------------");
			}
			finally
			{
				sw.Close();
			}
			//#if DEBUG
			//            Utility.ShowError(strFuncName, strMsg);
			//#endif
		}
	}
}