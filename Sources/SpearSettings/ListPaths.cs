using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpearSettings
{
    public class ListPaths
    {
        internal ListPaths()
        {
            this.listPaths = new List<string>();
            this.listPathLabels = new List<string>();
        }

        public List<string> listPaths { get; private set; }
        public string[] vecPaths 
        {
            get 
            {
                return listPaths.ToArray();
            }
        }
        internal List<string> listPathLabels { get; private set; }

        public bool checkLabels(ListPaths lpaths,out string errLabel1, out string errLabel2)
        {
            int len = lpaths.listPathLabels.Count;
            List<string> listIn = lpaths.listPathLabels;
            errLabel1 = "";
            errLabel2 = "";
            if (this.listPathLabels.Count == len)
            {
                for (int i = 0; i < len; i++)
                {
                    if (listIn[i].Trim() != this.listPathLabels[i].Trim())
                    {
                        errLabel1 = this.listPathLabels[i].Trim();
                        errLabel2 = listIn[i].Trim();
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        internal void addpath(string path, string label)
        {
            if (!path.Trim().Equals("") && !label.Trim().Equals(""))
            {
                this.listPaths.Add(path);
                this.listPathLabels.Add(label);
            }
        }

        internal void addpath(ListPaths lpaths)
        {
            List<string> ll = lpaths.listPathLabels;
            List<string> lp = lpaths.listPaths;
            this.listPathLabels.AddRange(ll);
            this.listPaths.AddRange(lp);
        }

    }
}
