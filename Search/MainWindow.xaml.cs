
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Search
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void AllDoc_DragEnter(object sender, DragEventArgs e)
        {
            AllDoc.Items.Clear();
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void AllDoc_Drop(object sender, DragEventArgs e)
        {
            AllDoc.Items.Clear();
            List<string> files = new List<string>();

            foreach (string obj in (string[])e.Data.GetData(DataFormats.FileDrop))
            {
                if (Directory.Exists(obj))
                {
                    files.AddRange(Directory.GetFiles(obj, "*.*", SearchOption.AllDirectories));
                }
                else
                {
                    files.Add(obj);
                }
            }
            for (int i = 0; i < files.Count; i++)
            {
                FileInfo fileses = new FileInfo(files[i]);
                ListBoxItem allDocs = new ListBoxItem();
                allDocs.Content = fileses.FullName;
                allDocs.Selected += Fi_Selected;
                AllDoc.Items.Add(allDocs);

            }
        }

        private void Fi_Selected(object sender, RoutedEventArgs e)
        {
            ListBoxItem s = (ListBoxItem)sender;
            MessageBox.Show(s.Content.ToString());
            //Process.Start(s.Content.ToString());
        }

        private void search_dir(object sender, RoutedEventArgs e)
        {
            AllDoc.Items.Clear();
            List<string> files = new List<string>();
            System.Windows.Forms.FolderBrowserDialog folderBrowser = new System.Windows.Forms.FolderBrowserDialog();

            System.Windows.Forms.DialogResult result = folderBrowser.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                

                foreach (string obj in Directory.GetFileSystemEntries(folderBrowser.SelectedPath))
                {
                    if (Directory.Exists(obj))
                    {
                        files.AddRange(Directory.GetFiles(obj, "*.*", SearchOption.AllDirectories));
                    }
                    else
                    {
                        files.Add(obj);
                    }
                }

                for (int i = 0; i < files.Count; i++)
                {
                    FileInfo fileses = new FileInfo(files[i]);
                    ListBoxItem allDocs = new ListBoxItem();
                    allDocs.Content = fileses.FullName;
                    allDocs.Selected += Fi_Selected;
                    AllDoc.Items.Add(allDocs);

                }
            }
        }
            
        
        public string Converter(string Element)
        {

            int index = Element.IndexOf(" ");
            int indexEnd = Element.Length;
            int r = indexEnd - index;
            string NewElement = Element.Substring(index, r);
            NewElement.Remove(1, 1);
            return NewElement;
        }
        public void progress(int i)
        {
                progr.Value = i;
        }

        [Obsolete]
        private void proc_Click(object sender, RoutedEventArgs e)
        {
            Otsev.Items.Clear();
            
            if (Converter(box.SelectedValue.ToString()) == " TXT")
            {
                List<string> files = new List<string>();

                for (int i = 0; i < AllDoc.Items.Count; i++)
                {
                    string[] ser = File.ReadAllLines(Converter(AllDoc.Items[i].ToString()));
                    for (int j = 0; j < ser.Length; j++)
                    {
                        if (ser[j].Contains(Search.Text))
                        {
                            files.Add(AllDoc.Items[i].ToString());
                            

                        }
                    }
                    progress(i);
                }
                for (int i = 0; i < files.LongCount(); i++)
                {
                    FileInfo fi = new FileInfo(Converter(files[i]));
                    ListBoxItem listBoxItem = new ListBoxItem();
                    listBoxItem.Content = fi.FullName;
                    listBoxItem.Selected += Fi_Selected;
                    Otsev.Items.Add(listBoxItem);
                }
            }
            if (Converter(box.SelectedValue.ToString()) == " PDF")
            {
                for (int i = 0; i < AllDoc.Items.Count; i++)
                {
                    FileInfo files = new FileInfo(Converter(AllDoc.Items[i].ToString()));
                    if (files.Extension.ToUpper() == ".PDF") {
                        PdfReader pdfReader = new PdfReader(Converter(AllDoc.Items[i].ToString()));
                        for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                        {
                            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

                            string currentPageText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                            if (currentPageText.Contains(Search.Text))
                            {
                                string ter = Converter(AllDoc.Items[i].ToString());
                                FileInfo fileses = new FileInfo(ter);
                                ListBoxItem allDocs = new ListBoxItem();
                                allDocs.Content = fileses.FullName;
                                allDocs.Selected += Fi_Selected;
                                Otsev.Items.Add(allDocs);
                            }
                        }
                        pdfReader.Close();
                    }
                }
                
            }
            if (Converter(box.SelectedValue.ToString()) == " DOXC")
            {
                
            }
            if(Converter(box.SelectedValue.ToString()) == " JPGandPNG")
            {

            }
            MessageBox.Show("Операция завершена");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
/*            for (int i = 0; i < AllDoc.Items.Count; i++)
            {
                FileInfo files = new FileInfo(Converter(AllDoc.Items[i].ToString()));
                if (files.Extension.ToUpper() == ".JPG" || files.Extension.ToUpper() == ".PNG")
                {
                    var converter = new GroupDocs.Conversion.Converter(files.FullName);
                    // Устанавливаем параметры конвертации для формата TXT
                    var convertOptions = converter.GetPossibleConversions()["txt"].ConvertOptions;
                    // Преобразовать в формат TXT
                    string txtfiles = files.FullName + $"_output.txt";
                    converter.Convert(txtfiles, convertOptions);
                    Otsev.Items.Add(txtfiles);
                    Otsev.MouseDoubleClick += Otsev_MouseDoubleClick;
                }
            }
            MessageBox.Show("Файлы конвектированны");*/
        }

        private void Otsev_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox box = sender as ListBox;
            string a = box.SelectedItem.ToString();
            
            Process.Start(a);
        }
    }
}
