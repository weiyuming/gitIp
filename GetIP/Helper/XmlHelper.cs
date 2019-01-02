using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using GetIP.vo;

namespace GetIP.Helper
{
    class XmlHelper
    {
        public static List<EmailVo> getEmailListByXml()
        {
            List<EmailVo> list = new List<EmailVo>();
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                XmlReader reader = XmlReader.Create(@"Email.xml", settings);
                doc.Load(reader);
                reader.Close();

                //获得跟节点  user
                XmlNode xn = doc.SelectSingleNode("emails");

                //获取子节点
                XmlNodeList xnl = xn.ChildNodes;

                
                //循环节点
                foreach (XmlNode xn1 in xnl)
                {
                    EmailVo email = new EmailVo();
                    // 将节点转换为元素，便于得到节点的属性值
                    XmlElement xe = (XmlElement)xn1;
                    // 得到Book节点的所有子节点
                    XmlNodeList xnl0 = xe.ChildNodes;
                    email.Name = xnl0.Item(0).InnerText;
                    email.EmailAddress = xnl0.Item(1).InnerText;
                    list.Add(email);
                }
            }
            catch (Exception ex)
            {

            }
            return list;

        }


        public static List<LastIp> getLastIpByXml()
        {
            List<LastIp> list = new List<LastIp>();
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                XmlReader reader = XmlReader.Create(@"LastIP.xml", settings);
                doc.Load(reader);
                reader.Close();

                //获得跟节点  user
                XmlNode xn = doc.SelectSingleNode("ips");

                //获取子节点
                XmlNodeList xnl = xn.ChildNodes;


                //循环节点
                foreach (XmlNode xn1 in xnl)
                {
                    LastIp lastIp = new LastIp();
                    // 将节点转换为元素，便于得到节点的属性值
                    XmlElement xe = (XmlElement)xn1;
                    // 得到Book节点的所有子节点
                    XmlNodeList xnl0 = xe.ChildNodes;
                    lastIp.Ip = xnl0.Item(0).InnerText;
                    list.Add(lastIp);
                }
            }
            catch (Exception ex)
            {

            }
            return list;

        }




        /// <summary>
        /// 更新XML中指定节点的值
        /// </summary>
        /// <param name="Path">XML文件路径</param>
        /// <param name="NodeName">需要更改的节点</param>
        /// <param name="NodeValue">需要更新的节点值</param>
        public static void UpdateNode(string Path, string NodeName, string NodeValue)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Path);
            XmlNode xn = doc.SelectSingleNode("//" + NodeName + "");
            xn.InnerText = NodeValue;
            doc.Save(Path);
        }








    }
}
