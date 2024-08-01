using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace browserManagment.Chrome
{
    class BrowserForm
    {
        private Form browser_form;

        //Controles
        private Rectangle browser_screen;

        //Zona de botones
        private Label label;
        private Label labelMin;
        private Label labelMax;
        private Process browser_process;



        private static IntPtr HWND_TOPMOST = new IntPtr(-1);

        private const int SWP_NOSIZE = 0x0001;

        private const int SWP_NOMOVE = 0x0002;

        private const int SWP_NOACTIVATE = 0x0010;

        private const int SW_MINIMIZA = 6;
        private const int SW_SHOWNORMAL = 3;


        const int label_size = 22;

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public BrowserForm()
        {
            iniciaFormBrowser();
        }


        private void iniciaFormBrowser()
        {
            browser_process = new Process();
            browser_process.StartInfo.UseShellExecute = true;
            browser_process.StartInfo.FileName = "chrome.exe";
            browser_process.StartInfo.Arguments = "--kiosk https://developer.chrome.com/";
            browser_process.Start();


            browser_form = new Form();
            browser_screen = Screen.FromControl(browser_form).Bounds;
            browser_form.StartPosition = FormStartPosition.Manual;
            //browser_form.Tag = gdtBrowser.Default.WINNAME;
            //browser_form.Text = gdtBrowser.Default.WINNAME;
            browser_form.ControlBox = false;
            browser_form.MaximizeBox = false;
            browser_form.MinimizeBox = false;
            browser_form.ShowIcon = false;
            browser_form.ShowInTaskbar = false;
            browser_form.BackColor = Color.Gray;
            browser_form.TransparencyKey = Color.Gray;
            browser_form.FormBorderStyle = FormBorderStyle.None;
            browser_form.WindowState = FormWindowState.Normal;
            browser_form.SizeGripStyle = SizeGripStyle.Hide;
            browser_form.Load += browser_form_load;
            browser_form.FormClosing += browser_form_closing;


            label = new Label();
            label.Size = new Size(label_size, label_size);
            label.Text = "X";
            label.Left = label_size;
            label.ForeColor = Color.White;
            label.BackColor = Color.Red;
            label.Font = new Font(label.Font, FontStyle.Bold);
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Cursor = Cursors.Hand;
            label.Click += browser_form_label_click;

            //Minimizar
            labelMin = new Label();
            labelMin.Size = new Size(label_size, label_size);
            labelMin.Text = "-";
            labelMin.Left = -2;
            labelMin.ForeColor = Color.White;
            labelMin.BackColor = Color.Red;
            labelMin.Font = new Font(label.Font, FontStyle.Bold);
            labelMin.TextAlign = ContentAlignment.MiddleCenter;
            labelMin.Cursor = Cursors.Hand;
            labelMin.Click += browser_form_label_min_click;

            //Maximizar
            labelMax = new Label();
            labelMax.Size = new Size(label_size, label_size);
            labelMax.Text = "[ ]";
            labelMax.Left = -2;
            labelMax.ForeColor = Color.White;
            labelMax.BackColor = Color.Red;
            labelMax.Font = new Font(label.Font, FontStyle.Bold);
            labelMax.TextAlign = ContentAlignment.MiddleCenter;
            labelMax.Cursor = Cursors.Hand;
            labelMax.Click += browser_form_label_max_click;

            browser_form.Controls.Add(label);
            browser_form.Controls.Add(labelMin);
            browser_form.Controls.Add(labelMax);
            browser_form.Location = new Point(browser_screen.Width - (2 * label_size), 0);
            browser_form.MaximumSize = new Size((2 * label_size + 5), label_size);
            browser_form.MinimumSize = new Size((2 * label_size + 5), label_size);
            browser_form.Size = new Size((2 * label_size + 5), label_size);



            Application.Run(browser_form);
        }

        private void browser_form_load(object sender, EventArgs ea)
        {
            //if (!(debug || noclose))
            //{
            //}
                SetWindowPos(browser_form.Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);

            //Thread t = new Thread(new ThreadStart(ProcessMonitor));
            //t.Start();
        }

        private static void browser_form_closing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        void browser_form_label_click(object sender, EventArgs e)
        {
            try
            {
                KillBrowser();
                Process.GetCurrentProcess().Kill();
            }
            catch (Exception ex)
            {
                Process.GetCurrentProcess().Kill();
            }
        }

        void browser_form_label_min_click(object sender, EventArgs e)
        {
            try
            {
                IntPtr handlid = IntPtr.Zero;
                IntPtr handGdt = IntPtr.Zero;
                //Process currentProcess = Process.GetCurrentProcess();
                //Console.WriteLine( currentProcess.Handle.ToString() );

                //Obtencion de procesos de chrome
                Process[] localByName = Process.GetProcessesByName("chrome");
                //Process[] gdt = Process.GetProcessesByName("gdtBrowser");

                foreach (var item in localByName)
                {
                    //Obtención del MainWindowHandle
                    if (handlid == IntPtr.Zero)
                    {
                        handlid = item.MainWindowHandle;
                    }
                }
                ShowWindow(handlid, SW_MINIMIZA);
                //browser_form.Controls.Bottom = 0;

                labelMin.Visible = false;
                browser_form.Location = new Point(browser_screen.Width - (2 * label_size), browser_screen.Height- 50);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void browser_form_label_max_click(object sender, EventArgs e)
        {
            try
            {
                IntPtr handlid = IntPtr.Zero;
                IntPtr handGdt = IntPtr.Zero;
                //Process currentProcess = Process.GetCurrentProcess();
                //Console.WriteLine( currentProcess.Handle.ToString() );

                //Obtencion de procesos de chrome
                Process[] localByName = Process.GetProcessesByName("chrome");
                //Process[] gdt = Process.GetProcessesByName("gdtBrowser");

                foreach (var item in localByName)
                {
                    //Obtención del MainWindowHandle
                    if (handlid == IntPtr.Zero)
                    {
                        handlid = item.MainWindowHandle;
                    }
                }
                ShowWindow(handlid, SW_SHOWNORMAL);
                //browser_form.Controls.Bottom = 0;

                labelMin.Visible = true;
                browser_form.Location = new Point(browser_screen.Width - (2 * label_size), 0);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void KillBrowser()
        {
            try
            {
                Process browserkill = new Process();
                browserkill.StartInfo.CreateNoWindow = true;
                browserkill.StartInfo.UseShellExecute = false;
                browserkill.StartInfo.FileName = "taskkill";
                browserkill.StartInfo.Arguments = "/F /T /PID " + browser_process.Id;
                browserkill.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
            }
        }




    }
}
