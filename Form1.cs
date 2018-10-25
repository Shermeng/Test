using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextDateAccess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SelectFile_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "文本文件|*.txt";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string url = openFile.FileName;
                FileAddr.Text = url;
            }
            
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            //读取TXT文件流
            System.IO.StreamReader sr = new System.IO.StreamReader(FileAddr.Text);
            string content= sr.ReadToEnd();

            //读取所有数据
            List<string> allData = new List<string>();
            List<string> timeData = new List<string>();
            List<string> tempData = new List<string>();
            List<string> CO2Data = new List<string>();
            List<string> RHData = new List<string>();

            string[] data = content.Split('"');
            foreach (var s in data)
            {
                if (s.Length > 18 || s.Length == 9 || s.Length == 11)
                {
                    allData.Add(s);
                }
            }
            allData.RemoveAt(0);
            allData.RemoveAt(0);
            //分项存储

            int i = 0;
            foreach (var s in allData)
            {
                switch (i % 4)
                {
                    case 0:
                        timeData.Insert(Convert.ToInt32(i / 4),s);
                        break;
                    case 1:
                        CO2Data.Insert(Convert.ToInt32(i / 4), s);
                        break;
                    case 2:
                        tempData.Insert(Convert.ToInt32(i / 4), s);
                        break;
                    case 3:
                        RHData.Insert(Convert.ToInt32(i / 4), s);
                        break;
                }
                i++;
            }


            //所有数据输出，字符串text
            string text = "";
            int k = 0;
            foreach (var s in allData)
            {
                text = text + s + " ";
                if(k % 4 == 3)
                {
                    text += "\r\n";
                }
                k++;
            }

            //ShowAverageValue();
            //数据平均
            float sum = 0f;
            foreach (var s in tempData)
                sum += Convert.ToSingle(s);

            text = (sum / tempData.Count).ToString();
            outputText.Text = "平均值"+text;
            
        }

        //30分钟的平均数据
        private void ShowAverageValue(int month, int date, int hour, int minute, List<string> Time, List<string> Data)
        {
            float sum = 0f;
            string text = "";
            //List<string> day = new List<string>();
            List<string> time = new List<string>();
            List<string> data = new List<string>();
            //int i = 0;
            //筛选出某一天的数据
            foreach (var t in Time)
            {
                if (t[0] + t[1] == month && t[3] + t[4] == date)
                {
                    
                    data.Add(Data[Time.IndexOf(t)]);
                }
            }
            //筛选出某时的数据
            foreach (var t in Time)
            {
                if (t[0] + t[1] != month && t[3] + t[4] != date)
                {
                    Time.Remove(t);
                }
            }


            foreach (var s in Data)
                sum += Convert.ToSingle(s);
            text = (sum / Data.Count).ToString();
            outputText.Text = "平均值" + text;
        }

    }




}
