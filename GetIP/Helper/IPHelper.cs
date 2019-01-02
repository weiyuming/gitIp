using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Collections;

namespace GetIP.Helper
{
    class IPHelper
    {
        public static String getip()
        {
            string tempip = "0.0.0.0";
            WebRequest wr = WebRequest.Create("http://www.ipip.net/");
            Stream s = wr.GetResponse().GetResponseStream();
            if (s != null)
            {
                StreamReader sr = new StreamReader(s, Encoding.UTF8);
                string all = sr.ReadToEnd();
                int start = all.IndexOf("您当前的IP：", StringComparison.Ordinal) + 7;
                int end = all.IndexOf("<", start, StringComparison.Ordinal);
                tempip = all.Substring(start, end - start);
                sr.Close();
                s.Close();
            }

            return tempip;
        }



        /// <summary>
        /// 获取本机所有的IP4地址
        /// </summary>
        private void getIP()
        {
            ArrayList ipList = new ArrayList();//存储IP的列表

            string hostName = Dns.GetHostName();//本机名   
            //System.Net.IPAddress[] addressList = Dns.GetHostByName(hostName).AddressList;//会警告GetHostByName()已过期，我运行时且只返回了一个IPv4的地址   
            System.Net.IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6   
            foreach (IPAddress ip in addressList)
            {
                if (System.Net.Sockets.AddressFamily.InterNetwork.Equals(ip.AddressFamily))//判断是IP4
                {
                    if (!ip.ToString().StartsWith("192"))
                    {
                        //outputLog("获取到的IP为 " + ip.ToString());
                        ipList.Add(ip.ToString());
                    }


                }
            }
        }



        public void getMacByIP(String ip)
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            //远程服务器ip
            string remoteHostNameAddress = ip;
            //构造Ping实例
            Ping pingSender = new Ping();
            //Ping选项设置
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            //测试数据
            string data = "test data abcabc";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            //设置超时时间
            int timeout = 120;
            //调用同步send方法发送数据，将返回结果保存至PingReply实例
            PingReply reply = pingSender.Send(remoteHostNameAddress, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                foreach (NetworkInterface adapter in adapters)
                {
                    //outputLog("答复的主机地址：" + reply.Address.ToString());
                    //outputLog("往返时间:" + reply.RoundtripTime);
                    //outputLog("生存时间（TTL）：" + reply.Options.Ttl);
                    //outputLog("MAC地址：" + adapter.GetPhysicalAddress());
                }
            }
            else
            {
                //outputLog(reply.Status.ToString());
            }
        }




        /// <summary>
        /// 获取本机物理网卡的ip
        /// </summary>
        /// <returns></returns>
        public static string IPAddress()
        {
            string userIP = "";
            System.Net.NetworkInformation.NetworkInterface[] fNetworkInterfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            foreach (System.Net.NetworkInformation.NetworkInterface adapter in fNetworkInterfaces)
            {
                string fRegistryKey = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\" + adapter.Id + "\\Connection";
                Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
                if (rk != null)
                {
                    // 区分 PnpInstanceID      
                    // 如果前面有 PCI 就是本机的真实网卡          
                    string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
                    int fMediaSubType = Convert.ToInt32(rk.GetValue("MediaSubType", 0));
                    if (fPnpInstanceID.Length > 3 &&
                    fPnpInstanceID.Substring(0, 3) == "PCI")
                    {
                        //string fCardType = "物理网卡";
                        System.Net.NetworkInformation.IPInterfaceProperties fIPInterfaceProperties = adapter.GetIPProperties();
                        System.Net.NetworkInformation.UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = fIPInterfaceProperties.UnicastAddresses;
                        foreach (System.Net.NetworkInformation.UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                        {
                            if (UnicastIPAddressInformation.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                userIP = UnicastIPAddressInformation.Address.ToString(); // Ip 地址     
                        }
                        break;
                    }
                }
            }
            return userIP;
        }
    }
}
