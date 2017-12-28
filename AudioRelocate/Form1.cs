using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioRelocate
{
    public partial class Form1 : Form
    {
        string audio = Environment.CurrentDirectory + "\\audio.txt";
        public Form1()
        {
            InitializeComponent();
            FileInfo audioFileInfo = new FileInfo(audio);
            if (!audioFileInfo.Exists)
            {
                MessageBox.Show("音频文件路径文件不存在");
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.ShowDialog();
            this.textBox1.Text = this.folderBrowserDialog1.SelectedPath;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog2.ShowDialog();
            this.textBox2.Text = this.folderBrowserDialog2.SelectedPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pathFrom = this.textBox1.Text;
            string pathTo = this.textBox2.Text;

            if ("".Equals(pathFrom) || "".Equals(pathTo))
            {
                MessageBox.Show("请选择扫描文件位置或者音频文件转移位置");
                return;
            }
            DirectoryInfo diFrom = new DirectoryInfo(pathFrom);
            if (!diFrom.Exists)
            {
                MessageBox.Show("选择的扫描位置不存在");
                return;
            }
            DirectoryInfo diTo = new DirectoryInfo(pathTo);
            if (!diTo.Exists)
            {
                MessageBox.Show("请选择音频文件转移的位置");
                return;
            }
            long sum = 0;
            long success = 0;
            try
            {
                List<string> lines = new List<string>(File.ReadAllLines(audio));

                List<FileInfo> audioFileList = GetAllFiles(diFrom);
                foreach (FileInfo audioFile in audioFileList)
                {
                    sum++;
                    foreach (string line in lines)
                    {
                        if (audioFile.Name.Replace(audioFile.Extension, "").Equals(line))
                        {
                            audioFile.CopyTo(diTo + "//" + audioFile.Name,true);
                            success++;
                            break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("迁移异常。" + ex.Message);
                return;
            }
            MessageBox.Show("迁移完成。总文件数：" + sum + " " + "迁移文件数：" + success);
        }

        public static List<FileInfo> GetAllFiles(DirectoryInfo dir)
        {
            List<FileInfo> FileList = new List<FileInfo>();
            FileInfo[] allFile = dir.GetFiles("*.wav");
            foreach (FileInfo fi in allFile)
            {
                FileList.Add(fi);
            }
            DirectoryInfo[] allDir = dir.GetDirectories();
            foreach (DirectoryInfo d in allDir)
            {
                GetAllFiles(d);
            }
            return FileList;
        }
    }
}
