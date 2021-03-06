﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using MeleeLib;
using MeleeLib.DatHandler;

namespace MasterHand {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window {
        public Window1() {
            InitializeComponent();
            this.Title += " " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        static void prettyPrint(object o, StringBuilder sb) {
            sb.AppendLine("<table>");
            foreach (PropertyInfo pi in o.GetType().GetProperties()) {

                if (pi.Name.Contains("Offset") && !pi.Name.Contains("Count"))
                    sb.AppendFormat("<tr><td>{0:6}</td><td>@0x{1:X8}</td></tr>\n", pi.Name, pi.GetValue(o, null));
                else if (pi.Name.Contains("Flags"))
                    sb.AppendFormat("<tr><td>{0:6}</td><td>{1:032}</td></tr>\n", pi.Name, Convert.ToString((uint)pi.GetValue(o, null), 2));
                else
                    sb.AppendFormat("<tr><td>{0:6}</td><td>{1}</td></tr>\n", pi.Name, pi.GetValue(o, null));

            }
            sb.AppendLine("</table>");
        }
        private void HTML_Output(File dat) { Overview.NavigateToString("Under construction."); 
            //string H1 = "<h1>{0}</h1>";
            //string H2 = "<h2>{0}</h2>";
            //var sb = new StringBuilder();
            ////PrettyPrint XD
            //sb.AppendFormat(H1,dat.Filename);
            //prettyPrint(dat.Header, sb);
            //sb.AppendFormat(H2,"Section Type 1's");
            //foreach (string name in dat.Section1Entries.Keys)
            //{
            //    sb.AppendLine(name);
            //    prettyPrint(dat.Section1Entries[name], sb);
            //}
            //sb.AppendFormat(H2,"Section Type 2's");
            //foreach (string name in dat.Section2Entries.Keys)
            //{
            //    sb.AppendLine(name);
            //    prettyPrint(dat.Section2Entries[name], sb);
            //}
            //sb.AppendFormat(H2,"FTHeader");
            //prettyPrint(dat.FTHeader, sb);
            //sb.AppendFormat(H2, "Attributes");
            //sb.AppendLine("<table>");
            //foreach (MeleeLib.Attribute a in dat.Attributes)
            //{
            //    sb.AppendFormat("<tr><td>0x{0:X3}</td><td>{1}</td></tr>\n", a.Offset, a.Value);
            //}
            //sb.AppendLine("</table>");
            //sb.AppendFormat(H2, "Subaction Headers");
            //foreach(MeleeLib.Subaction s in dat.Subactions)
            //{
            //    prettyPrint(s.Header, sb);
            //}
            //Overview.NavigateToString(sb.ToString());
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (!dialog.ShowDialog().Value)
                return;
            var datfile = new File(dialog.FileName);
            DataContext = datfile;
            var list = datfile.FtIndex.Select(ftData => ftData).ToList();
            // TODO foreach (SectionHeader sh in datfile.Section2Entries.Values) list.Add(sh);
            DSListBox.ItemsSource = list;
            HTML_Output(datfile);

        }
    }
}
