using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GetIP.Helper
{
    class LogHelper
    {
        public static void setlog(String msg)
        {
            String filePath = "log.txt";
            try
            {

                StreamWriter sw = new StreamWriter(filePath, true);
                sw.WriteLine(msg);
                sw.Close();
            }
            catch (Exception err)
            {

            }
            finally
            {
            }

        }
    }
}
