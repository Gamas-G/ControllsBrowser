using System;

using browserManagment.Chrome;

namespace browserManagment
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                BrowserForm bf = new BrowserForm();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
