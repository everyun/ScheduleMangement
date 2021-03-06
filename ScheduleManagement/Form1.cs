﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace ScheduleManagement
{

    public partial class MainForm : Form
    {
        public string connStr = "server=127.0.0.1;user=sa;password=sqlserver;database=schedule";
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string date = dateTimePicker2.Text;
            string sqlSelectTodayEvent = "select event from scheduleitem where time like '" + date + "%'";
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connStr;
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
            if (conn.State == ConnectionState.Open)
            {
                DataSet dataSetTodayEvent = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlSelectTodayEvent, conn);
                adapter.TableMappings.Add("Table", "scheduleitem");
                adapter.Fill(dataSetTodayEvent);
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    listBox1.Items.RemoveAt(i);
                }
                foreach(DataRow dr in dataSetTodayEvent.Tables[0].Rows)
                {
                    listBox1.Items.Add(dr[0].ToString());
                }
                if(listBox1.Items.Count > 0)
                {
                    listBox1.SetSelected(0,true);
                }
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string title = "";
            string date = "";
            string time = "";
            bool needInfo = true;
            string content = "";
            string dateTime = "";
            int needInfoInt = 0;
            title = (string)textBox1.Text.Trim();
            date = (string)dateTimePicker2.Text;
            time = (string)dateTimePicker1.Text;
            needInfo = (bool)checkBox1.Checked;
            content = (string)richTextBox1.Text.Trim();
            dateTime = date + " " + time;
            DateTime dateTimeInformat = DateTime.Parse(dateTime);
            string dateTimeInString = dateTimeInformat.ToString("yyyy-MM-dd HH:mm"); // 格式化日期为字符串

            if (title == "")
            {
                MessageBox.Show("事件标题不能为空", "错误");
            }
            else
            {
                if (needInfo)
                {
                    needInfoInt = 1; // 将是否需要通知转换为 int 
                }
                string sqlInsert = "insert into scheduleitem (event, time, remind, content) values ('" + title + "', '" + dateTimeInString + "', '" + needInfoInt + "', '" + content + "')";
                string sqlFindSameTime = "select * from scheduleitem where time = '" + dateTimeInString + "' or event = '" + title + "'";
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = connStr;
                try
                {
                    conn.Open();
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误");
                }
                if (conn.State == ConnectionState.Open)
                {
                    int existItemCount = 0;
                    DataSet dataSetItem = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlFindSameTime, conn);
                    adapter.TableMappings.Add("Table", "scheduleitem");
                    adapter.Fill(dataSetItem);
                    existItemCount = dataSetItem.Tables["scheduleitem"].Rows.Count;
                    
                    if (existItemCount > 0)
                    {
                        MessageBox.Show("事件重复", "错误");
                    }
                    else
                    {
                        SqlCommand insert = new SqlCommand();
                        insert.Connection = conn;
                        insert.CommandType = CommandType.Text;
                        insert.CommandText = sqlInsert;
                        SqlDataReader insertReader = insert.ExecuteReader();
                        insertReader.Close();
                        listBox1.Items.Add(title);
                    }
                }

                else
                    Text = "失败了";
            }
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int listIndex = listBox1.SelectedIndex;
            if (listIndex >= 0)
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = connStr;
                conn.Open();
                int selIndex = listBox1.SelectedIndex;
                string date = "";
                string time = "";
                string title = "";
                date = (string)dateTimePicker2.Text;
                time = (string)dateTimePicker1.Text;
                title = (string)listBox1.SelectedItem.ToString();
                string dateTime = date + " " + time;
                DateTime dateTimeInformat = DateTime.Parse(dateTime);
                string dateTimeInString = dateTimeInformat.ToString("yyyy-MM-dd HH:mm");
                string sqlselectitem = "delete from scheduleitem where time='" + dateTimeInString + "' and event = '" + title + "'";
                
                SqlCommand delete = new SqlCommand();
                delete.Connection = conn;
                delete.CommandType = CommandType.Text;
                delete.CommandText = sqlselectitem;
                SqlDataReader insertReader = delete.ExecuteReader();
                insertReader.Close();

                listBox1.Items.RemoveAt(selIndex);
                if (selIndex != 0)
                    listBox1.SetSelected(selIndex - 1, true);
                else
                    if(listBox1.Items.Count > 0)
                    listBox1.SetSelected(selIndex, true);
            }            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dateTimePicker1_ValueChanged_1(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string itemText = listBox1.Text;
            string date = dateTimePicker2.Text;
            string sqlFindItemDetial = "select * from scheduleitem where time like '" + date + "%' and  event = '" + itemText + "'";

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connStr;
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
            if (conn.State == ConnectionState.Open)
            {
                DataSet dataSetItemDetails = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlFindItemDetial, conn);
                adapter.TableMappings.Add("Table", "scheduleitem");
                adapter.Fill(dataSetItemDetails);
                DataRow dr = dataSetItemDetails.Tables[0].Rows[0];
                string time = dr["time"].ToString();
                int needInfo = (int)dr["remind"];
                string content = dr["content"].ToString();
                textBox1.Text = listBox1.Text;
                dateTimePicker1.Value = DateTime.Parse(time);
                if (needInfo == 1)
                {
                    checkBox1.CheckState = CheckState.Checked;
                }
                else
                {
                    checkBox1.CheckState = CheckState.Unchecked;
                }
                richTextBox1.Text = content;
               

            }
        }
    }
}
