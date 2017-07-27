using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace PublicClass
{

    public class TaskType
    {
        public string TaskSet_ComBox_TaskNum { get; set; }
        public string TaskSet_ComBox_DeviceSet { get; set; }
        public string TaskSet_ComBox_ReadWrite { get; set; }
        public string TaskSet_ComBox_DeviceType { get; set; }
        public string TaskSet_ComBox_DeviceNum { get; set; }
        public string TaskSet_Label_Is_Main { get; set; }
        public string TaskSet_ComBox_Protocol { get; set; }
        public string TaskSet_textBox_Spd { get; set; }
        public string TaskSet_textBox_Bit { get; set; }
        public string TaskSet_textBox_Sync_Bit { get; set; }
        public string TaskSet_textBox_Stop_Bit { get; set; }
        public string TaskSet_textBox_read_address { get; set; }
        public string TaskSet_textBox_read_len { get; set; }
        public string TaskSet_textBox_data_len { get; set; }
        public string TaskSet_textBox_write_address { get; set; }
        public string TaskSet_textBox_data_index { get; set; }
    }

    public class DeviceType
    {
        public string DeviceSet_ComBox_DeviceSet{ get; set; }
        public string DeviceSet_ComBox_Protocol{ get; set; }
        public string DeviceSet_ComBox_Spd { get; set; }
        public string DeviceSet_ComBox_Bit { get; set; }
        public string DeviceSet_ComBox_Sync_Bit { get; set; }
        public string DeviceSet_ComBox_Stop_Bit { get; set; }
        public bool DeviceSet_RadioBut_Is_Main { get; set; }
        public string DeviceSet_TextBox_Site { get; set; } 
        public string DropDevice_TextBox_stop { get; set; }
         
    }

    public class JsonType {
        public List<Serial> serial;
        public List<JTask> task;
        public List<Site> site;
        public int delay { get; set; }
        public string version { get; set; }
    }

    public class Serial {
        public int port { get; set; }
        public int[] attribute ;
        public string protocol { get; set; }
        public bool main { get; set; }
        //[ScriptIgnore]
        public string fault { get; set; }
        //[ScriptIgnore]
        public string stop { get; set; }
    }

    public class JTask {
        public List<Read> read;
        public List<Write> write;
    }

    public class Read {
        public int task_number { get; set; }
        public int port { get; set; }
        public string is_main { get; set; }
        public int site_number { get; set; }
        public string address { get; set; }
        public int read_length { get; set; }
        public int data_length { get; set; }
        public int[] data_index;
    }

    public class Write
    {
        public int task_number { get; set; }
        public int port { get; set; }
        public string is_main { get; set; }
        public int site_number { get; set; }
        public string address { get; set; }
        public int read_length { get; set; }
        public int data_length { get; set; }
        public int[] data_index;
    }

    public class Site {
        public int port { get; set; }
        public string fault { get; set; }
        public string stop { get; set; }
    }

    public class TaskNum : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }



}
