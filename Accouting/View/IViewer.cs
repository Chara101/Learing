﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.View
{
    internal interface IViewer
    {
        public void ShowAllRecord(List<Record> records, ListBox listbox);
    }
}
