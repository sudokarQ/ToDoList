using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    internal class FileIOService
    {
        private readonly string path;

        public FileIOService(string _path)
        {
            path = _path;
        }

        public BindingList<ToDoModel> LoadData()
        {
            var IsFileExist = File.Exists(path);
            if (!IsFileExist)
            {
                File.Create(path).Dispose();
                return new BindingList<ToDoModel>();
            }
            using (var stream = File.OpenText(path))
            {
                var filetext = stream.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<ToDoModel>>(filetext);
            }
        }

        public void SaveData(BindingList<ToDoModel> _toDoDataList)
        {
            using (StreamWriter writer = File.CreateText(path))
            {
                string output = JsonConvert.SerializeObject(_toDoDataList);
                writer.Write(output);
            }
        }
    }
}
