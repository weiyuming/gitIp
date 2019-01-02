using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using GetIP.Helper;
using System.Collections;
using GetIP.vo;
using System.Threading.Tasks;

namespace GetIP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<EmailVo> emailList = new List<EmailVo>();
        String emails = "";

        private void Form1_Load(object sender, EventArgs e)
        {



            Form1 f = new Form1();

            //设置图标
            Icon icon = new Icon("ip_address_128px_1169699_easyicon.net.ico");
            f.Icon = icon;
            //设置窗体居中FormStartPosition
            f.StartPosition = FormStartPosition.CenterScreen;


            richTextBoxLog.ReadOnly = true;//设置日志框为只读
            richTextBoxLog.BackColor = Color.White;//设置背景色


            outputLog("【系统启动中】");






            txtboxJiange.Text = "600000";//600秒 10分钟
                ;
            outputLog("【系统启动中】设置检测间隔时间为" + txtboxJiange.Text + "毫秒");


            timerStart.Interval = 1;//为了启动后就执行一次，此处设置为1
            timerStart.Enabled = true;//启动定时任务
            outputLog("【系统启动中】设置主定时是否启动为" + timerStart.Enabled);


            //emailList = XmlHelper.getEmailListByXml();

            //if (emailList != null && emailList.Count > 0)
            //{
            //    for (int i = 0; i < emailList.Count; i++)
            //    {
            //        EmailVo emailVo = emailList[i];
            //        outputLog("【系统启动中】邮件发送人 " + emailVo.Name + " " + emailVo.EmailAddress);
            //        emails += emailVo.EmailAddress + ",";
            //    }
            //    emails = emails.Substring(0,emails.Length-1);
            //    //outputLog(emails);
            //}

        }


        private void button1_Click(object sender, EventArgs e)
        {
            getLocalIP();
        }






        



        private void getLocalIP()
        {
            String ip = IPHelper.getip();
            String lastIp = "";


            //获取最后一次记录的IP 
            lastIp =XmlHelper.getLastIpByXml()[0].Ip;
            if (!StringHelper.isEqual(lastIp, ip))//如果两个IP不相同则发邮件
            {
                outputLog("【IP发生变化】之前的IP为" + lastIp);
                outputLog("【IP发生变化】最新的IP为" + ip );

                outputLog("【IP发生变化】更新为LastIP.xml中记录的IP为 " + ip);
                XmlHelper.UpdateNode("LastIP.xml", "ip", ip);



                String title = "服务器IP由" + lastIp + "更换为 " + ip;//标题
                String msg = title;
                //String email = emails;
                emailList = XmlHelper.getEmailListByXml();//实时获取邮箱列表
                for (int i = 0; i < emailList.Count; i++)
                {
                    EmailVo emailVo = emailList[i];
                    outputLog("【邮件通知】" + emailVo.Name+",邮箱地址为"+emailVo.EmailAddress);
                    //new Thread(Test) { IsBackground = false }.Start();      //.Net 在1.0的时候，就已经提供最基本的API.
                    //ThreadPool.QueueUserWorkItem(o => Test());              //线程池中取空闲线程执行委托（方法）
                    EmailHelper.sendMail(emailVo.EmailAddress, msg, title);
                }
            }
            else
            {
                outputLog("【IP无变化】获取到的IP为" + ip);
            }

        }





        //日志记录 最外层
        private void outputLog(String msg)
        {
            outputLog(msg, Color.Black, false);
        }

        //日志记录
        private void outputLog(String msg, Color color, Boolean isBold)
        {


            //让文本框获取焦点，不过注释这行也能达到效果
            this.richTextBoxLog.Focus();
            //设置光标的位置到文本尾   
            this.richTextBoxLog.Select(this.richTextBoxLog.TextLength, 0);
            //滚动到控件光标处   
            this.richTextBoxLog.ScrollToCaret();
            //设置字体颜色
            this.richTextBoxLog.SelectionColor = color;
            if (isBold)
            {
                this.richTextBoxLog.SelectionFont = new Font(Font, FontStyle.Bold);
            }
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now; //获取当前时间
            msg = currentTime + "  " + msg;
            this.richTextBoxLog.AppendText(msg);//输出到界面
            this.richTextBoxLog.AppendText(Environment.NewLine);




            LogHelper.setlog(msg);//写入日志文件
        }


        private void timer1_Tick(object sender, EventArgs e)
        {



            getLocalIP();
            //重新设置间隔时间
            timerStart.Interval = Int32.Parse(txtboxJiange.Text);
        }


        







        #region
        //创建NotifyIcon对象 
        NotifyIcon notifyicon = new NotifyIcon();
        //创建托盘图标对象 
        Icon ico = new Icon("cat16.ico");
        //创建托盘菜单对象 
        ContextMenu notifyContextMenu = new ContextMenu();
        #endregion


        #region 托盘提示
        //private void Form1_Load(object sender, EventArgs e)
        //{

        //}
        #endregion

        #region 隐藏任务栏图标、显示托盘图标
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //判断是否选择的是最小化按钮 
            if (WindowState == FormWindowState.Minimized)
            {
                //托盘显示图标等于托盘图标对象 
                //注意notifyIcon1是控件的名字而不是对象的名字 
                notifyIcon1.Icon = ico;
                //隐藏任务栏区图标 
                this.ShowInTaskbar = false;
                //图标显示在托盘区 
                notifyicon.Visible = true;
            }
        }
        #endregion

        #region 还原窗体
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            //判断是否已经最小化于托盘 
            if (WindowState == FormWindowState.Minimized)
            {
                //还原窗体显示 
                WindowState = FormWindowState.Normal;
                //激活窗体并给予它焦点 
                this.Activate();
                //任务栏区显示图标 
                this.ShowInTaskbar = true;
                //托盘区图标隐藏 
                notifyicon.Visible = false;


                //让文本框获取焦点，不过注释这行也能达到效果
                this.richTextBoxLog.Focus();
                //设置光标的位置到文本尾   
                this.richTextBoxLog.Select(this.richTextBoxLog.TextLength, 0);
                //滚动到控件光标处   
                this.richTextBoxLog.ScrollToCaret();
            }
        }
        #endregion


        //关闭窗体前执行
        private void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            //exitpassword form2 = new exitpassword();
            //form.FormBorderStyle = FormBorderStyle.None; 
            //隐藏子窗体边框（去除最小花，最大化，关闭等按钮）
            // form.TopLevel =false; 
            //指示子窗体非顶级窗体
            //this.panel1.Controls.Add(form);//将子窗体载入panel

            //form2.MdiParent = this;
            //form2.StartPosition = FormStartPosition.CenterScreen;

            e.Cancel = true;
            //form2.ShowDialog();

            this.WindowState = FormWindowState.Minimized;

            

            

            //方法二：指定父容器实现
            //Form2 
            //form=new 
            //Form2();
            //form.MdiParent=this;//指定当前窗体为顶级Mdi窗体
            //form.Parent=this.Panel1;//指定子窗体的父容器为
            //Frm.FormBorderStyle = FormBorderStyle.None;//隐藏子窗体边框，当然也可以在子窗体的窗体加载事件中实现
            //panelform.Show();
        }











        /*  以下为无用 的方法   */



        

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(IPHelper.getip());
        }

        


       


    }
}
