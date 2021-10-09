﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WPF6
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Entities.CarServiceEntities Context
        { get; } = new Entities.CarServiceEntities();

        public static Entities.User CurrentUser = null;
    }
}
