using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoapServiceWCF
{
    public class DataStorage
    {

        public static DataStorage Instance { get; set; }


        readonly Dictionary<int, MainData> data;


        public MainData Create(MainData mainData)
        {
            lock (data)
            {
                if (data.Keys.Count == 0)
                {
                    mainData.Id = 0;
                }
                else
                {
                    mainData.Id = data.Keys.OrderByDescending(k => k).First() + 1;
                }

                data[mainData.Id] = mainData;

                return mainData;
            }
        }

        public MainData Read(int key)
        {
            lock (data)
            {
                if (data.ContainsKey(key))
                {
                    return data[key];
                }
                else
                {
                    return null;
                }
            }
        }

        public void Update(MainData mainData)
        {
            lock (data)
            {
                data[mainData.Id] = mainData;
            }
        }

        public void Delete(int key)
        {
            lock (data)
            {
                data.Remove(key);
            }
        }


        static DataStorage()
        {
            Instance = new DataStorage();
        }

        private DataStorage()
        {
            data = new Dictionary<int, MainData>();
        }

    }
}